using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Manages your health and sends messages when certain health events happen
/// 
/// - To send a message to a GameObject you can use SendMessage or call them directly -
/// 
/// targetGameObject.SendMessage("Heal", new HealthEvent(gameObject, amount));
/// targetGameObject.SendMessage("Damage", new HealthEvent(gameObject, amount));
/// 
/// targetHealth.Heal(new HealthEvent(gameObject, amount));
/// targetHealth.Damage(new HealthEvent(gameObject, amount));
/// 
/// 
/// - These functions you can add to your script to receive the events -
/// 
/// void OnHealed(HealthEvent event) - Triggered on the object whose health has gone up
/// void OnCausedHeal(HealthEvent event) - Triggered on the object who has caused another object's health to go down
/// 
/// void OnDamaged(HealthEvent event) - Triggered on the object whose health has gone down
/// void OnCausedDamage(HealthEvent event) - Triggered on the object who has caused another object's health to go up
/// 
/// void OnDeath(HealthEvent event) - Triggered on the object whose health has gone on or below zero
/// void OnCausedDeath(HealthEvent event) - Triggered on the object who has caused another object's health to be at or below zero
/// </summary>
[AddComponentMenu("Health/Health")]
public class Health : MonoBehaviour {
	public float maxHealth = 100;
	public float health = 100;

	/// <summary>
	/// Disables health being changed from the Heal, Damage and ChangeHealth functions
	/// </summary>
	public bool disableChanges = false;

	public event Action<float> OnSetHealth;
	public event Action<HealthEvent> OnHealed;
	public event Action<GameObject, HealthEvent> OnCausedHeal;
	public event Action<HealthEvent> OnDamaged;
	public event Action<GameObject, HealthEvent> OnCausedDamage;
	public event Action<HealthEvent> OnDeath;
	public event Action<GameObject, HealthEvent> OnCausedDeath;

	protected virtual void Awake() {
		OnHealed += SendOnHealed;
		OnCausedHeal += SendOnCausedHeal;
		OnDamaged += SendOnDamaged;
		OnCausedDamage += SendOnCausedDamage;
		OnDeath += SendOnDeath;
		OnCausedDeath += SendOnCausedDeath;
	}

	void Start() {
		if(OnSetHealth != null) OnSetHealth(health);
	}

	/// <summary>
	/// Sets the health without causing messages to be sent
	/// </summary>
	/// <param name="amount">Health amount</param>
	public void SetHealth(float amount) {
		health = amount;

		if(OnSetHealth != null) OnSetHealth(health);
	}

	public float ChangeHealth(HealthEvent amount) {
		
    if(!disableChanges) {
			float healthChange = health;

			health = Mathf.Clamp(health + amount.amount, 0, maxHealth);
			if(OnSetHealth != null) OnSetHealth(health);

			healthChange = health - healthChange;

			HealthEvent received = new HealthEvent(amount.gameObject, healthChange);
			HealthEvent caused = new HealthEvent(gameObject, healthChange);

			if(health == 0) {
				if(healthChange < 0) {
					if(OnDeath != null)
						OnDeath(received);

					if(amount.gameObject != null && OnCausedDeath != null)
						OnCausedDeath(amount.gameObject, caused);
				}
			}
			else {
				if(healthChange > 0) {
					if(OnHealed != null)
						OnHealed(received);

					if(amount.gameObject != null && OnCausedHeal != null)
						OnCausedHeal(amount.gameObject, caused);

				}
				else if(healthChange < 0) {
					if(OnDamaged != null)
						OnDamaged(received);

					if(amount.gameObject != null && OnCausedDamage != null)
						OnCausedDamage(amount.gameObject, caused);
				}
			}
		}

		return health;
	}

	/// <summary>
	/// Subtracts health
	/// </summary>
	/// <param name="health"></param>
	public void Damage(HealthEvent health) {
		if(!disableChanges) {
			health.amount = -Mathf.Abs(health.amount);
			ChangeHealth(health);
		}
	}

	/// <summary>
	/// Adds health
	/// </summary>
	/// <param name="health"></param>
	public void Heal(HealthEvent health) {
		if(!disableChanges) {
			health.amount = Mathf.Abs(health.amount);
			ChangeHealth(health);
		}
	}

	void SendOnHealed(HealthEvent health) {
		SendMessage("OnHealed", health, SendMessageOptions.DontRequireReceiver);
	}
	void SendOnCausedHeal(GameObject cause, HealthEvent health) {
		cause.SendMessage("OnCausedHeal", health, SendMessageOptions.DontRequireReceiver);
	}

	void SendOnDamaged(HealthEvent health) {
		SendMessage("OnDamaged", health, SendMessageOptions.DontRequireReceiver);
	}
	void SendOnCausedDamage(GameObject cause, HealthEvent health) {
		cause.SendMessage("OnCausedDamage", health, SendMessageOptions.DontRequireReceiver);
	}

	void SendOnDeath(HealthEvent health) {
		SendMessage("OnDeath", health, SendMessageOptions.DontRequireReceiver);
	}
	void SendOnCausedDeath(GameObject cause, HealthEvent health) {
		cause.SendMessage("OnCausedDeath", health, SendMessageOptions.DontRequireReceiver);
	}

	/// <summary>
	/// Resets health
	/// </summary>
	public void Reset() {
		ChangeHealth(new HealthEvent(null, maxHealth));
	}

	/// <summary>
	/// If you have a respawn script you can send an OnRespawn message to this GameObject to reset the health
	/// </summary>
	public void OnRespawn() {
		Reset();
	}
}

public struct HealthEvent {
	/// <summary>
	/// The GameObject that caused the health event to happen
	/// </summary>
	public GameObject gameObject;

	/// <summary>
	/// The amount of health to change
	/// </summary>
	public float amount;

	public HealthEvent(GameObject gameObject, float amount) {
		this.gameObject = gameObject;
		this.amount = amount;
	}
}