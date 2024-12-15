using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    [SerializeField] private float speed = 100f;
    [SerializeField] private GameObject target;
    [SerializeField] private int focus = 0;
    [SerializeField] private GameObject playerTarget;

    [SerializeField] private int health = 1;

    [SerializeField]
    private GameObject BulletTemplate;
    public float shootPower = 500f;
    public float targetTime = 3f;
    public GameObject[] path;
    public int activeWaypoint = 0;

    Vector3 forwardDir;

    public AudioSource audioSource;

    [SerializeField] private bool shoota;
    [SerializeField] private bool stabba;
    [SerializeField] private bool pulla;
    [SerializeField] private bool rida;

    [SerializeField] private GameObject leftLeg;
    [SerializeField] private GameObject rightLeg;
    Vector3 forwardLeftRot;
    Vector3 forwardRightRot;
    Vector3 defaultLeftRot;
    Vector3 defaultRightRot;
    Quaternion forwardLeftQuat;
    Quaternion forwardRightQuat;
    Quaternion defaultLeftQuat;
    Quaternion defaultRightQuat;
    Vector3 strafeDir;
    
    // Start is called before the first frame update
    void Start()
    {
        defaultLeftRot += leftLeg.transform.eulerAngles;
        defaultLeftQuat.eulerAngles = defaultLeftRot;

        defaultRightRot += rightLeg.transform.eulerAngles;
        defaultRightQuat.eulerAngles = defaultRightRot;

        forwardLeftRot = new Vector3(leftLeg.transform.eulerAngles.x + 55f, leftLeg.transform.eulerAngles.y, leftLeg.transform.eulerAngles.z);
        forwardRightRot = new Vector3(rightLeg.transform.eulerAngles.x + 55f, rightLeg.transform.eulerAngles.y, rightLeg.transform.eulerAngles.z);
        
        forwardLeftQuat.eulerAngles = forwardLeftRot;
        forwardRightQuat.eulerAngles = forwardRightRot;
        strafeDir = transform.right;

        target = TargetClosestEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) {
            target = TargetClosestEnemy();
        }

        /*
        if (Vector3.Distance(transform.position, target.transform.position) < 1f) {
            Destroy(gameObject);
        }*/

        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

        //forwardDir = GetComponent<Rigidbody>().velocity;
        //forwardDir = GetComponent<Rigidbody>().velocity.normalized;
        //transform.LookAt(path[activeWaypoint].transform.position);

        Quaternion lookOnLook = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime);

        leftLeg.transform.rotation = Quaternion.Lerp(defaultLeftQuat, forwardLeftQuat, Mathf.PingPong(Time.time,1));
        rightLeg.transform.rotation = Quaternion.Lerp(defaultRightQuat, forwardRightQuat, Mathf.PingPong(Time.time,1));

        //Debug.Log(Time.timeScale);
        if (rida == false) {
            StickToGround();
        }

        targetTime -= Time.deltaTime;
    }

    private GameObject TargetClosestEnemy() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest_enemy = null;
        float min_distance = Mathf.Infinity;
        foreach(GameObject enemy in enemies) {
            Vector3 posDifference = enemy.transform.position - transform.position;
            float distanceTo = posDifference.sqrMagnitude;
            if (distanceTo < min_distance) {
                closest_enemy = enemy;
                min_distance = distanceTo;
            }
        }
        return closest_enemy;
    }

    /*
    private void OnTriggerEnter(Collider other) {
        
        if (other.tag == "Bullet"){
            if (health <= 1) {
                Destroy(other);
                Destroy(gameObject);
            }
            else {
                health = health - 1;
            }
        }
        else if (other.tag == "Player"){
            Destroy(other);
        }
        else if (other.tag == "Base"){
            Destroy(gameObject);
        }
    }*/

    private void StickToGround() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity)){
            transform.position -= new Vector3(0 , hit.distance, 0);
        }
    }
}
