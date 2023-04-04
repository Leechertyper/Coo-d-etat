using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;

public class HighScoreMenu : MonoBehaviour
{
    public Text[] localNameFields;
    public Text[] localScoreFields;
    public Text[] globalNameFields;
    public Text[] globalScoreFields;

    public List<(string name, int score)> localHighScores;
    public InputField inputField;

    private GameObject player;
    private DatabaseManager dbInstance;


    // Start is called before the first frame update
    void Start()
    {
        dbInstance = gameObject.GetComponent<DatabaseManager>();

        //Player not being destroyed after game currently, getting in the way TEMPORARY
        player = GameObject.Find("Player");
        if (player != null)
        {
            player.SetActive(false);
        }        


        inputField.characterLimit = 3;
        LoadLocalScoresText();
        LoadGlobalScoresText();
        if (Score.GetInstance() == null || !Score.GetInstance().IsLocalHighScore())
        {
            GameObject.Find("NameInputBox").SetActive(false);
        }        
    }    

    public void SubmitClick()
    {
        Score.GetInstance().AddHighScore(inputField.text);
        if (dbInstance.GetHostFound())
        {
            dbInstance.SubmitHighScore(inputField.text, Score.GetInstance().GetScore());
        }        
        LoadLocalScoresText();
        LoadGlobalScoresText();
        GameObject.Find("NameInputBox").SetActive(false);
    }

    private void LoadLocalScoresText()
    {
        string localHighScoresJson = PlayerPrefs.GetString("HighScores");
        Debug.Log(localHighScoresJson);
        if (!string.IsNullOrEmpty(localHighScoresJson) && localHighScoresJson != "null")
        {
            localHighScores = JsonConvert.DeserializeObject<List<(string, int)>>(localHighScoresJson);
            
        }
        else
        {
            localHighScores = new List<(string name, int score)>();
            for (int i = 0; i < 10; i++)
            {
                localHighScores.Add(("---", 0));
            }
        }
        //Debug.Log(localHighScores);

        for (int i = 0; i < localHighScores.Count; i++)
        {
            string name = localHighScores[i].Item1;
            int scoreValue = localHighScores[i].Item2;

            localNameFields[i].text = name;
            localScoreFields[i].text = scoreValue.ToString();
        }
    }

    private void LoadGlobalScoresText()
    {
        // Check if the database manager is connected to the database
        if (dbInstance.GetHostFound())
        {
            // Get the high scores from the database manager
            var globalHighScores = dbInstance.GetHighScore();

            if (globalHighScores != null && globalHighScores.Length > 0)
            for (int i = 0; i < globalHighScores.Length; i++)
            {
                globalNameFields[i].text = globalHighScores[i].Item1;
                globalScoreFields[i].text = globalHighScores[i].Item2.ToString();
            }
        }
        else
        {
            Debug.LogWarning("Not connected to the database.");
        }
        
    }

    /*
   *   When called, this will quit the game.
   *   For future, this could also check if the user likes the changes they made to the game to see if they want to push changes.
   */
    public void QuitGame()
    {
        Application.Quit();
    }

    /*
    *   When called, this will set the timescale back to normal and load the MainMenu scene.
    *   For future, this could also check if the user likes the changes they made to the game to see if they want to push changes.
    */
    public void BackToMenu()
    {
        AkSoundEngine.StopAll();
        AkSoundEngine.SetRTPCValue("Dead_Mute", 100);
        if (Score.GetInstance() != null)
        {
            Score.GetInstance().ResetScore();
            Score.GetInstance().UpdateScoreText(0);
            Score.GetInstance().scoreText.text = "";
            Score.GetInstance().scoreIncrease.text = "";
        }        
        if (player != null)
        {
            Destroy(player);
        }
        SceneManager.LoadScene("MainMenu");
    }
}
