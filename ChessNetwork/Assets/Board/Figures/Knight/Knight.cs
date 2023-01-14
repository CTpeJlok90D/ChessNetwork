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
                if (Board.IsOnABoard(turn.MovePosition))
                {
                    result.Add(turn);
                }
            }

            return result;
        }
    }
}
