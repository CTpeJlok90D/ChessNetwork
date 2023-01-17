using System.Collections.Generic;
using UnityEngine;

public class Pawn : Figure
{
    public override List<Turn> PossibleTurns
    {
        get
        {
            List<Turn> result = new();

            return result;
        }
    }

    public override List<Vector2Int> EatPostion
    {
        get
        {
            int teamSide = Team == Team.White ? -1 : 1;

            List<Vector2Int> result = new()
            {
                Position + new Vector2Int(1, 1) * teamSide,
                Position + new Vector2Int(-1, 1) * teamSide
            };

            return result;
        }
    }
}
