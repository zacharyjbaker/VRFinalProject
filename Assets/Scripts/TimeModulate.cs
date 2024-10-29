using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeModulate : MonoBehaviour {
	// Here is some code you might need to accomplish this task.

	// Create a public reference to the hand controller object 
	// so you can manually connect them
	[SerializeField]
	private GameObject rightHandRef;
	private Vector3 lastPos;

	void Update() {
		float distance = Vector3.Distance(rightHandRef.transform.position, lastPos);
		
		// Your code to check what the time scale should be ;)
		// Time.timeScale = 0.5f; // <- This will make the game move at half speed
		lastPos = rightHandRef.transform.position;
		//Debug.Log(Time.timeScale);
		if (distance <= 0.002) {
			if (Time.timeScale >= 0.50f) {Time.timeScale -= 0.07f;}
			else if (Time.timeScale >= 0.05f) {Time.timeScale -= 0.03f;}
		}
		if ((Time.timeScale += distance * 1.5f) <= 1.0f) {
			Time.timeScale += distance * 1.5f;
		}
		else {
			Time.timeScale = 1.0f;
		}
	}
}
