using UnityEngine;
using System.Collections.Generic;

public class BusterBall : MonoBehaviour {
	private const float SIXTEENTH_PI = Mathf.PI * .06125f;


	public float movementSpeed = 1;
	public float movementIncrease = .5f;
	public float maxMovementSpeed = 60f;

	public Vector2 velocityDirection;

	new private CircleCollider2D collider2D;
	private Rigidbody2D rigidBody;
	private List<Collider2D> ignoredColliders = new List<Collider2D>();

	// Use this for initialization
	void Start () {
		collider2D = GetComponent<CircleCollider2D> ();
		rigidBody = GetComponent<Rigidbody2D> ();

		RandomDirection ();
	}

	private void RandomDirection() {
		float randomAngle = Random.Range (0f, 1f) * Mathf.PI * 2;
		velocityDirection = new Vector2 (Mathf.Cos (randomAngle), Mathf.Sin (randomAngle));
		rigidBody.velocity = velocityDirection * movementSpeed;
	}

	private void TowardsCenter() {
		float newAngle = Mathf.Atan2 (-transform.position.y, -transform.position.x) + Random.Range (-SIXTEENTH_PI, SIXTEENTH_PI);
		velocityDirection = new Vector2 (Mathf.Cos (newAngle), Mathf.Sin (newAngle));
		rigidBody.velocity = velocityDirection * movementSpeed;
	}

	void FixedUpdate() {
		// When not moving at all or sliding against a wall, change direction towards the center
		if (Mathf.Abs(rigidBody.velocity.x) <= 0.001f || Mathf.Abs(rigidBody.velocity.y) <= 0.001f)
			TowardsCenter ();

		Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll (transform.position, collider2D.radius * .5f);
		if (nearbyColliders.Length > 0)
			for (int i = 0; i < ignoredColliders.Count; i++) {
				foreach (Collider2D nearby in nearbyColliders)
					if (nearby == ignoredColliders [i]) {
						ignoredColliders.RemoveAt(i--);
						Physics2D.IgnoreCollision (nearby, collider2D, false);
						break;
					}
			}
	}

	// TODO: Test trigger enter instead to prevent player standing on the ball
	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.layer == 8) {
			// Wall collision
			movementSpeed = Mathf.Clamp (movementSpeed + movementIncrease, 1, maxMovementSpeed);
		} else {
			// Player collision
			BallBuster playerInfo = col.gameObject.GetComponent<BallBuster>();
			movementSpeed = Mathf.Clamp (playerInfo.busting ? (movementSpeed + movementIncrease) : movementSpeed / 2, 1, maxMovementSpeed);
			Physics2D.IgnoreCollision (collider2D, col.collider);
			ignoredColliders.Add (col.collider);
		}

		velocityDirection = Vector2.Reflect (velocityDirection, col.contacts [0].normal).normalized;
		rigidBody.velocity = velocityDirection * movementSpeed;
	}
}
