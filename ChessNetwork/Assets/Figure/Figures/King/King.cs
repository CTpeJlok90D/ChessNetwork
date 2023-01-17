using System.Collections.Generic;
using UnityEngine;

public class King : Figure
{
    public override List<Turn> PossibleTurns 
    {
        get
        { 
            List<Turn> result = new();

            foreach (Vector2Int nearPosition in EatPostion)
            {
                if (Board.IsOnABoard(nearPosition) && GetByPosition(nearPosition).Team != Team)
                {
                    result.Add(new Turn(this, nearPosition, nearPosition));
                }
            }

            foreach (Figure figure in Figure.ActiveList)
            { 
                if (figure.Team == Team)
                {
                    continue;
                }
                foreach (Vector2Int eatPosition in figure.EatPostion)
                {
                    foreach (Turn kingTurn in result.ToArray())
                    {
                        if (kingTurn.MovePosition == eatPosition)
                        {
                            result.Remove(kingTurn);
                        }
                    }
                }
            }

            return result;
        }
    }

    public override List<Vector2Int> EatPostion
    {
        get
        {
            List<Vector2Int> result = new();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }
                    result.Add(new Vector2Int(x, y) + Position);
                }
            }

            return result;
        }
    }
}
