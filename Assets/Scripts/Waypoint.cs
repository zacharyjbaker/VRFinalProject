using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        
        if (other.tag == "Magic"){
            this.transform.parent.GetComponent<WaypointManager>().ProcessCollision(other, this.gameObject);
        }
    }
}
