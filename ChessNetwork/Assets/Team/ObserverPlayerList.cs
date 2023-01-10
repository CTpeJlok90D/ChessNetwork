using UnityEngine;

public class ObserverPlayerList : TeamPlayerList
{
    protected new void OnEnable()
    {
        base.OnEnable();
        Player.Connected.AddListener(OnPlayerConnect);
    }
    protected new void OnDisable()
    {
        base.OnDisable();
        Player.Connected.RemoveListener(OnPlayerConnect);
    }

    private void OnPlayerConnect(Player player)
    {
        AddPlayer(player);
    }
}
