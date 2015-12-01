using UnityEngine;
using System.Collections;

public class TimePause : MonoBehaviour, IPausable
{
  public float pauseDelay = 0.3f;

  private float timeScale;

  public void OnUnPause()
  {
    Debug.Log("TestPause.OnUnPause");
    Time.timeScale = timeScale;
  }

  public void OnPause()
  {
    Debug.Log("TestPause.OnPause");
    timeScale = Time.timeScale;
    Invoke("StopTime", pauseDelay);
  }

  public void OnSlow()
  {
    Debug.Log("TestPause.OnSlow");
    timeScale = Time.timeScale;
    Invoke("SlowTime", pauseDelay);
  }

  public void OnNormalize()
  {
    Debug.Log("TestPause.OnNormalize");
    timeScale = Time.timeScale;
    Invoke("NormalTime", pauseDelay);
  }

  void StopTime()
  {
    Time.timeScale = 0;
  }

  void SlowTime()
  {
    Time.timeScale = 0.5f;
  }

  void NormalTime()
  {
    Time.timeScale = 1f;
  }
}
