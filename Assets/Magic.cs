using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{

    [SerializeField] public float particleLifetime = 2f;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - timer > particleLifetime)
        {
            Destroy(gameObject);

        }
    }
}
