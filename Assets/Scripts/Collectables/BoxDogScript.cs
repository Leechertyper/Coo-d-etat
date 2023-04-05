using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDogScript : MonoBehaviour
{

    public Animator theAnimator;
    private Renderer _theRenderer;

    private GameObject _thisObject;
    private bool _isTriggered = false;
    
    private bool _playerHit = false;
    private float _decreaseTime = 0.1f;

    private float _alpha = 1f;

    private float _decressAlpha = 0.025f;

    private GameObject _deathItems;




    private BoxCollider2D _theCollider;
    // Start is called before the first frame update
    void Start()
    {
        _theCollider = GetComponent<BoxCollider2D>();
        _theRenderer = GetComponent<Renderer>();
        _thisObject = this.gameObject;
        _deathItems = GameObject.Find("DeathItems");

    }

    // public void sendDeathItems(GameObject DeathItemsToUse)
    // {
    //     _deathItems = DeathItemsToUse;
    // }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player" && !_isTriggered)
        {
            AkSoundEngine.PostEvent("Play_Cardboard_Box_Hit", this.gameObject);
            theAnimator.SetTrigger("Start");
            _isTriggered = true;
            _playerHit = true;
            StartCoroutine(FadeAway());
            //Debug.Log("Touched player");
        }
        else if(collider.gameObject.tag == "Enemy" && !_isTriggered)
        {
            //UnityEngine.Debug.Log("play sound in enemy");
            AkSoundEngine.PostEvent("Play_Cardboard_Box_Hit", this.gameObject);
            theAnimator.SetTrigger("Start");
            _isTriggered = true;
            StartCoroutine(FadeAway());
        }
        // {
        //     theAnimator.SetTrigger("Start");
            
        //     //Debug.Log("Touched player");
        // }
        // else if(collider.gameObject.tag == "Enemy")
        // {
        //     theAnimator.SetTrigger("Start");
        // }
    }


    IEnumerator FadeAway()
    {   
        bool spawnItem = false;
        while(true)
        {
            yield return new WaitForSeconds(_decreaseTime);
            _alpha -= _decressAlpha;
            _theRenderer.material.color = new Color(1, 1, 1, _alpha);
            if(_alpha <= 0.8f && !spawnItem && _playerHit)
            {
                spawnItem = true;
                _deathItems.GetComponent<deathItems>().SpawnItem(transform.position); 
                //Maybe add score?
            }
            if(_alpha <= 0)
            {
                Destroy(_thisObject);
            }
        }
    }

}
