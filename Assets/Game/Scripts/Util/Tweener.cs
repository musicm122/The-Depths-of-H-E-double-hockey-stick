using System.Collections;
using Assets.Game.Scripts.Extensions;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Assets.Game.Scripts.Util
{
  public static class TweenerExtension
  {
    public static float Radius = 100f;
    public static float Force = 100f;
    public static float Pushfactor = 20f;

    public static void Move(this Transform t, Vector2 newLocation, float speed)
    {
      Vector2.Lerp(t.localPosition, newLocation, speed);
    }

    public static Vector2 GetPushBackDirectionNormal(this Collision2D col)
    {
      var contactPoint = col.contacts[0].point;
      var center = col.collider.bounds.center;
      var normal = new Vector2(contactPoint.x - center.x, contactPoint.y - center.y).normalized;
      return normal;
    }

    /// <summary>
    /// </summary>
    /// <param name="c">Camera Game object</param>
    /// <param name="shakeIntensity">the degrees to shake the camera</param>
    /// <param name="shakeTime"></param>
    /// <param name="dropOffTime">How long it takes the shaking to settle down to nothing</param>
    public static void ShakeCamera(GameObject c, float shakeIntensity, float shakeTime, float dropOffTime = 1.6f)
    {
      var shakeAmt = shakeIntensity*0.2f; // the degrees to shake the camera

      Debug.Assert(c != null, "camera is null");

      var shakeTween = LeanTween.rotateAroundLocal(c, Vector3.right, shakeAmt, shakeTime)
        .setEase(LeanTweenType.easeShake) // this is a special ease that is good for shaking
        .setLoopClamp()
        .setRepeat(-1);

      Debug.Assert(shakeTween != null, "shakeTween is null");

      // Slow the camera shake down to zero
      LeanTween.value(c, shakeAmt, 0f, dropOffTime).setOnUpdate(
        (float val) => { shakeTween.setTo(Vector3.right*val); }
        ).setEase(LeanTweenType.easeOutQuad);
    }

    /// <summary>
    /// </summary>
    /// <param name="c">Camera Game object</param>
    /// <param name="shakeIntensity">the degrees to shake the camera</param>
    /// <param name="shakeTime"></param>
    /// <param name="dropOffTime">How long it takes the shaking to settle down to nothing</param>
    public static void ShakeCamera(float shakeIntensity, float shakeTime, float dropOffTime = 1.6f)
    {
      var c = UnityEngine.Camera.main.gameObject;
      ShakeCamera(c, shakeIntensity, shakeTime, dropOffTime);
    }

    public static void BlinkColor(GameObject gameObject, Color from, Color to, float time)
    {
      LeanTween.value(gameObject, from, to, time).setLoopPingPong(1).setOnUpdate(
        (Color col) => { gameObject.GetComponent<SpriteRenderer>().material.color = col; }
        );
    }

    public static void BlinkColor(GameObject gameObject, Color to, float time)
    {
      var starColor = gameObject.GetComponent<SpriteRenderer>().material.color;
      BlinkColor(gameObject, starColor, to, time);
    }

    public static void BlinkTransparency(GameObject gameObject, float time)
    {
      var starColor = gameObject.GetComponent<SpriteRenderer>().material.color;
      var transparentColor = new Color(1f, 1f, 1f, 5f);
      BlinkColor(gameObject, starColor, transparentColor, time);
    }

    public static void ShakeCam(float x = 1.0f, float y = 1.0f, float time = 0.01f)
    {
      var cam = UnityEngine.Camera.main.gameObject;
      var ht = new Hashtable();
      ht.Add("x", x);
      ht.Add("y", y);
      ht.Add("time", time);
      iTween.ShakePosition(cam, ht);
      //iTween.PunchPosition(gameObject, new Vector3(3.0f, 1.0f, 100.0f), 0.5f);
      //sprite.color = Color.white;
    }

    public static void Punch(GameObject obj, float x = 3.0f, float y = 1.0f, float z = 100f, float duration = 0.1f)
    {
      iTween.PunchPosition(obj, new Vector3(x, y, z), duration);
    }

    public static void TakeDamageEffects(GameObject target)
    {
      target.GetComponent<Rigidbody2D>().AddExplosionForce(Force*Pushfactor, target.transform.position, Radius);

      //var originalColor = target.GetComponent<SpriteRenderer>().color;
      //Punch(target, duration: 1f);
      ShakeCam(1f, 1f, 0.1f);
      //TweenerExtension.BlinkColor(target, new Color(255f, 1f, 1f, 0.8f), 1f);
      //target.GetComponent<SpriteRenderer>().color = originalColor;
    }
  }
}