using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBoss : MonoBehaviour
{
    // player reference. Will be removed when gameManager is added
    [SerializeField] private GameObject player;

    // the rooms boss grid
    [SerializeField] private DroneBossGrid grid;

    // enemies projectile object
    [SerializeField] private GameObject throwablePackage;

    // enemies base movespeed
    [SerializeField] private float moveSpeed = 0.01f;

    // enemys base health
    [SerializeField] private float health = 1000;

    //enemy will start using its special attack when it gets lower than a multiple of this number
    private float _healthIntervals = 250;

    private bool _inPosition = false;

    private bool _inMotion = false;

    private Vector2 _targetPos;

    private int _moves = 5;

    private int _movesLeft;


    // Start is called before the first frame update
    void Start()
    {
        _movesLeft = _moves;
        StartCoroutine(TimeUntilNextDirectAttack());
    }

    // Update is called once per frame
    void Update()
    {
        if (_inMotion)
        {
            transform.position = Vector2.Lerp(transform.position, _targetPos, moveSpeed * Time.deltaTime);
            if(Mathf.Round(transform.position.x) == _targetPos.x && Mathf.Round(transform.position.y) == _targetPos.y)
            {
                _inPosition = true;
            }
        }
        
    }

    /// <summary>
    /// Will move the boss to a tile at a rapid speed, will do this "fakes" # of times
    /// </summary>
    private void FakeMoveToTile()
    {
        if (!_inMotion)
        {
            Debug.Log("Moving...");
            _targetPos = grid.Grid[(int)Mathf.Round(Random.Range(0, grid.Grid.GetLength(0))), (int)Mathf.Round(Random.Range(0, grid.Grid.GetLength(1)))].GetPosAsVector();
            Debug.Log(_targetPos);
            _inMotion = true;
            _movesLeft--;
            StartCoroutine(Moving());
        }
    }

    /// <summary>
    /// Moves the enemy to a tile where they will begin attacking
    /// </summary>
    /// <param name="tilePos">The index of the tile to move to</param>
    private void MoveToTile(Vector2 tilePos)
    {

    }

    /// <summary>
    /// Will begin the bosses direct attack
    /// </summary>
    private void DirectAttack()
    {

    }

    /// <summary>
    /// Will begin the bosses AOE attack
    /// </summary>
    private void MassAttack()
    {

    }

    /// <summary>
    /// Will deal damage to the boss
    /// </summary>
    /// <param name="damage">The amount of damage to deal to the boss</param>
    private void TakeDamage(float damage)
    {

    }

    /// <summary>
    /// Waits until the player is done moving to a tile
    /// </summary>
    /// <returns></returns>
    IEnumerator Moving()
    {
        yield return new WaitUntil(() =>_inPosition);
        _inMotion = false;
        _inPosition = false;
        if(_movesLeft > 0)
        {
            FakeMoveToTile();
        } 
        else
        {
            _movesLeft = _moves;
            StartCoroutine(TimeUntilNextDirectAttack());
        }
    }

    /// <summary>
    /// Starts a timer for the next mass attack
    /// </summary>
    /// <returns></returns>
    IEnumerator TimeUntilNextDirectAttack()
    {
        yield return new WaitForSeconds(5);
        FakeMoveToTile();
    }
}
