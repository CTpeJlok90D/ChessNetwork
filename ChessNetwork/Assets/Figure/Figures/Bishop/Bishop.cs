using System.Collections.Generic;
using UnityEngine;

public class Bishop : Figure
{
    public override List<Turn> PossibleTurns 
    {

        get
        {
            List<Turn> result = new List<Turn>();
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
            List<Vector2Int> result = new List<Vector2Int>();

            bool AddPosition(Vector2Int position)
            {
                result.Add(position);
                return GetByPosition(position) != null;
            }

            for (int x = Position.x + 1, y = Position.y + 1; x < Board.Size.x && y < Board.Size.y; x++, y++)
            {
                if (AddPosition(new Vector2Int(x, y)))
                {
                    break;
                }
            }

            for (int x = Position.x - 1, y = Position.y + 1; x >= 0 && y < Board.Size.y; x--, y++)
            {
                if (AddPosition(new Vector2Int(x, y)))
                {
                    break;
                }
            }
            for (int x = Position.x + 1, y = Position.y - 1; x < Board.Size.x && y >= 0; x++, y--)
            {
                if (AddPosition(new Vector2Int(x, y)))
                {
                    break;
                }
            }
            for (int x = Position.x - 1, y = Position.y - 1; x >= 0 && y >= 0; x--, y--)
            {
                if (AddPosition(new Vector2Int(x, y)))
                {
                    break;
                }
            }

            return result;
        }
    }
}
