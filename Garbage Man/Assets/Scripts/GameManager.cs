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

    [Header("Trash")]
    public Text trashText;
    public int trashCollected;

    [Header("Distance")]
    public Text distanceText;
    public float totalDistance;
    public Transform targetPos;
    private Vector3 oldPos;

    [Header("Speeds")]
    public Speedometer speedometer;
    public Text aveSpeedText;
    public float finalTotal;
    private float averageTotal;
    private bool hasSet;
    public List<float> speedsList = new List<float>();

    [Header("MailBoxes")]
    public Text mailSmashedText;
    public int mailSmashed;

    [Header("Debug")]
    public bool isDebug;
    public GameObject debugPanel;

    private void Start()
    {
        StartGame();

        UpdateMailSmashed(0);

        oldPos = targetPos.position;
    }

    private void Update()
    {
        CheckGame();

        UpdateDistance();

        //Debug
        DebugDisplay();

    }

    private void FixedUpdate()
    {
        AddSpeedToList();
    }

    public void UpdateScore(int _score, int _trash)
    {
        //Score
        score += _score;
        scoreText.text = "Score: " + score;

        //Trash Bins collected
        trashCollected += _trash;
        trashText.text = "Trash Collected: " + trashCollected;
    }

    public void UpdateDistance()
    {
        //Total Distance Travelled
        Vector3 distanceVector = targetPos.position - oldPos;
        float distanceThisFrame = distanceVector.magnitude;
        totalDistance += distanceThisFrame * 0.000621371f;
        float roundedDistance = (int)(totalDistance * 100f) / 100f;

        distanceText.text = "Distance: " + roundedDistance + " miles";

        oldPos = targetPos.position;
    }

    public void AddSpeedToList()
    {
        if(!isGameOver)
        {
            speedsList.Add(speedometer.currentSpeed);
        }
    }

    public void SetFinalSpeed()
    {
        if(!hasSet)
        {
            float roundedAvgSpeed = Mathf.Round(ReturnAverageSpeed());

            aveSpeedText.text = "average speed: " + roundedAvgSpeed;
            hasSet = true;
        }
    }

    public float ReturnAverageSpeed()
    {
        for (int i = 0; i < speedsList.Count; i++)
        {
            averageTotal += speedsList[i];
        }

        finalTotal = averageTotal / speedsList.Count;
        return finalTotal;
    }

    public void UpdateMailSmashed(int _smashed)
    {
        mailSmashed += _smashed;
        mailSmashedText.text = "Mail boxes smashed: " + mailSmashed;
    }

    public void SetCarPos()
    {
        playerCar.transform.rotation = playerStartPos.rotation;
        playerSphere.transform.position = playerStartPos.position;
    }

    public void StartGame()
    {
        SetCarPos();

        UpdateScore(0, 0);

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
        Time.timeScale = 1f;

        SceneManager.LoadScene(sceneName);
    }

    public void CheckGame()
    {
        //If timer is zero
        if(timer.currentTime <= 0 && !isGameOver)
        {
            timer.currentTime = 0f;

            SetFinalSpeed();

            isGameOver = true;
            EndGame(0f, isGameOver);
        }
    }

    public void DebugDisplay()
    {
        debugPanel.SetActive(isDebug);
    }
}
