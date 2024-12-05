using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{

    [SerializeField] public GameObject[] waypoints;
    [SerializeField] public AudioClip[] audioClips;

    [SerializeField] private GameObject player;
    public AudioSource audioSource;
    public int magicCount = 1;
    

    // Update is called once per frame
    public void ProcessCollision(Collider other, GameObject waypoint) {
        Debug.Log(magicCount);
        if (other.tag == "Magic"){
            if (waypoint.name == "W" + magicCount) {
                Debug.Log("W" + magicCount);
                audioSource.PlayOneShot(audioClips[magicCount-1]);
                waypoint.SetActive(false);
                magicCount++;
                if (magicCount == 7) {
                    magicCount = 1;
                    player.GetComponent<PlayerShoot>().Charge();
                }
            }
            else {
                audioSource.PlayOneShot(audioClips[6]);
                Debug.Log("Wrong Order");
                magicCount = 1;
            }
        }
    }
            
}
