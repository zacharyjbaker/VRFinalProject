using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject MagicParticles1;
    [SerializeField] private GameObject MagicParticles2;
    [SerializeField] private GameObject BulletTemplate;
    public GameObject origin;
    public GameObject fireOrigin;

    public float shootPower;

    public InputActionReference trigger;

    public AudioSource audioSource;
    public AudioClip magicSound;
    public AudioClip fireSound;
    [SerializeField] public AudioClip[] audioClips;

    [SerializeField] private float distanceSum = 0f;
    [SerializeField] private int segmentCount = 0;

    Vector3 newPoint;
    Vector3 previousPoint = new Vector3(0,0,0);

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.magenta);
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
                    if (distanceSum > 2.2f) {
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
                    if (distanceSum > 2.2f) {
                        distanceSum = 0;
                        audioSource.PlayOneShot(audioClips[segmentCount]);
                        segmentCount += 1;
                    }
                }
                else if (segmentCount == 4) {
                    distanceSum += Mathf.Abs(newPoint.x - previousPoint.x) + Mathf.Abs(newPoint.y - previousPoint.y);
                    Debug.Log("Total Distance: " + distanceSum);
                    if (distanceSum > 6f) {
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
            else 
            {
                Shoot();
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

    void Shoot() {
        GameObject newBullet = Instantiate(BulletTemplate, fireOrigin.transform.position += fireOrigin.transform.forward * 1.5f , fireOrigin.transform.rotation);
        newBullet.GetComponent<Rigidbody>().AddForce(fireOrigin.transform.forward * shootPower);
        
        if (!audioSource.isPlaying){
            audioSource.PlayOneShot(fireSound);
        }
        //GameObject newBullet = Instantiate(BulletTemplate, transform.position += transform.forward * 2.5f, transform.rotation);
        //newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * shootPower);
        //audioSource.PlayOneShot(audioSource.clip);
    }
}