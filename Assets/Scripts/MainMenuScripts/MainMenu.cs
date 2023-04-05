using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public int startScene;

    public bool isDataBaseEnabled;
    public GameObject DataBaseButton;

    public void Start()
    {
        ///<TODO> UPDATE BASE BALANCEVARIABLES HERE </TODO>
        AkSoundEngine.PostEvent("Stop_Controller_Switch", this.gameObject);
        AkSoundEngine.SetState("PlayerLife", "None");
        AkSoundEngine.PostEvent("Play_Controller_Switch", this.gameObject);
        if(PlayerPrefs.HasKey("BalanceDataBase") == false)
        {
            PlayerPrefs.SetInt("BalanceDataBase", 1);
        }

        if(PlayerPrefs.GetInt("BalanceDataBase") == 1)
        {
            isDataBaseEnabled = true;
            DataBaseButton.GetComponentInChildren<Text>().text = "Disable Balance DataBase";
        }
        else
        {
            isDataBaseEnabled = false;
            DataBaseButton.GetComponentInChildren<Text>().text = "Enable Balance DataBase";
        }

        // Destroy Player if exists
        if (GameObject.Find("Player") != null)
        {
            Destroy(GameObject.Find("Player"));
        }

        AkSoundEngine.SetRTPCValue("Master_Volume", PlayerPrefs.GetFloat("VolumeLevel", 100f));
        AkSoundEngine.SetRTPCValue("FX_Volume", PlayerPrefs.GetFloat("VolumeLevel_SFX", 100f));
        AkSoundEngine.SetRTPCValue("Music_Volume", PlayerPrefs.GetFloat("VolumeLevel_Music", 100f));
    }

    /*
    *   When called, this will load the scene from an int in the build scene order.
    *   The startScene can be updated in unity under the MenuCanvas
    */
    public void StartGame()
    {
        AkSoundEngine.PostEvent("Play_Hover_Click_1", this.gameObject);
        SceneManager.LoadScene(startScene); 
        if(GameObject.Find("ScoreManager"))
        {
            GameObject.Find("ScoreManager").GetComponent<Score>().ResetScore();
            GameObject.Find("ScoreManager").GetComponent<Score>().ResetHighScore();
        }
    }

    /*
    *   When called, this will quit the game.
    *   For future, this could also check if the user likes the changes they made to the game to see if they want to push changes.
    */
    public void QuitGame()
    {
        AkSoundEngine.PostEvent("Play_Hover_Click_1", this.gameObject);
        Application.Quit();
    }

    public void Clickybutton()
    {
        AkSoundEngine.PostEvent("Play_Hover_Click_1", this.gameObject);
    }
    
    public void Hoverbutton()
    {
        AkSoundEngine.PostEvent("Play_Hover_Click_2", this.gameObject);
    }

    public void ChangeDataBasePref()
    {
        if (isDataBaseEnabled)
        {
            isDataBaseEnabled = false;
            DataBaseButton.GetComponentInChildren<Text>().text = "Enable Balance DataBase";
            PlayerPrefs.SetInt("BalanceDataBase", 0);
        }
        else
        {
            isDataBaseEnabled = true;
            DataBaseButton.GetComponentInChildren<Text>().text = "Disable Balance DataBase";
            PlayerPrefs.SetInt("BalanceDataBase", 1);
        }
    }

    public void HighScores()
    {
        SceneManager.LoadScene("HighScores");
    }
}