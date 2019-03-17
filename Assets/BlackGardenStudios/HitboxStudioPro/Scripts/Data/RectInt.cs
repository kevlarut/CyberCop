using System.Runtime.Serialization;
using System.Xml.Serialization;
using UnityEngine;

#if UNITY_2017_2_OR_NEWER
#else
[System.Serializable, XmlType(TypeName = "rect")]
public struct RectInt : ISerializable
{
    public int x, y, width, height;
    public Vector2Int position { get {return new Vector2Int(x, y); } set {x = value.x; y = value.y; } }
    public Vector2Int size { get { return new Vector2Int(width, height); } set { width = value.x; height = value.y; } }

    public RectInt(int X, int Y, int Width, int Height)
    {
        x = X;
        y = Y;
        width = Width;
        height = Height;
    }

    public RectInt(Vector2Int Position, Vector2Int Size)
    {
        x = Position.x;
        y = Position.y;
        width = Size.x;
        height = Size.y;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.FullTypeName = "rect";
        info.AddValue("x", x);
        info.AddValue("y", y);
        info.AddValue("width", width);
        info.AddValue("height", height);
    }
}
#endif