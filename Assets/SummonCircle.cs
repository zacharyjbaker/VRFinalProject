using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonCircle : MonoBehaviour
{
    [SerializeField] private GameObject zombieSummon;
    [SerializeField] public float summonTime;
    [SerializeField] public float particleLifetime;
    float timer;
    float summonTimer;
    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        summonTimer += Time.deltaTime;
        if (timer >= particleLifetime)
        {
            Destroy(gameObject);

        }
        if (summonTimer >= summonTime)
        {
            Instantiate(zombieSummon, transform.position + new Vector3(0, 0, 0.24f), transform.rotation);
            summonTimer = 0;
        }
       
    }
}
