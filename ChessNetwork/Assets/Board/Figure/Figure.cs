using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public abstract class Figure : NetworkBehaviour
{
    [SerializeField] private Board _board;
    [SerializeField] private Team _team;

    private NetworkVariable<Vector2Int> _position = new();

    private UnityEvent _selected = new();
    private UnityEvent _deselected = new();

    public UnityEvent Selected => _selected;
    public UnityEvent Deselected => _deselected;
    public Board Board => _board;
    public Vector2Int Position => _position.Value;

    private static List<Figure> _all = new();
    private static Figure _choosen;
    public static List<Figure> All => _all;
    public static Figure Choosen => _choosen;

    public abstract List<Turn> PossibleTurns 
    {
        get;
    }

    private void Awake()
    {
        _position.Value = new Vector2Int((int)transform.position.x,(int)transform.position.z);
    }

    private void OnEnable()
    {
        _all.Add(this);
    }

    private void OnDisable()
    {
        _all.Remove(this);
    }

    private void OnMouseDown()
    {
        if (Player.IsConnected == false || Player.Local.Team != _team)
        {
            return;
        }
        if (_choosen == this)
        {
            Deselect();
            return;
        }
        foreach (Figure figure in All)
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
        Deselect();
    }
}
