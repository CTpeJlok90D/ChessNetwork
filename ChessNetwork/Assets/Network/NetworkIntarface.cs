using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;

public class NetworkIntarface : MonoBehaviour
{
    [SerializeField] private UNetTransport _transport;
    [SerializeField] private GameObject _connectPanel;
    [SerializeField] private NetworkManager _networkManager;

    public void OnConnectClick()
    {
        if (string.IsNullOrEmpty(LocalPlayer.Nickname.ToString()))
        {
            return;
        }
        _networkManager.StartClient();
        _connectPanel.SetActive(false);
    }

    public void OnServerClick()
    {
        if (string.IsNullOrEmpty(LocalPlayer.Nickname.ToString()))
        {
            return;
        }
        _networkManager.StartServer();
        _connectPanel.SetActive(false);
    }

    public void OnHostClick()
    {
        if (string.IsNullOrEmpty(LocalPlayer.Nickname.ToString()))
        {
            return;
        }
        _networkManager.StartHost();
        _connectPanel.SetActive(false);
    }

    public void OnIpFieldChanged(string newIP)
    {
        _transport.ConnectAddress = newIP;
    }

    public void OnNicknameChange(string newNickname) 
    {
        LocalPlayer.Nickname = newNickname;
    }
}
