using Assets.Game.Scripts.Player;
using Common;
using UnityEngine;

namespace Assets.Game.Scripts.Camera
{
  public class CameraFollowPlayer : Entity
  {
    public Vector3 CameraVelocity = Vector3.zero;
    public Vector2 maxXAndY; // The maximum x and y coordinates the camera can have.
    public Vector2 minXAndY; // The minimum x and y coordinates the camera can have.
    public PlayerControls Player;
    public float SmoothingTime = 0.3f;
    public bool UseDynamicPlayerCamera = true;
    public float xMargin = 1f; // Distance in the x axis the player can move before the camera follows.
    public float XOffset;
    public float xSmooth = 8f; // How smoothly the camera catches up with it's target movement in the x axis.
    public float yMargin = 1f; // Distance in the y axis the player can move before the camera follows.
    public float YOffset;
    public float ySmooth = 8f; // How smoothly the camera catches up with it's target movement in the y axis.
    

    private bool CheckXMargin()
    {
      // Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
      return Mathf.Abs(transform.position.x - transform.position.x + XOffset) > xMargin;
    }

    private bool CheckYMargin()
    {
      // Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
      return Mathf.Abs(transform.position.y - transform.position.y + YOffset) > yMargin;
    }

    private void CalculateXOffsets()
    {
      if (Player.FacingRight && XOffset < 0)
      {
        XOffset = -XOffset;
      }

      if (!Player.FacingRight && XOffset > 0)
      {
        XOffset = -XOffset;
      }
    }

    private void CalculateYOffsets()
    {
      if (YOffset < 0)
      {
        YOffset = -YOffset;
      }
    }

    private void FixedUpdate()
    {
      if (UseDynamicPlayerCamera)
      {
        CalculateXOffsets();
        CalculateYOffsets();
      }
      TrackPlayer();
    }

    private void TrackPlayer()
    {
      // By default the target x and y coordinates of the camera are it's current x and y coordinates.
      var targetX = transform.position.x;
      var targetY = transform.position.y;

      var playerPosX = Player.transform.position.x;
      var playerPosY = Player.transform.position.y;

      //float targetX = transform.position.x;
      //float targetY = transform.position.y;


      // If the player has moved beyond the x margin...
      if (CheckXMargin())
        // ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
        targetX = Mathf.Lerp(transform.position.x, playerPosX + XOffset, xSmooth*Time.deltaTime);


      // If the player has moved beyond the y margin...
      if (CheckYMargin())
        // ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
        targetY = Mathf.Lerp(transform.position.y, playerPosY + YOffset, ySmooth*Time.deltaTime);

      // The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
      targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
      targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

      // Set the camera's position to the target position with the same z component.
      var targetPosition = new Vector3(targetX, targetY, transform.position.z);
      transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref CameraVelocity, SmoothingTime);
    }
  }
}