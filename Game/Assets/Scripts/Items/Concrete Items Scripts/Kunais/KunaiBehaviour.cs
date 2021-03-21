using System.Collections;
using UnityEngine;

/// <summary>
/// Base class for kunai behaviours.
/// </summary>
public abstract class KunaiBehaviour : MonoBehaviour, IKunaiBehaviour
{
    public Enemy ParentEnemy { get; set; }

    /// <summary>
    /// Current target.
    /// </summary>
    public abstract Transform KunaiCurrentTarget { get; set; }

    /// <summary>
    /// Happens when IUsableItem execute is called.
    /// </summary>
    /// <param name="baseClass">Kunai base class.</param>
    public virtual void Execute(ItemBehaviour baseClass) { }

    /// <summary>
    /// Happens after kunai hits something.
    /// </summary>
    /// <param name="damageableBody">Damageable body.</param>
    /// <param name="bodyTohit">Body that this object hit.</param>
    /// <param name="player">Player transform.</param>
    public abstract void Hit(IDamageable damageableBody, Transform bodyTohit, Transform player);

    /// <summary>
    /// Happens on start.
    /// </summary>
    /// <param name="player">Player transform.</param>
    public abstract void OnStart(Transform player);
}
