using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject MagicParticles1;
    [SerializeField] private GameObject MagicParticles2;
    public GameObject origin;

    public InputActionReference trigger;

    public AudioSource audioSource;
    public AudioClip magicSound;
    [SerializeField] public AudioClip[] audioClips;

    [SerializeField] private float distanceSum = 0f;
    [SerializeField] private int segmentCount = 0;

    Vector3 newPoint;
    Vector3 previousPoint = new Vector3(0,0,0);

    void Start()
    {

    }

    void Update()
    {
        if (trigger.action.IsPressed()) {
            if (SpellBook.GameMode == "Trace")
            {
                Trace();
            }
            else if (SpellBook.GameMode == "Swipe")
            {
                newPoint = Swipe(previousPoint);
                if (segmentCount < 2) {
                    if (segmentCount == 0 & newPoint.y > previousPoint.y) {
                        distanceSum += Mathf.Abs(newPoint.y - previousPoint.y);
                    }
                    else if (segmentCount == 1 & newPoint.y < previousPoint.y) {
                        distanceSum += Mathf.Abs(newPoint.y - previousPoint.y);
                    }
                    Debug.Log("Vert Distance: " + distanceSum);
                    if (distanceSum > 3f) {
                        distanceSum = 0;
                        audioSource.PlayOneShot(audioClips[segmentCount]);
                        segmentCount += 1;
                    }
                }
                else if (segmentCount < 4) {
                    if (segmentCount == 2 & newPoint.x < previousPoint.x) {
                        distanceSum += Mathf.Abs(newPoint.x - previousPoint.x);
                    }
                    else if (segmentCount == 3 & newPoint.x > previousPoint.x) {
                        distanceSum += Mathf.Abs(newPoint.x - previousPoint.x);
                    }
                    Debug.Log("Hori Distance: " + distanceSum);
                    if (distanceSum > 2.4f) {
                        distanceSum = 0;
                        audioSource.PlayOneShot(audioClips[segmentCount]);
                        segmentCount += 1;
                    }
                }
                else if (segmentCount == 5) {
                    distanceSum += Mathf.Abs(newPoint.x - previousPoint.x) + Mathf.Abs(newPoint.y - previousPoint.y);
                    Debug.Log("Total Distance: " + distanceSum);
                    if (distanceSum > 8f) {
                        distanceSum = 0;
                        audioSource.PlayOneShot(audioClips[segmentCount]);
                        segmentCount += 1;
                    }
                }
                else {
                    distanceSum = 0;
                    segmentCount = 0;
                    audioSource.PlayOneShot(audioClips[5]);
                }
                previousPoint = newPoint;
            }
        }
    }

    void Trace() {
        RaycastHit hit;
        if (Physics.Raycast(origin.transform.position, origin.transform.forward, out hit)){
            if (hit.transform.name == "HitPlane"){
                GameObject magic = Instantiate(MagicParticles1, hit.point, transform.rotation);
                //newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * shootPower);
                if (!audioSource.isPlaying){
                    audioSource.PlayOneShot(magicSound);
                }
            }
        } 
    }

    Vector3 Swipe(Vector3 previousPos) {
        RaycastHit hit;
        if (Physics.Raycast(origin.transform.position, origin.transform.forward, out hit)){
            if (hit.transform.name == "HitPlane"){
                GameObject magic = Instantiate(MagicParticles2, hit.point, transform.rotation);
                //newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * shootPower);
                if (!audioSource.isPlaying){
                    audioSource.PlayOneShot(magicSound);
                }
                return hit.point;
            }
        }
        return previousPos;
    }
}