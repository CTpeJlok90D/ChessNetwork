using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 ToVector3(this Vector2Int @this)
    {
        return new Vector3(@this.x, @this.y);
    }

    public static Vector2Int ToVector2Int(this Vector3 @this)
    {
        return new Vector2Int((int)@this.x, (int)@this.y);
    }
}
