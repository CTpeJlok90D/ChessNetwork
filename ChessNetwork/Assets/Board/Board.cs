using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Vector2Int _size = new Vector2Int(8,8);

    public Vector2Int Size => _size;

    public bool IsOnABoard(Vector2Int point)
    {
        return point.x >= 0 && point.x < _size.x && point.y >= 0 && point.y < _size.y;
    }
}
