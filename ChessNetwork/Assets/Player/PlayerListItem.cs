using System.Collections;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _nicknameField;
    [SerializeField] private TMP_Text _readyField;
    [SerializeField] private string _readyLabel = "Ready";
    [SerializeField] private string _notReadyLabel = "Not ready";
    [SerializeField] private string _observerLabel = "Observer";
    [SerializeField] private Image _phoneImage;
    [SerializeField] private UnityDictionarity<Team, Color> _colorByTeam;

    private Player _owner;

    public PlayerListItem Init(Player player)
    {
        _owner = player;
        _colorByTeam.Init();

        _owner.TeamChanged.AddListener(OnPlayerChangeTeam);
        _owner.ReadyStateChanged.AddListener(OnReadyStateChanged);

        _phoneImage.color = _colorByTeam[player.Team];
        _nicknameField.text = _owner.Nickname;

        StartCoroutine(NicknameSetcorutine());
        UpdateReadyState(player.IsReady);
        return this;
    }

    private void OnEnable()
    {
        Player.Disconected.AddListener(OnPlayerDisconect);
    }

    private void OnDisable()
    {
        Player.Disconected.RemoveListener(OnPlayerDisconect);
    }

    private void OnPlayerDisconect(Player player) 
    {
        if (player == _owner)
        {
            _owner.TeamChanged.RemoveListener(OnPlayerChangeTeam);
            Destroy(gameObject);
        }
    }

    private void OnPlayerChangeTeam(Player player)
    {
        _phoneImage.color = _colorByTeam[player.Team];
        UpdateReadyState(player.IsReady);
    }

    private void OnReadyStateChanged(bool newValue)
    {
        UpdateReadyState(newValue);
    }
    
    private void UpdateReadyState(bool newValue)
    {
        if (_owner.Team == Team.Observer)
        {
            _readyField.text = _observerLabel;
            return;
        }
        _readyField.text = newValue ? _readyLabel : _notReadyLabel;
    }


    private IEnumerator NicknameSetcorutine()
    {
        while (string.IsNullOrEmpty(_owner.Nickname))
        {
            yield return null;
        }
        _nicknameField.text = _owner.Nickname;
        _readyField.text = _owner.IsReady ? _readyLabel : _notReadyLabel;
    }
}
