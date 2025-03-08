using UnityEngine;

public class Projection
{
    public static Vector3 ProjectToOrthogonalSpace(Vector3 vec)
    {
        return vec.x * new Vector3(1f, 0f, 0f) + vec.y * new Vector3(0f, 0.9f, 0f);
    }
}
