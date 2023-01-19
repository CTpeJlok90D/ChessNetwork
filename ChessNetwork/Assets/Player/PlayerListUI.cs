using System.Collections.Generic;
using UnityEngine;

public class PlayerListUI : MonoBehaviour
{
    [SerializeField] private Transform _content;
    [SerializeField] private PlayerListItem _playerListItemPrefub;

    private Dictionary<Player, PlayerListItem> _playerListItems = new();

    private void OnEnable()
    {
        Player.Connected.AddListener(AddPlayer);
        Player.Disconected.AddListener(RemovePlayer);

        foreach (PlayerListItem playerListItem in _playerListItems.Values)
        {
            Destroy(playerListItem.gameObject);
        }
        _playerListItems.Clear();

        foreach (Player player in Player.List)
        {
            AddPlayer(player);
        }
    }

    private void OnDisable()
    {
        Player.Connected.RemoveListener(AddPlayer);
        Player.Disconected.RemoveListener(RemovePlayer);
    }

    private void AddPlayer(Player player)
    {
        PlayerListItem instance = Instantiate(_playerListItemPrefub, _content).Init(player);
        _playerListItems.Add(player, instance);
    }

    private void RemovePlayer(Player player)
    {
        _playerListItems.Remove(player);
    }
}
