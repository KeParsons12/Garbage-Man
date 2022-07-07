using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _highscoreText;

    private void Update()
    {
        _scoreText.text = "Score: " + ScoreManager.instance.Score;
        _highscoreText.text = "Highscore: " + ScoreManager.instance.Highscore;
    }
}
