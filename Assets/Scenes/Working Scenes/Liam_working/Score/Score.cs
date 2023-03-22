using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int _score = 0;
    public int _highScore = 0;
    private static Score instance;
    public Text scoreText;
    public bool _startTimer = false;
    public float _invulnTimer = 0;
    public Text scoreIncrease;

    void Update()
    {
        if(_startTimer)
        {
            _invulnTimer += Time.deltaTime;
            if(_invulnTimer >= 3f)
            {
                _startTimer = false;
                _invulnTimer = 0;
                scoreIncrease.text = "";
            }
        }

    }
    // Load the high _score from PlayerPrefs on start
    void Start()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        LoadScore();
    }

    public void UpdateScoreText(int amount)
    {
        scoreText.text = "Score: " + _score;
        scoreIncrease.text = " +" + amount;
        _startTimer = true;
    }




    // Add _score to current _score, update UI, and update high _score if current _score is higher
    public void AddScore(int amount)
    {
        _score += amount;
        if (_score > _highScore)
        {
            _highScore = _score;
        }
        UpdateScoreText(amount);
        GameObject.Find("ShopManager").GetComponent<Shop>().AddMoney(amount);
        
    }

    // Reset the current _score and update UI
    public void ResetScore()
    {
        _score = 0;
    }

    public int GetScore()
    {
        return _score;
    }

    public bool IsHighScore()
    {
        return _score > _highScore;
    }

    // Save the high _score to PlayerPrefs on application quit
    void OnApplicationQuit()
    {
        SaveScore();
    }

    // Load the high _score from PlayerPrefs
    private void LoadScore()
    {
        _highScore = PlayerPrefs.GetInt("_highScore");
    }

    // Save the high _score to PlayerPrefs
    private void SaveScore()
    {
        PlayerPrefs.SetInt("_highScore", _highScore);
    }
}