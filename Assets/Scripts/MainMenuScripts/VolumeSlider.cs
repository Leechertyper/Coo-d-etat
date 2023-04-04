using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeSlider : MonoBehaviour
{
    public Text volumeLevel;

    void Start()
    {
        // Set the volume level to the current volume level
        volumeLevel.text = PlayerPrefs.GetFloat("VolumeLevel", 100).ToString() + "%";
    }

    /*
    *   Param newVolumeLevel = The value from the slider on the options page
    *   this function in the future will also change the volume of all music in the game
    */
    public void SetValuetext(float newVolumeLevel)
    {
        volumeLevel.text = newVolumeLevel.ToString() + "%";
        PlayerPrefs.SetFloat("VolumeLevel", newVolumeLevel);
    }

}
