using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Shop : MonoBehaviour
{
    public GameObject itemButtonPrefab;
    public GameObject contentTransform;
    public int playerBalance = 0;
    public Text playerBalanceText;

    public static List<ShopItem> shopItemList = new List<ShopItem>();
    private List<ShopItem> starterShopItemList = new List<ShopItem>(new ShopItem[]{
        new ShopItem("Increase Battery", 10, "Upgrade your battery and keep your lazer powered for longer!",1f, ""),
        new ShopItem("Increase Health", 10, "Boost your health and increase your chances of survival!",1f, ""),
        new ShopItem("Increase Damage", 10, "Upgrade your weapon and deal more destruction than ever before.",1f, ""),
    });

    // Start is called before the first frame update will create the shop items and display them
    void Start()
    {
        int _savedShopItemCount = PlayerPrefs.GetInt("itemDataCount");
        int playerBalance = PlayerPrefs.GetInt("playerBalance");
        for (int i = 0; i < _savedShopItemCount; i++)
        {
            string itemData = PlayerPrefs.GetString("itemData" + i);
            shopItemList.Add(JsonUtility.FromJson<ShopItem>(itemData));
        }
        CheckForNewItem();
        CheckForOldRemovedItem();
        DisplayItemButtons();
    }


    public void CloseShop()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    private void DisplayPlayerBalance()
    {
        playerBalanceText.text = "Balance: " + playerBalance;
    }

    public void OpenShop()
    {
        this.gameObject.SetActive(true);
        Time.timeScale = 0f;
        DisplayPlayerBalance();
        
    }

    // will check if there is a new item added to the shop
    private void CheckForNewItem()
    {
        bool exists = false;
        foreach (ShopItem starterItem in starterShopItemList)
        {
            foreach (ShopItem item in shopItemList)
            {
                if (item.name == starterItem.name)
                {
                    exists = true;
                    break;
                }
            }
            if (!exists)
            {
                shopItemList.Add(starterItem);
            }
            exists = false;
        }
    }

    // will get the item value
    public float GetItemValue(string itemName)
    {
        foreach (ShopItem item in shopItemList)
        {
            if (item.name == itemName)
            {
                return item.value;
            }
        }
        return 0;
    }

    // will get the player balance
    public int GetPlayerBalance()
    {
        return playerBalance;
    }

    // will add money to the player balance
    public void AddMoney(int amount)
    {
        playerBalance += amount;
    }

    // will check if there is an item removed from the shop
    private void CheckForOldRemovedItem()
    {
        for (int i = 0; i < shopItemList.Count; i++)
        {
            bool exists = false;
            foreach (ShopItem starterItem in starterShopItemList)
            {
                if (shopItemList[i].name == starterItem.name)
                {
                    exists = true;
                    break;
                }
            }
            if (!exists)
            {
                shopItemList.RemoveAt(i);
            }
        }
    }

    // will save the shop items when the game is closed
    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("playerBalance", playerBalance);
        PlayerPrefs.SetInt("itemDataCount", shopItemList.Count);
        for(int i = 0; i < shopItemList.Count; i++)
        {
            PlayerPrefs.SetString("itemData"+i, JsonUtility.ToJson(shopItemList[i]));
        } 
    }

    // will display the shop items
    public void DisplayItemButtons()
    {
        foreach (ShopItem item in shopItemList)
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

    // will buy the item and increase the price
    public void BuyItem(ShopItem item, Text itemPriceText)
    {
        if (playerBalance >= item.price)
        {
            playerBalance -= item.price;
            item.price = Mathf.RoundToInt(item.price * 1.5f);
            itemPriceText.text = "$" + item.price.ToString();
            item.value += 0.1f;
            DisplayPlayerBalance();
        }
    }
}


[Serializable]
public class ShopItem
{
    public string name;
    public int price;
    public string description;
    public string sprite;
    public float value;

    public ShopItem(string name, int price, string description, float value, string sprite)
    {
        this.name = name;
        this.price = price;
        this.description = description;
        this.sprite = sprite;
        this.value = value;

    }
}