using UnityEngine;

/// <summary>
/// Scriptable object for controlling enemy temporary blindness state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Temporary Blindness State")]
public class EnemyTemporaryBlindnessState : EnemyState
{
    [Header("Enemy gets blind for x seconds")]
    [Range(0.5f, 10f)] [SerializeField] private float secondsToBeBlind;
    private float timePassed;

    private Animator anim;

    private bool triggerDeath;

    public override void Start()
    {
        base.Start();
        anim = enemy.Anim;
        triggerDeath = false;
    }

    /// <summary>
    /// Happens once when the enemy enters this state. Sets current time passed.
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        timePassed = Time.time;
        agent.isStopped = true;
        anim.SetTrigger("Blind");
        stats.MeleeDamageOnEnemy += Die;
    }

    /// <summary>
    /// Happens on fixed update. Checks if enemy is blind. After the limit time
    /// passes, the enemy returns to LostPlayerState.
    /// </summary>
    /// <returns></returns>
    public override IState FixedUpdate()
    {
        base.FixedUpdate();

        if (Blind())
        {
            if (triggerDeath)
                return enemy.DeathState;

            return enemy.TemporaryBlindnessState;
        }
        return enemy.LostPlayerState;
    }

    /// <summary>
    /// Happens once when the enemy leaves this state.
    /// </summary>
    public override void OnExit()
    {
        agent.isStopped = false;
        anim.SetTrigger("CancelBlind");
        stats.MeleeDamageOnEnemy -= Die;
    }

    /// <summary>
    /// Method that checks if enemy is blind.
    /// </summary>
    /// <returns>Returns true if enemy is blind, else returns false.</returns>
    private bool Blind()
    {
        if (Time.time - timePassed > secondsToBeBlind)
            return false;
        return true;
    }

    /// <summary>
    /// Kills this enemy.
    /// </summary>
    private void Die() => triggerDeath = true;
}
