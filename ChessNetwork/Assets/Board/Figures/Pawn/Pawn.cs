using System.Collections.Generic;

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
}
