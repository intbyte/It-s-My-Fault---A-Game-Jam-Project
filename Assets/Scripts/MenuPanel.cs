using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPanel : MonoBehaviour {

	public GameObject menuPanel;
	public GameObject helpPanel;

	public GameObject loaderAnim;

	void Start () {
		FindObjectOfType <AudioManager> ().StopAll ();
		FindObjectOfType <AudioManager> ().Play ("Menu BG");
		loaderAnim.SetActive (true);
	}

	public void Button_Play () {
		loaderAnim.SetActive (true);
		Debug.Log ("Play button pressed");
		loaderAnim.GetComponent<LevelChanger> ().FadeToLevel (1);
	}

	public void Button_Help () {
		Debug.Log ("How To Play button pressed");
		menuPanel.SetActive (false);
		helpPanel.SetActive (true);
	}

	public void Button_Exit () {
		Debug.Log ("Exit button pressed");
		Application.Quit ();
	}

	public void Button_Back () {
		Debug.Log ("Back button pressed");
		helpPanel.SetActive (false);
		menuPanel.SetActive (true);
	}
}
