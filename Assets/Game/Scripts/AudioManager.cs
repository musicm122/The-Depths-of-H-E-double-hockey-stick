using UnityEngine;

namespace Assets.Game.Scripts
{
  public class AudioManager : MonoBehaviour
  {
    public AudioSource BackgroundMusic;

    private void Start()
    {
      PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
      if (BackgroundMusic != null)
      {
        BackgroundMusic.loop = true;
        BackgroundMusic.Play();
      }
    }
  }
}