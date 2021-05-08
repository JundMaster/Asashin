using UnityEngine;

/// <summary>
/// Class responsible for blocking level changer in case the player is currently fighting.
/// </summary>
public class BlockerOnLevelChanger : MonoBehaviour, IFindPlayer
{
    [SerializeField] private LayerMask playerLayer;

    private BoxCollider boxCollider; // blocks player
    private BoxCollider triggerBoxCollider; // shows warning text

    // Components
    private Player player;
    private Animator anim;

    private float timerSincePlayerWasFighting;
    private float delayAfterPlayerStoppedFighting;
    private bool showingText;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        player = FindObjectOfType<Player>();
        anim = GetComponent<Animator>();

        // Box col to show text
        triggerBoxCollider = gameObject.AddComponent<BoxCollider>();
    }

    private void Start()
    {
        boxCollider.enabled = false;
        boxCollider.isTrigger = false;
        showingText = false;
        delayAfterPlayerStoppedFighting = 5f;

        // Box col to show text
        triggerBoxCollider.enabled = true;
        triggerBoxCollider.isTrigger = true;
        triggerBoxCollider.size = boxCollider.size + Vector3.one;
    }

    private void Update()
    {
        anim.SetBool("ShowText", showingText);

        // If player is fighting, starts a timer and activates box collider
        if (player != null)
        {
            // Only if player is close
            if (player.PlayerCurrentlyFighting > 0 && 
                Vector3.Distance(transform.position, player.transform.position) < 30)
            {
                timerSincePlayerWasFighting = Time.time;

                // Checks if player is already inside the blocker
                Collider[] boxCollisions =
                    Physics.OverlapBox(transform.position, boxCollider.size / 2, Quaternion.identity, playerLayer);

                // Only activates the blocker if the player isn't already inside it
                if (boxCollisions.Length == 0)
                {
                    if (boxCollider.enabled == false)
                    {
                        boxCollider.enabled = true;
                    }
                }

                return;
            }
        }
        // else if the player isn't fighting, and the delay after fighting
        // is over, it deactivates the box collider, in case it is activated
        if (boxCollider.enabled)
            if (Time.time - timerSincePlayerWasFighting > delayAfterPlayerStoppedFighting)
                boxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            if (Time.time - timerSincePlayerWasFighting < delayAfterPlayerStoppedFighting)
            {
                showingText = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            if (Time.time - timerSincePlayerWasFighting > delayAfterPlayerStoppedFighting)
            {
                showingText = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            showingText = false;
        }
    }

    public void FindPlayer()
    {
        player = FindObjectOfType<Player>();
    }

    public void PlayerLost()
    {
        // Left blank on purpose
    }
}
