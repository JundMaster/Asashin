using System;
using UnityEngine;

/// <summary>
/// Class responsible for showing black borders UI and changing camera when 
/// the player is entering a scene changer area.
/// </summary>
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            OnEnteredArea(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            insideArea = false;
            OnEnteredArea(false);
        }
    }

    private void Update()
    {
        anim.SetBool("InsideArea", insideArea);
    }

    protected virtual void OnEnteredArea(bool condition) => 
        EnteredArea?.Invoke(condition);

    /// <summary>
    /// Event registered on CinemachineTarget.
    /// </summary>
    public event Action<bool> EnteredArea;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 1, 0.2f);
        Gizmos.DrawCube(transform.position + boxCollider.center, boxCollider.size);
    }
}
