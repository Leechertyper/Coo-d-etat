using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;
using UnityEngine.UI;
public class VolumeSliderMusic : MonoBehaviour
{
    public Text volumeLevel_Music;

    void Start()
    {
        // Set the volume level to the current volume level
        
        if (PlayerPrefs.HasKey("VolumeLevel_Music"))
        {
            var newVolumeLevel = PlayerPrefs.GetFloat("VolumeLevel_Music");
            volumeLevel_Music.text = newVolumeLevel + "%";
            AkSoundEngine.SetRTPCValue("Music_Volume", newVolumeLevel);
        }
        else
        {
            PlayerPrefs.SetFloat("VolumeLevel_Music", 100f);
            volumeLevel_Music.text = PlayerPrefs.GetFloat("VolumeLevel_Music") + "%";
            AkSoundEngine.SetRTPCValue("Music_Volume", 100f);
        }
    }
    
    private void OnEnable()
    {
        GetComponent<Slider>().value = PlayerPrefs.GetFloat("VolumeLevel_Music", 100f);
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
