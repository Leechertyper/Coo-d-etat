using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using theNameSpace;

public class Lazer : MonoBehaviour
{

    public Rigidbody2D rb;
    private int _power = 1;
    private float _playerPower, _dronePower;

    public GameManager theGameManager;

    // I tried to use Game Object tags but I couldn't get it to work, so I use this emun  
    private theNameSpace.TheParentTypes _parentType;

    // When off screen Destroy Lazer
    // NOTE* this only deletes when off camera including the inspector camera
    private void Awake()
    {
       
        List<float> tempList =  theGameManager.GetPowerValues();
        //_playerPower = tempList[0];
        _playerPower = tempList[0];
        _dronePower = tempList[1];
        _parentType = theNameSpace.TheParentTypes.nullType;
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);   
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && this.gameObject.tag != "PlayerProjectile")
        {
            

            float tempVar = 0;
            switch(_parentType){
                case theNameSpace.TheParentTypes.droneType:
                    tempVar = _dronePower;
                    break;
                case theNameSpace.TheParentTypes.playerType:
                    Debug.LogWarning("Lazer Warning: a players lazer is attacking the player");
                    break;
                default:
                    // This warning should never go off, if it does something really bad has happened
                    Debug.LogWarning("Lazer Warning: A lazers Parent type has not been set, or defined in the switch statement"); 
                    break;
            }
            collision.gameObject.GetComponent<Player>().TakeDamage((int)tempVar);
            
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Enemy" && this.gameObject.tag == "PlayerProjectile")
        {   // Remove type casting later
            collision.gameObject.GetComponent<Health>().TakeDamage((int)_playerPower);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Boss" && this.gameObject.tag == "PlayerProjectile")
        {
            collision.gameObject.GetComponent<DroneBoss>().TakeDamage(_playerPower);
            Destroy(gameObject);
        }
    }
    public void SetPower(int power){
        //_power = power;
    }

    //I tried to use enum for this, but chatGPT was not helpful enough to get it done correct
    public void setParentType(theNameSpace.TheParentTypes theType)
    {
        if(_parentType == theNameSpace.TheParentTypes.nullType)
        {
            _parentType = theType;
        }
        else
        {
            Debug.LogWarning("Lazer Warning: tried to set a Parent Type when Parent Type of " + _parentType+(" is already set"));
        }
    }
}
