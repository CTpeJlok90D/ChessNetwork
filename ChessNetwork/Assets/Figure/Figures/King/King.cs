using System.Collections.Generic;
using UnityEngine;

public class King : Figure
{
    public override List<Turn> PossibleTurns 
    {
        get
        { 
            List<Turn> result = new();

            foreach (Vector2Int position in EatPostions)
            {
                Turn newTurn = new Turn(this, position, position);
                if (ThisTurnIsPossible(newTurn))
                {
                    result.Add(newTurn);
                }
            }

            return result;
        }
    }

    public override List<Vector2Int> EatPostions
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

    private new void Awake()
    {
        base.Awake();
        TeamKing[Team] = this;
    }
}
