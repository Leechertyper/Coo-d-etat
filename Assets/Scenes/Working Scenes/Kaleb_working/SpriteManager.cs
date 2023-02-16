using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] spriteArray;

    public void Up()
    {
        spriteRenderer.sprite = spriteArray[0];
    }

    public void Side()
    {
        spriteRenderer.sprite = spriteArray[1];
    }

    public void Down()
    {
        spriteRenderer.sprite = spriteArray[2];
    }
}
