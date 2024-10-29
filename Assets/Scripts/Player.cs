using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
	public AudioSource audioSource;
    public AudioClip deathSound;
    private void OnTriggerEnter(Collider other) {
		//Debug.Log(other.gameObject);
		if (other.gameObject.tag == "Damage") {
			//Debug.Log("Reset");
			audioSource.PlayOneShot(audioSource.clip);
			Destroy(other.gameObject);
	      	string currentSceneName = SceneManager.GetActiveScene().name;
	      	SceneManager.LoadScene(currentSceneName);
	  	}
	}
}
