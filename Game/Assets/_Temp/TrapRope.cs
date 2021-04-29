using System.Collections;
using UnityEngine;

public class TrapRope : MonoBehaviour
{
    private BoxCollider col;

    [SerializeField] private Kunai kunai;
    [SerializeField] private Transform[] kunaiSpawns;

    [SerializeField] private GameObject[] ropeCornerAnchors;
    private HingeJoint[] joints;
    private int initialNumOfJoints;

    [SerializeField] private GameObject rope;
    private Transform[] ropeChildren;

    private void Awake()
    {
        col = GetComponent<BoxCollider>();
        joints = GetComponentsInChildren<HingeJoint>();
        initialNumOfJoints = joints.Length;
        ropeChildren = GetComponentsInChildren<Transform>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            joints = GetComponentsInChildren<HingeJoint>();

            if (initialNumOfJoints != joints.Length)
            {
                col.enabled = false;

                foreach (Transform kunaiSpawn in kunaiSpawns)
                    Instantiate(kunai, kunaiSpawn.position, Quaternion.identity);

                StartCoroutine(DestroyRope());
            }
        }
    }

    private IEnumerator DestroyRope()
    {
        foreach (GameObject ropeCornerAnchor in ropeCornerAnchors)
            Destroy(ropeCornerAnchor.gameObject);

        foreach (Transform ropeChild in ropeChildren)
            ropeChild.gameObject.layer = 30; // Ignores everything

        yield return new WaitForSeconds(2);

        foreach (Transform ropeChild in ropeChildren)
        {
            if (ropeChild != null) Destroy(ropeChild.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.25f, 0.25f, 0.75f, 0.5f);
        foreach (Transform kunaiSpawn in kunaiSpawns)
            Gizmos.DrawSphere(kunaiSpawn.position, 0.25f);
    }

    	

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
