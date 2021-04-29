using System.Collections;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private Transform[] kunaiSpawns;
    [SerializeField] private float plateDelay;

    private void Awake()
    {
        anim = GetComponent<Animator>();    
    }		

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 11)
        {
            Debug.Log("temp");
            anim.SetTrigger("Trigger");
            StartCoroutine(BackToNormal());
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 11)
        {
            Debug.Log("2");
        }
    }

    private IEnumerator BackToNormal()
    {
        float timeEntered = Time.time;
        while (Time.time - timeEntered < plateDelay)
        {
            yield return null;
        }
        anim.ResetTrigger("Trigger");
        anim.SetTrigger("BackToNormal");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.25f, 0.25f, 0.25f, 0.5f);
        foreach (Transform kunaiSpawn in kunaiSpawns)
            Gizmos.DrawSphere(kunaiSpawn.position, 0.25f);
    }
}
