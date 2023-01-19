using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public abstract class Figure : NetworkBehaviour
{
    [SerializeField] private Board _board;
    [SerializeField] private Team _team;

    [SerializeField] private NetworkVariable<Vector2Int> _position = new(Vector2Int.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private Vector2Int _localPosition;


    private UnityEvent _selected = new();
    private UnityEvent _deselected = new();
    private UnityEvent _moved = new();

    public UnityEvent Selected => _selected;
    public UnityEvent Deselected => _deselected;
    public UnityEvent Moved => _moved;
    public Board Board => _board;
    public Vector2Int Position => _localPosition;
    public Team Team => _team;


    private static List<Figure> _list = new();

    private static Figure _choosen;
    private static Dictionary<Team, Figure> _teamKing = new();
    
    public static Dictionary<Team, Figure> TeamKing => _teamKing;
    public static List<Figure> List => new(_list);
    public static List<Figure> ByTeam(Team team)
    {
        List<Figure> result = new List<Figure>();
        foreach (Figure figure in _list)
        {
            if (figure.Team == team)
            {
                result.Add(figure);
            }
        }
        return result;
    }


    public Figure GetByPosition(Vector2Int position)
    {
        foreach (Figure figure in _list)
        {
            if (figure.Position == position)
            {
                return figure;
            }
        }
        return null;
    }

    public abstract List<Turn> PossibleTurns 
    {
        get;
    }

    public abstract List<Vector2Int> EatPostions
    {
        get;
    }

    public bool ThisTurnIsPossible(Turn turn)
    {
        return TeamKing[Team].WillBeDanguresAfter(turn) == false && 
               Board.IsOnABoard(turn.MovePosition) && 
               (GetByPosition(turn.EatPosition) == null ||
               GetByPosition(turn.EatPosition).Team != Team);
    }

    public bool WillBeDanguresAfter(Turn turn)
    {
        Vector2Int oldPosition = turn.Figure.Position;
        Figure eatFigure = GetByPosition(turn.EatPosition);

        turn.Figure._localPosition = turn.MovePosition;
        eatFigure?.gameObject.SetActive(false);

        foreach (Figure figure in Figure.List)
        {
            if (figure.Team == Team)
            {
                continue;
            }
            foreach (Vector2Int eatPosition in figure.EatPostions)
            {
                if (eatPosition == Position)
                {
                    turn.Figure._localPosition = oldPosition;
                    eatFigure?.gameObject.SetActive(true);

                    return true;
                }
            }
        }

        turn.Figure._localPosition = oldPosition;
        eatFigure?.gameObject.SetActive(true);

        return false;
    }

    protected void Awake()
    {
        _position.Value = new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.z);
        _localPosition = _position.Value;
    }

    protected void OnEnable()
    {
        _list.Add(this);
        _position.OnValueChanged += OnPositionChanged;
    }

    protected void OnDisable()
    {
        _list.Remove(this);
        _position.OnValueChanged += OnPositionChanged;
    }

    private void OnPositionChanged(Vector2Int oldValue, Vector2Int newValue)
    {
        transform.localPosition = new Vector3(newValue.x, 0, newValue.y);
        _localPosition = newValue;
    }

    public override void OnNetworkSpawn()
    {
        gameObject.SetActive(true);
    }

    private void OnMouseDown()
    {
        if (Player.IsConnected == false || Player.Local.Team != _team || GameStateChanger.Singletone.CurrentTurn != Team)
        {
            return;
        }
        if (_choosen == this)
        {
            Deselect();
            return;
        }
        foreach (Figure figure in List)
        {
            figure.Deselect();
        }
        Select();
    }

    public void Select()
    {
        _choosen = this;
        _selected.Invoke();
    }

    public void Deselect()
    {
        _choosen = null;
        _deselected.Invoke();
    }

    public void UseTurn(Turn turn)
    {
        if (GameStateChanger.Singletone.CurrentTurn != Team)
        {
            Debug.Log("LOL");
            return;
        }

        Figure eatenFigure = GetByPosition(turn.EatPosition);
        Deselect();

        if (turn.IsEatTurn && eatenFigure != null)
        {
            eatenFigure.OnFigureEatServerRpc();
        }
        MoveFigureServerRpc(turn.MovePosition);

        _moved.Invoke();

        GameStateChanger.Singletone.NextTurnServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void MoveFigureServerRpc(Vector2Int movePosition)
    {
        _position.Value = movePosition;
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnFigureEatServerRpc()
    {
        OnFigureEatClientRpc();
    }

    [ClientRpc]
    private void OnFigureEatClientRpc()
    {
        transform.localPosition = new Vector3(-1, 0, -1);
        gameObject.SetActive(false);
    }
}
