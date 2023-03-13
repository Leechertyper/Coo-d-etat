using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDogScript : MonoBehaviour
{

    public Animator theAnimator;

    private BoxCollider2D _theCollider;
    // Start is called before the first frame update
    void Start()
    {
        _theCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            theAnimator.SetTrigger("Start");
            //Debug.Log("Touched player");
        }
        else if(collider.gameObject.tag == "Enemy")
        {
            theAnimator.SetTrigger("Start");
        }
    }
}
