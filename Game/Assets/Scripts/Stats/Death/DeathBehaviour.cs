using UnityEngine;

/// <summary>
/// Class responsible for handlign death of a character.
/// </summary>
public abstract class DeathBehaviour : MonoBehaviour
{
    /// <summary>
    /// What happens when the character dies.
    /// </summary>
    public abstract void Die();
}
