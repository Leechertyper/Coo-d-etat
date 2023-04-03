using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class Score : MonoBehaviour
{
    public int _score = 0;
    public int _highScore = 0;
    private static Score instance;
    public Text scoreText;
    public bool _startTimer = false;
    public float _invulnTimer = 0;
    public Text scoreIncrease;

    [SerializeField] public static List<(string, int)> highScores;

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
    }

    public void UpdateScoreText(int amount)
    {
        scoreText.text = "Score: " + _score;
        scoreIncrease.text = " +" + amount;
        _startTimer = true;
    }

    public static Score GetInstance()
    {
        return instance;
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

    public bool IsLocalHighScore()
    {
        LoadHighScores();
        for (int i = 0; i < highScores.Count; i++)
        {
            if (_score > highScores[i].Item2)
            {
                return true;
            }
        }
        return false;
    }

    // Save the high _score to PlayerPrefs on application quit
    void OnApplicationQuit()
    {
        SaveHighScores();
    }

    public void AddHighScore(string name)
    {
        LoadHighScores();
        highScores.Add((name, _score));
        highScores.Sort((a, b) => b.Item2.CompareTo(a.Item2));
        if (highScores.Count > 10)
        {
            highScores.RemoveAt(highScores.Count - 1);
        }
        SaveHighScores();
    }

    public void SaveHighScores()
    {
        string highScoresJson = JsonConvert.SerializeObject(highScores);
        PlayerPrefs.SetString("HighScores", highScoresJson);
    }

    public void LoadHighScores()
    {
        string highScoresJson = PlayerPrefs.GetString("HighScores");

        if (!string.IsNullOrEmpty(highScoresJson))
        {
            highScores = JsonConvert.DeserializeObject<List<(string, int)>>(highScoresJson);
        }
        else
        {
            highScores = new List<(string name, int score)>();
            for (int i = 0; i < 10; i++)
            {
                highScores.Add(("---", 0));
            }
        }
    }
}