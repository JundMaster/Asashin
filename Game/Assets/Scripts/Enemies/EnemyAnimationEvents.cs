using UnityEngine;

/// <summary>
/// Class responsible for handling animation events.
/// </summary>
public class EnemyAnimationEvents : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetRootMotion()
    {
        anim.applyRootMotion = true;
    }

    public void SetRootMotionOff()
    {
        anim.applyRootMotion = false;
    }
}
