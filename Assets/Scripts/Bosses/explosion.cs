using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    [SerializeField] private GameObject parent;


    private int _updateIterator = 0;

    private bool _frame2 = false;


    /// <summary>
    /// When the animation ends, destroy self
    /// </summary>
    public void OnTimeOut()
    {
        //check if player is in explosion
        Destroy(parent);
    }

    public void Start()
    {
        Destroy(GetComponent<CircleCollider2D>(), .1f);
    }

    /// <summary>
    /// Called every frame
    /// </summary>
    private void Update()
    {
        //if(_updateIterator == 2)
        //{
        //    _frame2 = true;
        //} else
        //{
        //    _updateIterator++;
        //}
        //if (_frame2)
        //{
        //    Destroy(GetComponent<CircleCollider2D>());
        //    _frame2 = false;
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(BalanceVariables.droneBoss["explosionDamage"]);
        }
    }

}
