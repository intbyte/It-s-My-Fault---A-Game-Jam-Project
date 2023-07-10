using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	public Transform targetToFollow;

	public Vector3 offset;

	Camera mainCamera;

	public float smoothSpeed = 0.02f;

	void Start () {
		mainCamera = GameObject.Find ("Main Camera").GetComponent<Camera> ();
	}

	void FixedUpdate () {
		Vector2 desiredPosition = targetToFollow.position + offset;
		Vector2 smoothingOperation = Vector2.Lerp (transform.position, desiredPosition, smoothSpeed);
		transform.position = new Vector3 (smoothingOperation.x, smoothingOperation.y, -10);
	}

	public IEnumerator Shake (float duration,float magnitude) {
		Vector3 originalPos = mainCamera.transform.localPosition;

		float elasped = 0.0f;

		while (elasped < duration) {
			float x = Random.Range (-1f, 0f) * magnitude;
			float y = Random.Range (-1f, 0f) * magnitude;

			mainCamera.transform.localPosition = new Vector3 (x, y, originalPos.z);

			elasped += Time.deltaTime;

			yield return null;
		}
		mainCamera.transform.localPosition = originalPos;
	}
}
