using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DatabaseVisual : MonoBehaviour
{
    public GameObject connectedCanvas;
    // Start is called before the first frame update
    void Start()
    {
        connectedCanvas.SetActive(true);
            if(GameManager.dbInstance.GetHostFound() && PlayerPrefs.GetInt("BalanceDataBase") == 1)
            {
                connectedCanvas.GetComponentInChildren<Text>().GetComponent<Text>().text = "Connected To Database for Balance System";
            }
            else
            {
                connectedCanvas.GetComponentInChildren<Text>().GetComponent<Text>().text = "Could Not Connect To Database for Balance System";
            }
    }

    // Update is called once per frame
    void Update()
    {
        if(connectedCanvas.activeSelf)
        {
            StartCoroutine(databaseVisualTimer());
        }
    }
    private System.Collections.IEnumerator databaseVisualTimer()
    {
        yield return new WaitForSeconds(3);
        connectedCanvas.GetComponentInChildren<Text>().GetComponent<Text>().text = "";
        connectedCanvas.SetActive(false);
    }
}
