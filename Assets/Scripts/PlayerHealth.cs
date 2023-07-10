using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	public GameManager manager;

	public float health;

	public Slider healthSlider;

	public GameObject damageEffect;

	public float interval1 = 0.1f;
	public float interval2 = 0.05f;

	bool animPlaying = false;

	bool alive = true;

	PlayerMovement pm;

	public void Start () {
		health = 100f;
		healthSlider.value = health;
		pm = GetComponent<PlayerMovement> ();
		healthSlider.maxValue = 15f;
		healthSlider.wholeNumbers = true;
	}

	public void Damage (float damageValue) {
		if (!alive)
			return;
		health -= damageValue * Time.deltaTime;
		healthSlider.value = 15f / 100f * health;
		if (!animPlaying) {
			StartCoroutine (DamageEffect ());
		}
		if (health < 0)
			Die ();
	}

	public void Die () {
		alive = false;
		Debug.Log ("You died");
		// play death animation
		manager.GameOver ("You got killed by you :(");
		return;
	}

	IEnumerator DamageEffect () {
		animPlaying = true;
		float elapsed = 0f;
		pm.enabled = false;
		damageEffect.SetActive (true);
		while (elapsed < interval1) {
			elapsed += Time.deltaTime;
			yield return null;
		}
		pm.enabled = true;
		elapsed = 0f;
		damageEffect.SetActive (false);
		while (elapsed < interval2) {
			elapsed += Time.deltaTime;
			yield return null;
		}
		animPlaying = false;
	}
}
