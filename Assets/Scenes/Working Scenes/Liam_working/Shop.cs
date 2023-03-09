using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Shop : MonoBehaviour
{
    public GameObject itemButtonPrefab;
    public GameObject contentTransform;

    public List<ShopItem> itemList = new List<ShopItem>();

    void Start()
    {
        int x = PlayerPrefs.GetInt("itemDataCount");
        if(x > 0)
        {
            for(int i = 0; i < x; i++)
            {
                string itemData = PlayerPrefs.GetString("itemData"+i);
                itemList.Add(JsonUtility.FromJson<ShopItem>(itemData));
            }
        }
        else
        {
            itemList.Add(new ShopItem("Increase Battery", 10, "Upgrade your battery and keep your lazer powered for longer!", ""));
            itemList.Add(new ShopItem("Increase Health", 10, "Boost your health and increase your chances of survival!", ""));
            itemList.Add(new ShopItem("Increase Damage", 10, "Upgrade your weapon and deal more destruction than ever before.", ""));

        }

        DisplayItemButtons();
    }

    void OnApplicationQuit()
    {
        for(int i = 0; i < itemList.Count; i++)
        {
            PlayerPrefs.SetString("itemData"+i, JsonUtility.ToJson(itemList[i]));
        }
        PlayerPrefs.SetInt("itemDataCount", itemList.Count);
        
    }


    public void DisplayItemButtons()
{
    foreach (ShopItem item in itemList)
    {
        GameObject buttonObject = Instantiate(itemButtonPrefab, contentTransform.transform);

        Text itemNameText = buttonObject.transform.Find("ItemName").GetComponent<Text>();
        Text itemPriceText = buttonObject.transform.Find("ItemPrice").GetComponent<Text>();
        Image itemSprite = buttonObject.transform.Find("ItemImage").GetComponent<Image>();
        Text itemDescriptionText = buttonObject.transform.Find("ItemDescription").GetComponent<Text>();
        Button buyButton = buttonObject.transform.Find("BuyButton").GetComponent<Button>();

        itemNameText.text = item.name;
        itemPriceText.text = "$" + item.price.ToString();
        itemSprite.sprite = Resources.Load<Sprite>(item.sprite);
        itemDescriptionText.text = item.description;

        buyButton.onClick.AddListener(() => BuyItem(item, itemPriceText));
    }
}


public void BuyItem(ShopItem item, Text itemPriceText)
{
    item.price = Mathf.RoundToInt(item.price * 1.1f);
    itemPriceText.text = "$" + item.price.ToString();
}
}

[Serializable]
public class ShopItem
{
    public string name;
    public int price;
    public string description;
    public string sprite;

    public ShopItem(string name, int price, string description, string sprite)
    {
        this.name = name;
        this.price = price;
        this.description = description;
        this.sprite = sprite;
    }
}