using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalHealth : MonoBehaviour {

	public float health = 100f;
	public float damageFactor = 1f;
	public float damageSpriteInterval = 0.2f;

	public int damageCost = 100;

	public bool gameOverOnDestroy = false;

	[Space]
	public float interval1 = 0.1f;
	public float interval2 = 0.05f;

	public Sprite damageSprite;
	public Sprite deadSprite;

	public string message;

	Sprite mainSprite;

	bool alive = true;
	bool animPlaying = false;

	SpriteRenderer sr;

	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		mainSprite = sr.sprite;
	}

	public void Damage (float damage) {
		if (!alive)
			return;
		health -= damage * Time.deltaTime * damageFactor;
		if (!animPlaying) {
			StopAllCoroutines ();
			StartCoroutine (SetHealthValue ());
		}
		if (health < 0) {
			alive = false;
			Die ();
		}
	}

	public void Die () {
		Debug.Log ("This thing died :(");
		FindObjectOfType<GameManager> ().Cost (damageCost);
		if (gameOverOnDestroy) {
			if (message != "")
				FindObjectOfType<GameManager> ().GameOver (message);
			else FindObjectOfType<GameManager> ().GameOver ();
		}
		else Destroy (this.gameObject);
	}

	IEnumerator SetHealthValue () {
		if (GetComponent<Animator> ())
			GetComponent<Animator> ().enabled = false;
		float elapsed = 0f;
		animPlaying = true;
		sr.sprite = damageSprite;
		while (elapsed < interval1) {
			elapsed += Time.deltaTime;
			yield return null;
		}
		sr.sprite = mainSprite;
		elapsed = 0f;
		while (elapsed < interval2) {
			elapsed += Time.deltaTime;
			yield return null;
		}
		animPlaying = false;
		if (GetComponent<Animator> ())
			GetComponent<Animator> ().enabled = true;
	}
}
