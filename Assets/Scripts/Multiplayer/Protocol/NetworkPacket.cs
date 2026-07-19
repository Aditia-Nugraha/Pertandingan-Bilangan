using System;

[Serializable]
public class NetworkPacket
{
    public NetworkCommand Command;
    public string Data;
}