using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class AdminPanel : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private TMP_Text _stateField;
    [SerializeField] private string _somePlayerNotReady = "not all player's is ready or some team is empty";
    private void OnEnable()
    { 
        _startButton.onClick.AddListener(OnStartClick);
    }
    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(OnStartClick);
    }
    private void Start()
    {
        if (NetworkManager.Singleton.IsHost == false)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnStartClick()
    {
        if (NetworkManager.Singleton.IsServer == false)
        {
            return;
        }
        int notReadyPlayerCount = 0;
        int whitePlayerCount = 0;
        int blackPlayerCount = 0;
        foreach (Player player in Player.List)
        {
            if (player.Team != Team.Observer && player.IsReady == false)
            {
                notReadyPlayerCount++;
            }
            if (player.Team == Team.White)
            {
                whitePlayerCount++;
            }
            if (player.Team == Team.Black)
            {
                blackPlayerCount++;
            }
        }
        if (notReadyPlayerCount == 0 && whitePlayerCount != 0 && blackPlayerCount != 0)
        {
            GameStateChanger.Singletone.StartSession();
            return;
        }
        _stateField.text = _somePlayerNotReady;
    }
}
