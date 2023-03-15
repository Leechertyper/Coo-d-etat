using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int score = 0;
    public int highScore = 0;


    // Load the high score from PlayerPrefs on start
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        LoadScore();
    }

    // Display high score when 'H' key is pressed
    void Update()
    {
    }

    // Add score to current score, update UI, and update high score if current score is higher
    public void AddScore(int amount)
    {
        score += amount;
        if (score > highScore)
        {
            highScore = score;
        }
    }

    // Save the high score to PlayerPrefs on application quit
    void OnApplicationQuit()
    {
        SaveScore();
    }

    // Load the high score from PlayerPrefs
    private void LoadScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore");
    }

    // Save the high score to PlayerPrefs
    private void SaveScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
    }
}