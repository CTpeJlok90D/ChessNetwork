using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private GameStateChanger _gameStateChanger;
    [SerializeField] private GameObject _inGameUI; 

    private void OnEnable()
    {
        _gameStateChanger.SessionStart.AddListener(OnRoundStart);
    }
    private void OnDisable()
    {
        _gameStateChanger.SessionStart.RemoveListener(OnRoundStart);
    }

    private void OnRoundStart()
    {
        _inGameUI.SetActive(false);
    }
}
