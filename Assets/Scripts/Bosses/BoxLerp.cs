using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxLerp : MonoBehaviour
{

    private GameObject deathItems;
    // 0 = boss, 1 = enemy
    public int boxType;
    public int direction;
    private float _boxSpeed = 0.15f;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Test2(new Vector3(10,-8,0)));

        deathItems = GameObject.Find("DeathItems");
    }

    // Update is called once per frame
    void Update()
    {
        if(boxType == 1)
        {
            MoveInDirection(direction);
        }
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

    public void MoveInDirection(int direction)
    {
        switch (direction)
        {
            case 0:
                transform.position += ((Vector3.up) * _boxSpeed);
                GetComponent<Animator>().SetBool("goUp", true);
                break;
            case 1:
                transform.position += ((Vector3.down) * _boxSpeed);
                GetComponent<Animator>().SetBool("goDown", true);
                break;
            case 2:
                transform.position += ((Vector3.right) * _boxSpeed);
                GetComponent<Animator>().SetBool("goRight", true);
                break;
            case 3:
                transform.position += ((Vector3.left) * _boxSpeed);
                GetComponent<Animator>().SetBool("goLeft", true);
                break;
            case 4:
                transform.position += ((Vector3.up + Vector3.left) * _boxSpeed);
                GetComponent<Animator>().SetBool("goLeft", true);
                break;
            case 5:
                transform.position += ((Vector3.up + Vector3.right) * _boxSpeed);
                GetComponent<Animator>().SetBool("goRight", true);
                break;
            case 6:
                transform.position += ((Vector3.down + Vector3.right) * _boxSpeed);
                GetComponent<Animator>().SetBool("goRight", true);
                break;
            case 7:
                transform.position += ((Vector3.down + Vector3.left) * _boxSpeed);
                GetComponent<Animator>().SetBool("goLeft", true);
                break;
            default:
                break;
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && boxType == 1)
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(BalanceVariables.pirateEnemy["attackDamage"] * (Mathf.Sqrt(GameManager.Instance.getLevelNum())));
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Walls" && boxType == 1)
        {
            Destroy(gameObject);
        }
    }
}
