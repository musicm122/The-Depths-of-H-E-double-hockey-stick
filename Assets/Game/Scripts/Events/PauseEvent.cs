using System;
using UnityEngine.Events;

namespace Assets.Game.Scripts.Events
{
  [Serializable]
  public class PauseEvent : UnityEvent<string>
  {
  }
}