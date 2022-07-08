using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttackStateBehaviour : StateMachineBehaviour
{
    [SerializeField] private float hitboxEnableTriggerAnimationProgress;
    [SerializeField] private float hitboxDisableTriggerAnimationProgress;
    [SerializeField] private int attackIndex;

    private bool hitboxEnableTriggered = false;
    private bool hitboxDisableTriggered = false;
    private bool attackEndTriggered = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hitboxEnableTriggered = false;
        hitboxDisableTriggered = false;
        attackEndTriggered = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= hitboxEnableTriggerAnimationProgress && !hitboxEnableTriggered)
        {
            hitboxEnableTriggered = true;
            GameManager.instance.player.GetControlledCharacter().characterCombatManager.EnableLightAttackHitbox(attackIndex);
        }
        if (stateInfo.normalizedTime >= hitboxDisableTriggerAnimationProgress && !hitboxDisableTriggered)
        {
            hitboxDisableTriggered = true;
            GameManager.instance.player.GetControlledCharacter().characterCombatManager.DisableLightAttackHitbox(attackIndex);
        }
        if (stateInfo.normalizedTime >= 1f && !attackEndTriggered)
        {
            attackEndTriggered = true;
            GameManager.instance.player.GetControlledCharacter().characterCombatManager.EndLightAttack();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!hitboxDisableTriggered)
        {
            hitboxDisableTriggered = true;
            GameManager.instance.player.GetControlledCharacter().characterCombatManager.DisableLightAttackHitbox(attackIndex);
        }
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
