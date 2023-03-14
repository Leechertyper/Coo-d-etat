using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAI : Enemy
{


    public Animator animator;
    public Health hp;

    private GlobalGrid _grid;
    private Vector2Int _gridPos;
    private Transform _target;
    private state _myState;
    private float _distX;
    private float _distY;
    enum state { Dash, Leap, Next };

    // Start is called before the first frame update
    void Start()
    {
        _myState = state.Next;
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        animator.SetInteger("Direction", 2);
        _grid = GameManager.Instance.Grid.GetComponent<GlobalGrid>();
        _gridPos = new Vector2Int(8, 4);
        transform.position = _grid.TileLocation(transform.position, new Vector2Int(8, 4));
        
        //_grid.MoveToTile(gameObject, new Vector2Int(5,5), new Vector2Int(5,6));
    }

    // Update is called once per frame
    void Update()
    {
/*        if (_myState == state.Next)
        {

        }

        if(_myState == state.Dash)
        {
            _myState = state.Next;
            _distX = _target.transform.position.x - transform.transform.position.x;
            _distY = _target.transform.position.y - transform.transform.position.y;
            if (Mathf.Abs(_distX) > Mathf.Abs(_distY))
            {
                if (_distX > 0)
                {
                    StartCoroutine(Dash(1, 2));
                }
                else
                {
                    StartCoroutine(Dash(3, 2));
                }
            }
            else
            {
                if (_distY > 0)
                {
                    StartCoroutine(Dash(0, 2));
                }
                else
                {
                    StartCoroutine(Dash(2, 2));
                }
            }
        }*/
/*        if (_myState == state.Leap)
        {
            StartCoroutine(LeapAtPlayer());
            _myState = state.Dash;
        }
        if (Vector2.Distance(_target.position, transform.position) > 20)
        {
            _myState = state.Leap;
        }*/



        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            StartCoroutine(Dash(0, 2));
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            StartCoroutine(Dash(1, 2));
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            StartCoroutine(Dash(2, 2));
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            StartCoroutine(Dash(3, 2));
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(LeapAtPlayer());
        }
    }


    private IEnumerator Dash(int direction, int distance)
    {
        animator.SetInteger("Direction", direction);
        animator.SetBool("IsRunning", true);
        distance *= 3;
        float timeElapsed = 0;
        float runTime = 1f;
        Vector3 move;
        switch (direction)
        {
            case 0:
                _gridPos += new Vector2Int(0, 1);
                move = _grid.TileLocation(transform.position, _gridPos);
                break;
            case 1:
                _gridPos += new Vector2Int(1, 0);
                move = _grid.TileLocation(transform.position, _gridPos);
                break;
            case 2:
                _gridPos += new Vector2Int(0, -1);
                move = _grid.TileLocation(transform.position, _gridPos);
                break;
            case 3:
                _gridPos += new Vector2Int(-1, 0);
                move = _grid.TileLocation(transform.position, _gridPos);
                break;
            default:
                move = new Vector3(0, 0, 0);
                break;
        }

        Vector3 inital = transform.position;
        Vector3 goal = new Vector3(move.x, move.y, 0);
        Debug.Log(goal);
        while (timeElapsed < runTime)
        {
            transform.position = Vector2.Lerp(inital, goal, timeElapsed / runTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = goal;
        animator.SetBool("IsRunning", false);
        yield break;
    }

    private IEnumerator LeapAtPlayer()
    {
        Vector3 player = _target.transform.position;
        float timeElapsed = 0;
        float runTime = 1f;
        Vector3 inital = transform.position;
        while (timeElapsed < runTime)
        {
            transform.position = SampleParabola(inital, player, 2, timeElapsed / runTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = player;
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

    public override void Die()
    {
        StopAllCoroutines();
        this.enabled = false;
    }

    public override void Awaken()
    {
       
    }

    public override void Sleep()
    {
        StopAllCoroutines();
        //_rb.velocity = Vector2.zero;
    }
}
