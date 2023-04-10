using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool gameIsPaused = false;
    public GameObject pauseMenuUI;

    public GameObject options;

    public GameObject cursor;

    // Return To Main Menu Confirm Screen
    public GameObject mmConfirm;

    // Quit to Desktop Confirm Screen
    public GameObject qConfirm;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            
            gameIsPaused = !gameIsPaused;
            if(gameIsPaused)
            {
                cursor.GetComponent<CursorScript>().UnsetCursor();
                Pause();
            } else
            {
                cursor.GetComponent<CursorScript>().SetCursor();
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
        GameManager.Instance.inGame = true;
        pauseMenuUI.SetActive(false);
        options.SetActive(false);
        mmConfirm.SetActive(false);
        qConfirm.SetActive(false);
    }

    /*
    *   When called, this will pause the game.
    */
    void Pause()
    {
        AkSoundEngine.PostEvent("Play_Pause_SFX", this.gameObject);
        AkSoundEngine.SetRTPCValue("Dead_Mute", 0);
        AkSoundEngine.SetRTPCValue("Game_Is_Paused", 100);
        GameManager.Instance.inGame = false;
        Time.timeScale = 0f;
        gameIsPaused = true;
        pauseMenuUI.SetActive(true);
        mmConfirm.SetActive(false);
        qConfirm.SetActive(false);
        
    }

    public void ShowOptions()
    {
        options.SetActive(true);
        pauseMenuUI.SetActive(false);
    }

    public void HideOptions()
    {
        options.SetActive(false);
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

    public void GoToLeaderboard()
    {
        Resume();
        AkSoundEngine.StopAll();
        //AkSoundEngine.PostEvent("Stop_Controller_Switch", this.gameObject);
        AkSoundEngine.SetRTPCValue("Game_Is_Paused", 0);
        GameManager.Instance.StartBalanceMenu(true);
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