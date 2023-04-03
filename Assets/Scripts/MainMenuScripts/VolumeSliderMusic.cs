using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeSliderMusic : MonoBehaviour
{
    public Text volumeLevel_Music;

    void Start()
    {
        // Set the volume level to the current volume level
        volumeLevel_Music.text = PlayerPrefs.GetFloat("VolumeLevel_Music", 100).ToString() + "%";
        AkSoundEngine.SetRTPCValue("Music_Volume", PlayerPrefs.GetFloat("volumeLevel_Music", 100));
    }

    /*
    *   Param newVolumeLevel = The value from the slider on the options page
    *   this function in the future will also change the volume of all music in the game
    */
    public void SetValueTextmusic(float newVolumeLevel)
    {
        volumeLevel_Music.text = newVolumeLevel.ToString() + "%";
        PlayerPrefs.SetFloat("VolumeLevel_Music", newVolumeLevel);
        AkSoundEngine.SetRTPCValue("Music_Volume", newVolumeLevel);
    }

}
