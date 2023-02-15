using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindings : MonoBehaviour
{
    private KeyCode newKey;
    public Text leftKeyText;
    public Text rightKeyText;
    public Text upKeyText;
    public Text downKeyText;
    public Text fireKeyText;
    public Text keyBindText;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("upKey"))
        {
            upKeyText.text = "Up: " + PlayerPrefs.GetString("upKey");
        }

        if (PlayerPrefs.HasKey("downKey"))
        {
            downKeyText.text = "Down: " + PlayerPrefs.GetString("downKey");
        }

        if (PlayerPrefs.HasKey("leftKey"))
        {
            leftKeyText.text = "Left: " + PlayerPrefs.GetString("leftKey");
        }

        if (PlayerPrefs.HasKey("rightKey"))
        {
            rightKeyText.text = "Right: " + PlayerPrefs.GetString("rightKey");
        }

        if (PlayerPrefs.HasKey("fireKey"))
        {
            rightKeyText.text = "Shoot: " + PlayerPrefs.GetString("fireKey");
        }
    }

    public void ButtonClick(string keyName)
    {
        StartCoroutine(ChangeKey(keyName));
    }

    public IEnumerator ChangeKey(string keyName)
    {
        switch (keyName)
        {
            case "Up":
                upKeyText.text = "Press new key for " + keyName;
                break;
            case "Down":
                downKeyText.text = "Press new key for " + keyName;
                break;
            case "Left":
                leftKeyText.text = "Press new key for " + keyName;
                break;
            case "Right":
                rightKeyText.text = "Press new key for " + keyName;
                break;
            case "Shoot":
                rightKeyText.text = "Press new key for " + keyName;
                break;
        }
        
        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        newKey = GetNewKey();

        switch (keyName)
        {
            case "Up":
                PlayerPrefs.SetString("upKey", newKey.ToString());
                upKeyText.text = "Up: " + newKey;
                break;
            case "Down":
                PlayerPrefs.SetString("downKey", newKey.ToString());
                downKeyText.text = "Down: " + newKey;
                break;
            case "Left":
                PlayerPrefs.SetString("leftKey", newKey.ToString());
                leftKeyText.text = "Left: " + newKey;
                break;
            case "Right":
                PlayerPrefs.SetString("rightKey", newKey.ToString());
                rightKeyText.text = "Right: " + newKey;
                break;
            case "Shoot":
                PlayerPrefs.SetString("fireKey", newKey.ToString());
                fireKeyText.text = "Shoot: " + newKey;
                break;
        }
    }

    // Read Key Input
    private KeyCode GetNewKey()
    {
        while (true)
        {
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(vKey))
                {
                    return vKey;
                }
            }
        }
    }
}
