﻿using UnityEngine;

namespace Assets.Game.Scripts.Extensions
{
  public static class Rigidbody2dExtensions
  {
    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition,
      float explosionRadius)
    {
      var dir = (body.transform.position - explosionPosition);
      var wearoff = 1 - (dir.magnitude/explosionRadius);
      body.AddForce(dir.normalized*explosionForce*wearoff);
    }

    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition,
      float explosionRadius, float upliftModifier)
    {
      var dir = (body.transform.position - explosionPosition);
      var wearoff = 1 - (dir.magnitude/explosionRadius);
      var baseForce = dir.normalized*explosionForce*wearoff;
      body.AddForce(baseForce);

      var upliftWearoff = 1 - upliftModifier/explosionRadius;
      Vector3 upliftForce = Vector2.up*explosionForce*upliftWearoff;
      body.AddForce(upliftForce);
    }
  }
}