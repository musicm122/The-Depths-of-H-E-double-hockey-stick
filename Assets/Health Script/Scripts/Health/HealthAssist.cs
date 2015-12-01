using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Health/Health Assist")]
public class HealthAssist : Health {

	public float maxAssistTime = 1;

	/// <summary>
	/// Upon the death of this object it will send an OnAssist message using SendMessage to each object that assisted
	/// and will also use this object as the parameter
	/// </summary>
	public event Action<GameObject> OnAssist;

	/// <summary>
	/// List of objects that assisted in hurting this object
	/// </summary>
	Dictionary<GameObject, AssistTimestamp> assists = new Dictionary<GameObject, AssistTimestamp>();

	protected override void Awake() {
		base.Awake();

		OnAssist += SendOnAssist;

		OnDamaged += healthEvent => {
			if(healthEvent.gameObject)
				AddAssist(healthEvent.gameObject);
		};

		OnDeath += healthEvent => {
			if(healthEvent.gameObject)
				AddAssist(healthEvent.gameObject);
		};

		OnDeath += healthEvent => {
			if(OnAssist != null)
				OnAssist(gameObject);
		};
	}

	public void AddAssist(GameObject assist) {
		AssistTimestamp timestamp;

		RemoveNullAndLateAssists();

		if(assists.TryGetValue(assist, out timestamp))
			timestamp.time = Time.time;
		else {
			timestamp = new AssistTimestamp(assist, Time.time);
			assists.Add(assist, timestamp);
		}
	}

	void RemoveNullAndLateAssists() {
		AssistTimestamp[] timestamps = new AssistTimestamp[assists.Values.Count];

		assists.Values.CopyTo(timestamps, 0);

		for(int i = 0; i < assists.Count; i++) {
			AssistTimestamp timestamp = timestamps[i];

			if(!timestamp.gameObject || Time.time - timestamp.time > maxAssistTime)
				assists.Remove(timestamp.gameObject);
		}
	}

	public void ClearAssists() {
		assists.Clear();
	}

	public GameObject[] GetAssists() {
		RemoveNullAndLateAssists();

		List<GameObject> list = new List<GameObject>();

		foreach(KeyValuePair<GameObject, AssistTimestamp> a in assists)
			list.Add(a.Key);

		return list.ToArray();
	}

	void SendOnAssist(GameObject go) {
		go.SendMessage("OnAssist", gameObject, SendMessageOptions.DontRequireReceiver);
	}
}

[Serializable]
public class AssistTimestamp {
	public GameObject gameObject;
	public float time;

	public AssistTimestamp(GameObject gameObject, float time) {
		this.gameObject = gameObject;
		this.time = time;
	}
}
