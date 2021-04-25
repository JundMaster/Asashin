using UnityEngine;

/// <summary>
/// Class responsible for blocking level changer in case the player is currently fighting.
/// </summary>
public class BlockerOnLevelChanger : MonoBehaviour, IFindPlayer
{
    [SerializeField] private BoxCollider boxCollider;
    private BoxCollider triggerBoxCollider;

    // Components
    private Player player;
    private Animator anim;

    private float timerSincePlayerWasFighting;
    private float delayAfterPlayerStoppedFighting;
    private bool showingText;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        anim = GetComponent<Animator>();
        triggerBoxCollider = gameObject.AddComponent<BoxCollider>();
    }

    private void Start()
    {
        boxCollider.enabled = false;
        boxCollider.isTrigger = false;
        showingText = false;
        delayAfterPlayerStoppedFighting = 2f;

        triggerBoxCollider.enabled = true;
        triggerBoxCollider.isTrigger = true;
        triggerBoxCollider.size = boxCollider.size + Vector3.one;
    }

    private void Update()
    {
        anim.SetBool("ShowText", showingText);

        // If player is fighting, starts a timer and activates box collider
        if (player?.PlayerCurrentlyFighting > 0)
        {
            timerSincePlayerWasFighting = Time.time;

            if (boxCollider.enabled == false)
            {
                boxCollider.enabled = true;
                return;
            }
            return;
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
