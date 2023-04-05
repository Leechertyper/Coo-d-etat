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
    public GameObject shopUI;
    private static Shop instance;
    public static List<ShopItem> shopItemList = new List<ShopItem>();
    private List<ShopItem> starterShopItemList = new List<ShopItem>(new ShopItem[]{
        new ShopItem("Increase Battery", 100, "Upgrade your battery and keep your lazer powered for longer!",1f),
        new ShopItem("Increase Health", 100, "Boost your health and increase your chances of survival!",1f),
        new ShopItem("Increase Damage", 100, "Upgrade your weapon and deal more destruction than ever before.",1f),
    });

    private void Awake()
    {
        if (instance == null) // If there is no instance already
        {
            DontDestroyOnLoad(gameObject); // bugs the game with this line
            instance = this;


        }
        else if (instance != this) // If there is already an instance and it's not `this` instance
        {
            Destroy(gameObject); // Destroy the GameObject, this component is attached to
        }
    }
    void Update()
    {

    }

    /* 
    * Start is called before the first frame update will create the shop items and display them
    */
    void Start()
    {

        shopUI.SetActive(false);
        shopItemList.Clear();
        CheckForNewItem();
        CheckForOldRemovedItem();
        DisplayItemButtons();

    }

    /*
    * This returns the damage multiplier
    */
    public float GetDamageMultiplier()
    {
        foreach (ShopItem item in shopItemList)
        {
            if (item.name == "Increase Damage")
            {
                return item.value;
            }
        }
        return 1;
    }

    /*
    * This returns the health multiplier
    */
    public float GetHealthMultiplier()
    {
        foreach (ShopItem item in shopItemList)
        {
            if (item.name == "Increase Health")
            {
                return item.value;
            }
        }
        return 1;
    }

    /*
    * This returns the battery multiplier
    */
    public float GetBatteryMultiplier()
    {
        foreach (ShopItem item in shopItemList)
        {
            if (item.name == "Increase Battery")
            {
                return item.value;
            }
        }
        return 1;
    }

    /*
    * This will close the shop's ui
    */
    public void CloseShop()
    {
        shopUI.SetActive(false);
        Time.timeScale = 1f;
        GameManager.Instance.inGame = true;
    }

    /*
    * This will display balance for the players points
    */
    private void DisplayPlayerBalance()
    {
        playerBalanceText.text = "Balance: " + GameObject.Find("ScoreManager").GetComponent<Score>().GetScore().ToString();
    }

    /*
    * This will open the shop's ui
    */
    public void OpenShop()
    {
        shopUI.SetActive(true);
        Time.timeScale = 0f;
        DisplayPlayerBalance();
        GameManager.Instance.inGame = false;

    }

    /*
    *  will check if there is a new item added to the shop
    */
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

    /*
    * will get the item value
    */
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

    /*
    * will check if there is an item removed from the shop
    */
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

    /*
    * will display the shop items
    */
    public void DisplayItemButtons()
    {
        foreach (ShopItem item in shopItemList)
        {
            GameObject buttonObject = Instantiate(itemButtonPrefab, contentTransform.transform);

            Text itemNameText = buttonObject.transform.Find("ItemName").GetComponent<Text>();
            Text itemPriceText = buttonObject.transform.Find("ItemPrice").GetComponent<Text>();
            Text itemDescriptionText = buttonObject.transform.Find("ItemDescription").GetComponent<Text>();
            Button buyButton = buttonObject.transform.Find("BuyButton").GetComponent<Button>();

            itemNameText.text = item.name;
            itemPriceText.text =  item.price.ToString() + " Points";
            itemDescriptionText.text = item.description;

            buyButton.onClick.AddListener(() => BuyItem(item, itemPriceText));
        }
    }

    /*
    * will buy the item and increase the price
    */
    public void BuyItem(ShopItem item, Text itemPriceText)
    {
        if (GameObject.Find("ScoreManager").GetComponent<Score>().GetScore() >= item.price)
        {
            GameObject.Find("ScoreManager").GetComponent<Score>().AddScore(-item.price);
            item.price = Mathf.RoundToInt(item.price * 1.5f);
            itemPriceText.text = item.price.ToString() + " Points";
            item.value += 0.1f;
            DisplayPlayerBalance();
        }
    }
}

/*
* This is the shop item class
*/
[Serializable]
public class ShopItem
{
    public string name;
    public int price;
    public string description;
    public float value;

    public ShopItem(string name, int price, string description, float value)
    {
        this.name = name;
        this.price = price;
        this.description = description;
        this.value = value;

    }
}