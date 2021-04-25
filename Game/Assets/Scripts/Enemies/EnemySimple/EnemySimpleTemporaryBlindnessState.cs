using UnityEngine;

/// <summary>
/// Scriptable object for controlling enemy temporary blindness state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Common Blindness State")]
public sealed class EnemySimpleTemporaryBlindnessState : 
    EnemySimpleAbstractState
{
    [Header("Enemy gets blind for x seconds")]
    [Range(0.5f, 10f)] [SerializeField] private float secondsToBeBlind;
    private float timePassed;

    public override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// Happens once when the enemy enters this state. Sets current time passed.
    /// </summary>
    public override void OnEnter()
    {
        stats.MeleeDamageOnEnemy += SwitchToDeathState;

        enemy.InCombat = true;
        timePassed = Time.time;
        agent.isStopped = true;
        anim.ResetTrigger("CancelBlind");
        anim.SetTrigger("Blind");
    }

    /// <summary>
    /// Happens on fixed update. Checks if enemy is blind. After the limit time
    /// passes, the enemy returns to LostPlayerState.
    /// </summary>
    /// <returns></returns>
    public override IState FixedUpdate()
    {
        base.FixedUpdate();

        if (die)
            return enemy.DeathState;

        if (Blind())
        {
            return enemy.TemporaryBlindnessState;
        }

        return enemy.LostPlayerState ?? enemy.PatrolState;
    }

    /// <summary>
    /// Happens once when the enemy leaves this state.
    /// </summary>
    public override void OnExit()
    {
        agent.isStopped = false;
        anim.SetTrigger("CancelBlind");
        stats.MeleeDamageOnEnemy -= SwitchToDeathState;
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
}
