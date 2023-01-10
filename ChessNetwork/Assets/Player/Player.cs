using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Player : NetworkBehaviour
{
    private NetworkVariable<FixedString128Bytes> _nickname = new(new FixedString128Bytes(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private Team _team = Team.Observer;
    private UnityEvent<Player> _teamChanged = new();
    private NetworkVariable<bool> _isReady = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private UnityEvent<bool> _readyStateChanged = new();

    private static List<Player> list = new();
    private static UnityEvent<Player> _connected = new();
    private static UnityEvent<Player> _disconected = new();
    private static Dictionary<ulong, Player> _playerById = new();

    public static List<Player> List => new(list);
    public static UnityEvent<Player> Connected => _connected;
    public static UnityEvent<Player> Disconected => _disconected;
    public string Nickname => _nickname.Value.ToString();
    public static Dictionary<ulong, Player> ById => new(_playerById);
    public static Player Local => ById[NetworkManager.Singleton.LocalClientId];

    public UnityEvent<Player> TeamChanged => _teamChanged;
    public UnityEvent<bool> ReadyStateChanged => _readyStateChanged;
    public Team Team
    {
        get
        {
            return _team;
        }
        set
        {
            _team = value;
            _teamChanged.Invoke(this);
        }
    }
    public bool IsReady
    {
        get
        {
            return _isReady.Value;
        }
        set
        {
            _isReady.Value = value;
            SetReadyInvokeIventServerRpc(value);
        }
    }
    [ServerRpc]
    private void SetReadyInvokeIventServerRpc(bool value)
    {
        SetReadyInvokeIventClientRpc(value);
    }
    [ClientRpc]
    private void SetReadyInvokeIventClientRpc(bool value)
    {
        _readyStateChanged.Invoke(value);
    }

    public Info InstanceInfo => new Info() 
    {
        Nickname = _nickname,
        ClientID = OwnerClientId,
        Team = _team
    };

    public override void OnNetworkSpawn()
    {
        if (IsClient && IsLocalPlayer)
        {
            _nickname.Value = LocalPlayer.Nickname;
        }
        list.Add(this);
        _playerById.Add(OwnerClientId, this);
        _connected.Invoke(this);
    }

    public override void OnNetworkDespawn()
    {
        _disconected.Invoke(this);
        list.Remove(this);
        _playerById.Remove(OwnerClientId);
    }

    public struct Info
    {
        public NetworkVariable<FixedString128Bytes> Nickname;
        public ulong ClientID;
        public Team Team;
    }
}
