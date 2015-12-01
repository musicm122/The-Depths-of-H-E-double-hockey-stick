using System;
using UnityEngine;

namespace Steer2D
{
  public class Pursue : SteeringBehaviour
  {
    public bool DefaultToPlayerTarget = true;
    public SteeringAgent TargetAgent;


    public override Vector2 GetVelocity()
    {
      if (DefaultToPlayerTarget)
      {
        TargetAgent = TargetAgent ?? GameObject.Find("Player 1").GetComponent<SteeringAgent>();

      }
      float t = Vector3.Distance(transform.position, TargetAgent.transform.position) / TargetAgent.MaxVelocity;
      Vector2 targetPoint = (Vector2)TargetAgent.transform.position + TargetAgent.CurrentVelocity * t;

      return ((targetPoint - (Vector2)transform.position).normalized * agent.MaxVelocity) - agent.CurrentVelocity;
    }
  }
}
