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

    /// <summary>
    /// Checks if a Vector3 is similiar to another with a determined 
    /// compensation.
    /// </summary>
    /// <param name="thisVector">Vector3 to analyze.</param>
    /// <param name="desiredPosition">Desired Vector3 to check against.</param>
    /// <param name="compensation">Compensation of values to check.</param>
    /// <returns>Returns true if this vector3 is similiar to another one,
    /// else it returns false.</returns>
    public static bool Similiar(this Vector3 thisVector,
        Vector3 otherVector, float compensation = 0.01f)
    {
        Vector3 finalCompensation = 
            new Vector3(compensation, compensation, compensation);
        Vector3 finalPositive = otherVector + finalCompensation;
        Vector3 finalNegative = otherVector - finalCompensation;

        if (thisVector.x < finalPositive.x &&
            thisVector.x > finalNegative.x &&
            thisVector.y < finalPositive.y &&
            thisVector.y > finalNegative.y &&
            thisVector.z < finalPositive.z &&
            thisVector.z > finalNegative.z)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if a vector3 can see another transform in any direction. 
    /// Parameter "to" must have the desired layer to check
    /// (this layer must be on layers parameter as well).
    /// </summary>
    /// <param name="from">From this vector3.</param>
    /// <param name="to">Final transform. Must have the desired layer
    /// to check.</param>
    /// <param name="layers">Layers to check.</param>
    /// <returns>Returns true if source vector3 can see the final 
    /// transform.</returns>
    public static bool CanSee(this Vector3 from, Transform to,
        LayerMask layers)
    {
        Ray rayTo = new Ray(from, from.Direction(to.position));
        float distance = Vector3.Distance(from, to.position);
        if (Physics.Raycast(rayTo, out RaycastHit hit, distance, layers))
        {
            if (hit.collider.gameObject.layer == to.gameObject.layer)
                return true;
        }
        return false;
    }
}
