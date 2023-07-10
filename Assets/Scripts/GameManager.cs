using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public GameObject gameOverPanel;
	public GameObject levelFinishedPanel;
	public GameObject loaderAnim;
	
	public PlayerMovement playerMovement;
	
	public LineRenderer lineRenderer;

	public LaserReflect laserReflect;

	public Text countDownText;
	public Text costText;
	public Text gameOverDescription;

	public int maxCostAllowed = 1000;

	void Start () {
		StartCoroutine (StartCount ());
		int index = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().buildIndex;
		if (index > 1)
			return;
		FindObjectOfType<AudioManager> ().StopAll ();
		FindObjectOfType<AudioManager> ().Play ("GP BG");
	}

	System.Collections.IEnumerator StartCount () {
		playerMovement.enabled = false;
		lineRenderer.enabled = false;
		laserReflect.enabled = false;
		countDownText.transform.parent.gameObject.SetActive (true);
		for (int i = 0; i < 3; i++) {
			countDownText.text = (4 - (i + 1)).ToString ();
			yield return new WaitForSeconds (1);
		}
		countDownText.transform.parent.gameObject.SetActive (false);
		playerMovement.enabled = true;
		lineRenderer.enabled = true;
		laserReflect.enabled = true;
	}

	public void GameOver () {
		playerMovement.enabled = false;
		lineRenderer.enabled = false;
		laserReflect.DisableEveryBeamPoint ();
		laserReflect.enabled = false;

		FindObjectOfType<AudioManager> ().Stop ("Laser");

		gameOverPanel.SetActive (true);
	}

	public void GameOver (string message) {
		playerMovement.enabled = false;
		lineRenderer.enabled = false;
		laserReflect.DisableEveryBeamPoint ();
		laserReflect.enabled = false;

		FindObjectOfType<AudioManager> ().Stop ("Laser");

		gameOverPanel.SetActive (true);
		gameOverDescription.text = message;
	}

	public void Button_Menu () {
		// back to main menu or scene index 0
		loaderAnim.SetActive (true);
		loaderAnim.GetComponent<LevelChanger> ().FadeToLevel (0);
	}

	public void Button_Next () {
		loaderAnim.SetActive (true);
		int index = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().buildIndex;
		loaderAnim.GetComponent<LevelChanger> ().FadeToLevel (index + 1);
	}

	public void LevelFinished () {
		playerMovement.enabled = false;
		lineRenderer.enabled = false;
		laserReflect.DisableEveryBeamPoint ();
		laserReflect.enabled = false;

		FindObjectOfType<AudioManager> ().Stop ("Laser");
		
		levelFinishedPanel.SetActive (true);
	}

	int totalCost = 0;

	public void Cost (int cost) {
		totalCost += cost;
		costText.text = "Damage: " + totalCost + "$";
		if (totalCost > maxCostAllowed)
			GameOver ("You have damaged things worth more than " + maxCostAllowed + "$");
	}
}
