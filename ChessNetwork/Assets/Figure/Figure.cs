using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public abstract class Figure : NetworkBehaviour
{
    [SerializeField] private Board _board;
    [SerializeField] private Team _team;

    [SerializeField] private NetworkVariable<Vector2Int> _position = new(Vector2Int.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private UnityEvent _selected = new();
    private UnityEvent _deselected = new();

    public UnityEvent Selected => _selected;
    public UnityEvent Deselected => _deselected;
    public Board Board => _board;
    public Vector2Int Position => _position.Value;
    public Team Team => _team;

    private static List<Figure> _activeList = new();
    private static Dictionary<ulong, Figure> _byID = new();
    private static Figure _choosen;
    public static List<Figure> ActiveList => new(_activeList);
    public static Figure Choosen => _choosen;
    public static Dictionary<ulong, Figure> ByID => new(_byID);

    public Figure GetByPosition(Vector2Int position)
    {
        foreach (Figure figure in _activeList)
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

    public virtual List<Vector2Int> EatPostion
    {
        get
        {
            List<Vector2Int> result = new();

            foreach (Turn turn in PossibleTurns)
            {
                result.Add(turn.EatPosition);
            }

            return result;
        }
    }

    private void OnEnable()
    {
        _activeList.Add(this);
        _position.OnValueChanged += OnPositionChanged;
    }

    private void OnDisable()
    {
        _activeList.Remove(this);
        _position.OnValueChanged += OnPositionChanged;
    }

    private void OnPositionChanged(Vector2Int oldValue, Vector2Int newValue)
    {
        transform.localPosition = new Vector3(newValue.x, 0, newValue.y);
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
        foreach (Figure figure in ActiveList)
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
            return;
        }

        Figure eatenFigure = GetByPosition(turn.EatPosition);
        Deselect();

        if (turn.IsEatTurn && eatenFigure != null)
        {
            eatenFigure.OnFigureEatServerRpc();
        }
        UseTurnServerRpc(turn.MovePosition);

        GameStateChanger.Singletone.NextTurnServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void UseTurnServerRpc(Vector2Int movePosition)
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

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isEditor)
        {
            _position.Value = new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.z);
        }
    }
#endif
}
