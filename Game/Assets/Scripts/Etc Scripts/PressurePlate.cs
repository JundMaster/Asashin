using System.Collections;
using UnityEngine;

public class PressurePlate : MonoBehaviour, IFindPlayer
{
    // Components
    private Animator anim;
    private BoxCollider col;
    private AbstractSoundBase pressurePlateSound;

    [SerializeField] private Kunai kunai;
    [SerializeField] private Transform[] kunaiSpawns;
    [Range(2f, 10f)][SerializeField] private float plateTriggerDelay;

    [Header("Sound emission variables")]
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] private IntensityOfSound intensityOfSound;
    private Player player;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider>();
        pressurePlateSound = GetComponent<AbstractSoundBase>();
        player = FindObjectOfType<Player>();
    }		

    /// <summary>
    /// Only triggers once. Every time the player enters the collider, the
    /// collider is disabled.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            anim.SetTrigger("Trigger");

            gameObject.EmitSound(player, intensityOfSound, enemyLayer);

            pressurePlateSound.PlaySound(Sound.PressurePlate);

            foreach (Transform kunaiSpawn in kunaiSpawns)
                Instantiate(kunai, kunaiSpawn.position, Quaternion.identity);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11)
            StartCoroutine(BackToNormal());
    }

    private IEnumerator BackToNormal()
    {
        col.enabled = false;

        // Waits for delay to go back to normal
        float timeEntered = Time.time;
        while (Time.time - timeEntered < plateTriggerDelay)
        {
            yield return null;
        }

        anim.ResetTrigger("Trigger");
        anim.SetTrigger("BackToNormal");

        // Waits for animation time
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        col.enabled = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.25f, 0.25f, 0.75f, 0.5f);
        foreach (Transform kunaiSpawn in kunaiSpawns)
            Gizmos.DrawSphere(kunaiSpawn.position, 0.25f);
    }

    public void FindPlayer() =>
        player = FindObjectOfType<Player>();

    public void PlayerLost()
    {
        // Left blank on purpose
    }
}
