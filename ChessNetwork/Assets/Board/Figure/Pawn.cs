using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Pawn : Figure
{
    public override List<Turn> GetPossibleTurns()
    {
        List<Turn> result = new();

        return result;
    }

    [ServerRpc]
    private void MoveServerRpc()
    {

    }
}
