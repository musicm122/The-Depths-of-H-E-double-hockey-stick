//todo: try Steer2D for enemy behaviors

using System;
using System.Linq;
using Assets.Game.Scripts.Events;
using Common.Base;
using UnityEngine;
using D = System.Diagnostics.Debug;

namespace Common.Components
{
  [RequireComponent(typeof(BoxCollider2D))]
  [RequireComponent(typeof(SpriteRenderer))]
  [Serializable]
  public class Damagable : EntityAction, IDeathHandler
  {
    public PauseManager _PauseManager;
    public Animator Anim;
    public string AnimationVariable;
    public string[] CanBeDamagedBy;
    public BoxCollider2D DamageCollider2D;
    public bool DeathActionPreformed;
    public SpriteRenderer EffectSprite;
    public float Hp = 1;
    public bool IsAlive = true;
    public GameObject Owner;

    public void OnDeathTrigger(DeathEventData eventData)
    {
      if (eventData.TargetData == "Player")
      {
      }
      else if (eventData.TargetData == "Enemy")
      {
        //clean up
      }
    }

    private void Start()
    {
      DamageCollider2D.enabled = true;
      EffectSprite.enabled = false;
      _PauseManager = _PauseManager ?? GameObject.FindObjectOfType<PauseManager>();
    }

    private void Awake()
    {
      ActionCooldown = 1.0f;
      ActionTime = 1.0f;
      _PauseManager = _PauseManager ?? GameObject.FindObjectOfType<PauseManager>();
    }

    public virtual void TakeDamage(string callerTag, int amt = 1)
    {
      if (CanPreformAction() && CanBeDamagedBy.Contains(callerTag))
      {
        Hp -= amt;
        IsAlive = (Hp > 0);
        //todo: call gameover logic instead of Damage when player recieves killing blow.

        D.Assert(Owner != null, "Owner is null");

        if (!IsAlive && Owner.tag == "Player")
        {
          DeathActionPreformed = true;

          Debug.Log("Calling GameOver");
          D.Assert(_PauseManager != null, "_PauseManager is null");
          _PauseManager.OnGameOver();
        }
        else
        {
          gameObject.SendMessage("Damage", new HealthEvent(Owner, amt), SendMessageOptions.DontRequireReceiver);
        }
      }

      //TweenerExtension.BlinkColor(this.gameObject, Color.red, 1f);


      PreformActionSequence();
    }

    private void Update()
    {
    }

    //public virtual void OnDeath()
    //{
    //  //GAMEOVER
    //  DeathActionPreformed = true;
    //  if (Owner.tag != "Player")
    //  {
    //    //OnDeathAction.Invoke(this.gameObject.tag);
    //    Debug.Log("Calling GameOver");

    //    //_PauseManager.OnGameOver();

    //  }
    //}

    public override void DoAction()
    {
      if (Anim != null) Anim.SetBool(AnimationVariable, true);
      EffectSprite.enabled = true;
      DamageCollider2D.enabled = false;
      base.DoAction();
    }

    public override void DoActionComplete()
    {
      base.DoActionComplete();
      if (Anim != null) Anim.SetBool(AnimationVariable, false);

      EffectSprite.enabled = false;
      DamageCollider2D.enabled = true;
    }

    public override bool CanPreformAction()
    {
      return !IsActionCoolingDown && !IsPreformingAction;
    }
  }
}