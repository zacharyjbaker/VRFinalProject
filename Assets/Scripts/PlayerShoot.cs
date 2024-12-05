using System.Collections;
using System.Collections.Generic;
using System.Net.Cache;

//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject MagicParticles1;
    [SerializeField] private GameObject MagicParticles2;
    [SerializeField] private GameObject BulletTemplate;

    [SerializeField] private GameObject StaffMagic1;

    [SerializeField] private GameObject ProjMagic1;

    [SerializeField] private GameObject ChargedEmitter;

    [SerializeField] private GameObject HitPlane;

    public GameObject origin;
    public GameObject fireOrigin;

    public float shootPower;

    public InputActionReference trigger;

    public InputActionReference cast;

    public AudioSource audioSource;
    public AudioClip magicSound;
    public AudioClip fireSound;
    [SerializeField] public AudioClip[] audioClips;

    [SerializeField] private float distanceSum = 0f;
    [SerializeField] private int segmentCount = 0;

    private int magicType = 1;
    
    private bool projectileCharged = false;

    private int charges = 5;

    Vector3 newPoint;
    Vector3 previousPoint = new Vector3(0,0,0);

    void Start() {
        cast.action.performed += CastSpell;
    }

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.magenta);
        if (trigger.action.IsPressed()) {
            if (SpellBook.GameMode == "Trace")
            {
                magicType = 1;
                Trace();
            }
            else if (SpellBook.GameMode == "Swipe")
            {
                magicType = 2;
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
                magicType = 3;
                Shoot();
            }
        }
    }

    public void Charge() {
        charges = 5;
        projectileCharged = true;
        ChargedEmitter.SetActive(true);
        //SpellBook.MagicWaypoints[1].SetActive(false);
        //SpellBook.MagicWaypoints[0].SetActive(false);
    }

    private void CastSpell(InputAction.CallbackContext __) {
        Debug.Log("Cast");
        if (projectileCharged == true) {
            Debug.Log("Successful Cast");
            if (magicType == 1) {
                charges -= 1;
                if (charges == 0) {
                    projectileCharged = false;
                    ChargedEmitter.SetActive(false);
                }
                GameObject projectile = Instantiate(ProjMagic1, fireOrigin.transform.position += fireOrigin.transform.forward * 1.5f , fireOrigin.transform.rotation);
                projectile.GetComponent<Rigidbody>().AddForce(fireOrigin.transform.forward * shootPower * 2.5f);
                //SpellBook.MagicWaypoints[1].SetActive(false);
                //SpellBook.MagicWaypoints[0].SetActive(true);
            }
            if (!audioSource.isPlaying) {
                audioSource.PlayOneShot(magicSound);
            }
        }
    }

    void Trace() {
        RaycastHit hit;
        if (Physics.Raycast(origin.transform.position, origin.transform.forward, out hit)){
            Debug.Log(hit.transform.name);
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