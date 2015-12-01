//TODO: FIX ISSUE WITH UNITS SPAWNING NOT GENERATING PATHS DUE TO EMPTY PATHPOINT VARIABLE
using System.Collections.Generic;
using System.Linq;
using Assets.Game.Scripts.Extensions;
using UnityEngine;

namespace Steer2D
{
  public class FollowPath : SteeringBehaviour
  {
    public bool RandomizePath = true;
    private int currentPoint;
    public bool DrawGizmos = false;
    public bool Loop = false;
    public float NextCoordRadius = 0.2f;
    List<Vector2> Path;
    List<Transform> PathPoints;
    public float SlowRadius = 1;
    public float StopRadius = 0.2f;

    private void Awake()
    {
      if (RandomizePath)
      {
        PathPoints = GenPath();
      }
      Path = Path ?? new List<Vector2>();
      if (!PathPoints.Any())
      {
        ConvertToPath();
      }
    }

    private List<Transform> GenPath()
    {
      var retval = GameObject.FindGameObjectsWithTag("Patrol").Select(x => x.transform).Shuffle<Transform>().ToList();
      return retval;

    }

    private void ConvertToPath()
    {
      for (int index = 0; index < PathPoints.Count(); index++)
      {
        Path.Add(PathPoints[index].transform.position);
      }
    }

    public bool Finished
    {
      get { return currentPoint >= Path.Count(); }
    }

    public void SetNewPath(Vector2[] path)
    {
      Path = path.ToList();
      ConvertToPath();
      currentPoint = 0;
    }

    public override Vector2 GetVelocity()
    {
      Vector2 velocity;

      if (currentPoint >= Path.Count())
        return Vector2.zero;
      if (!Loop && currentPoint == Path.Count() - 1)
        velocity = arrive(Path[currentPoint]);
      else
        velocity = seek(Path[currentPoint]);

      var distance = Vector3.Distance(transform.position, Path[currentPoint]);
      if ((currentPoint == Path.Count() - 1 && distance < StopRadius) || distance < NextCoordRadius)
      {
        currentPoint++;
        if (Loop && currentPoint == Path.Count())
          currentPoint = 0;
      }

      return velocity;
    }

    private Vector2 seek(Vector2 targetPoint)
    {
      return ((targetPoint - (Vector2)transform.position).normalized * agent.MaxVelocity) - agent.CurrentVelocity;
    }

    private Vector2 arrive(Vector2 targetPoint)
    {
      var distance = Vector3.Distance(transform.position, targetPoint);
      var desiredVelocity = (targetPoint - (Vector2)transform.position).normalized;

      if (distance < StopRadius)
        desiredVelocity = Vector3.zero;
      else if (distance < SlowRadius)
        desiredVelocity = desiredVelocity * agent.MaxVelocity * ((distance - StopRadius) / (SlowRadius - StopRadius));
      else
        desiredVelocity = desiredVelocity * agent.MaxVelocity;

      return desiredVelocity - agent.CurrentVelocity;
    }

    private void OnDrawGizmos()
    {
      if (DrawGizmos)
      {
        if (currentPoint < Path.Count())
        {
          Gizmos.color = Color.red;
          Gizmos.DrawSphere(Path[currentPoint], .05f);

          if (currentPoint == Path.Count() - 1)
          {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(Path[currentPoint], SlowRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Path[currentPoint], StopRadius);
          }
          else
          {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(Path[currentPoint], NextCoordRadius);
          }
        }

        Gizmos.color = Color.magenta;
        for (var i = 0; i < Path.Count() - 1; ++i)
        {
          Gizmos.DrawLine(Path[i], Path[i + 1]);
        }
      }
    }
  }
}