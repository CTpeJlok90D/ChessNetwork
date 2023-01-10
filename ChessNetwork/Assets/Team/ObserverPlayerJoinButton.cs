using UnityEngine;
using UnityEngine.UI;

public class ObserverPlayerJoinButton : MonoBehaviour
{
    [SerializeField] private TeamPlayerList _team;
    [SerializeField] private Button _joinButton;

    private void OnEnable()
    {
        _joinButton.onClick.AddListener(OnJoinClick);
    }
    private void OnDisable()
    {
        _joinButton.onClick.RemoveListener(OnJoinClick);
    }

    private void OnJoinClick()
    {
        _team.AddPlayer(Player.Local);
    }
}
