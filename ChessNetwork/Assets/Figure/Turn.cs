using Unity.Netcode;
using UnityEngine;

public record Turn
{
    private Vector2Int _movePosition;
    private Vector2Int _eatPosition;
    private bool _isEatTurn;
    private Figure _ownFigure;

    public Vector2Int MovePosition => _movePosition;
    public Vector2Int EatPosition => _eatPosition;
    public bool IsEatTurn => _isEatTurn;
    public Figure Figure => _ownFigure;

    public Turn(Figure ownFigure, Vector2Int movePosition, Vector2Int eatPosition)
    {
        _movePosition = movePosition;
        _eatPosition = eatPosition;
        _isEatTurn = true;
        _ownFigure = ownFigure;
    }

    public Turn(Figure ownFigure, Vector2Int movePosition)
    {
        _movePosition = movePosition;
        _eatPosition = new Vector2Int(-1,-1);
        _isEatTurn = false;
        _ownFigure = ownFigure;
    }

    public void Execute()
    {
        Figure.UseTurn(this);
    }
}