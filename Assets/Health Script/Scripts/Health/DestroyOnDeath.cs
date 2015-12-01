using UnityEngine;
using System.Collections;

[AddComponentMenu("Health/Destroy On Death")]
public class DestroyOnDeath : MonoBehaviour {

	/// <summary>
	/// The object that will be destroyed on death
	/// </summary>
	public GameObject destroyThis = null;

	void Reset() {
		destroyThis = gameObject;
	}

	public void OnDeath(HealthEvent health) {
		Destroy(destroyThis);
	}
}
