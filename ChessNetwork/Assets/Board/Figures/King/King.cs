using System.Collections.Generic;
using UnityEngine;

public class King : Figure
{
    public override List<Turn> PossibleTurns 
    {
        get
        { 
            List<Turn> result = new();

            foreach (Vector2Int nearPosition in NearPositions)
            {
                if (Board.IsOnABoard(nearPosition))
                {
                    result.Add(new Turn(this, nearPosition, nearPosition));
                }
            }

            foreach (Figure figure in Figure.ActiveList)
            { 
                if (figure.Team == Team || figure is King)
                {
                    if (figure is King && Team != figure.Team)
                    {
                        King kingFigure = figure as King;
                        foreach (Vector2Int nearPosition in kingFigure.NearPositions)
                        {
                            foreach (Turn turn in result.ToArray())
                            {
                                if (turn.MovePosition == nearPosition)
                                {
                                    result.Remove(turn);
                                }
                            }
                        }
                    }
                    continue;
                }
                foreach (Turn turn in figure.PossibleTurns)
                {
                    foreach (Turn kingTurn in result.ToArray())
                    {
                        if (kingTurn.MovePosition == turn.EatPosition)
                        {
                            result.Remove(kingTurn);
                        }
                    }
                }
            }

            return result;
        }
    }

    private List<Vector2Int> NearPositions
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

    public bool IsCheck => ThreateningFigures.Count == 0;

    public List<Figure> ThreateningFigures
    {
        get
        {
            List<Figure> result = new();

            foreach (Figure figure in Figure.ActiveList)
            {
                if (figure.Team == Team)
                {
                    continue;
                }
                foreach (Turn turn in figure.PossibleTurns)
                {
                    if (turn.EatPosition == Position)
                    {
                        result.Add(figure);
                        continue;
                    }
                }
            }

            return result;
        }
    }
}
