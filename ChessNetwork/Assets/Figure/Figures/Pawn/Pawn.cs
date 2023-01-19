using System.Collections.Generic;
using UnityEngine;

public class Pawn : Figure
{
    private bool _isMoved = false;

    private new void Awake()
    {
        base.Awake();
        Moved.AddListener(() => { _isMoved = true; });
    }

    public override List<Turn> PossibleTurns
    {
        get
        {
            int teamSide = Team == Team.White ? -1 : 1;

            List<Turn> result = new();

            Turn turn = new Turn(this, Position + Vector2Int.up * teamSide);
            if (CanMoveHere(turn))
            {
                result.Add(turn);
            }

            turn = new Turn(this, Position + Vector2Int.up * teamSide * 2);
            if (_isMoved == false && CanMoveHere(turn))
            {
                result.Add(turn);
            }

            foreach (Vector2Int eatPosition in EatPostions)
            {
                Figure figure = GetByPosition(eatPosition);
                Turn moveTurn = new Turn(this, eatPosition, eatPosition);
                if (figure != null && figure.Team != Team && CanMoveHere(moveTurn))
                {
                    result.Add(moveTurn);
                }
            }

            return result;
        }
    }

    private bool CanMoveHere(Turn turn) => Board.IsOnABoard(turn.MovePosition) && TeamKing[Team].WillBeDanguresAfter(turn) == false && GetByPosition(turn.MovePosition) == null;

    public override List<Vector2Int> EatPostions
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
