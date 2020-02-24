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
    private int roundNumber;
    private int playerNumber;
    private int team1;
    private int team2;
    public PlayerController unit;
    public Text text;
    public PlayerController selectedUnit;
    public GameObject mainCamera;
    public bool playerSelected = false;


    private void Start()
    {
        timeLeft = turnTime;
        roundNumber = 1;
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
        if(timeLeft > 0 && selectedUnit.playerNumber == playerTurn)
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
            team1 = GameObject.FindGameObjectsWithTag("Player1").Length;
            team2 = GameObject.FindGameObjectsWithTag("Player2").Length;
            playerNumber = team1 + team2;
            Debug.Log("Players " + playerNumber);
            if (roundNumber >= playerNumber)
            {
                foreach (PlayerController playerController in FindObjectsOfType<PlayerController>())
                {
                    playerController.hasMoved = false;
                    roundNumber = 0;
                }          
            }

            if (playerTurn == 1)
            {
                playerTurn = 2;
                selectedUnit.playerCamera.active = !selectedUnit.playerCamera.active;
                mainCamera.active = !mainCamera.active;              
                timeLeft = turnTime;
                playerSelected = false;
                roundNumber++;

            }
            else if (playerTurn == 2)
            {
                playerTurn = 1;
                selectedUnit.playerCamera.active = !selectedUnit.playerCamera.active;
                mainCamera.active = !mainCamera.active;
                timeLeft = turnTime;
                playerSelected = false;
                roundNumber++;
                
            }
            Debug.Log("Number " + roundNumber);
        }

    }
    
}
