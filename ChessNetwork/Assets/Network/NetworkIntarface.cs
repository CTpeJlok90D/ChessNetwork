using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;
using UnityEngine.UI;

public class NetworkIntarface : MonoBehaviour
{
    [SerializeField] private UNetTransport _transport;
    [SerializeField] private GameObject _connectPanel;
    [SerializeField] private Button _connectButton;
    [SerializeField] private Button _hostButton;
    [SerializeField] private TMP_InputField _ipInputField;
    [SerializeField] private TMP_InputField _nicknameInputField;
    [SerializeField] private GameObject _lobbyUI;
    [SerializeField] private GameStateChanger _gameStateChanger;

    private void Awake()
    {
        _nicknameInputField.text = LocalPlayer.Nickname;
    }

    private void OnEnable()
    {
        _connectButton.onClick.AddListener(OnConnectClick);
        _hostButton.onClick.AddListener(OnHostClick);
        _nicknameInputField.onEndEdit.AddListener(OnNicknameChange);
        _ipInputField.onEndEdit.AddListener(OnIpFieldChanged);
        _gameStateChanger.StateChanged.AddListener(OnSessionStart);
    }

    private void OnDisable()
    {
        _connectButton.onClick.RemoveListener(OnConnectClick);
        _hostButton.onClick.RemoveListener(OnHostClick);
        _nicknameInputField.onEndEdit.RemoveListener(OnNicknameChange);
        _ipInputField.onEndEdit.RemoveListener(OnIpFieldChanged);
        _gameStateChanger.StateChanged.RemoveListener(OnSessionStart);
    }

    private void OnConnectClick()
    {
        if (string.IsNullOrEmpty(LocalPlayer.Nickname.ToString()))
        {
            return;
        }
        NetworkManager.Singleton.OnClientConnectedCallback += OnConnect;
        NetworkManager.Singleton.StartClient();
    }


    private void OnHostClick()
    {
        if (string.IsNullOrEmpty(LocalPlayer.Nickname.ToString()))
        {
            return;
        }
        NetworkManager.Singleton.OnClientConnectedCallback += OnConnect;
        NetworkManager.Singleton.StartHost();
    }

    private void OnConnect(ulong id)
    {
        _connectPanel.SetActive(false);
        _lobbyUI.SetActive(true);
        Player.Disconected.AddListener(OnServerDisconect);
        NetworkManager.Singleton.OnClientConnectedCallback -= OnConnect;
    }

    private void OnServerDisconect(Player player)
    {
        if (player.OwnerClientId == NetworkManager.Singleton.LocalClientId) 
        {
            _connectPanel.SetActive(true);
            _lobbyUI.SetActive(false);
            Player.Disconected.RemoveListener(OnServerDisconect);
        }
    }

    private void OnSessionStart(GameStateChanger.State state)
    {
        if (state is GameStateChanger.State.Launched)
        {
            _lobbyUI.SetActive(false);
        }
    }

    private void OnIpFieldChanged(string newIP)
    {
        _transport.ConnectAddress = newIP;
    }

    private void OnNicknameChange(string newNickname) 
    {
        LocalPlayer.Nickname = newNickname;
    }
}
