using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class GameStateChanger : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<State> _currentState = new NetworkVariable<State>(State.WaitingLobby, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] private NetworkVariable<Team> _currentTurn = new NetworkVariable<Team>(Team.White, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server); 

    private UnityEvent _sessionStart = new();
    private UnityEvent _sessionEnd = new();

    private UnityEvent<Team> _turnChanged = new();

    public UnityEvent SessionStart => _sessionStart;
    public UnityEvent SessionEnd => _sessionEnd;
    public UnityEvent<Team> TurnChanged => _turnChanged;
    public Team CurrentTurn => _currentTurn.Value;
    public State CurrentState => _currentState.Value;
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
        _currentTurn.OnValueChanged += OnTurnChanged;
    }

    private void OnDisable()
    {
        _currentState.OnValueChanged -= OnStateChanged;
        _currentTurn.OnValueChanged -= OnTurnChanged;
    }

    private void OnStateChanged(State oldState, State newState)
    {
        switch (newState)
        {
            case State.Launched:
                _sessionStart.Invoke();
            break;
            case State.WaitingLobby:
                _sessionEnd.Invoke(); 
            break;
        }
    }

    private void OnTurnChanged(Team oldValue, Team newValue)
    {
        _turnChanged.Invoke(newValue);
        List<Turn> possibleTurns = new List<Turn>();
        foreach (Figure figure in Figure.ByTeam(newValue))
        {
            possibleTurns.AddRange(figure.PossibleTurns);
        }
        if (possibleTurns.Count == 0) 
        {
            TeamWin(oldValue);
        }
    }

    private void TeamWin(Team team)
    {
        Debug.Log(team);
        EndGame();
    }
    private void EndGame()
    {
        _currentState.Value = State.WaitingLobby;
    }

    public void StartSession()
    {
        _currentState.Value = State.Launched;
        _currentTurn.Value = Team.White;
    }

    [ServerRpc(RequireOwnership = false)]
    public void NextTurnServerRpc()
    {
        if (_currentTurn.Value == Team.White)
        {
            _currentTurn.Value = Team.Black;
        }
        else
        {
            _currentTurn.Value = Team.White;
        }
    }

    public enum State
    {
        WaitingLobby,
        Launched
    }
}
