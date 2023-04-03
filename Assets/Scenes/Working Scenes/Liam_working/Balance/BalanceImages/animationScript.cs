using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class animationScript : MonoBehaviour
{
    private int spriteCategory = 0;
    public Sprite[] catSprites;
    public Sprite[] dogSprites;
    public Sprite[] pirateSprites;
    public Sprite[] droneSprites;
    public Sprite[] pigeonSprites;
    public Sprite[] collectablesSprites;
    public Sprite[] otherSprites;
    private List<Sprite[]> spriteList = new List<Sprite[]>();
    private int currentFrame = 0; // The current frame of the animation
    private float timer = 0.0f;
    void Start()
    {
        spriteList.Add(catSprites);
        spriteList.Add(dogSprites);
        spriteList.Add(pirateSprites);
        spriteList.Add(droneSprites);
        spriteList.Add(pigeonSprites);
        spriteList.Add(collectablesSprites);
        spriteList.Add(otherSprites);
    }

    public void ChangeNameofCardSprite(string _name)
    {
        if(_name == "catEnemy")
        {
            spriteCategory = 0;
            GetComponent<Image>().sprite = catSprites[0];
        }
        else if(_name == "dogEnemy")
        {
            spriteCategory = 1;
            GetComponent<Image>().sprite = dogSprites[0];
        }
        else if(_name == "pirateEnemy")
        {
            spriteCategory = 2;
            GetComponent<Image>().sprite = pirateSprites[0];
        }
        else if(_name == "droneEnemy" || _name == "droneBoss")
        {
            spriteCategory = 3;
            GetComponent<Image>().sprite = droneSprites[0];
        }
        else if(_name == "player")
        {
            spriteCategory = 4;
            GetComponent<Image>().sprite = pigeonSprites[0];
        }
        else if(_name == "collectables")
        {
            spriteCategory = 5;
            GetComponent<Image>().sprite = collectablesSprites[0];
        }
        else
        {
            spriteCategory = 6;
            GetComponent<Image>().sprite = otherSprites[0];
        }

    }

    void NextFrame()
    {
        currentFrame++;
        if (currentFrame >= spriteList[spriteCategory].Length)
        {
            currentFrame = 0;
        }
        GetComponent<Image>().sprite = spriteList[spriteCategory][currentFrame];
    }
    void Update()
    {   
        timer+=1;
        if(timer >= 25)
        {
            timer = 0;
            NextFrame();
        }
    }
    // private IEnumerator PrintOneEveryThreeSeconds()
    // {
    //     while (true)
    //     {
    //         yield return new WaitWhile(() => Time.timeScale != 0f);
    //         yield return new WaitForSeconds(0.1f);
    //         NextFrame();
    //     }
    // }
}
