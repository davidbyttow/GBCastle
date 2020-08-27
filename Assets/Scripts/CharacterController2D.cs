using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterController2D : MonoBehaviour {

	private const float groundCheckRadius = 0.2f;
	private const float ceilingCheckRadius = 0.2f;
	private const float gravity = -10;

	[Range(0, 0.3f)] [SerializeField] private float movementSmoothing;
	[SerializeField] private LayerMask groundMask;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private Transform ceilingCheck;
	[SerializeField] private float walkSpeed = 10f;
	[SerializeField] private float walkAcceleration = 40f;
	[SerializeField] private float groundDeceleration = 100f;
	[SerializeField] private float inAirSpeedScale = 0.8f;
	[SerializeField] private float jumpForce = 20f;
	[SerializeField] private float fallingGravityScale = 2f;


	private BoxCollider2D boxCollider;
	private float inAirStartTime = 0;

	private Vector3 velocity = Vector3.zero;
	private bool isGrounded = true;

	void Awake() {
		boxCollider = GetComponent<BoxCollider2D>();
	}
	void Start() {
		inAirStartTime = Time.time;
	}

	void Update() {
		if (isGrounded) {
			velocity.y = 0;
		}

		var falling = !isGrounded && velocity.y < 0;
		var gravityScale = 1f; //falling ? fallingGravityScale : jumpingGravityScale;

		if (!isGrounded && velocity.y < 0) {
			gravityScale = fallingGravityScale;
			Debug.Log("Falling");
		}

		velocity.y += Physics2D.gravity.y * gravityScale * Time.deltaTime;
	
		transform.Translate(velocity * Time.deltaTime);

		CheckGround();
	}

	void FixedUpdate() {
	}

	private void CheckGround() {
		bool wasGrounded = isGrounded;
		isGrounded = false;

		Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, groundMask);
		foreach (Collider2D hit in hits) {
			if (hit == boxCollider) {
				continue;
			}
			if (hit.isTrigger) {
				continue;
			}
			ColliderDistance2D dist = hit.Distance(boxCollider);
			if (dist.isOverlapped) {
				transform.Translate(dist.pointA - dist.pointB);
			}
			if (Vector2.Angle(dist.normal, Vector2.up) < 90 && velocity.y < 0) {
				isGrounded = true;
			}
		}

		if (wasGrounded && !isGrounded) {
			inAirStartTime = Time.time;
		}

		if (isGrounded && !wasGrounded) {
			if (Time.time - inAirStartTime > 0.2f) {
				// TODO: Sound fx
			}
		}
	}

	public void Move(float horizontalInput) {
		if (horizontalInput != 0) {
			velocity.x = Mathf.MoveTowards(velocity.x, walkSpeed * horizontalInput, walkAcceleration * Time.deltaTime);
		} else {
			velocity.x = Mathf.MoveTowards(velocity.x, 0, groundDeceleration * Time.deltaTime);
		}
	}

	public void Jump() {
		if (isGrounded) {
			Debug.Log("JUMP");
			// TODO: Sound fx
			isGrounded = false;
			velocity.y += jumpForce;
			inAirStartTime = Time.time;
		}
	}

	public void CancelJump() {
		if (!isGrounded) {
			var damping = velocity.y > 0 ? 0.5f : 1f;
			velocity.y *= damping;
		}
	}

	void OnDrawGizmosSelected() {
		if (groundCheck) {
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
		}
	}
}
