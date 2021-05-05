using System.Collections;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    // Components
    private Animator anim;
    private BoxCollider col;
    private AbstractSoundBase pressurePlateSound;

    [SerializeField] private Kunai kunai;
    [SerializeField] private Transform[] kunaiSpawns;
    [Range(2f, 10f)][SerializeField] private float plateTriggerDelay;

    [Header("Alert variables")]
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] private LayerMask wallsWithEnemy;
    [SerializeField] private float sizeOfAlert;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider>();
        pressurePlateSound = GetComponent<AbstractSoundBase>();
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

            AlertSurroundings();

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

    /// <summary>
    /// In case this enemy finds the player, it alerts the surrounding enemies
    /// that this enemy can see.
    /// </summary>
    private void AlertSurroundings()
    {
        Collider[] enemiesAround =
            Physics.OverlapSphere(transform.position, sizeOfAlert, enemyLayer);

        if (enemiesAround.Length > 0)
        {
            foreach (Collider enemyCollider in enemiesAround)
            {
                if (enemyCollider.TryGetComponent(out EnemySimple otherEnemy))
                {
                    if (transform.CanSee(otherEnemy.transform, wallsWithEnemy))
                    {
                        if (otherEnemy.gameObject != gameObject)
                        {
                            otherEnemy.OnAlert();
                        }
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.25f, 0.25f, 0.75f, 0.5f);
        foreach (Transform kunaiSpawn in kunaiSpawns)
            Gizmos.DrawSphere(kunaiSpawn.position, 0.25f);
    }
}
