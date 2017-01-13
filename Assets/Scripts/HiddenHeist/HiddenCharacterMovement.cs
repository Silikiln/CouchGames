using UnityEngine;
using System.Collections;

public class HiddenCharacterMovement : MonoBehaviour {
	public bool playerControlled = false;

	public float movementMaxDelay = 10;
	public float movementMinDelay = .5f;

	private Rigidbody2D rigidBody;
	private bool moving = false;
	private float movementTimer;
	private Coroutine randomMovement;
	private BasicMovement movementHandler;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D> ();
		movementTimer = Random.Range (movementMinDelay, movementMaxDelay);
		GetComponent<SpriteRenderer> ().color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f));

		movementHandler = GetComponent<BasicMovement> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!playerControlled && !moving && (movementTimer -= Time.deltaTime) <= 0)
			randomMovement = StartCoroutine (RandomMovement ());
	}

	public IEnumerator RandomMovement() {
		if (moving)
			yield break;
		moving = true;

		/*
		float targetAngle = Random.Range (0f, 1f) * Mathf.PI * 2;
		Vector3 offsetPosition = new Vector3 (Mathf.Cos (targetAngle), Mathf.Sin (targetAngle));
		Vector3 targetPosition = transform.position + offsetPosition;
		Debug.Log (transform.position + " -> " + targetPosition);
		*/

		Vector3 targetPosition = new Vector3 (Random.Range (-11f, 11f), Random.Range (-6f, 6f));
		Vector3 offsetPosition = targetPosition - transform.position;

		Debug.DrawLine (transform.position, targetPosition, Color.black);

		Vector3 speed = offsetPosition.normalized * movementHandler.movementSpeed;
		float distance = offsetPosition.magnitude;

		while (distance > 0) {
			yield return new WaitForFixedUpdate();
			rigidBody.velocity = speed;
			distance -= Time.fixedDeltaTime * speed.magnitude;
		}

		moving = false;
		movementTimer = Random.Range (movementMinDelay, movementMaxDelay);
		rigidBody.velocity = Vector2.zero;
	}
}
