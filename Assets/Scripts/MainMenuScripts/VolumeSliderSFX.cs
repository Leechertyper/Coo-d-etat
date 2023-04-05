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
        
        if (PlayerPrefs.HasKey("VolumeLevel_SFX"))
        {
            var newVolumeLevel = PlayerPrefs.GetFloat("VolumeLevel_SFX");
            volumeLevel_SFX.text = newVolumeLevel + "%";
            AkSoundEngine.SetRTPCValue("FX_Volume", newVolumeLevel);
        }
        else
        {
            PlayerPrefs.SetFloat("VolumeLevel_SFX", 100f);
            volumeLevel_SFX.text = PlayerPrefs.GetFloat("VolumeLevel_SFX") + "%";
            AkSoundEngine.SetRTPCValue("FX_Volume", 100f);
        }
    }
    
    private void OnEnable()
    {
        GetComponent<Slider>().value = PlayerPrefs.GetFloat("VolumeLevel_SFX", 100f);
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
