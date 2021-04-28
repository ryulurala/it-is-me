using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PacketId
{
    // S: Server's packet, C: Client's packet
    S_Connected = 1,
    C_CreateRoom = 2,
    C_EnterRoom = 3,
    S_BroadcastEnterRoom = 4,
}

public abstract class Packet
{
    public ushort Protocol;
}

#region Server's packet
[Serializable]
public class S_Connected : Packet
{
    public ushort playerId;
}

[Serializable]
public class S_BroadcastEnterRoom : Packet
{
    // List<ushort> playerIds
}
#endregion

#region Client's packet
[Serializable]
public class C_CreateRoom : Packet
{
    public C_CreateRoom()
    {
        Protocol = (ushort)PacketId.C_CreateRoom;
    }
}

[Serializable]
public class C_EnterRoom : Packet
{
    public string roomId;

    public C_EnterRoom(string roomId)
    {
        Protocol = (ushort)PacketId.C_EnterRoom;
        this.roomId = roomId;
    }
}
#endregion