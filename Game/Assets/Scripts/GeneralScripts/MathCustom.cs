using UnityEngine;

public static class MathCustom
{
    /// <summary>
    /// Method to return if a position is left or right of a target.
    /// </summary>
    /// <param name="fwd">Forward vector.</param>
    /// <param name="targetDir">Direction.</param>
    /// <param name="up">Up vector.</param>
    /// <returns>-1, 0, 1 depending on the target's position.</returns>
    public static float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

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
