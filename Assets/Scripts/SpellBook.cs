using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SpellBook : MonoBehaviour
{

    public static string GameMode = "";  

    [SerializeField] private GameObject player;
    [SerializeField] private LayerMask teleportMask;
    [SerializeField] private InputActionReference teleportButtonPress;
    [SerializeField] private InputActionReference restartButtonPress;
    [SerializeField] private VideoClip[] spellVids;

    [SerializeField] private VideoPlayer vidPlayer;

    [SerializeField] private GameObject[] MagicWaypoints;

    void Start() {
        teleportButtonPress.action.performed += ChangeSpell;
        restartButtonPress.action.performed += Restart;
        GameMode = "Trace";
        vidPlayer.clip = spellVids[0];  
    }

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
    }

    void Restart(InputAction.CallbackContext __) {
        string currentSceneName = SceneManager.GetActiveScene().name;
	    SceneManager.LoadScene(currentSceneName);
    }

    void ChangeSpell(InputAction.CallbackContext __) {
        if (GameMode == "Trace") {
            GameMode = "Swipe";
            vidPlayer.clip = spellVids[1]; 
            //MagicWaypoints[1].SetActive(true);
            MagicWaypoints[0].SetActive(false);
        }
        else if (GameMode == "Swipe") { 
            GameMode = "Trace";
            vidPlayer.clip = spellVids[0];
            //MagicWaypoints[1].SetActive(true);
            MagicWaypoints[0].SetActive(true);
        }
    }
}