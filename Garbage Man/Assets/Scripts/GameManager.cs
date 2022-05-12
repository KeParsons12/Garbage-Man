using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public GameObject playerSphere;
    public GameObject playerCar;
    public Transform playerStartPos;

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public bool isGameOver;

    [Header("Timer")]
    public Timer timer;

    [Header("Score")]
    public Text scoreText;
    public int score;

    [Header("Debug")]
    public bool isDebug;
    public GameObject debugPanel;

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        CheckGame();

        //Debug
        DebugDisplay();
    }

    public void UpdateScore(int value)
    {
        score += value;
        scoreText.text = "Score: " + score;
    }

    public void DebugDisplay()
    {
        debugPanel.SetActive(isDebug);
    }

    public void SetCarPos()
    {
        playerCar.transform.rotation = playerStartPos.rotation;
        playerSphere.transform.position = playerStartPos.position;
    }

    public void StartGame()
    {
        SetCarPos();

        UpdateScore(0);

        //Sets game over to false
        isGameOver = false;
        //Resets the end game
        EndGame(1f, isGameOver);
    }

    public void EndGame(float time, bool _isGameOver)
    {
        //Stop game
        Time.timeScale = time;
        //Display Game over panel
        gameOverPanel.SetActive(_isGameOver);
    }

    public void RestartGame()
    {
        //Reloads the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void CheckGame()
    {
        //If timer is zero
        if(timer.currentTime <= 0)
        {
            timer.currentTime = 0f;
            isGameOver = true;
            EndGame(0f, isGameOver);
        }
    }
}
