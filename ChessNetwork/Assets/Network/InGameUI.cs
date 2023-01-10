using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private GameStateChanger _gameStateChanger;
    [SerializeField] private GameObject _inGameUI; 

    private void OnEnable()
    {
        _gameStateChanger.StateChanged.AddListener(OnGameStateChange);
    }

    private void OnGameStateChange(GameStateChanger.State state)
    {
        switch (state)
        {
            case GameStateChanger.State.Launched:
                OnRoundStart();
            break;
            case GameStateChanger.State.WaitingLobby:
                OnRoundEnd(); 
            break;
        }
    }

    private void OnRoundStart()
    {
        _inGameUI.SetActive(false);
    }

    private void OnRoundEnd()
    {
        _inGameUI.SetActive(true);
    }
}
