using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int startScene;

    /*
    *   When called, this will load the scene from an int in the build scene order.
    *   The startScene can be updated in unity under the MenuCanvas
    */
    public void StartGame()
    {
        SceneManager.LoadScene(startScene);
    }

    /*
    *   When called, this will quit the game.
    *   For future, this could also check if the user likes the changes they made to the game to see if they want to push changes.
    */
    public void QuitGame()
    {
        Application.Quit();
    }
}