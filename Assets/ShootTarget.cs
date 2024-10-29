using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTarget : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Target Hit");
        if (other.tag == "Bullet"){
                
                Destroy(other);
                if (!audioSource.isPlaying) {
                    audioSource.Play();
                }
                StartCoroutine(destroy());  
        }
    }
}

