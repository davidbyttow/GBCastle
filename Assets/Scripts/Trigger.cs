using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Trigger : MonoBehaviour {

	[SerializeField] private UnityEvent onTrigger;
	[SerializeField] private bool once = true;
	[SerializeField] private float waitTime = 0;
	[SerializeField] private bool canLeaveTrigger = false;

	private bool triggered = false;
	private bool inTrigger = false;
	private float elapsed = 0;

	void Update() {
		if (!triggered && inTrigger) {
			elapsed += Time.deltaTime;
			if (elapsed >= waitTime) {
				triggered = true;
				onTrigger.Invoke();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (once && triggered) {
			return;
		}
		if (other.TryGetComponent(out Player player)) {
			if (!player.isDead) {
				elapsed = 0;
				inTrigger = true;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.TryGetComponent(out Player player)) {
			if (!canLeaveTrigger) {
				inTrigger = false;
			}
		}
	}
}