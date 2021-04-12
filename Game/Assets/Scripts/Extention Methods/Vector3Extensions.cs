using UnityEngine;

/// <summary>
/// Class responsible for adding extension methods to vector3.
/// </summary>
public static class Vector3Extensions
{
    /// <summary>
    /// Returns direction to some position.
    /// </summary>
    /// <param name="from">Initial position.</param>
    /// <param name="to">Final position.</param>
    /// <returns>Returns a normalized vector3 with direction.</returns>
    public static Vector3 Direction(this Vector3 from, Vector3 to)
    {
        return (to - from).normalized;
    }

    /// <summary>
    /// Returns inverted direction to some position.
    /// </summary>
    /// <param name="from">Initial position.</param>
    /// <param name="to">Final position.</param>
    /// <returns>Returns a normalized vector3 with direction.</returns>
    public static Vector3 InvertedDirection(this Vector3 from, Vector3 to)
    {
        return (from - to).normalized;
    }

    public static bool CanSee(this Transform from, Transform to, LayerMask layers)
    {
        Ray rayTo = new Ray(from.position, from.position.Direction(to.position));
        float distance = Vector3.Distance(from.position, to.position);
        if (Physics.Raycast(rayTo, out RaycastHit hit, distance, layers))
        {
            if (hit.collider.gameObject.layer == to.gameObject.layer)
            {
                return true;
            }
        }
        return false;
    }
}
