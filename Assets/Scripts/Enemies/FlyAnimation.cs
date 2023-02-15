using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyAnimation : MonoBehaviour
{
    public float speed = 0.0001f;
    public float maxScale = 1f;
    public float minScale = 0.75f;
    private bool _pause;
    private Vector3 _scaleChange;

    // Start is called before the first frame update
    void Start()
    {
        _scaleChange = new Vector3(-speed, -speed, -speed);
        _pause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_pause)
        {
            if(transform.localScale.x < minScale || transform.localScale.x > maxScale)
            {
                _scaleChange = -_scaleChange;
            }
        
            transform.localScale += _scaleChange;
        }
    }

    /// <summary>
    /// Pauses animation
    /// </summary>
    public void pause()
    {
        _pause = true;
    }

    /// <summary>
    /// Resumes animation
    /// </summary>
    public void resume()
    {
        _pause = false;
    }
}
