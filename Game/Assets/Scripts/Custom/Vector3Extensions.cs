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
    /// <returns>Returns true if this vector3 is in the desired position's
    /// range, else it returns false.</returns>
    public static bool Similiar(this Vector3 thisVector,
        Vector3 desiredPosition, float compensation = 1)
    {
        Vector3 finalCompensation = 
            new Vector3(compensation, compensation, compensation);
        Vector3 finalPositive = desiredPosition + finalCompensation;
        Vector3 finalNegative = desiredPosition - finalCompensation;

        if (thisVector.x > finalPositive.x ||
            thisVector.x < finalNegative.x &&
            thisVector.y > finalPositive.y ||
            thisVector.y < finalNegative.y &&
            thisVector.z > finalPositive.z ||
            thisVector.z < finalNegative.z)
        {
            return true;
        }
        return false;
    }
}
