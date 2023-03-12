using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int score = 0;
    public int highScore = 0;

    public GameObject scoreText;
    public GameObject highScoreText;

    // Load the high score from PlayerPrefs on start
    void Start()
    {
        LoadScore();
    }

    // Display high score when 'H' key is pressed
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            DisplayHighScore();
        }
    }

    // Add score to current score, update UI, and update high score if current score is higher
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
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

    // Reset the current score and update UI
    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }

    // Go to the next level and reset the current score
    public void GoNext()
    {
        highScoreText.SetActive(false);
        ResetScore();
    }

    // Display the high score in the UI
    void DisplayHighScore()
    {
        if(score>highScore)
        {
            highScoreText.GetComponent<Text>().text = "New High Score: " + highScore;
        }
        else
        {
            highScoreText.GetComponent<Text>().text = "High Score: " + highScore;
        }
        highScoreText.SetActive(true);
        
    }

    // Update the score UI with the current score
    private void UpdateScoreUI()
    {
        scoreText.GetComponent<Text>().text = "Score: " + score;
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