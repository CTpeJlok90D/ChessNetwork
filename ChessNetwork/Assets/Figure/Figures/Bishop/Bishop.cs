using System.Collections.Generic;
using UnityEngine;

public class Bishop : Figure
{
    public override List<Turn> PossibleTurns 
    {
        get 
        {
            List<Turn> result = new();

            bool AddPosition(Vector2Int position)
            {
                if (Board.IsOnABoard(Position) == false)
                {
                    return true;
                }

                Figure figure = GetByPosition(position);
                if (figure != null && figure.Team == Team)
                {
                    return true;
                }
                result.Add(new Turn(this, position, position));

                return figure != null;
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
