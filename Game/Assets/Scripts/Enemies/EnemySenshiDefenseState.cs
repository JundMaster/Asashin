using System.Collections;
using UnityEngine;

/// <summary>
/// Scriptable object responsible for controlling senshi movement state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Senshi Defense State")]
public class EnemySenshiDefenseState : EnemyAbstractDefenseState
{
    private const byte KUNAILAYER = 15;

    [Header("Kunai to spawn")]
    [SerializeField] private GameObject kunai;

    [Header("Kunai attack spawn delay")]
    [Range(1f, 10f)][SerializeField] private float kunaiDelay;
    [Header("Waits this time in order to spawn kunai in the right time")]
    [Range(0f, 5f)] [SerializeField] private float kunaiSpawnAfterAnimation;
    //private bool kunaiCoroutine;
    private bool whileThrowingKunai;
    private IEnumerator kunaiCoroutine;

    /// <summary>
    /// Happens once when this state is enabled. Sets a kunai timer.
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();

        kunaiCoroutine = null;

        whileThrowingKunai = false;
    }

    /// <summary>
    /// Goes to defense position. If the player is fighting an enemy,
    /// it keeps throwing kunais.
    /// </summary>
    /// <returns>An IState.</returns>
    public override IState FixedUpdate()
    {
        base.FixedUpdate();

        if (instantKill)
            return enemy.DeathState;

        // Only if the player isn't fighting an enemy yet
        if (enemy.PlayerCurrentlyFighting == false)
        {
            if (enemy.AggressiveState != null)
                return enemy.AggressiveState;
        }

        // If the enemy is not moving towards the end position
        if (MoveToDefensiveRange() == false)
        {
            // If the enemy loses sight of the player it instantly
            // stops kunai coroutine and goes to another state
            if (myTarget.CanSee(playerTarget, collisionLayers) == false)
            {
                return enemy.LostPlayerState;
            }

            // If the enemy can see and is facing the player
            if (PlayerInRange() && FacingPlayer())
            {
                if (kunaiCoroutine == null)
                {
                    kunaiCoroutine = ThrowKunaiCoroutine();
                    enemy.StartCoroutine(kunaiCoroutine);
                }
            }
            // If the enemy can NOT see and is facing the player
            // Happens while the enemy is rotating after reaching final path
            else if (PlayerInRange() == false && FacingPlayer())
            {
                // Meaning it's not inside throw kunai coroutine
                if (kunaiCoroutine == null)
                    return enemy.LostPlayerState ?? enemy.PatrolState;
            }

            // Keeps rotating the enemy towards the player
            enemy.transform.RotateToSmoothly(
                playerTarget.position, ref smoothTimeRotation, turnSpeed);
        }

        // Keeps rotating the enemy towards the player
        enemy.transform.RotateTo(playerTarget.position);

        // Else it moves to the enemy without rotating towards the player
        return enemy.DefenseState;
    }

    /// <summary>
    /// Happens once when leaving this state.
    /// Sets player's last known position.
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();

        if (kunaiCoroutine != null)
        {
            enemy.StopCoroutine(kunaiCoroutine);
            kunaiCoroutine = null;
        }

        whileThrowingKunai = false;
    }

    /// <summary>
    /// Moves the enemy towards the desired defense position.
    /// </summary>
    /// <returns>Returns true if it needs to move. 
    /// Returns false if it's in the desired position.</returns>
    protected override bool MoveToDefensiveRange()
    {
        float distance = 
            Vector3.Distance(myTarget.position, playerTarget.position);

        // If the enemy is NOT in the desired position
        if (distance > randomDistance + 2 || 
            distance < randomDistance - 2)
        {
            if (distance < randomDistance - 2) runningBack = true;
            else runningBack = false;
            anim.SetBool("RunningBack", runningBack);

            agent.isStopped = false;

            // Direction from player to enemy.
            Vector3 desiredDirection =
                myTarget.position.Direction(playerTarget.position);

            // Ray from player to final destination
            Ray finalPosition = 
                new Ray(
                    playerTarget.position,
                    -desiredDirection * randomDistance);

            // If there isn't any wall in the way
            if (Physics.Raycast(
                finalPosition, randomDistance, collisionLayers) == false)
            {
                // Moves the enemy in order to keep a random distance 
                // from the player
                // Only happens if the enemy is not throwing a kunai
                if (whileThrowingKunai == false)
                {
                    agent.SetDestination(
                        playerTarget.position - desiredDirection *
                        randomDistance);
                    return true;
                }
                // Stops enemy, only happens if the enemy is throwing a kunai
                else
                {
                    agent.SetDestination(myTarget.position);
                }
            }
            // Else if there is a wall
            else
            {
                // Keeps the enemy in the same place and final destination.
                agent.SetDestination(myTarget.position);
                agent.isStopped = true;
                return false;
            }
        }
        // Else if the enemy is in the final destination
        agent.isStopped = true;
        runningBack = false;
        return false;
    }

    /// <summary>
    /// Throws a kunai towards the player future position.
    /// </summary>
    private IEnumerator ThrowKunaiCoroutine()
    {
        YieldInstruction wfd = new WaitForSeconds(kunaiDelay);
        YieldInstruction wfks = new WaitForSeconds(kunaiSpawnAfterAnimation);

        while(true)
        {
            yield return wfd;

            whileThrowingKunai = true;

            enemy.transform.RotateTo(playerTarget.position);
            anim.SetTrigger("ThrowKunai");

            // Waits this time in order to spawn kunai in the right time
            // inside the kunai animation
            yield return wfks;

            // Spawns a kunai
            GameObject thisKunai = Instantiate(
                kunai,
                myTarget.position + myTarget.forward,
                Quaternion.identity);

            // Sets layer and parent enemy of the kunai
            thisKunai.layer = KUNAILAYER;
            thisKunai.GetComponent<Kunai>().Behaviour.ParentEnemy = enemy;

            // Waits until throw kunai animation ends
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                yield return null;
            }

            whileThrowingKunai = false;

            break;
        }
        kunaiCoroutine = null;
    }
}
