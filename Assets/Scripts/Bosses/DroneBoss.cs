using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneBoss : MonoBehaviour
{
    // player reference. Will be removed when gameManager is added
    [SerializeField] private GameObject player;

    // the rooms boss grid
    [SerializeField] private DroneBossGrid grid;

    // enemies projectile object
    [SerializeField] private GameObject throwablePackage;


    [SerializeField] private GameObject quickTarget;

    // enemies base movespeed
    [SerializeField] private float moveSpeed = 1f;

    // enemys base health
    [SerializeField] private float maxHealth = 100;
    
    [SerializeField] private float timeBetweenMoves = 5;

    [SerializeField] private Image healthBar;

    [SerializeField] private Image healthTrail;

    [SerializeField] private GameObject keycard;


    //enemy will start using its special attack when it gets lower than a multiple of this number
    private float _healthIntervals = 25;

    private float _currentHealth;

    private float _nextLargeAttack;
    
    private bool _inPosition = false;

    private bool _inMotion = false;

    private Vector2 _targetPos;

    private int _moves = 5;

    private int _movesLeft;

    private bool _healthChanging = false;

    private bool _healthTrailChanging = false;

    private float _time;

    private bool _massAttacking = false;

    private bool _dead = false;

    private float _attackDamage;


    // Start is called before the first frame update
    void Start()
    {
        
        _movesLeft = _moves;
        _currentHealth = maxHealth;
        _healthIntervals = maxHealth / 4;
        _nextLargeAttack = maxHealth - _healthIntervals;
        player = GameManager.Instance.GetPlayerObject();
    }

    /// <summary>
    /// Called when the player enters the room
    /// </summary>
    public void Awaken()
    {
        grid.GetComponent<Animator>().SetBool("AreaEntered", true);
        StartCoroutine(TimeUntilNextDirectAttack());
        //GameManager.Instance.SetBossStats();
    }

    /// <summary>
    /// Called when the player leaves the room
    /// </summary>
    public void Sleep()
    {
        StopAllCoroutines();
    }

    // Update is called once per frame
    void Update()
    {
        if (_healthChanging)
        {
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, _currentHealth/maxHealth, 3f * Time.deltaTime) ;
            if(Mathf.Round(healthBar.fillAmount * maxHealth) == Mathf.Round(_currentHealth)){
                _healthChanging = false;
                _healthTrailChanging = true;
            }
            else
            {
                _healthTrailChanging = false;
            }
        }

        if (_healthTrailChanging)
        {
            healthTrail.fillAmount = Mathf.Lerp(healthTrail.fillAmount, _currentHealth/maxHealth, 5f * Time.deltaTime);
            if (Mathf.Round(healthTrail.fillAmount * maxHealth) == Mathf.Round(_currentHealth))
            {
                _healthTrailChanging = false;
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
            _time = Time.realtimeSinceStartup + 1;
            StartCoroutine(LerpFunction(_targetPos, 0.1f));
            StartCoroutine(Moving());

        }
    }

    /// <summary>
    /// Will begin the bosses direct attack
    /// </summary>
    private void DirectAttack()
    {
        DroneBossGrid.Tile[,] _grid = grid.Grid;
        float distance = float.PositiveInfinity;
        DroneBossGrid.Tile target = _grid[0, 0];
        Vector2 posInGrid = new Vector2(0,0);
        for (int i = 0; i < grid.Grid.GetLength(0); i++)
        {
            for(int j = 0; j < grid.Grid.GetLength(1); j++)
            {
                if(distance > Vector2.Distance(player.transform.position, _grid[i, j].GetPosAsVector()))
                {
                    distance = Vector2.Distance(player.transform.position, _grid[i, j].GetPosAsVector());
                    target = _grid[i, j];
                    posInGrid = new Vector2(i, j);
                }
            }
        }
        GameObject projectile = Instantiate(throwablePackage);
        projectile.transform.position = transform.position;
        projectile.GetComponent<BoxLerp>().Throw(new Vector3(target.GetX(), target.GetY(), 0));
        SpawnTargets(target, posInGrid);
    }

    /// <summary>
    /// Spawns the start target for a basic attack then calls the Ring2Wait timer
    /// </summary>
    /// <param name="pos">the start tile</param>
    /// <param name="gridPos">position of the start tile in the grid</param>
    void SpawnTargets(DroneBossGrid.Tile pos, Vector2 gridPos)
    {
        Vector2 offsetX = new Vector2(-1, 1);
        Vector2 offsetY = new Vector2(-1, 1);
        DroneBossGrid.Tile[,] _grid = grid.Grid;
        GameObject target = Instantiate(quickTarget);
        target.transform.position = new Vector3(pos.GetX(), pos.GetY(), 0);
        //Debug.Log("X Offset: " + offsetX + "/nY Offset" + offsetY);
        StartCoroutine(Ring2Wait(gridPos, offsetX, offsetY));
    }

    /// <summary>
    /// Spawns the second ring of the mini attack
    /// </summary>
    /// <param name="gridPos">the position of the start tile in the grid</param>
    /// <param name="offsetX">the x offset to check if a target can spawn</param>
    /// <param name="offsetY">the y offset to check if a target can spawn</param>
    void SpawnRing2(Vector2 gridPos, Vector2 offsetX, Vector2 offsetY)
    {
        DroneBossGrid.Tile[,] _grid = grid.Grid;
        if (offsetX.y + gridPos.x >= _grid.GetLength(0))
        {
            offsetX.y = 0;
        }
        if (gridPos.x + offsetX.x < 0)
        {
            offsetX.x = 0;
        }
        if (offsetY.y + gridPos.y >= _grid.GetLength(1))
        {
            offsetY.y = 0;
        }
        if (gridPos.y + offsetY.x < 0)
        {
            offsetY.x = 0;
        }

        for (int i = (int)(gridPos.x + offsetX.x); i <= (gridPos.x + offsetX.y); i++)
        {
            for (int j = (int)(gridPos.y + offsetY.x); j <= (gridPos.y + offsetY.y); j++)
            {
                if(!Equals(new Vector2(i, j), gridPos)){
                    GameObject target = Instantiate(quickTarget);
                    target.transform.position = _grid[i, j].GetPosAsVector();
                }
                        
            }
        }
    }

    /// <summary>
    /// Will begin the bosses AOE attack
    /// </summary>
    private void MassAttack()
    {
        _massAttacking = true;
        grid.StartBombAttack();
        healthBar.color = new Color(1, 0.2028302f, 0.2028302f);
        StartCoroutine(MassAttackExit());

    }

    /// <summary>
    /// Will deal damage to the boss
    /// </summary>
    /// <param name="damage">The amount of damage to deal to the boss</param>
    public void TakeDamage(float damage)
    {
        if (!_massAttacking)
        {
            _currentHealth -= damage;
            _healthChanging = true;
            if (_currentHealth < _nextLargeAttack)
            {
                _nextLargeAttack -= _healthIntervals;
                MassAttack();
            }

            if (_currentHealth <= 0)
            {
                StopAllCoroutines();
                grid.GetComponent<Animator>().SetBool("BossDead", true);
                //GameObject key = Instantiate(keycard);
                //key.transform.position = transform.position;
                StartCoroutine(Death());

            }
        }

    }

    /// <summary>
    /// Waits until the player is done moving to a tile
    /// </summary>
    /// <returns></returns>
    IEnumerator Moving()
    {
        yield return new WaitUntil(() =>_inPosition);
        if (!_massAttacking)
        {
            _inMotion = false;
            _inPosition = false;
            if(_movesLeft > 0)
            {
                StartCoroutine(Pause(0.25f));
            } 
            else
            {
                _movesLeft = _moves;
                StartCoroutine(TimeUntilNextDirectAttack());
                DirectAttack();
            }
        }
        else
        {
            _movesLeft = _moves;
        }
    }

    /// <summary>
    /// Pauses for a set time before moving again
    /// </summary>
    /// <param name="duration">the amount of time to pause</param>
    /// <returns></returns>
    IEnumerator Pause(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (!_massAttacking)
        {
            FakeMoveToTile();
        }
        
    }

    /// <summary>
    /// Starts a timer for the next mass attack
    /// </summary>
    /// <returns></returns>
    IEnumerator TimeUntilNextDirectAttack()
    {
        yield return new WaitForSeconds(timeBetweenMoves);
        FakeMoveToTile();
    }

    /// <summary>
    /// After a mass attack is finished, this is called
    /// </summary>
    /// <returns></returns>
    IEnumerator MassAttackExit()
    {
        yield return new WaitUntil(() => !grid.isAttacking);
        _massAttacking = false;
        healthBar.color = new Color(1, 0.9116114f, 0.2877358f);
        StartCoroutine(TimeUntilNextDirectAttack());
    }

    /// <summary>
    /// A time that waits to spawn the second ring
    /// </summary>
    /// <param name="gridPos">the position of the start tile in the grid</param>
    /// <param name="offsetX">the x offset to check if a target can spawn</param>
    /// <param name="offsetY">the y offset to check if a target can spawn</param>
    /// <returns></returns>
    IEnumerator Ring2Wait(Vector2 gridPos, Vector2 offsetX, Vector2 offsetY)
    {
        yield return new WaitForSeconds(0.5f);
        SpawnRing2(gridPos, offsetX, offsetY);
        
    }

    /// <summary>
    /// Timer before destroying the boss
    /// </summary>
    /// <returns></returns>
    IEnumerator Death()
    {
        yield return new WaitForSeconds(1);
        Destroy(grid.gameObject);
    }

    /// <summary>
    /// Moves the boss from one point to another
    /// </summary>
    /// <param name="endValue">the target position</param>
    /// <param name="duration">the time it takes to get there</param>
    /// <returns></returns>
    IEnumerator LerpFunction(Vector2 endValue, float duration)
    {
        float time = 0;
        Vector2 startValue = transform.position;
        while (time < duration)
        {
            transform.position = Vector2.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        _inMotion = false;
        _inPosition = true;
        transform.position = endValue;
    }

    public void SetAttackSpeed(float newAttackSpeed)
    {
        timeBetweenMoves = newAttackSpeed;
    }

    public void SetDamage(float newAttackDamage)
    {
       _attackDamage = newAttackDamage; // Don't know what to do with this one
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        maxHealth = newMaxHealth;
    }

    public void SetMoveSpeed(float newMoveSpeed){
        moveSpeed = newMoveSpeed;
    }

}
