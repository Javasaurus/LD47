using System.Collections.Generic;
using UnityEngine;

public class LoopUtils
{
    public enum Winding
    {
        CLOCKWISE, COUNTERCLOCKWISE
    }
    public static Vector2 CrossProduct( Vector2 p1, Vector2 p2, Winding winding )
    {
        return (winding == Winding.CLOCKWISE ? 1 : -1) * new Vector2((p2.y - p1.y), -(p2.x - p1.x));
    }

    public static Winding CalculateWinding( List<LoopSegment> segments )
    {
        float sum = 0;
        foreach (LoopSegment segment in segments)
        {
            sum += ((segment.end.x - segment.start.x) * (segment.end.y + segment.start.y));
        }
        return sum >= 0 ? Winding.CLOCKWISE : Winding.COUNTERCLOCKWISE;
    }

    [System.Serializable]
    public struct LoopSegment
    {
        public Vector2 start;
        public Vector2 end;
    }
}
