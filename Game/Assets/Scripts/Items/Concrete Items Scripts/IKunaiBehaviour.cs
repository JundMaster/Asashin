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
    /// <param name="bodyTohit">Body that this object hit.</param>
    /// <param name="player">Player transform.</param>
    void Hit(IDamageable damageableBody, Transform bodyTohit, Transform player);

    /// <summary>
    /// Happens when IUsableItem execute is called.
    /// </summary>
    /// <param name="baseClass">Kunai base class.</param>
    void Execute(ItemBehaviour baseClass);
}
