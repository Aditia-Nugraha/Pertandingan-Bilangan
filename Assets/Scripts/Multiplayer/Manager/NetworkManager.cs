using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class NetworkManager : MonoBehaviour
{
    [Header("Connection")]
    [SerializeField] private int _port = 7777;

    public static NetworkManager Instance { get; private set; }
    public NetworkConnectionState ConnectionState { get; private set; }
    public bool IsConnected { get; private set; }
    public bool IsHost => NetworkSession.Role == PlayerRole.Host;

    public event Action Connected;
    public event Action Disconnected;
    public event Action<NetworkPacket> PacketReceived;
    public event Action ConnectionFailed;

    private TcpListener _listener;
    private TcpClient _client;
    private NetworkStream _stream;
    private StreamReader _reader;
    private StreamWriter _writer;
    private Thread _receiveThread;
    private Thread _acceptThread;
    private SynchronizationContext _mainThread;

    public bool IsHosting => _listener != null;
    public bool IsClient => _client != null;

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

    public void Host()
    {
        if (_listener != null)
        {
            return;
        }

        _listener = new TcpListener(IPAddress.Any, _port);
        _listener.Start();
        ConnectionState = NetworkConnectionState.Hosting;
        _acceptThread = new Thread(AcceptClientLoop);
        _acceptThread.IsBackground = true;
        _acceptThread.Start();
    }

    private void AcceptClientLoop()
    {
        try
        {
            TcpClient client = _listener.AcceptTcpClient();

            _mainThread.Post(_ =>
            {
                HandleClientConnected(client);
            }, null);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    private void HandleClientConnected(TcpClient client)
    {
        _client = client;
        InitializeStream();
        StartReceiveLoop();
        ConnectionState = NetworkConnectionState.Connected;
        RaiseConnected();
    }

    private void HandleConnectedToHost(TcpClient client)
    {
        _client = client;
        InitializeStream();
        StartReceiveLoop();
        ConnectionState = NetworkConnectionState.Connected;
        RaiseConnected();
    }

    private void InitializeStream()
    {
        _stream = _client.GetStream();
        _reader = new StreamReader(_stream);
        _writer = new StreamWriter(_stream);
        _writer.AutoFlush = true;
    }

    public void Join(string ip)
    {
        if (_client != null)
        {
            return;
        }

        ConnectionState = NetworkConnectionState.Connecting;

        Thread connectThread = new Thread(() =>
        {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(ip, _port);

                _mainThread.Post(_ =>
                {
                    HandleConnectedToHost(client);
                }, null);
            }
            catch (SocketException exception)
            {
                Debug.LogError(exception.Message);

                _mainThread.Post(_ =>
                {
                    Disconnect();
                    ConnectionFailed?.Invoke();
                }, null);
            }
        });

        connectThread.IsBackground = true;
        connectThread.Start();
    }

    private void StartReceiveLoop()
    {
        _receiveThread = new Thread(ReceiveLoop);
        _receiveThread.IsBackground = true;
        _receiveThread.Start();
    }

    private void ReceiveLoop()
    {
        try
        {
            while (_client != null && _client.Connected)
            {
                string message = _reader.ReadLine();

                if (string.IsNullOrEmpty(message))
                {
                    continue;
                }

                _mainThread.Post(_ =>
                {
                    HandleReceivedMessage(message);
                },null);
            }
        }
        catch (IOException)
        {
            _mainThread.Post(_ =>
            {
                Disconnect();
            }, null);
        }
    }

    private NetworkPacket Decode(string message)
    {
        string[] split = message.Split('|', 2);

        return new NetworkPacket
        {
            Command = Enum.Parse<NetworkCommand>(split[0]),
            Data = split.Length > 1
                ? split[1]
                : string.Empty
        };
    }

    private void HandleReceivedMessage(string message)
    {
        NetworkPacket packet = Decode(message);
        RaisePacket(packet);
    }

    public void Send(NetworkCommand command)
    {
        Send(command, string.Empty);
    }

    public void Send(NetworkCommand command, string data)
    {
        if (_writer == null)
        {
            return;
        }

        string message = Encode(command, data);
        _writer.WriteLine(message);
    }

    private string Encode(NetworkCommand command, string data)
    {
        return $"{command}|{data}";
    }

    public void Disconnect()
    {
        _receiveThread?.Interrupt();
        _receiveThread = null;

        _acceptThread?.Interrupt();
        _acceptThread = null;

        _reader?.Close();
        _writer?.Close();
        _stream?.Close();

        _client?.Close();
        _listener?.Stop();

        _reader = null;
        _writer = null;
        _stream = null;
        _client = null;
        _listener = null;

        ConnectionState = NetworkConnectionState.None;
        RaiseDisconnected();
    }

    private void RaiseConnected()
    {
        IsConnected = true;
        Connected?.Invoke();
    }

    private void RaiseDisconnected()
    {
        IsConnected = false;
        Disconnected?.Invoke();
    }

    private void RaisePacket(NetworkPacket packet)
    {
        PacketReceived?.Invoke(packet);
    }

    private void OnDestroy()
    {
        Disconnect();
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }
}