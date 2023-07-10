using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float movementSpeed = 5f;

	public Rigidbody2D rb;

	public Animator animator;

	public Transform laser;

	Vector2 movement;

	void Update () {
		movement.x = Input.GetAxisRaw ("Horizontal");
		movement.y = Input.GetAxisRaw ("Vertical");

		animator.SetFloat ("Horizontal", movement.x);
		animator.SetFloat ("Vertical", movement.y);
		animator.SetFloat ("Speed", movement.sqrMagnitude);
	}

	void FixedUpdate () {
		rb.MovePosition (rb.position + movement * movementSpeed * Time.fixedDeltaTime);
	}
}
