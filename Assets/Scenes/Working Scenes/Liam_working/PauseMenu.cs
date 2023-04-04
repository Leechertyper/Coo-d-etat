using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    
    public GameObject DataBaseButton;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;
            if(gameIsPaused)
            {
                Pause();
            } else
            {
                Resume();
            }
        }
    }

    /*
    *   When called, this will resume the game.
    */
    public void Resume()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().LoadControls(); // Load Player Controls
        AkSoundEngine.PostEvent("Play_Unpause_SFX", this.gameObject);
        AkSoundEngine.SetRTPCValue("Dead_Mute", 100);
        AkSoundEngine.SetRTPCValue("Game_Is_Paused", 0);
        Time.timeScale = 1.0f;
        gameIsPaused = false;
        pauseMenuUI.SetActive(false);        
    }

    /*
    *   When called, this will pause the game.
    */
    void Pause()
    {
        AkSoundEngine.PostEvent("Play_Pause_SFX", this.gameObject);
        AkSoundEngine.SetRTPCValue("Dead_Mute", 0);
        AkSoundEngine.SetRTPCValue("Game_Is_Paused", 100);
        Time.timeScale = 0f;
        gameIsPaused = true;
        pauseMenuUI.SetActive(true);
        
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
        Resume();
        AkSoundEngine.StopAll();
        //AkSoundEngine.PostEvent("Stop_Controller_Switch", this.gameObject);
        AkSoundEngine.SetRTPCValue("Game_Is_Paused", 0);

        SceneManager.LoadScene("MainMenu");
    }
    public void Clickybutton()
    {
        AkSoundEngine.PostEvent("Play_Hover_Click_1", this.gameObject);
    }
    
    public void Hoverbutton()
    {
        AkSoundEngine.PostEvent("Play_Hover_Click_2", this.gameObject);
    }
}