using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] private GameObject[] EnemyTypes;
    [SerializeField] private GameObject[] Waypoints;
    private float targetTime = 3f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetTime -= Time.deltaTime;

        if (targetTime <= 0.1f)
        {
            GameObject enemy = Instantiate(EnemyTypes[0], transform.position, transform.rotation);
            enemy.GetComponent<Target>().path = Waypoints;
            enemy.transform.forward = gameObject.transform.forward;
            targetTime = 10f;
        }
    }
}
