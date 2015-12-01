using UnityEngine;
using System.Collections;

[AddComponentMenu("Health/Assist/Connect With Kill Display")]
/// <summary>
/// Add to an object that has a HealthAssist component
/// </summary>
public class ConnectWithKillDisplay : MonoBehaviour {
	public DisplayKills display;

	void Awake() {
		HealthAssist assist = GetComponent<HealthAssist>();

		if(!display)
			display = FindObjectOfType(typeof(DisplayKills)) as DisplayKills;

		if(assist && display)
			assist.OnAssist += AddKillToDisplay;
	}

	void AddKillToDisplay(GameObject obj) {
		HealthAssist assist = obj.GetComponent<HealthAssist>();
		GameObject[] timestamps = assist.GetAssists();

		assist.ClearAssists();
		display.AddKill(timestamps, obj);
	}
}
