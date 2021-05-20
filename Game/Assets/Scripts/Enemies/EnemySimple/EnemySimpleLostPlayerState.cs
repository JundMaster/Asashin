using UnityEngine;
using System.Collections;

/// <summary>
/// Scriptable object responsible for controlling enemy state after losing the 
/// player.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Common Lost Player State")]
public class EnemySimpleLostPlayerState : EnemySimpleAbstractStateWithVision, 
    IUpdateOptions
{
    [Header("Time the enemy will spend looking for player")]
    [Range(0.1f,15)][SerializeField] private float timeToLookForPlayer;

    [Header("Rotation speed after reaching final point (less means faster)")]
    [Range(0.1f, 1f)] [SerializeField] private float turnSpeed;

    [Header("Exclamation and interrogation mark prefabs")]
    [SerializeField] private GameObject exclamationMarkPrefab;
    [SerializeField] private GameObject interrogationMarkPrefab;
    [SerializeField] private Vector3 offset;
    private enum TypeOfMark { Interrogation, Exclamation };
    private GameObject currentPunctuationMark;

    // State variables
    private IEnumerator lookForPlayerCoroutine;
    private bool breakState;
    private Vector3 enteredInPosition;
    private float distanceFromPositionToCheck;

    // Components
    private VisionCone visionCone;
    private Options options;

    /// <summary>
    /// Type of movement after something happened.
    /// </summary>
    private enum TypeOfMovement { FollowSound, HitFromBehind }

    /// <summary>
    /// Runs once on start. Sets vision cone to be the same as the enemy's one.
    /// </summary>
    public override void Start()
    {
        base.Start();

        options = FindObjectOfType<Options>();

        visionCone = enemy.VisionConeScript;

        distanceFromPositionToCheck = 0.6f;

        // Updates options dependant values as soon as the enemy spawns
        enemy.StartCoroutine(UpdateValuesCoroutine());
    }

    /// <summary>
    /// Happens once when this state is enabled.
    /// Sets a path to player's last known position.
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();

        // So the enemy will only start checking the final destination after
        // leaving the initial position
        enteredInPosition = enemy.transform.position;

        lookForPlayerCoroutine = null;
        breakState = false;
        enemy.InCombat = true;
        agent.isStopped = false;

        // Moves the agent to a position + a random offset
        Vector3 finalDestination = enemy.PositionOnLostPlayerState + 
            new Vector3(
                Random.Range(-5, 5),
                0,
                Random.Range(-5, 5));

        agent.SetDestination(finalDestination);

        enemy.CollisionWithPlayer += TakeImpact;
        options.UpdatedValues += UpdateValues;
    }

    /// <summary>
    /// Runs on update.
    /// Moves the enemy towards player's last known position. When the enemy
    /// reaches that position, it starts a coroutine to look for the player.
    /// </summary>
    /// <returns>An IState.</returns>
    public override IState Update()
    {
        base.Update();

        if (die)
            return enemy.DeathState;

        if (alert)
            return enemy.DefenseState;

        // Listened to a sound
        if (followSound && hitFromBehind == false)
            SetNewPosition(TypeOfMovement.FollowSound);

        // If it got hit from afar inside this state
        else if (followSound == false && hitFromBehind)
            SetNewPosition(TypeOfMovement.HitFromBehind);
        
        // If enemy is in range, it stops looking for player coroutine
        if (PlayerInRange())
        {
            // Instantiates an exclamation mark
            InstantiatePunctuationMark(TypeOfMark.Exclamation);
            return enemy.DefenseState ?? enemy.PatrolState;
        }

        // If the enemy reached the player last known position and is not near
        // the entrance position, starts looking for player
        if (ReachedLastKnownPosition() && 
            Vector3.Distance(enemy.transform.position, enteredInPosition) >
            distanceFromPositionToCheck)
        {
            if (lookForPlayerCoroutine == null)
            {
                lookForPlayerCoroutine = LookForPlayer();
                enemy.StartCoroutine(lookForPlayerCoroutine);
            }
        }

        // Breaks from this state back to patrol state
        // Happens if the enemy didn't find the player
        if (breakState)
            return enemy.PatrolState;

        return enemy.LostPlayerState;
    }

    /// <summary>
    /// Happens when leaving this state.
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();

        // Resets variables
        breakState = false;
        if (lookForPlayerCoroutine != null)
        {
            enemy.StopCoroutine(lookForPlayerCoroutine);
            lookForPlayerCoroutine = null;
        }

        agent.isStopped = false;
        enemy.VisionConeGameObject.SetActive(false);

        enemy.CollisionWithPlayer -= TakeImpact;
        options.UpdatedValues -= UpdateValues;
    }

    /// <summary>
    /// Instantiates an interrogation or exclamation mark.
    /// </summary>
    private void InstantiatePunctuationMark(TypeOfMark type)
    {
        // Instantiates an exclamation mark
        if (type == TypeOfMark.Interrogation)
        {
            currentPunctuationMark = Instantiate(
                interrogationMarkPrefab,
                enemy.transform.position + offset,
                Quaternion.identity);
        }
        else
        {
            currentPunctuationMark = Instantiate(
                exclamationMarkPrefab,
                enemy.transform.position + offset,
                Quaternion.identity);
        }

        currentPunctuationMark.transform.parent = enemy.transform;
    }

    /// <summary>
    /// Method to check if the enemy is at player's last known position.
    /// If it didn't not reach the last position, it deactivates the vision cone.
    /// </summary>
    /// <returns>True if it is.</returns>
    private bool ReachedLastKnownPosition()
    {
        if (agent.remainingDistance < distanceFromPositionToCheck)
            return true;
        else
        {
            if (enemy.VisionConeGameObject.activeSelf)
                enemy.VisionConeGameObject.SetActive(false);

            return false;
        }
    }

    /// <summary>
    /// Updates entered position of this state, stops looking for player 
    /// coroutine, sets a new position depending on the type of movement.
    /// </summary>
    /// <param name="type">If the enemy is following a sound or was hit.</param>
    private void SetNewPosition(TypeOfMovement type)
    {
        enteredInPosition = enemy.transform.position;

        // Instantiates an interrogation mark
        InstantiatePunctuationMark(TypeOfMark.Interrogation);

        // Stops coroutine in case it's already searching for the player
        if (lookForPlayerCoroutine != null)
        {
            enemy.StopCoroutine(lookForPlayerCoroutine);
            lookForPlayerCoroutine = null;
        }

        Vector3 finalDestination;

        if (type == TypeOfMovement.FollowSound)
        {
            // Moves the agent to a position close to the sound
            finalDestination =
                positionOfSound + GetRandomPositionWithSound();
        }
        else
        {
            // Moves the agent to a position close to the sound
            finalDestination = playerTarget.position + new Vector3(
                    Random.Range(-4, 4),
                    0,
                    Random.Range(-4, 4));
        }

        agent.SetDestination(finalDestination);

        positionOfSound = default;
        hitFromBehind = false;
        followSound = false;
        breakState = false;
    }

    /// <summary>
    /// Returns a random position depending on the distance between
    /// the enemy and the sound.
    /// </summary>
    /// <returns>Returns a Vector3.</returns>
    private Vector3 GetRandomPositionWithSound()
    {
        float distance = Vector3.Distance(positionOfSound, myTarget.position);

        Vector3 randomPosition;
        if (distance <= 7)
        {
            randomPosition = Vector3.zero;
        }
        else if (distance <= 13)
        {
            randomPosition = new Vector3(
                Random.Range(-2, 2),
                0,
                Random.Range(-2, 2));
        }
        else
        {
            randomPosition = new Vector3(
                Random.Range(-4, 4),
                0,
                Random.Range(-4, 4));
        }

        return randomPosition;
    }

    /// <summary>
    /// Rotates every x seconds, looking for the player.
    /// </summary>
    /// <returns>Returns null.</returns>
    private IEnumerator LookForPlayer()
    {
        YieldInstruction wffu = new WaitForFixedUpdate();
        YieldInstruction wfs = new WaitForSeconds(Random.Range(1f, 3f));
        float timePassed = Time.time;

        float yRotationCurrent = enemy.transform.eulerAngles.y;
        float yRotationMax = Mathf.Clamp(yRotationCurrent + 45f, 1, 359);
        float yRotationMin = Mathf.Clamp(yRotationCurrent - 45f, 1, 359);
        float multiplier = 1;

        // While the enemy can't see the player or while the time is less than
        // the time allowed searching for the player.
        // As soon as the enemy leaves this state, is this coroutine is turned
        // to false.
        while (PlayerInRange() == false && 
            Time.time - timePassed < timeToLookForPlayer)
        {
            if (followSound || hitFromBehind)
            {
                if (enemy.VisionConeScript == null &&
                    enemy.VisionConeGameObject.activeSelf)
                    enemy.VisionConeGameObject.SetActive(false);

                break;
            }

            // Triggers rotation to right
            if (enemy.transform.eulerAngles.y >= yRotationMax)
            {
                yRotationMax = 
                    Mathf.Clamp(enemy.transform.eulerAngles.y, 1, 359);
                multiplier *= -1;
                yield return wfs;
            }

            // Triggers rotation to left
            else if (enemy.transform.eulerAngles.y <= yRotationMin)
            {
                yRotationMin =
                    Mathf.Clamp(enemy.transform.eulerAngles.y, 1, 359);
                multiplier *= -1;
                yield return wfs;
            }

            // Rotates
            enemy.transform.eulerAngles += 
                new Vector3(0, turnSpeed * multiplier, 0);

            // Activates/Deactivates and calculates vision cone
            if (enemy.VisionConeScript != null &&
                enemy.VisionConeGameObject.activeSelf == false)
                enemy.VisionConeGameObject.SetActive(true);

            if (enemy.VisionConeScript == null &&
                enemy.VisionConeGameObject.activeSelf)
                enemy.VisionConeGameObject.SetActive(false);

            if (enemy.VisionConeScript != null)
            {
                if (visionCone == null) visionCone = enemy.VisionConeScript;
                visionCone?.Calculate();
            } 

            yield return wffu;
        }
        breakState = true;
    }

    /// <summary>
    /// Invokes coroutine to update vision cone variables.
    /// </summary>
    public void UpdateValues()
    {
        // Will only call if enemy already respawned
        // (will only happen if the player changes options in pause menu)
        if (enemy != null) enemy.StartCoroutine(UpdateValuesCoroutine());
    }

    /// <summary>
    /// Updates vision cone variables.
    /// </summary>
    private IEnumerator UpdateValuesCoroutine()
    {
        yield return new WaitForFixedUpdate();

        if (enemy != null)
        {
            // If vision cone option is on
            if (enemy.Options.EnemyVisionCones)
            {
                enemy.VisionConeScript = visionCone;
                enemy.VisionConeGameObject.SetActive(true);
            }
            else // If vision cone option is off
            {
                enemy.VisionConeGameObject.SetActive(false);
                enemy.VisionConeScript = null;
            }
        }
    }
}
