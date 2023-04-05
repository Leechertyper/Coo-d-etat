using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Beta Main"){
            scoreText = GameObject.Find("Player/Score Canvas/ScoreText").GetComponent<Text>();
            scoreIncrease = GameObject.Find("Player/Score Canvas/ScoreIncrease").GetComponent<Text>();
        }
    }
    public void UpdateScoreText(int amount)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + _score;
            scoreIncrease.text = " +" + amount;
            _startTimer = true;
        }        
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
        
    }

    // remove _score from current _score, update UI, and update high _score if current _score is higher
    public void RemoveScore(int amount)
    {
        _score -= amount;
        if (_score > _highScore)
        {
            _highScore = _score;
        }
        UpdateScoreText(amount*(-1));
    }

    // Reset the current _score and update UI
    public void ResetScore()
    {
        _score = 0;
    }

    public void ResetHighScore()
    {
        _highScore = 0;
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

    // Adds High Score to list of high scores
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

    //Saves High Scores to PlayerPrefs as Serialized JSON
    public void SaveHighScores()
    {
        string highScoresJson = JsonConvert.SerializeObject(highScores);
        PlayerPrefs.SetString("HighScores", highScoresJson);
    }

    //Loads High Scores from PlayerPrefs as Serialized JSON
    public void LoadHighScores()
    {
        string highScoresJson = PlayerPrefs.GetString("HighScores");

        if (!string.IsNullOrEmpty(highScoresJson) && highScoresJson != "null")
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