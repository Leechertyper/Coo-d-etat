using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyAnimation : MonoBehaviour
{
    public float speed = 0.0001f;
    public float maxScale = 1f;
    public float minScale = 0.75f;
    private Vector3 _scaleChange;

    // Start is called before the first frame update
    void Start()
    {
        _scaleChange = new Vector3(-speed, -speed, -speed);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.x < minScale || transform.localScale.x > maxScale)
        {
            _scaleChange = -_scaleChange;
        }
        
        transform.localScale += _scaleChange;
        
    }
}
