using Unity.Netcode;
using UnityEngine;

public record Turn
{
    private Vector2Int _movePosition;
    private Vector2Int _eatPosition;
    private bool _isEatTurn;
    private Figure _ownFigureId;

    public Vector2Int MovePosition => _movePosition;
    public Vector2Int EatPosition => _eatPosition;
    public bool IsEatTurn => _isEatTurn;
    public Figure OwnFigure => _ownFigureId;

    public Turn(Figure ownFigure, Vector2Int movePosition, Vector2Int eatPosition)
    {
        _movePosition = movePosition;
        _eatPosition = eatPosition;
        _isEatTurn = true;
        _ownFigureId = ownFigure;
    }

    public Turn(Figure ownFigure, Vector2Int movePosition)
    {
        _movePosition = movePosition;
        _eatPosition = new Vector2Int(-1,-1);
        _isEatTurn = false;
        _ownFigureId = ownFigure;
    }

    public void Execute()
    {
        OwnFigure.UseTurn(this);
    }
}