using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private GameStateChanger _gameStateChanger;
    [SerializeField] private GameObject _inGameUI; 

    private void OnEnable()
    {
        _gameStateChanger.SessionStart.AddListener(OnRoundStart);
        _gameStateChanger.SessionEnd.AddListener(OnRoundEnd);
    }
    private void OnDisable()
    {
        _gameStateChanger.SessionStart.RemoveListener(OnRoundStart);
        _gameStateChanger.SessionEnd.RemoveListener(OnRoundEnd);
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
