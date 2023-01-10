using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class GameStateChanger : NetworkBehaviour
{
    private NetworkVariable<State> _currentState = new NetworkVariable<State>(State.WaitingLobby, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private UnityEvent<State> _stateChanged = new();

    public UnityEvent<State> StateChanged => _stateChanged;

    public static GameStateChanger Singletone => _singletone;
    private static GameStateChanger _singletone;

    private void Awake()
    {
        if (_singletone != null)
        {
            Debug.LogError("GameStateChanger class is singletone! You must destroy this object");
            Destroy(this);
            return;
        }
        _singletone = this;
    }

    public override void OnDestroy()
    {
        _singletone = null;
    }

    public State CurrentState
    {
        get
        {
            return _currentState.Value;
        }
        set
        {
            _currentState.Value = value;
        }
    }

    [ServerRpc]
    public void StartRoundServerRpc()
    {
        _currentState.Value = State.Launched;
        StartRound();
        StartRoundClientRpc();
    }
    [ClientRpc]
    private void StartRoundClientRpc()
    {
        StartRound();
    }
    private void StartRound()
    {
        _stateChanged.Invoke(State.Launched);
    }

    [ServerRpc]
    public void EndRoundServerRpc()
    {
        _currentState.Value = State.WaitingLobby;
        EndRound();
        EndRoundClientRpc();
    }
    [ClientRpc]
    private void EndRoundClientRpc()
    {
        EndRound();
    }
    private void EndRound()
    {
        _stateChanged.Invoke(State.WaitingLobby);
    }

    public enum State
    {
        WaitingLobby,
        Launched
    }
}
