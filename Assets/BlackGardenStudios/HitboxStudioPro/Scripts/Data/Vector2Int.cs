using UnityEngine;

#if UNITY_2017_2_OR_NEWER
#else
#pragma warning disable 0661, 0659
public struct Vector2Int
{
    public int x, y;

    public Vector2Int(int X, int Y)
    {
        x = X;
        y = Y;
    }

    public static bool operator==(Vector2Int a, Vector2Int b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator!=(Vector2Int a, Vector2Int b)
    {
        return a.x != b.x && a.y != b.y;
    }

    public static implicit operator Vector2(Vector2Int a)
    {
        return new Vector2(a.x, a.y);
    }

    public override bool Equals(object obj)
    {
        return obj is Vector2Int && ((Vector2Int)obj) == this;
    }

    static public Vector2Int zero = new Vector2Int(0, 0);
}
#endif