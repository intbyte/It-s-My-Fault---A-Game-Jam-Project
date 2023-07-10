using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent (typeof (LineRenderer))]
public class LaserReflect : MonoBehaviour {

	public Transform laser;

	public float laserDamage;

	public GameObject beamPoint;

	public Slider energySlider;
	public Sprite energyUseImage;
	Sprite sprite;

	const int Infinity = 999;

	int maxReflections = 6;
	int currentReflections = 0;

	Vector2 startPoint, direction;
	List<Vector3> points;
	List<GameObject> beamPoints;
	int defaultRayDistance = 100;
	LineRenderer lr;

	public Image image;

	void Start () {
		points = new List<Vector3> ();
		lr = GetComponent<LineRenderer> ();
		beamPoints = new List<GameObject> ();
		for (int i = 0; i < 10; i++) {
			GameObject temp = Instantiate (beamPoint);
			temp.SetActive (false);
			beamPoints.Add (temp);
		}
		sprite = image.sprite;
	}

	float timer = 0f, laserEffectTimer = 0f, targetTime = 3f;

	bool toggle = false, laserOn = false;

	void Update () {
		Vector2 mousePosition = Input.mousePosition;
		mousePosition = Camera.main.ScreenToWorldPoint (mousePosition);

		direction = Vector2.MoveTowards (startPoint, mousePosition, Infinity);

		Vector2 dir = new Vector2 (mousePosition.x - laser.transform.position.x, mousePosition.y - laser.transform.position.y);

		laser.up = dir;

		startPoint = laser.transform.position;

		if (timer > targetTime) {
			toggle = !toggle;
			if (toggle) targetTime = 10f;
			else targetTime = 2.5f;
			timer = 0f;
			if (laserOn) {
				FindObjectOfType<AudioManager> ().Stop ("Laser");
				laserOn = false;
			} else {
				FindObjectOfType<AudioManager> ().Play ("Laser");
				laserOn = true;
			}
		} else {
			timer += Time.deltaTime;
			if (!toggle) {
				energySlider.maxValue = 20;
				energySlider.value = timer / targetTime * 20;
			}
			else {
				energySlider.maxValue = 40;
				energySlider.value = 40 - timer / targetTime * 40;
			}
		}

		if (!toggle) {
			lr.positionCount = 0;
			for (int j = 0; j < beamPoints.Count; j++) {
				beamPoints[j].SetActive (false);
			}
			return;
		}

		if (laserEffectTimer > 0.1f) {
			lr.SetWidth (Random.Range (0.15f, 0.085f), Random.Range (0.15f, 0.085f));
			laserEffectTimer = 0;
		} else laserEffectTimer += Time.deltaTime;

		RaycastHit2D hitData = Physics2D.Raycast (startPoint, (direction - startPoint).normalized);
		currentReflections = 0;

		points.Clear ();
		points.Add (startPoint);

		if (hitData) {
			Reflect (startPoint, hitData);
		} else
			points.Add (startPoint + (direction - startPoint).normalized * Infinity);

		lr.positionCount = points.Count;
		lr.SetPositions (points.ToArray ());

		if (lr.positionCount > 2) {
			beamPoint.transform.position = points[points.Count - 2];
			beamPoint.SetActive (true);
		} else beamPoint.SetActive (false);
		int i;
		for (i = 0; i < lr.positionCount - 1; i++) {
			beamPoints[i].transform.position = points[i + 1];
			beamPoints[i].SetActive (true);
		}
		for (; i < beamPoints.Count; i++) {
			beamPoints[i].SetActive (false);
		}
	}

	void Reflect (Vector2 origin, RaycastHit2D hitData) {
		if (currentReflections > maxReflections)
			return;
		points.Add (hitData.point);
		if (hitData.collider.CompareTag ("Player")) {
			hitData.collider.GetComponent<PlayerHealth> ().Damage (laserDamage);
			return;
		} else if (hitData.collider.CompareTag ("NonReflective") || hitData.collider.CompareTag ("ExitGate")) {
			return;
		} else if (hitData.collider.CompareTag ("Friend") || hitData.collider.CompareTag ("ExitGate")) {
			hitData.collider.GetComponent<UniversalHealth> ().Damage (laserDamage);
			return;
		}
		currentReflections++;

		Vector2 inDirection = (hitData.point - origin).normalized;
		Vector2 newDirection = Vector2.Reflect (inDirection, hitData.normal);

		if (hitData.collider.CompareTag ("Fragile")) {
			hitData.collider.GetComponent<UniversalHealth> ().Damage (laserDamage);
		}

		var newHitData = Physics2D.Raycast (hitData.point + (newDirection * 0.1f), newDirection * 100);

		if (newHitData) {
			Reflect (hitData.point, newHitData);
		} else {
			points.Add (hitData.point + newDirection * defaultRayDistance);
		}
	}
	bool ok = false;
	public void EnergyBarAnim () {
		if (toggle) {
			if (ok)
				image.sprite = energyUseImage;
			else image.sprite = sprite;
			ok = !ok;
		}
		else image.sprite = sprite;
	}

	public void DisableEveryBeamPoint () {
		foreach (GameObject g in beamPoints) {
			g.SetActive (false);
		}
		beamPoint.SetActive (false);
	}
}
