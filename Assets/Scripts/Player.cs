using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
	[SerializeField]
    private GameObject player;

    [SerializeField]
    private LayerMask teleportMask;
    [SerializeField]
    private InputActionReference teleportButtonPress;
    [SerializeField]
    private InputActionReference restartButtonPress;
	public AudioSource audioSource;
    public AudioClip deathSound;
	private int currentTower = 0;

/*
	void DoRaycast(InputAction.CallbackContext __) {
        RaycastHit hit;
        bool didHit = Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, teleportMask);
        // Does the ray intersect any objects excluding the player layer
        // Parameters: position to start the ray, direction to project the ray, output for raycast, limit of ray length, and layer mask
        if (didHit) {
            // The object we hit will be in the collider property of our hit object.
            // We can get the name of that object by accessing it's Game Object then the name property
            Debug.Log(hit.collider.gameObject.name);
            
                    // Don't forget to attach the player origin in the editor!
            player.transform.position = hit.point + new Vector3(0,0.4f,0);
        }
    } */

    void Restart(InputAction.CallbackContext __) {
        string currentSceneName = SceneManager.GetActiveScene().name;
	    SceneManager.LoadScene(currentSceneName);
    }

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
