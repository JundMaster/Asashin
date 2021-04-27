using UnityEngine;

public static class Custom
{
    /// <summary>
    /// Returns a Vector2 in a plane inside a square. First transform must 
    /// be on the opposite corner of the last transform.
    /// </summary>
    /// <param name="positions">Transforms to limit the positions.</param>
    public static Vector2 RandomPositionInSquare(Transform[] positions)
    {
        if (positions.Length != 4)
            throw new System.Exception("Transform length must be 4.");

        Vector2 randomPosition = new Vector2(Random.Range(
                positions[0].localPosition.x,
                positions[2].localPosition.x),
            Random.Range(
                positions[0].localPosition.z,
                positions[1].localPosition.z));

        // Finds center point
        Vector3 spawnPos =
            (positions[0].position +
            (positions[3].position - positions[0].position) / 2);

        //if (spawnPos.x + positions[0].localPosition.x > )

        return new Vector2(
                spawnPos.x + randomPosition.x,
                spawnPos.z + randomPosition.y);
    }
}
