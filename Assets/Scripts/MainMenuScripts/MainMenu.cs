using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int startScene;

    public void Start()
    {
<<<<<<< Updated upstream
        ///<TODO> UPDATE BASE BALANCEVARIABLES HERE </TODO>
=======
        AkSoundEngine.SetState("PlayerLife", "None");
        AkSoundEngine.PostEvent("Play_Controller_Switch", this.gameObject);
>>>>>>> Stashed changes
    }

    /*
    *   When called, this will load the scene from an int in the build scene order.
    *   The startScene can be updated in unity under the MenuCanvas
    */
    public void StartGame()
    {
        AkSoundEngine.PostEvent("Play_Hover_Click_1", this.gameObject);
        SceneManager.LoadScene(startScene); 
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

}