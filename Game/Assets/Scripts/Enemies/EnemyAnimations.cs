using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class responsible for handling player animations.
/// </summary>
public class EnemyAnimations : MonoBehaviour
{
    [SerializeField] private Animator anim;

    // Components
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Movement animation
        if (agent.enabled) anim.SetFloat("Speed", agent.velocity.magnitude);
        else anim.SetFloat("Speed", 0);
    }
}
