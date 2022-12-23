using System.Collections.Generic;
using UnityEngine;

public class PlayerListUI : MonoBehaviour
{
    [SerializeField] private Transform _content;
    [SerializeField] private PlayerListItem _playerListItemPrefub;

    private Dictionary<ulong, PlayerListItem> _playerListItems = new();

    private void Start()
    {
        Player.Connected.AddListener(AddPlayer);
        Player.Disconected.AddListener(RemovePlayer);
    }

    private void OnDestroy()
    {
        Player.Connected.RemoveListener(AddPlayer);
        Player.Disconected.RemoveListener(RemovePlayer);
    }

    private void AddPlayer(Player.Info player)
    {
        PlayerListItem instance = Instantiate(_playerListItemPrefub, _content).Init(player);
        _playerListItems.Add(player.ClientID, instance);
    }

    private void RemovePlayer(Player.Info player)
    {
        _playerListItems.Remove(player.ClientID);
    }
}
