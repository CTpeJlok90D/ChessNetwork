using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class TeamPlayerList : NetworkBehaviour
{
    [SerializeField] private List<ulong> _playerList = new();
    [SerializeField] private Team _team;
    [SerializeField] private bool _ulimitedPlayerCount;
    [SerializeField] private int _maxPlayers;

    private UnityEvent<ulong> _playerAdded = new();
    private UnityEvent<ulong> _playerRemoved = new();

    public List<ulong> Players => _playerList;
    public Team Team => _team;
    public int MaxPlayers 
    { 
        get 
        { 
            if (_ulimitedPlayerCount || _maxPlayers < 0)
            {
                return int.MaxValue;
            }
            return _maxPlayers; 
        } 
    }
    public bool IsFull => _maxPlayers <= _playerList.Count;
    public UnityEvent<ulong> PlayerJoined => _playerAdded;
    public UnityEvent<ulong> PlayerLeave => _playerRemoved;


    #region Syncronization
    public override void OnNetworkSpawn()
    {
        _playerList.Clear();
        SynchronizeTeamServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SynchronizeTeamServerRpc(ulong ownerID)
    {
        foreach (ulong player in _playerList)
        {
            AddPlayerSynchronizationClientRpc(player, ownerID);
        }
    }

    [ClientRpc]
    private void AddPlayerSynchronizationClientRpc(ulong playerID, ulong owner)
    {
        if (NetworkManager.LocalClientId != owner)
        {
            return;
        }
        AddPlayerLocal(playerID);
    }
    #endregion
    #region Events
    protected void OnEnable()
    {
        Player.Disconected.AddListener(OnPlayerDisconect);
    }
    protected void OnDisable()
    {
        Player.Disconected.RemoveListener(OnPlayerDisconect);
    }

    private void OnPlayerDisconect(Player player)
    {
        RemovePlayer(player);
        if (player.IsLocalPlayer)
        {
            _playerList.Clear();
        }
    }

    public void AddPlayer(Player player)
    {
        AddPlayer(player.OwnerClientId);
    }
    public void AddPlayer(ulong playerID)
    {
        AddPlayerServerRpc(playerID);
    }
    #endregion
    #region Add and remove playrs
    [ServerRpc(RequireOwnership = false)]
    private void AddPlayerServerRpc(ulong playerID)
    {
        AddPlayerLocal(playerID);
        AddPlayerClientRpc(playerID);
    }
    [ClientRpc]
    private void AddPlayerClientRpc(ulong playerID)
    {
        AddPlayerLocal(playerID);
    }
    private void AddPlayerLocal(ulong playerID)
    {
        if (_playerList.Contains(playerID) || _playerList.Count >= MaxPlayers)
        {
            return;
        }

        Player player = Player.ById[playerID];
        player.Team = _team;
        player.TeamChanged.AddListener(OnPlayerTeamChange);
        _playerList.Add(playerID);
        _playerAdded.Invoke(player.OwnerClientId);
    }

    public void RemovePlayer(Player player)
    {
        RemovePlayer(player.OwnerClientId);
    }
    public void RemovePlayer(ulong playerID)
    {
        RemovePlayerServerRpc(playerID);
    }
    [ServerRpc(RequireOwnership = false)]
    private void RemovePlayerServerRpc(ulong playerID)
    {
        RemovePlayerClientRpc(playerID);
    }
    [ClientRpc]
    private void RemovePlayerClientRpc(ulong playerID)
    {
        Player player = Player.ById[playerID];

        player.TeamChanged.RemoveListener(OnPlayerTeamChange);
        _playerList.Remove(playerID);
        _playerRemoved.Invoke(playerID);
    }

    private void OnPlayerTeamChange(Player player)
    {
        if (player.Team != _team)
        {
            RemovePlayer(player);
        }
    }
    #endregion
}