using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool gameIsPaused;
    public GameObject pauseMenuUI;

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
        Time.timeScale = 1.0f;
        gameIsPaused = false;
        pauseMenuUI.SetActive(false);
        
    }

    /*
    *   When called, this will pause the game.
    */
    void Pause()
    {
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
        SceneManager.LoadScene("MainMenu");
    }
}