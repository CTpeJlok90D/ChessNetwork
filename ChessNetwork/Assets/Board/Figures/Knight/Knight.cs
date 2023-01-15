using System.Collections.Generic;
using UnityEngine;

public class Knight : Figure
{
    public override List<Turn> PossibleTurns
    {
        get
        {
            List<Turn> points = new()
            {
                new Turn(this, Position + new Vector2Int(1, 2), Position + new Vector2Int(1, 2)),
                new Turn(this, Position + new Vector2Int(1, -2), Position + new Vector2Int(1, -2)),
                new Turn(this, Position + new Vector2Int(-1, 2), Position + new Vector2Int(-1, 2)),
                new Turn(this, Position + new Vector2Int(-1, -2), Position + new Vector2Int(-1, -2)),
                new Turn(this, Position + new Vector2Int(2, 1), Position + new Vector2Int(2, 1)),
                new Turn(this, Position + new Vector2Int(2, -1), Position + new Vector2Int(2, -1)),
                new Turn(this, Position + new Vector2Int(-2, 1), Position + new Vector2Int(-2, 1)),
                new Turn(this, Position + new Vector2Int(-2, -1), Position + new Vector2Int(-2, -1))
            };

            List<Turn> result = new();
            foreach (Turn turn in points.ToArray())
            {
                Figure eatenFigure = GetByPosition(turn.MovePosition);
                if (Board.IsOnABoard(turn.MovePosition) && (GetByPosition(turn.MovePosition) == null || eatenFigure.Team != Team))
                {
                    result.Add(turn);
                }
            }

            return result;
        }
    }
}
