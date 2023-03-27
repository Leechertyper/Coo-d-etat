using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointBalanceTimer : MonoBehaviour
{
    public int counter = 0;

    private static PointBalanceTimer _instance;

    public static PointBalanceTimer Instance { get { return _instance; } }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        StartCoroutine(IncrementCounter());
    }

    IEnumerator IncrementCounter()
    {
        while (true)
        {
            yield return new WaitForSeconds(BalanceVariables.other["balancePointTimerSeconds"]); // wait for 5 minutes
            if(SceneManager.GetActiveScene().buildIndex ==2 )
            {
                counter++;
            }
        }
    }
}