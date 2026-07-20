using System;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class LanDiscovery : MonoBehaviour
{
    public static LanDiscovery Instance { get; private set; }
    public event Action<string> HostFound;
    public bool IsDiscovering { get; private set; }

    private const int DiscoveryPort = 7778;
    private const string DiscoveryMessage = "PERTANDINGAN_BILANGAN";

    private UdpClient _broadcastClient;
    private Thread _broadcastThread;
    private UdpClient _listenerClient;
    private Thread _listenerThread;
    private SynchronizationContext _mainThread;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _mainThread = SynchronizationContext.Current;
    }

    public void StartDiscovery()
    {
        if (IsDiscovering)
        {
            return;
        }

        IsDiscovering = true;
        _broadcastClient = new UdpClient();
        _broadcastClient.EnableBroadcast = true;
        _broadcastThread = new Thread(BroadcastLoop);
        _broadcastThread.IsBackground = true;
        _broadcastThread.Start();
        Debug.Log("LAN Discovery Started");
    }

    public void StartListening()
    {
        if (_listenerThread != null)
        {
            return;
        }

        _listenerClient = new UdpClient(DiscoveryPort);
        _listenerThread = new Thread(ListenLoop);
        _listenerThread.IsBackground = true;
        _listenerThread.Start();
        Debug.Log("LAN Listener Started");
    }

    private void BroadcastLoop()
    {
        IPEndPoint endPoint = new(IPAddress.Broadcast, DiscoveryPort);

        while (IsDiscovering)
        {
            try
            {
                string ip = GetLocalIPAddress();
                string message = $"{DiscoveryMessage}|{ip}";
                byte[] data = Encoding.UTF8.GetBytes(message);

                _broadcastClient.Send(
                    data,
                    data.Length,
                    endPoint);

                Thread.Sleep(1000);
            }
            catch
            {
                
            }
        }
    }

    private void ListenLoop()
    {
        IPEndPoint remoteEndPoint = new(IPAddress.Any, DiscoveryPort);

        while (true)
        {
            try
            {
                byte[] data = _listenerClient.Receive(ref remoteEndPoint);
                string message = Encoding.UTF8.GetString(data);

            string[] split = message.Split('|');

            if (split.Length != 2)
            {
                continue;
            }

            if (split[0] != DiscoveryMessage)
            {
                continue;
            }

            string ip = split[1];

            Debug.Log($"Host Found : {ip}");

            _mainThread.Post(_ =>
            {
                RaiseHostFound(ip);
            }, null);
            }
            catch
            {
                break;
            }
        }
    }

    private string GetLocalIPAddress()
    {
        foreach (IPAddress ip in Dns.GetHostAddresses(Dns.GetHostName()))
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        return "127.0.0.1";
    }

    public void StopDiscovery()
    {
        if (!IsDiscovering)
        {
            return;
        }

        IsDiscovering = false;
        _broadcastThread?.Interrupt();
        _broadcastThread = null;
        _broadcastClient?.Close();
        _broadcastClient = null;
        Debug.Log("LAN Discovery Stopped");
    }

    public void StopListening()
    {
        _listenerThread?.Interrupt();
        _listenerThread = null;
        _listenerClient?.Close();
        _listenerClient = null;
        Debug.Log("LAN Listener Stopped");
    }

    protected void RaiseHostFound(string ipAddress)
    {
        HostFound?.Invoke(ipAddress);
    }

    private void OnDestroy()
    {
        StopDiscovery();
        StopListening();
    }
}