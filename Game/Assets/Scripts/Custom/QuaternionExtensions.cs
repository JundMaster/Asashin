using UnityEngine;

/// <summary>
/// Class reponsible for adding extensions to Quaternion.
/// </summary>
public static class QuaternionExtensions
{
    /// <summary>
    /// Rotates a transform in Y to another transform in Y.
    /// </summary>
    /// <param name="from">Transform to rotate.</param>
    /// <param name="to">Final position to rotate towards.</param>
    /// <returns>Returns a quaternion with values to rotate towards.</returns>
    public static Quaternion RotateTo(this Transform from, Vector3 to)
    {
        Vector3 dir = from.position.Direction(to);
        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        return from.transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
    }

    /// <summary>
    /// Rotates a transform in Y to another transform in Y with a smooth
    /// transition.
    /// </summary>
    /// <param name="from">Transform to rotate.</param>
    /// <param name="to">Final position to rotate towards.</param>
    /// <returns>Returns a quaternion with values to rotate towards.</returns>
    public static Quaternion RotateToSmoothly(this Transform from, Vector3 to,
        ref float smoothTimeRotation, float turnSmoothSpeed)
    {
        Vector3 dir = from.position.Direction(to);
        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(
                from.transform.eulerAngles.y,
                targetAngle,
                ref smoothTimeRotation,
                turnSmoothSpeed);
        return from.transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
}
