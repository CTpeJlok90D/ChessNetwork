using System.Collections.Generic;
using UnityEngine;

public class Knight : Figure
{
    public override List<Turn> PossibleTurns
    {
        get
        {
            List<Turn> points = new();

            foreach (Vector2Int position in EatPostion)
            {
                points.Add(new Turn(this, position, position));
            }

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

    public override List<Vector2Int> EatPostion 
    {
        get
        {
            List<Vector2Int> result = new()
            {
                Position + new Vector2Int(1, 2),
                Position + new Vector2Int(1, -2),
                Position + new Vector2Int(-1, 2),
                Position + new Vector2Int(-1, -2),
                Position + new Vector2Int(2, 1),
                Position + new Vector2Int(2, -1),
                Position + new Vector2Int(-2, 1),
                Position + new Vector2Int(-2, -1)
            };

            return result;
        }
    }
}
