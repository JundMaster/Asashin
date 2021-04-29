using UnityEngine;

public class PlayerRollAnimationBehaviour : StateMachineBehaviour
{
    private PlayerRoll playerRoll;

    private void Awake()
    {
        playerRoll = FindObjectOfType<PlayerRoll>();
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerRoll.Anim.applyRootMotion = true;
        playerRoll.Performing = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerRoll.Anim.applyRootMotion = true;
        playerRoll.Performing = true;
        playerRoll.PerformingTime = playerRoll.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerRoll.PerformingTime = 0;
        playerRoll.Performing = false;
        playerRoll.Anim.applyRootMotion = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
