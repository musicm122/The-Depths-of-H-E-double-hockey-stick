using UnityEngine;
using System.Collections;

/// <summary>
/// Stores a GameObject that acts as an owner
/// Also re-routes health events to the owner which "should" have a Health component
/// </summary>
public class Owner : MonoBehaviour {

	public GameObject owner;

	public void SetOwner(GameObject owner) {
		this.owner = owner;
	}

	public void Heal(HealthEvent health) {
		owner.SendMessage("Heal", health, SendMessageOptions.DontRequireReceiver);
	}

	public void Damage(HealthEvent health) {
		owner.SendMessage("Damage", health, SendMessageOptions.DontRequireReceiver);
	}
}
