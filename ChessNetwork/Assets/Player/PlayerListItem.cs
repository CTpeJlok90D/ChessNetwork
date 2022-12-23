using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _nicknameField;

    private Player.Info _owner;

    public PlayerListItem Init(Player.Info player)
    {
        _owner = player;
        _nicknameField.text = _owner.Nickname.Value.ToString();
        StartCoroutine(NicknameSetcorutine());
        return this;
    }

    private void Update()
    {
        Debug.Log(_owner.Nickname.Value.ToString());
    }

    private IEnumerator NicknameSetcorutine()
    {
        while (string.IsNullOrEmpty(_owner.Nickname.Value.ToString()))
        {
            yield return null;
        }
        _nicknameField.text = _owner.Nickname.Value.ToString();
    }
}
