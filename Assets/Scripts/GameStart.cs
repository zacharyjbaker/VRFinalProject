using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    [SerializeField] private string gameScene;
    public InputActionReference start;
    public float targetTime = 3.0f;

    // Update is called once per frame
    void Start()
    {
        //start.action.performed += StartGame;
    }

    void Update()
    {
        targetTime -= Time.deltaTime;
        if (targetTime <= 0.0f)
        {
            StartGame();
        }
    }
    void StartGame() {
        SceneManager.LoadScene(gameScene);
    }
}
