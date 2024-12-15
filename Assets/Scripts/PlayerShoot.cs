using System.Collections;
using System.Collections.Generic;
using System.Net.Cache;

//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject MagicParticles1;
    [SerializeField] private GameObject MagicParticles2;
    [SerializeField] private GameObject MagicParticles3;
    [SerializeField] private GameObject MagicParticles4;
    [SerializeField] private GameObject HitPlane;
    [SerializeField] public GameObject Board;
    private GameObject MagicTraceType;
    [SerializeField] private GameObject BulletTemplate;
    [SerializeField] private GameObject SummonCircle;

    [SerializeField] public GameObject WPManager;
    [SerializeField] public GameObject WPManager2;

    [SerializeField] public GameObject StaffMagic1;
    [SerializeField] public GameObject StaffMagic2;
    [SerializeField] public GameObject StaffMagic3;

    [SerializeField] private GameObject ProjMagic1;
    [SerializeField] public GameObject[] StaffEmitters;
    public int ChargedEmitter;


    public GameObject origin;
    public GameObject fireOrigin;

    public float shootPower;

    public InputActionReference trigger;
    public InputActionReference restart;
    public InputActionReference cast;
    public InputActionReference restartSpell;

    public AudioSource audioSource;
    public AudioClip magicSound;
    public AudioClip fireSound;
    [SerializeField] public AudioClip[] audioClips;

    [SerializeField] private float distanceSum = 0f;
    [SerializeField] private int segmentCount = 0;

    public int magicType = 1;
    
    public bool projectileCharged = false;

    private int charges = 5;

    Vector3 newPoint;
    Vector3 previousPoint = new Vector3(0,0,0);

    void Start() {
        cast.action.performed += CastSpell;
        restart.action.performed += Restart;
        restartSpell.action.performed += RestartSpell;
        
        ChargedEmitter = 0;
    }   

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.magenta);
        if (cast.action.IsPressed()) {
            
            if (magicType == 2 && projectileCharged){
                Board.SetActive(false);
                Shoot();
                charges -= 1;
                if (charges <= 0) {
                    projectileCharged = false;
                    StaffEmitters[ChargedEmitter].SetActive(false);
                    StartCoroutine(RechargeWaypoint());
                }
            }
        }
        if (trigger.action.IsPressed()) {
            
            if (SpellBook.GameMode == "Trace")
            {
                magicType = 1;
                MagicTraceType = MagicParticles1;
                Trace();
            }
            else if (SpellBook.GameMode == "Shoot")
            {
                magicType = 2;
                MagicTraceType = MagicParticles2;
                Trace();
            }
            else if (SpellBook.GameMode == "Swipe")
            {
                magicType = 3;
                MagicTraceType = MagicParticles4;
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
                    Charge();

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

    IEnumerator RechargeWaypoint() {
        yield return new WaitForSeconds(1.5f);
        foreach (GameObject wp in WPManager.GetComponent<WaypointManager>().waypoints){
            wp.SetActive(true);
        }
        WPManager.GetComponent<WaypointManager>().fireChargeCount = 0;
        foreach (GameObject wp in WPManager2.GetComponent<WaypointManager>().waypoints){
            wp.SetActive(true);
        }
        WPManager2.GetComponent<WaypointManager>().fireChargeCount = 0;
        Board.SetActive(true);
    }

    private void Restart(InputAction.CallbackContext __) {
        string currentSceneName = SceneManager.GetActiveScene().name;
	    SceneManager.LoadScene(currentSceneName);   
    }

    private void RestartSpell(InputAction.CallbackContext __) {
        /*
        foreach (GameObject wp in WPManager.GetComponent<WaypointManager>().waypoints){
            wp.SetActive(true);
        }
        WPManager.GetComponent<WaypointManager>().fireChargeCount = 0;
        WPManager.GetComponent<WaypointManager>().magicCount = 0;
        foreach (GameObject wp in WPManager2.GetComponent<WaypointManager>().waypoints){
            wp.SetActive(true);
        }
        WPManager2.GetComponent<WaypointManager>().fireChargeCount = 0;
        WPManager2.GetComponent<WaypointManager>().magicCount = 0;
        projectileCharged = false;
        StaffEmitters[ChargedEmitter].SetActive(false);
        Board.SetActive(true);*/
        Debug.Log("AGGGGH");
    }

    public void Charge() {
        if (magicType == 1) {
            charges = 5;
            ChargedEmitter = 0;
        }
        else if (magicType == 2) {
            charges += 60;
            ChargedEmitter = 1;
        }
        else if (magicType == 3) {
            charges = 1;
            ChargedEmitter = 2;
        }

        projectileCharged = true;
        StaffEmitters[ChargedEmitter].SetActive(true);
        //SpellBook.MagicWaypoints[1].SetActive(false);
        //SpellBook.MagicWaypoints[0].SetActive(false);
    }

    private void CastSpell(InputAction.CallbackContext __) {
        Debug.Log("Cast");
        
        if (projectileCharged == true) {
            Board.SetActive(false);
            Debug.Log("Successful Cast");
            if (magicType == 1) {
                charges -= 1;
                
                GameObject projectile = Instantiate(ProjMagic1, fireOrigin.transform.position += fireOrigin.transform.forward * 1.5f , fireOrigin.transform.rotation);
                projectile.GetComponent<Rigidbody>().AddForce(fireOrigin.transform.forward * shootPower * 5.0f);
                //SpellBook.MagicWaypoints[1].SetActive(false);
                //SpellBook.MagicWaypoints[0].SetActive(true);
                if (!audioSource.isPlaying) {
                    audioSource.PlayOneShot(magicSound);
                }
                if (charges == 0) {
                    projectileCharged = false;
                    StaffEmitters[ChargedEmitter].SetActive(false);
                    StartCoroutine(RechargeWaypoint());
                }
                
            }
            
            else if (magicType == 3) {
                charges -= 1;
                if (charges == 0) {
                    projectileCharged = false;
                    StaffEmitters[ChargedEmitter].SetActive(false);
                }
                
                RaycastHit hit;
                if (Physics.Raycast(origin.transform.position, origin.transform.forward, out hit)){
                    Debug.Log(hit.transform.name);
                    //GameObject projectile = Instantiate(MagicParticles3, transform.position + transform.forward * 40, origin.transform.rotation);
                    Instantiate(SummonCircle, hit.point + new Vector3(0f, 0f, 0f), transform.rotation);
                    //float distance = Vector3.Distance(origin.transform.position, hit.point);
                    //newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * shootPower);
                    if (!audioSource.isPlaying){
                        audioSource.PlayOneShot(magicSound);
                    }
                } 
                Board.SetActive(true);
            }
        }
        
    }

    void Trace() {
        RaycastHit hit;
        if (Physics.Raycast(origin.transform.position, origin.transform.forward, out hit)){
            Debug.Log(hit.transform.name);
            if (hit.transform.name == "HitPlane"){
                GameObject magic = Instantiate(MagicTraceType, hit.point, transform.rotation);
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
                GameObject magic = Instantiate(MagicTraceType, hit.point, transform.rotation);
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