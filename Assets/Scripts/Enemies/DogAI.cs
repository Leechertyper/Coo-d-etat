using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAI : Enemy
{



    public Animator animator;
    public Health hp;

    [SerializeField] private int leapDistance = 5;
    private GlobalGrid _grid;
    private Vector2Int _gridPos;
    private Transform _target;
    [SerializeField] private state _myState;
    private float _distX;
    private float _distY;
    private bool _awake;
    private bool _moveUP = true;
    private bool _attackReady = true;
    enum state {Attact, PaceUp, PaceDown, Next, Wait}


    public override void Awaken()
    {
        _awake = true;
        _myState = state.PaceUp;
    }

    public override void Sleep()
    {
        StopAllCoroutines();
        _awake = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        _awake = false;
        _myState = state.Next;
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        animator.SetInteger("Direction", 2);
        _grid = GameManager.Instance.Grid;
        _gridPos = _grid.GetTileFromPos(transform.position);
        BalanceVariables.seenDictionaries["dogEnemy"] = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_awake) return;
        switch (_myState)
        {
            case state.Next:
                if ((_target.transform.position.y - transform.transform.position.y) == 0 & _attackReady)
                {
                    _myState = state.Attact;
                } 
                else 
                {
                    if (_moveUP)
                    {
                        _myState = state.PaceUp;
                    }
                    else
                    {
                        _myState = state.PaceDown;
                    }
                }
                break;

            case state.PaceUp:
                _myState = state.Wait;
                if (_grid.TileOpen(_grid.GetTileFromPos(transform.position + (Vector3.up * 3))))
                {
                    StartCoroutine(Move(0, 1));
                }
                else
                {
                    _moveUP = false;
                    _myState = state.Next;
                }
                break;

            case state.PaceDown:
                _myState = state.Wait;
                if (_grid.TileOpen(_grid.GetTileFromPos(transform.position + (Vector3.down * 3))))
                {
                    StartCoroutine(Move(2, 1));
                }
                else
                {
                    _moveUP = true;
                    _myState = state.Next;
                }
                break;

            case state.Attact:
                _myState = state.Wait;
                int tempPos = 1;
                if((_target.transform.position.x - transform.transform.position.x) > 0)
                {
                    while (_grid.TileOpen(_grid.GetTileFromPos(transform.position + ((Vector3.right * 3) * tempPos ))))
                    {
                        tempPos++;
                    }
                    StartCoroutine(Attack(1, tempPos - 1, 0.25f));
                }
                else
                {
                    while (_grid.TileOpen(_grid.GetTileFromPos(transform.position + ((Vector3.left * 3) * tempPos))))
                    {
                        tempPos++;
                    }
                    StartCoroutine(Attack(3, tempPos - 1, 0.25f));
                }
                break;

            case state.Wait:
                break;

            default:
                _myState = state.Next;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (_attackReady)
            {
                collision.gameObject.GetComponent<Player>().TakeDamage(2);
                StopAllCoroutines();
                transform.position = _grid.GetTile(transform.position);
                animator.SetBool("IsRunning", false);
                StartCoroutine(AttackCooldown(2));
                _myState = state.Next;
            }
            else
            {

            }
        }
    }

    private IEnumerator AttackCooldown(float time)
    {
        _attackReady = false;
        while (true)
        {
            yield return new WaitForSeconds(time);
            _attackReady = true;
        }
    }

    private IEnumerator Attack(int direction, int distance, float speed)
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
                move = transform.position + (Vector3.up * distance);
                break;
            case 1:
                move = transform.position + (Vector3.right * distance);
                break;
            case 2:
                move = transform.position + (Vector3.down * distance);
                break;
            case 3:
                move = transform.position + (Vector3.left * distance);
                break;
            default:
                move = new Vector3(0, 0, 0);
                break;

        }
        Vector3 inital = transform.position;
        Vector3 goal = move;
        // Debug.Log(goal);
        while (timeElapsed < runTime)
        {
            transform.position = Vector2.Lerp(inital, goal, timeElapsed / runTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = _grid.GetTile(goal);
        animator.SetBool("IsRunning", false);
        _myState = state.Next;
        yield break;
    }

    private IEnumerator Move(int direction, int distance)
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
                move = transform.position + (Vector3.up * distance);
                break;
            case 1:
                move = transform.position + (Vector3.right * distance);
                break;
            case 2:
                move = transform.position + (Vector3.down * distance);
                break;
            case 3:
                move = transform.position + (Vector3.left * distance);
                break;
            default:
                move = new Vector3(0, 0, 0);
                break;

        }
        Vector3 inital = transform.position;
        Vector3 goal = move;
       // Debug.Log(goal);
        while (timeElapsed < runTime)
        {
            transform.position = Vector2.Lerp(inital, goal, timeElapsed / runTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = _grid.GetTile(goal);
        animator.SetBool("IsRunning", false);
        _myState = state.Next;
        yield break;
    }

    public override void Die()
    {
        GameObject.Find("ScoreManager").GetComponent<Score>().AddScore(100);
        StopAllCoroutines();
        this.enabled = false;
    }

    public override void TakeDamage()
    {

    }
}
