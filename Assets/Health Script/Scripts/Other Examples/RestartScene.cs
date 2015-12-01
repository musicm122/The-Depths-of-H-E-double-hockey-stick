using UnityEngine;
using System.Collections;

public class RestartScene : MonoBehaviour {
	void OnGUI() {
		if(GUI.Button(new Rect(Screen.width - 75, Screen.height - 30, 75, 30), "Restart"))
			Application.LoadLevel(Application.loadedLevel);
	}
}
