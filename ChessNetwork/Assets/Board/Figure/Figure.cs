using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Figure : NetworkBehaviour
{
    [SerializeField] private FigurePositionNetcode _ownerPositionNetcode;
    [SerializeField] private Team _team;

    private Board _ownBoard;

    public Board OwnBoard => _ownBoard;

    public Vector2Int Position => _ownerPositionNetcode.position;

    public abstract List<Turn> GetPossibleTurns();

    public Figure Init(Board ownBoard)
    {
        _ownBoard = ownBoard;
        return this;
    }
}
