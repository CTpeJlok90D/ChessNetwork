using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamPlayerListView : MonoBehaviour
{
    [SerializeField] private TeamPlayerList _team;
    [SerializeField] private Button _joinButton;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private string _joinCaption = "+";

    private void OnEnable()
    {
        _team.PlayerJoined.AddListener(OnPlayerAdded);
        _team.PlayerLeave.AddListener(OnPlayerLeave);
        _joinButton.onClick.AddListener(OnJoinClick);
    }
    private void OnDisable()
    {
        _team.PlayerJoined.RemoveListener(OnPlayerAdded);
        _team.PlayerLeave.RemoveListener(OnPlayerLeave);
        _joinButton.onClick.RemoveListener(OnJoinClick);
    }

    private void OnPlayerAdded(ulong playerID)
    {
        if (_team.IsFull)
        {
            _joinButton.enabled = false;
            _text.text = Player.ById[playerID].Nickname;
        }
    }

    private void OnPlayerLeave(ulong playerID)
    {
        if (_team.IsFull) 
        {
            return;
        }
        _joinButton.enabled = true;
        _text.text = _joinCaption;
    }

    private void OnJoinClick()
    {
        _team.AddPlayer(Player.Local);
    }
}
