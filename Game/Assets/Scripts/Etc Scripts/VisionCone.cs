using UnityEngine;

/// <summary>
/// Class responsible for creating a vision cone.
/// </summary>
public class VisionCone
{
    // Cone angles variables
    private readonly Quaternion startingAngle;
    private readonly Quaternion stepAngle;

    // Cone Mesh components
    private readonly Mesh mesh;
    private readonly Vector3[] vertices;
    private readonly int[] triangles;
    private readonly byte coneRange;
    private readonly LayerMask collisionLayers;

    // Vision cone parent
    private readonly Transform parent;

    /// <summary>
    /// Construct to create a vision cone.
    /// </summary>
    /// <param name="meshFilter">Mesh filter of the object.</param>
    /// <param name="meshRenderer">Mesh renderer of the object.</param>
    /// <param name="material">Material to render.</param>
    /// <param name="amountOfVertices">Amount of vertices to create the 
    /// cone.</param>
    /// <param name="desiredConeAngle">Cone minimum angle.</param>
    /// <param name="coneRange">Cone maximum distance.</param>
    /// <param name="collisionLayers">Layers to collide with the cone.</param>
    /// <param name="parent">Transform of this cone's origin.</param>
    public VisionCone(
        MeshFilter meshFilter,
        MeshRenderer meshRenderer,
        Material material,
        byte amountOfVertices, 
        byte desiredConeAngle, 
        byte coneRange, 
        LayerMask collisionLayers,
        Transform parent)
    {
        // Mesh setup
        mesh = new Mesh();
        meshFilter.mesh = mesh;
        meshRenderer.material = material;
        
        // Cone setup
        vertices = new Vector3[amountOfVertices];
        triangles = new int[vertices.Length * 3];
        this.coneRange = coneRange;
        this.collisionLayers = collisionLayers;
        this.parent = parent;

        // Cone angles setup
        startingAngle = Quaternion.AngleAxis(-desiredConeAngle, Vector3.up);
        stepAngle = Quaternion.AngleAxis(
            (desiredConeAngle + desiredConeAngle) / vertices.Length,
            Vector3.up);
    }

    /// <summary>
    /// Calculates vision cone's mesh vertices.
    /// </summary>
    public void Calculate()
    {
        Quaternion angle = parent.rotation * startingAngle;
        Vector3 direction = angle * Vector3.forward;
        Vector3 currentPos = parent.position;

        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertices.Length; i++)
        {       
            // If it hits something
            if (Physics.Raycast(currentPos, direction, out RaycastHit hit,
                coneRange, collisionLayers))
            {
                if (i != 0)
                {
                    // Sets this vertice's position for that collision
                    vertices[i] = 
                        Vector3.MoveTowards(
                            vertices[i], 
                            parent.transform.InverseTransformPoint(hit.point), 
                            Time.fixedDeltaTime * 22);
                }

            }
            // Else if it doesn't hit anything
            else
            {
                if (i != 0)
                {
                    // Sets this vertice's position to final range
                    vertices[i] = 
                        Vector3.MoveTowards(
                            vertices[i], 
                            parent.transform.InverseTransformPoint(
                            currentPos + direction * coneRange), 
                            Time.fixedDeltaTime * 22);
                }
            }
            direction = stepAngle * direction;

            // Creates all triangles for the mesh
            if (i < vertices.Length - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
