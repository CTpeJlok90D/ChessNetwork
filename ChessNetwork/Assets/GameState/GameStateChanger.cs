using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class GameStateChanger : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<State> _currentState = new NetworkVariable<State>(State.WaitingLobby, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private UnityEvent _sessionStart = new();

    private State current
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

    public UnityEvent SessionStart => _sessionStart;
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

    private void OnEnable()
    {
        _currentState.OnValueChanged += OnStateChanged;
    }
    private void OnDisable()
    {
        _currentState.OnValueChanged -= OnStateChanged;
    }

    public void StartSession()
    {
        current = State.Launched;
    }

    private void OnStateChanged(State oldState, State newState)
    {
        switch (newState)
        {
            case State.Launched:
                _sessionStart.Invoke();
            break;
        }
    }

    public enum State
    {
        WaitingLobby,
        Launched
    }
}
