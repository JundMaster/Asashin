using UnityEngine;

/// <summary>
/// Class responsible for add Transform extensions.
/// </summary>
public static class TransformExtentions
{
    /// <summary>
    /// Checks if a transform forward is looking to a position. 
    /// Has a maximum angle to look, but 10 is the max. recommended.
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
}
