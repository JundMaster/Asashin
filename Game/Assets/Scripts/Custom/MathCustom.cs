using UnityEngine;

public static class MathCustom
{
    /// <summary>
    /// Checks if a direction is left or right of another target's forward.
    /// </summary>
    /// <param name="forwardVector">Forward vector from target.</param>
    /// <param name="direction">Direction.</param>
    /// <param name="upVector">Up vector.</param>
    /// <returns>Returns -1 if direction is to the left. 
    /// Returns 0 if direction is in the middle.
    /// Returns 1 if direction is to the right.</returns>
    public static float AngleDirection(Vector3 forwardVector, Vector3 direction, 
        Vector3 upVector)
    {
        Vector3 crossProduct = Vector3.Cross(forwardVector, direction);
        float dir = Vector3.Dot(crossProduct, upVector);

        if (dir > 0f)
        {
            return 1f;
        }
        else if (dir < 0f)
        {
            return -1f;
        }
        else
        {
            return 0f;
        }
    }
}
