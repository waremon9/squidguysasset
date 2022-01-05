using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtilities 
{
     public static Vector3 Bezier3(Vector3 a, Vector3 b , float t)
    {
        return Vector3.Lerp(a, b, t);
    }
    public static Vector3 Bezier3(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 u1 = Vector3.Lerp(a, b, t);
        Vector3 u2 = Vector3.Lerp(b, c, t);
        
        return Vector3.Lerp(u1, u2, t);
    }
    public static Vector3 Bezier3(Vector3 a, Vector3 b, Vector3 c , Vector3 d, float t)
    {

        Vector3 u1 = Vector3.Lerp(a, b, t);
        Vector3 u2 = Vector3.Lerp(b, c, t);
        Vector3 u3 = Vector3.Lerp(c, d, t);
        Vector3 v1 = Vector3.Lerp(u1, u2, t);
        Vector3 v2 = Vector3.Lerp(u2, u3, t);
        return Vector3.Lerp(v1, v2, t);
        
    }

    public static Vector2 Bezier2(Vector2 a, Vector2 b, float t)
    {
        return Vector2.Lerp(a, b, t);
    }
    public static Vector2 Bezier2(Vector2 a, Vector2 b, Vector2 c, float t)
    {
        Vector2 u1 = Vector2.Lerp(a, b, t);
        Vector2 u2 = Vector2.Lerp(b, c, t);

        return Vector2.Lerp(u1, u2, t);
    }
    public static Vector2 Bezier2(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t)
    {

        Vector2 u1 = Vector2.Lerp(a, b, t);
        Vector2 u2 = Vector2.Lerp(b, c, t);
        Vector2 u3 = Vector2.Lerp(c, d, t);
        Vector2 v1 = Vector2.Lerp(u1, u2, t);
        Vector2 v2 = Vector2.Lerp(u2, u3, t);
        return Vector2.Lerp(v1, v2, t);

    }
}
