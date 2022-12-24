using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Player : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<FixedString128Bytes> _nickname = new(new FixedString128Bytes(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private static List<Player> list = new();
    private static UnityEvent<Info> _connected = new();
    private static UnityEvent<Info> _disconected = new();
    private static Dictionary<ulong, Player> _playerById = new();

    public static List<Player> List => new(list);
    public static UnityEvent<Info> Connected => _connected;
    public static UnityEvent<Info> Disconected => _disconected;
    public string Nickname => _nickname.Value.ToString();
    public static Dictionary<ulong, Player> PlayerById => _playerById;
    public Info InstanceInfo => new Info() 
    {
        Nickname = _nickname,
        ClientID = OwnerClientId
    };

    public override void OnNetworkSpawn()
    {
        if (IsClient && IsLocalPlayer)
        {
            _nickname.Value = LocalPlayer.Nickname;
        }
        list.Add(this);
        _playerById.Add(OwnerClientId, this);
        _connected.Invoke(InstanceInfo);
    }

    public override void OnDestroy()
    {
        list.Remove(this);
        _playerById.Remove(OwnerClientId);
        _disconected.Invoke(InstanceInfo);
    }

    public struct Info
    {
        public NetworkVariable<FixedString128Bytes> Nickname;
        public ulong ClientID;
    }
}
