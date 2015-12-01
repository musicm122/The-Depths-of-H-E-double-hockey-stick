using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Assets.Game.Scripts;
using Assets.Game.Scripts.Util;

public class PauseManager : MonoBehaviour
{
  public GameObject pausable;
  public Canvas pauseCanvas;
  public Canvas GameOverCanvas;
  public Canvas WinGameCanvas;
  private TimePause _TimePause;

  private bool isPaused = false;
  private Animator anim;
  private Component[] pausableInterfaces;
  private Component[] quittableInterfaces;

  void Start()
  {
    // PauseManager requires the EventSystem - make sure there is one
    if (FindObjectOfType<EventSystem>() == null)
    {
      var es = new GameObject("EventSystem", typeof(EventSystem));
      es.AddComponent<StandaloneInputModule>();
    }

    pausableInterfaces = pausable.GetComponents(typeof(IPausable));
    quittableInterfaces = pausable.GetComponents(typeof(IQuittable));
    _TimePause = gameObject.GetComponent<TimePause>();
    anim = pauseCanvas.GetComponent<Animator>();

    pauseCanvas.enabled = false;
    GameOverCanvas.enabled = false;
    WinGameCanvas.enabled = false;


  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Pause) || Input.GetKeyDown(KeyCode.P))
    {
      if (isPaused)
      {
        OnUnPause();
      }
      else
      {
        OnPause();
      }
    }

    pauseCanvas.enabled = isPaused;
    anim.SetBool("IsPaused", isPaused);
  }

  public void OnQuit()
  {
    Debug.Log("PauseManager.OnQuit");

    foreach (var quittableComponent in quittableInterfaces)
    {
      IQuittable quittableInterface = (IQuittable)quittableComponent;
      if (quittableInterface != null)
        quittableInterface.OnQuit();
    }
  }

  public void OnUnPause()
  {
    Debug.Log("PauseManager.OnUnPause");
    isPaused = false;

    foreach (var pausableComponent in pausableInterfaces)
    {
      IPausable pausableInterface = (IPausable)pausableComponent;
      if (pausableInterface != null)
        pausableInterface.OnUnPause();
    }
  }

  public void OnPause()
  {
    Debug.Log("PauseManager.OnPause");
    isPaused = true;

    foreach (var pausableComponent in pausableInterfaces)
    {
      IPausable pausableInterface = (IPausable)pausableComponent;
      if (pausableInterface != null)
        pausableInterface.OnPause();
    }
  }

  public void OnRestart()
  {
    Debug.Log("PauseManager.OnRestart");
    Application.LoadLevel(Application.loadedLevel);

  }

  public void OnGameOver()
  {
    Debug.Log("PauseManager.OnGameOver");
    var obj = GameObject.Find("DarkOverlay").GetComponent<SpriteRenderer>().color = Color.red;
    GameObject.FindObjectOfType<AudioManager>().BackgroundMusic.Stop();
    _TimePause.OnSlow();

    StartCoroutine(CoroutineUtil.DeferredExecutor(1f, () =>
    {
      _TimePause.OnPause();
      GameOverCanvas.enabled = true;
    }));
  }

  public void OnGameWin()
  {
    Debug.Log("PauseManager.OnGameWin");
    var obj = GameObject.Find("DarkOverlay").GetComponent<SpriteRenderer>().color = Color.white;
    GameObject.FindObjectOfType<AudioManager>().BackgroundMusic.Stop();
    _TimePause.OnSlow();

    WinGameCanvas.enabled = true;
  }
}
