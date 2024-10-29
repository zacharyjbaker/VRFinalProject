using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string gameScene;
    GameObject[] enemies;
    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //Debug.Log(enemies.Length);
        if (enemies.Length == 0){
            SceneManager.LoadScene(gameScene);
        }

    }
}
