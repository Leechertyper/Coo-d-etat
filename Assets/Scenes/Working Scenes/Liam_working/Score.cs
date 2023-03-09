using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    public int score = 0;
    public GameObject scoreText;
    public int highScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        LoadScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
        if (score > highScore)
        {
            highScore = score;
        }
    }

    private void SaveScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
    }

    private void LoadScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore");
    }

    void OnApplicationQuit()
    {
        SaveScore();
    }

    private void UpdateScoreUI()
    {
        scoreText.GetComponent<Text>().text = "Score: " + score;
    }
}
