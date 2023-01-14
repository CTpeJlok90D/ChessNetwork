using UnityEngine;

public record Turn
{
    public Vector2Int MovePosition;
    public Vector2Int EatPosition;
    public bool IsEatTurn = true;
    public Figure OwnFigure;

    public Turn(Figure ownFigure, Vector2Int movePosition, Vector2Int eatPosition)
    {
        MovePosition = movePosition;
        EatPosition = eatPosition;
        IsEatTurn = true;
        OwnFigure = ownFigure;
    }

    public Turn(Figure ownFigure, Vector2Int movePosition)
    {
        MovePosition = movePosition;
        EatPosition = new Vector2Int(-1,-1);
        IsEatTurn = false;
        OwnFigure = ownFigure;
    }

    public void Execute()
    {
        OwnFigure.UseTurn(this);
    }
}