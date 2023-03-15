using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxLerp : MonoBehaviour
{

    public GameObject deathItems;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Test2(new Vector3(10,-8,0)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Test(Vector3 goal)
    {
        float timeElapsed = 0;
        float halfTime = 2;
        Vector3 inital = transform.position;
        while (timeElapsed < halfTime)
        {
            transform.position = Vector3.Lerp(inital, goal, timeElapsed / halfTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        inital = transform.position;
        while (timeElapsed < halfTime)
        {
            transform.position = Vector3.Lerp(inital, goal, timeElapsed / halfTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = goal;
        yield break;
    }

    private IEnumerator Test2(Vector3 goal)
    {
        AkSoundEngine.PostEvent("Play_Missle_Whistle", this.gameObject);
        float timeElapsed = 0;
        float runTime = 1f;
        Vector3 inital = transform.position;
        while (timeElapsed < runTime)
        {
            transform.position = SampleParabola(inital, goal, 2, timeElapsed/runTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = goal;
        deathItems.GetComponent<deathItems>().JustSpawnBattery(goal);
        Destroy(gameObject);
        yield break;
    }
    Vector3 SampleParabola(Vector3 start, Vector3 end, float height, float t)
    {
        float parabolicT = t * 2 - 1;
        if (Mathf.Abs(start.y - end.y) < 0.1f)
        {
            //start and end are roughly level, pretend they are - simpler solution with less steps
            Vector3 travelDirection = end - start;
            Vector3 result = start + t * travelDirection;
            result.y += (-parabolicT * parabolicT + 1) * height;
            return result;
        }
        else
        {
            //start and end are not level, gets more complicated
            Vector3 travelDirection = end - start;
            Vector3 levelDirecteion = end - new Vector3(start.x, end.y, start.z);
            Vector3 right = Vector3.Cross(travelDirection, levelDirecteion);
            Vector3 up = Vector3.Cross(right, travelDirection);
            if (end.y > start.y)
                up = -up;
            Vector3 result = start + t * travelDirection;
            result += ((-parabolicT * parabolicT + 1) * height) * up.normalized;
            return result;
        }
    }

    public void Throw(Vector3 target)
    {
        StartCoroutine(Test2(target));
    }
}
