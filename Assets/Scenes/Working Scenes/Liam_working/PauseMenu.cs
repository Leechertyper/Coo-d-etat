using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;

    private static PauseMenu _instance;

    public static PauseMenu Instance { get { return _instance; } }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }

    /*
    *   When called, this will resume the game.
    */
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    /*
    *   When called, this will pause the game.
    */
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
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
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}