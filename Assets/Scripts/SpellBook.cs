using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SpellBook : MonoBehaviour
{

    public static string GameMode = "";  

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerController;
    [SerializeField] private GameObject board;
    [SerializeField] private GameObject teleportParticle;
    [SerializeField] private LayerMask teleportMask;
    public InputActionReference spellButtonPress;
    
    public InputActionReference teleportButtonPress;
    [SerializeField] private VideoClip[] spellVids;
    [SerializeField] private GameObject[] towers;

    [SerializeField] private VideoPlayer vidPlayer;

    [SerializeField] private GameObject[] MagicWaypoints;
    private int currentTower = 0;
    private PlayerShoot pController;

    private UnityEngine.Vector3 boardPos1;
    private UnityEngine.Vector3 boardPos2;

    void Start() {
        spellButtonPress.action.performed += ChangeSpell;
        teleportButtonPress.action.performed += MoveToTower;
        
        GameMode = "Trace";
        vidPlayer.clip = spellVids[0];
        pController = playerController.GetComponent<PlayerShoot>();
        pController.ChargedEmitter = 0;
        boardPos1 = new UnityEngine.Vector3(-5.84f, 4.84f, 5.59f);
        boardPos2 = new UnityEngine.Vector3(11.46f, 4.84f, 5.59f);
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
            player.transform.position = hit.point + new UnityEngine.Vector3(0,0.4f,0);
        }
    }

    void MoveToTower(InputAction.CallbackContext __) {
		if (currentTower == 0) {
            currentTower = 1;
            board.transform.position = boardPos2;
        }
		else if (currentTower == 1) {
            currentTower = 0;
            board.transform.position = boardPos1;
        }
		player.transform.position = towers[currentTower].transform.position;
        Instantiate(teleportParticle, player.transform.position + new UnityEngine.Vector3(0f, -2f, 0f), teleportParticle.transform.rotation);
	}

    

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.8f);
    }

    void ChangeSpell(InputAction.CallbackContext __) {
        if (GameMode == "Trace" && pController.projectileCharged == false) { //Switch from trace to swipe
            GameMode = "Shoot";
            vidPlayer.clip = spellVids[2];
            //MagicWaypoints[1].SetActive(true);
            MagicWaypoints[1].SetActive(true);
            MagicWaypoints[0].SetActive(false);
            pController.ChargedEmitter = 1;
            pController.Board.SetActive(true);
            //StartCoroutine(wait());
        }
        else if (GameMode == "Shoot" && pController.projectileCharged == false) {//Switch from trace to swipe
            GameMode = "Swipe";
            vidPlayer.clip = spellVids[1]; 
            MagicWaypoints[1].SetActive(false);
            MagicWaypoints[0].SetActive(false);
            pController.ChargedEmitter = 2;
            pController.Board.SetActive(true);
            //StartCoroutine(wait());
        }
        else if (GameMode == "Swipe" && pController.projectileCharged == false) { //Switch from swipe to shoot
            GameMode = "Trace";
            vidPlayer.clip = spellVids[0]; 
            MagicWaypoints[1].SetActive(false);
            MagicWaypoints[0].SetActive(true);
            pController.ChargedEmitter = 0;
            pController.Board.SetActive(true);
            
        }
        
        
        
    }
}