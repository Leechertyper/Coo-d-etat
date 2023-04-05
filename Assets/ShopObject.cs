using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopObject : MonoBehaviour
{
    [SerializeField] private Shop shopManager;
    // Start is called before the first frame update
    void Start()
    {
        shopManager = GameManager.Instance.GetPlayerObject().GetComponent<Player>().returnShop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("AYO WE TOUCHIN");
        if(collision.gameObject.CompareTag("Player"))
        {
            shopManager.GetComponent<Shop>().shopUI.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
