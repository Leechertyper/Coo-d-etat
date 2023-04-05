using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopObject : MonoBehaviour
{
    [SerializeField] private GameObject shopManager;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.GetPlayerObject().GetComponent<Player>().returnShop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            shopManager.GetComponent<Shop>().shopUI.SetActive(true);
            Time.timeScale = 0;
        }
    }
}