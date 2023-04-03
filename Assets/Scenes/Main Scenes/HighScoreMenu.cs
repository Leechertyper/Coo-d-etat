using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighScoreMenu : MonoBehaviour
{
    public Text[] nameFields;
    public Text[] scoreFields;

    public List<(string name, int score)> highScores;
    public InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        inputField.characterLimit = 3;
        LoadScoreText();
    }    

    public void SubmitClick()
    {
        Score score = Score.GetInstance();
        score.AddHighScore(inputField.text);
        LoadScoreText();
        GameObject.Find("NameInputBox").SetActive(false);
    }

    private void LoadScoreText()
    {
        Score score = Score.GetInstance();
        List<(string name, int score)> highScores = Score.highScores;

        for (int i = 0; i < highScores.Count; i++)
        {
            string name = highScores[i].Item1;
            int scoreValue = highScores[i].Item2;

            nameFields[i].text = name;
            scoreFields[i].text = scoreValue.ToString();
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
        SceneManager.LoadScene("MainMenu");
    }
}
