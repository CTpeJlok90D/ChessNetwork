using System.Collections.Generic;
using UnityEngine;

public class Knight : Figure
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
