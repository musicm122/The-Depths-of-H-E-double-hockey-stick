//todo:add enemy
//todo:add inventory system and menu
//todo:add day night cycle
//todo:make asset animations

using System.Collections.Generic;
using System.Linq;
using Assets.Game.Scripts.Common.Enum;
using Assets.Game.Scripts.Player;
using Common.Base;
using Common.Components;
using Common.Constant;
using UnityEngine;

namespace Assets.Game.Scripts.Common.Actions
{
  [RequireComponent(typeof (BoxCollider2D))]
  [RequireComponent(typeof (SpriteRenderer))]
  public class MeleeAttackAction : EntityAction
  {
    public Animator Anim;
    public string AnimationName = "MeleeAttackBool";
    public string[] AttackEffectTags = {"Enemy", "Breakable"};
    public List<Vector2> AttackOffset;
    private SpriteRenderer AttackSprite;
    private BoxCollider2D AttactkCollider2D;
    public int DamageAmt = 1;
    public PlayerControls PlayerControls;

    private void Start()
    {
      AttactkCollider2D.enabled = false;
      AttackSprite.enabled = false;
    }

    private void Awake()
    {
      Anim = Anim ?? (Animator) GetComponent(typeof (Animator));
      AttactkCollider2D = (BoxCollider2D) GetComponent(typeof (BoxCollider2D));
      AttackSprite = (SpriteRenderer) GetComponent(typeof (SpriteRenderer));
    }

    private void Update()
    {
      if (Input.GetButton(InputConstants.Attack))
      {
        PreformActionSequence();
      }
    }

    private void InitAttactDirection(Direction attackDirection)
    {
      ApplyPositionOffset(attackDirection);
      var index = (int) attackDirection;
      AttactkCollider2D.enabled = true;
      AttackSprite.enabled = true;
    }

    private void ApplyPositionOffset(Direction attackDirection)
    {
      var index = (int) attackDirection;
      AttactkCollider2D.transform.localPosition = AttackOffset[index];
      AttackSprite.transform.localPosition = AttackOffset[index];
    }

    public override void DoAction()
    {
      InitAttactDirection(PlayerControls.CurrentFacingDirection);
      Anim.SetBool(AnimationName, true);
      base.DoAction();
    }

    public override void DoActionComplete()
    {
      base.DoActionComplete();

      Anim.SetBool(AnimationName, false);
      AttackSprite.enabled = false;
      AttactkCollider2D.enabled = false;
    }

    public override void ActionCheck(Collider2D c)
    {
      base.ActionCheck(c);
      if (IsPreformingAction && AttackEffectTags.Contains(c.gameObject.tag) && AttactkCollider2D.enabled)
      {
        if (c.gameObject.tag == "Enemy")
        {
          //var enemies = c.GetComponentsInParent<Damagable>();
          var enemies = c.GetComponents<Damagable>();
          for (var i = 0; i < enemies.Count(); i++)
          {
            enemies[i].TakeDamage("Player", DamageAmt);
            //todo: figure out the angle of approach and bounce enemy\player off when damaged
            //c.GetComponentsInParent( )
            ////c.OverlapPoint()
            //enemies[i].transform.Move();
          }
        }

        if (c.gameObject.tag == "Player")
        {
          var player = c.GetComponentsInParent<Damagable>();
          for (var i = 0; i < player.Count(); i++)
          {
            player[i].TakeDamage("Enemy", DamageAmt);
            //todo: figure out the angle of approach and bounce enemy\player off when damaged

            //c.GetComponentsInParent( )
            ////c.OverlapPoint()
            //enemies[i].transform.Move();
          }
        }
      }
    }

    public void OnTriggerEnter2D(Collider2D c)
    {
      Debug.Log("In Meelee Attack Action OnTriggerEnter2D");
      ActionCheck(c);
    }

    public void OnTriggerStay2D(Collider2D c)
    {
      Debug.Log("In Meelee Attack Action OnTriggerStay2D");
      ActionCheck(c);
    }

    public void OnTriggerLeave2D(Collider2D c)
    {
      Debug.Log("In Meelee Attack Action OnTriggerLeave2D");
      ActionCheck(c);
    }
  }
}