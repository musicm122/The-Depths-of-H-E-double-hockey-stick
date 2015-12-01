using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Game.Scripts.Common.Components
{
  public class WinLevel : MonoBehaviour
  {
    public PauseManager _PauseManager;

    public void OnTriggerEnter2D(Collider2D c)
    {
      WinGameCheck(c);
    }

    private void WinGameCheck(Collider2D c)
    {
      if (c.tag == "Player")
      {
        Debug.Log("In Meelee Attack Action OnTriggerEnter2D");
        _PauseManager.OnGameWin();
      }
    }

  }
}
