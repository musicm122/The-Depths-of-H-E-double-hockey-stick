using UnityEngine;
using System.Collections;
using Assets.Game.Scripts.Common.Enum;
using Common.Components;
using Assets.Game.Scripts.Extensions;
using Assets.Game.Scripts.Util;
using JetBrains.Annotations;
using D = System.Diagnostics.Debug;

public class OnPlayerDamage : StateMachineBehaviour
{

  // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    if (stateInfo.IsName("TakeDamage"))
    {

      var player = GameObject.Find("Player 1");
      var originalColor = player.GetComponent<SpriteRenderer>().color;

      D.Assert(player != null, "Can't find player object");

      //var player = GameObject.Find("Player");
      //D.Assert(sprite != null, "Can't find Sprite Renderer on player object");
      //var startColor = sprite.color;
      //todo: play particle effect on damage
      TweenerExtension.TakeDamageEffects(player);
      //GameObject.Find("Player 1").GetComponent<SpriteRenderer>().color = originalColor;
      //GameObject.Find("PlayerHealthComponent").GetComponent<SpriteRenderer>().color = originalColor;

      //TweenerExtension.ShakeCam(2f,2f,1f);
      //TweenerExtension.BlinkColor(player, new Color(10f, 1f, 1f, 0.8f), 1f);
      ResetAnimationState(animator);
    }


  }

  public void ResetAnimationState(Animator animator)
  {
    var currentDirection = (Direction)animator.GetInteger("FaceDirection");
    switch (currentDirection)
    {
      case Direction.Up:
        animator.CrossFade("P_Idle_Up", 0.5f);
        break;
      case Direction.Down:
        animator.CrossFade("P_Idle_Down", 0.5f);
        break;
      case Direction.Left:
        animator.CrossFade("P_Idle_Left", 0.5f);
        break;
      case Direction.Right:
        animator.CrossFade("P_Idle_Right", 0.5f);
        break;
    }
  }

  // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
  //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
  //
  //}

  // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
  //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
  //
  //}

  // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
  //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
  //
  //}

  // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
  //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
  //
  //}
}
