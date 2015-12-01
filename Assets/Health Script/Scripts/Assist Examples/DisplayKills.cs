using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Health/Assist/Display Kills")]
public class DisplayKills : MonoBehaviour {

	public Queue<Kill> killList = new Queue<Kill>();

	public float displayTime = 5;

	/// <summary>
	/// This is called by the ConnectWithKillDisplay component
	/// </summary>
	public void AddKill(GameObject[] killers, GameObject killed) {
		killList.Enqueue(new Kill(Time.time, killers, killed));
	}

	void Update() {
		List<Kill> list = new List<Kill>();

		foreach(Kill message in killList) 
			if(Time.time - message.time < displayTime)
				list.Add(message);

		killList = new Queue<Kill>(list);
	}

	void OnGUI() {
		foreach(Kill message in killList) {
			string killers = "";

			for(int i = 0; i < message.killers.Length; i++) {
				killers += message.killers[i].name;

				if(i < message.killers.Length - 1)
					killers += "+";
			}

			GUILayout.Box(killers + " - " + message.killed.name);
		}
	}

	[System.Serializable]
	public class Kill {
		public float time;
		public GameObject[] killers;
		public GameObject killed;

		public Kill(float time, GameObject[] killers, GameObject killed) {
			this.time = time;
			this.killers = killers;
			this.killed = killed;
		}
	}
}
