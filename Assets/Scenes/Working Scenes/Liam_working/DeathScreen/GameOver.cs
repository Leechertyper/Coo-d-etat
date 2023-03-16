using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("ScoreManager").GetComponent<Score>().IsHighScore())
        {
            scoreText.text = "New High Score: " + GameObject.Find("ScoreManager").GetComponent<Score>().GetScore();
        }
        else
        {
            scoreText.text = "Score: " + GameObject.Find("ScoreManager").GetComponent<Score>().GetScore();
        }
        GameObject.Find("ScoreManager").GetComponent<Score>().ResetScore();
    }

    // Update is called once per frame
    void Update()
    {
        
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
    
    public void Clickybutton()
    {
        AkSoundEngine.PostEvent("Play_Hover_Click_1", this.gameObject);
    }
}
