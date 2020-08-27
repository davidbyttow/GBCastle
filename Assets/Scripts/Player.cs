using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class Player : MonoBehaviour {

	[SerializeField] private ParticleSystem deathEffect;

	private SpriteRenderer sprite;
	private CharacterController2D controller;
	public bool isDead { get; private set; }

	void Awake() {
		sprite = GetComponent<SpriteRenderer>();
		controller = GetComponent<CharacterController2D>();
	}

	void Start() {
	}

	void Update() {
		bool jump = Input.GetButtonDown("Jump");
		if (jump) {
			controller.Jump();
		}
		if (Input.GetButtonUp("Jump")) {
			controller.CancelJump();
		}

		float horizontalInput = Input.GetAxisRaw("Horizontal");
		controller.Move(horizontalInput);
	}

	void FixedUpdate() {
	}

	public void Die() {
		if (isDead) {
			return;
		}

		// TODO: Camera shake and sound FX

		isDead = true;

		if (deathEffect) {
			var effect = Instantiate(deathEffect, transform.position, Quaternion.Euler(0, -90, 0));
			effect.Play();
		}

		gameObject.SetActive(false);
		GameManager.global.QueueRestart();
	}
}
