using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for breakable objects.
/// </summary>
public class BreakableBox : MonoBehaviour, IBreakable
{
    [SerializeField] private GameObject brokenObject;

    /// <summary>
    /// Method that defines what happens when something collides with this object.
    /// </summary>
    public void Execute()
    {
        Instantiate(brokenObject, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
