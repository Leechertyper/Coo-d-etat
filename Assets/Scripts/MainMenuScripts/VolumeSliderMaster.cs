using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeSliderMaster : MonoBehaviour
{
    public Text volumeLevel_Master;

    void Start()
    {
        // Set the volume level to the current volume level
        volumeLevel_Master.text = PlayerPrefs.GetFloat("VolumeLevel_Master", 100).ToString() + "%";
        AkSoundEngine.SetRTPCValue("Master_Volume", PlayerPrefs.GetFloat("volumeLevel_Master",100));
    }

    /*
    *   Param newVolumeLevel = The value from the slider on the options page
    *   this function in the future will also change the volume of all music in the game
    */
    public void SetValueTextmaster(float newVolumeLevel)
    {
        volumeLevel_Master.text = newVolumeLevel.ToString() + "%";
        PlayerPrefs.SetFloat("VolumeLevel_Master", newVolumeLevel);
        AkSoundEngine.SetRTPCValue("Master_Volume", newVolumeLevel);
    }
}
