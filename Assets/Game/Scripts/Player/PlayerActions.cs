using Common;
using Common.Constant;
using UnityEngine;

namespace Player
{
  public class PlayerActions : Entity
  {
    public BoxCollider2D AttackCollider;
    public BoxCollider2D ExamineCollider;

    private void CheckAttack()
    {
      if (Input.GetButton(InputConstants.Attack))
      {
      }

      if (Input.GetButton(InputConstants.Examine))
      {
      }
    }

    public void Attack()
    {
    }
  }
}