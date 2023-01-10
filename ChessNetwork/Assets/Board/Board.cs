using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Board : NetworkBehaviour
{
    [SerializeField] private List<Figure> _figures;

    Dictionary<ulong, Figure> _figureById = new();

    private void Awake()
    {
        foreach (Figure figure in _figures)
        {
            _figureById.Add(figure.NetworkObjectId, figure);
            figure.Init(this);
        }
    }

    public Figure GetFigureByPosition(Vector2Int position)
    {
        foreach (Figure figure in _figures)
        {
            if (figure.Position == position)
            {
                return figure;
            }
        }
        return null;
    }
}