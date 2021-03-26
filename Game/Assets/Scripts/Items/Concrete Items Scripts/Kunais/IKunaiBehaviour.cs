using UnityEngine;

/// <summary>
/// Interface responsible for kunai behaviours.
/// </summary>
public interface IKunaiBehaviour
{
    /// <summary>
    /// Current target.
    /// </summary>
    Transform KunaiCurrentTarget { get; set; }

    /// <summary>
    /// Happens on start.
    /// </summary>
    /// <param name="player">Player transform.</param>
    void OnStart(Transform player);

    /// <summary>
    /// Happens after kunai hits something.
    /// </summary>
    /// <param name="damageableBody">Damageable body.</param>
    /// <param name="collider">Collider of the collision.</param>
    /// <param name="player">Player transform.</param>
    void Hit(IDamageable damageableBody, Collider collider, Transform player);

    /// <summary>
    /// Happens when IUsableItem execute is called.
    /// </summary>
    /// <param name="baseClass">Kunai base class.</param>
    void Execute(ItemBehaviour baseClass);
}
