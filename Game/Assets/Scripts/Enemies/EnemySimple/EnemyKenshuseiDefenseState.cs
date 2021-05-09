using UnityEngine;

/// <summary>
/// Scriptable object responsible for controlling kensusei defense state.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Kenshusei Defense State")]
public class EnemyKenshuseiDefenseState : EnemySimpleAbstractDefenseState
{
    /// <summary>
    /// Goes to defense position. If the player is fighting an enemy,
    /// it keeps throwing kunais.
    /// </summary>
    /// <returns>An IState.</returns>
    public override IState FixedUpdate()
    {
        base.FixedUpdate();

        if (die)
            return enemy.DeathState;

        // Only if the player isn't fighting an enemy yet
        if (enemy.PlayerCurrentlyFighting < 2)
        {
            if (enemy.AggressiveState != null)
                return enemy.AggressiveState;
        }

        // If the enemy is not moving towards the end position
        if (MoveToDefensiveRange() == false)
        {
            // If the enemy loses sight of the player it instantly
            // goes to another state
            if (myTarget.CanSee(playerTarget, collisionLayers) == false)
            {
                return enemy.LostPlayerState;
            }

            // If the enemy can NOT see and is facing the player
            // Happens while the enemy is rotating after reaching final path
            if (PlayerInRange() == false && FacingPlayer())
            {
                return enemy.LostPlayerState ?? enemy.PatrolState;
            }

            // Keeps rotating the enemy towards the player
            enemy.transform.RotateTo(playerTarget.position);
        }

        // Keeps rotating the enemy towards the player
        enemy.transform.RotateToSmoothly(
            playerTarget.position, ref smoothRotation, ROTATIONSPEED);

        // Else it moves to the enemy without rotating towards the player
        return enemy.DefenseState;
    }

    /// <summary>
    /// Moves the enemy towards the desired defense position.
    /// If the enemy is already in the desired position, it will starting
    /// to walk in circles around the player.
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
            // If the enemy is moving to end position, it keeps updating time
            CancelWalkSideWaysVariables();

            if (distance < randomDistance - 2)
            {
                agent.speed = walkingSpeed;
                runningBack = true;
            }
            else if (distance > randomDistance + 2)
            {
                agent.speed = runningSpeed;
                runningBack = false;
            }

            agent.isStopped = false;

            // Direction from player to enemy.
            Vector3 desiredDirection = Vector3.zero;

            if (distance < randomDistance - 2)
                desiredDirection =
                    myTarget.position.InvertedDirection(playerTarget.position);
            else if (distance > randomDistance + 2)
                desiredDirection =
                    myTarget.position.Direction(playerTarget.position);

            // Ray from player to final destination
            Ray finalPosition =
                new Ray(myTarget.position, desiredDirection);

            // If there isn't any wall in the way
            if (Physics.Raycast(
                finalPosition, MINDISTANCEFROMWALL, collisionLayers) == false)
            {
                // Moves the enemy in order to keep a random distance 
                // from the player
                agent.SetDestination(
                    myTarget.position + desiredDirection * 1.1f);
                return true;
            }
            // Else if there is a wall
            else
            {
                // Keeps the enemy in the same place and final destination.
                agent.SetDestination(myTarget.position);
                agent.speed = 0;
                runningBack = false;
                CancelWalkSideWaysVariables();
                agent.isStopped = true;
                return false;
            }
        }
        // Else if the enemy is in the final destination
        WalkSideways();
        runningBack = false;
        return false;
    }
}
