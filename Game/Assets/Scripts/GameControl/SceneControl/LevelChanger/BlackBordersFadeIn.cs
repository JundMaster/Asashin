using UnityEngine;

public class BlackBordersFadeIn : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCollider;

    private Animator anim;
    private bool insideArea;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        insideArea = false;
        boxCollider.isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            insideArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            insideArea = false;
        }
    }

    private void Update()
    {
        anim.SetBool("InsideArea", insideArea);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 1, 0.2f);
        Gizmos.DrawCube(transform.position + boxCollider.center, boxCollider.size);
    }
}
