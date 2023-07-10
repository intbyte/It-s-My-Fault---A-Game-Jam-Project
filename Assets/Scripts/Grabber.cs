using UnityEngine;

public class Grabber : MonoBehaviour {

	public int keys = 0;

	public int requiredKeys = 1;

	public SpriteRenderer gate;

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.CompareTag ("Key")) {
			collider.gameObject.SetActive (false);
			keys++;
			if (keys == requiredKeys)
				gate.color = Color.green;
		} else if (collider.CompareTag ("ExitGate")) {
			if (keys == requiredKeys) {
				StartCoroutine (Wait ());
			}
		}
	}

	void OnTriggerStay2D (Collider2D collider) {
		if (collider.CompareTag ("Harmful")) {
			GetComponent<PlayerHealth> ().Damage (20f);
		}
	}

	System.Collections.IEnumerator Wait () {
		yield return new WaitForSeconds (0.5f);
		FindObjectOfType<GameManager> ().LevelFinished ();
	}
}
