using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private int _score;
    private int _highscore;

    public int Score { get { return _score; } }
    public int Highscore { get { return _highscore; } }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _highscore = PlayerPrefs.GetInt("highscore", 0);
    }

    public void AddPoint(int score)
    {
        _score += score;
        UpdateHighscore();
    }

    public void UpdateHighscore()
    {
        if(_highscore < _score)
        {
            PlayerPrefs.SetInt("highscore", _score);
        }
    }
}
