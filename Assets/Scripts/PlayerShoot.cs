using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject MagicParticles;
    public GameObject origin;

    public InputActionReference trigger;

    public AudioSource audioSource;

    void Start()
    {

    }

    void Update()
    {
        if (trigger.action.IsPressed()) {
            Shoot();
        }
    }

    void Shoot() {
        RaycastHit hit;
        if (Physics.Raycast(origin.transform.position, origin.transform.forward, out hit)){
            if (hit.transform.name == "HitPlane"){
                GameObject magic = Instantiate(MagicParticles, hit.point, transform.rotation);
                //newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * shootPower);
                if (!audioSource.isPlaying){
                    audioSource.Play();
                }
            }

        }

        
    }
}