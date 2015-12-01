using UnityEngine;
using System.Collections;
using System.Linq;
using Common.Base;
using Common.Components;

[RequireComponent(typeof(BoxCollider2D))]
public class DamagableObstacle : EntityAction
{

    public string[] AttackEffectTags = new[] { "Enemy", "Player" };
    public BoxCollider2D DamageAreaCollider2D;
    public int DamageAmt = 1;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void ActionCheck(Collider2D c)
    {
        base.ActionCheck(c);
        if (AttackEffectTags.Contains(c.gameObject.tag) && this.DamageAreaCollider2D.enabled)
        {
            if (AttackEffectTags.Contains(c.gameObject.tag))
            {
                var enemies = c.GetComponentsInParent<Damagable>();
                for (int i = 0; i < enemies.Count(); i++)
                {
                    enemies[i].TakeDamage("Obstacle", DamageAmt);
                }
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D c)
    {
        Debug.Log("In DamageableObject Action OnTriggerEnter2D");
        ActionCheck(c);
    }
    public void OnTriggerStay2D(Collider2D c)
    {
        Debug.Log("In DamageableObject Action OnTriggerStay2D");
        ActionCheck(c);

    }
    public void OnTriggerLeave2D(Collider2D c)
    {
        Debug.Log("In DamageableObject Action OnTriggerLeave2D");
        ActionCheck(c);
    }

}
