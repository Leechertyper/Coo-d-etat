using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeSliderSFX : MonoBehaviour
{
    public Text volumeLevel_SFX;
    void Start()
    {
        // Set the volume level to the current volume level
        volumeLevel_SFX.text = PlayerPrefs.GetFloat("VolumeLevel_SFX", 100).ToString() + "%";
        AkSoundEngine.SetRTPCValue("FX_Volume", PlayerPrefs.GetFloat("volumeLevel_SFX", 100));
    }

    /*
    *   Param newVolumeLevel = The value from the slider on the options page
    *   this function in the future will also change the volume of all music in the game
    */

    public void SetValueTextSFX(float newVolumeLevel)
    {
        volumeLevel_SFX.text = newVolumeLevel.ToString() + "%";
        PlayerPrefs.SetFloat("VolumeLevel_SFX", newVolumeLevel);
        AkSoundEngine.SetRTPCValue("FX_Volume", newVolumeLevel);
    }
}
