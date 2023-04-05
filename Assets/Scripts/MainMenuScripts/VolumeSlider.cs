using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeSlider : MonoBehaviour
{
    public Text volumeLevel;

    private void Start()
    {

        if (PlayerPrefs.HasKey("VolumeLevel"))
        {
            var newVolumeLevel = PlayerPrefs.GetFloat("VolumeLevel");
            volumeLevel.text = newVolumeLevel + "%";
            AkSoundEngine.SetRTPCValue("Master_Volume", newVolumeLevel);
        }
        else
        {
            PlayerPrefs.SetFloat("VolumeLevel", 100f);
            volumeLevel.text = PlayerPrefs.GetFloat("VolumeLevel") + "%";
            AkSoundEngine.SetRTPCValue("Master_Volume", 100f);
        }
        // Set the volume level to the current volume level
        
    }

    private void OnEnable()
    {
        GetComponent<Slider>().value = PlayerPrefs.GetFloat("VolumeLevel", 100f);
    }

    /*
    *   Param newVolumeLevel = The value from the slider on the options page
    *   this function in the future will also change the volume of all music in the game
    */
    public void SetValuetext(float newVolumeLevel)
    {
        volumeLevel.text = newVolumeLevel + "%";
        PlayerPrefs.SetFloat("VolumeLevel", newVolumeLevel);
        AkSoundEngine.SetRTPCValue("Master_Volume", newVolumeLevel);
    }

}
