using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EscMenu : MonoBehaviour
{
    [SerializeField] private GameObject _escMenuPanel;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _disconectButton;
    [SerializeField] private Button _exitButton;

    private StandartInputActions _inputActions => StaticInput.Singletone;

    private void OnEnable()
    {
        _inputActions.Standart.OpenEscMenu.started += OpenMenu;
        _continueButton.onClick.AddListener(OnContinueClick);
        _disconectButton.onClick.AddListener(OnDisconectClick);
        _exitButton.onClick.AddListener(OnExitClick);
    }
    private void OnDisable()
    {
        _inputActions.Standart.OpenEscMenu.started -= OpenMenu;
        _continueButton.onClick.RemoveListener(OnContinueClick);
        _disconectButton.onClick.RemoveListener(OnDisconectClick);
        _exitButton.onClick.RemoveListener(OnExitClick);
    }

    private void OpenMenu(InputAction.CallbackContext context)
    {
        _escMenuPanel.SetActive(_escMenuPanel.activeSelf == false);
    }
    private void OnContinueClick()
    {
        _escMenuPanel.SetActive(false);
    }
    private void OnDisconectClick()
    {
        NetworkManager.Singleton.Shutdown();
    }
    private void OnExitClick()
    {
        NetworkManager.Singleton.Shutdown();
        Application.Quit();
    }
}
