using UnityEngine;

/// <summary>
/// Scriptable object for controlling tutorial enemy temporary blindness state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Tutorial Blindness State")]
public class EnemyTutorialTemporaryBlindnessState : EnemyTutorialAbstractState
{
    [Header("Enemy gets blind for x seconds")]
    [Range(0.5f, 10f)] [SerializeField] private float secondsToBeBlind;
    private float timePassed;

    /// <summary>
    /// Runs once on start.
    /// </summary>
    public override void Start() =>
        base.Start();

    /// <summary>
    /// Happens once when the enemy enters this state. Sets current time passed.
    /// </summary>
    public override void OnEnter()
    {
        enemy.InCombat = true;
        timePassed = Time.time;
        agent.isStopped = true;
        anim.ResetTrigger("CancelBlind");
        anim.SetTrigger("Blind");
    }

    /// <summary>
    /// Runs on update. Checks if enemy is blind. After the limit time
    /// passes, the enemy returns to LostPlayerState.
    /// </summary>
    /// <returns>Returns an IState.</returns>
    public override IState Update()
    {
        base.Update();

        if (Blind())
        {
            return enemy.TemporaryBlindnessState;
        }
        // After blind is over

        return enemy.AggressiveState ?? enemy.PatrolState;
    }

    /// <summary>
    /// Happens once when the enemy leaves this state.
    /// </summary>
    public override void OnExit()
    {
        agent.isStopped = false;
        anim.SetTrigger("CancelBlind");
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
