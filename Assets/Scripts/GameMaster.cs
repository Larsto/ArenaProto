using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public int playerTurn = 1;
    [HideInInspector] public float timeLeft;
    public float turnTime;
    public Text text;
    public GameObject mainCamera;
    public GameObject player1Camera;
    public GameObject player2Camera;

    private void Start()
    {
        timeLeft = turnTime;
    }

    private void Update()
    {
        changeCamera();
        reloadScene();
        endTurn();
        timer();
    }

    void reloadScene()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void timer()
    {
        if(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }

        text.text = "Time Left:" + Mathf.Round(timeLeft);
    }

    void changeCamera()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            mainCamera.active = !mainCamera.active;
        }
    }

    public void endTurn()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            if (playerTurn == 1)
            {
                playerTurn = 2;
                player1Camera.active = !player1Camera.active;
                player2Camera.active = !player2Camera.active;
                timeLeft = turnTime;
            }
            else if (playerTurn == 2)
            {
                playerTurn = 1;
                player1Camera.active = !player1Camera.active;
                player2Camera.active = !player2Camera.active;
                timeLeft = turnTime;
            }
        }
       
    }
    
}
