using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {

	public Animator animator;

	int levelToLoad;

	public void FadeToLevel (int levelInd) {
		levelToLoad = levelInd;
		animator.SetTrigger ("FadeOut");
	}

	public void OnFadeComplete () {
		SceneManager.LoadScene (levelToLoad);
	}

	public void Invisible () {
		this.gameObject.SetActive (false);
	}
}
