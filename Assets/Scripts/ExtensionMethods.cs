using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    /// <summary> Create a quaternion rotated 0-360 degrees at Z axis. </summary>
    public static Quaternion MakeRandomZ(this Quaternion quaternion)
    {
        return Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
    }

    public static void SetXPosition(this Transform _transform, float newValue)
    {
        Vector3 pos = _transform.position;
        pos.x = newValue;
        _transform.position = pos;
    }

    public static void SetYPosition(this Transform _transform, float newValue)
    {
        Vector3 pos = _transform.position;
        pos.y = newValue;
        _transform.position = pos;
    }

    public static void SetZPosition(this Transform _transform, float newValue)
    {
        Vector3 pos = _transform.position;
        pos.z = newValue;
        _transform.position = pos;
    }

    public static void Clamp(this Vector2 vector2, float min, float max)
    {
        Vector2 newVector2 = vector2;
        vector2.x = Mathf.Clamp(vector2.x, min, max);
        vector2.y = Mathf.Clamp(vector2.y, min, max);
        vector2 = newVector2;
    }

    public static void SetRandomBool(this bool _bool)
    {
        _bool = Random.Range(0, 1) == 1;
    }
}
