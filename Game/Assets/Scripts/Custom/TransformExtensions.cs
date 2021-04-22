using UnityEngine;

/// <summary>
/// Class responsible for add Transform extensions.
/// </summary>
public static class TransformExtentions
{
    /// <summary>
    /// Checks if a transform forward is looking to a position. 
    /// Has a maximum angle to look.
    /// </summary>
    /// <param name="from">This transform.</param>
    /// <param name="finalPosition">Final position to check.</param>
    /// <returns>Returns true if this transform is looking towards that 
    /// position.</returns>
    public static bool IsLookingTowards(this Transform from,
        Vector3 finalPosition, float maximumAngle = 10)
    {
        Vector3 dir = from.position.Direction(finalPosition);
        if (Vector3.Angle(dir, from.forward) < maximumAngle) return true;
        return false;
    }

    /// <summary>
    /// Checks if a transform forward is looking to a position. 
    /// Has a maximum angle to look.
    /// </summary>
    /// <param name="from">This transform.</param>
    /// <param name="finalPosition">Final position to check.</param>
    /// <returns>Returns true if this transform is looking towards that 
    /// position.</returns>
    public static bool IsLookingTowards(this Transform from,
        Transform finalPosition, float maximumAngle = 10)
    {
        Vector3 dir = from.position.Direction(finalPosition.position);
        if (Vector3.Angle(dir, from.forward) < maximumAngle) return true;
        return false;
    }

    /// <summary>
    /// Checks if a transform can see another transform in any direction. 
    /// Parameter "to" must have the desired layer to check
    /// (this layer must be on layers parameter as well).
    /// </summary>
    /// <param name="from">From this transform.</param>
    /// <param name="to">Final transform. Must have the desired layer
    /// to check.</param>
    /// <param name="layers">Layers to check.</param>
    /// <returns>Returns true if source transform can see the final 
    /// transform.</returns>
    public static bool CanSee(this Transform from, Transform to, LayerMask layers)
    {
        Ray rayTo = new Ray(from.position, from.position.Direction(to.position));
        float distance = Vector3.Distance(from.position, to.position);
        if (Physics.Raycast(rayTo, out RaycastHit hit, distance, layers))
        {
            if (hit.collider.gameObject.layer == to.gameObject.layer)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Returns direction to some position.
    /// </summary>
    /// <param name="from">Initial transform position.</param>
    /// <param name="to">Final transform position.</param>
    /// <returns>Returns a normalized vector3 with direction.</returns>
    public static Vector3 Direction(this Transform from, Transform to)
    {
        return (to.position - from.position).normalized;
    }

    /// <summary>
    /// Checks if a transform is look at the same direction as another transform.
    /// </summary>
    /// <param name="from">From this transform.</param>
    /// <param name="target">Target transform.</param>
    /// <param name="value">Float value between 0 and 1. 0 means the transforms
    /// are perpendicular, 1 means the transforms are exactly in the same
    /// direction.</param>
    /// <returns></returns>
    public static bool SameDirectionAs(this Transform from, Transform target, 
        float value = 0.9f)
    {
        if (value < 0) value = 0;
        if (value > 1) value = 1;
        if (Vector3.Dot(from.forward, target.forward) >= value) return true;
        return false;
    }
}
