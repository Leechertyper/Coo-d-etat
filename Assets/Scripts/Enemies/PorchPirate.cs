using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorchPirate : Enemy
{

    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private GameObject projectile;
    private float currentHealth;
    private bool _attacking = false;
    private bool _moving = false;
    private Vector2Int _gridPos;
    private int _dir = 0;
    private bool _attackOnCooldown = false;
    private bool _altDirAttack = false;
    private bool _sleeping = false;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        GameManager.Instance.Grid.GetComponent<GlobalGrid>().GetTile(transform.position, out _gridPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_sleeping)
        {
            Move();
        }

    }

    /// <summary>
    /// Determines where to move
    /// </summary>
    /// <returns>Where to move</returns>
    private Vector2 CheckMove()
    {
        if (!_moving && !_attacking)
        {
            // if it is to the down by more than 2 tiles
            if (GameManager.Instance.GetPlayerObject().transform.position.y > transform.position.y + 6)
            {
                _dir = 0;
                _gridPos += new Vector2Int(0, -1);
                return GameManager.Instance.Grid.GetComponent<GlobalGrid>().TileLocation(transform.position, _gridPos);
            }
            // if it is to the up by more than 2 tiles
            else if (GameManager.Instance.GetPlayerObject().transform.position.y < transform.position.y - 6)
            {
                _dir = 1;
                _gridPos += new Vector2Int(0, 1);
                return GameManager.Instance.Grid.GetComponent<GlobalGrid>().TileLocation(transform.position, _gridPos);
            }
            // right
            else if (GameManager.Instance.GetPlayerObject().transform.position.x > transform.position.x + 6)
            {
                _dir = 3;
                _gridPos += new Vector2Int(1, 0);
                return GameManager.Instance.Grid.GetComponent<GlobalGrid>().TileLocation(transform.position, _gridPos);
            }
            // left
            else if (GameManager.Instance.GetPlayerObject().transform.position.x < transform.position.x - 6)
            {
                _dir = 2;
                _gridPos += new Vector2Int(-1, 0);
                return GameManager.Instance.Grid.GetComponent<GlobalGrid>().TileLocation(transform.position, _gridPos);
            }
            else
            {
                GetComponent<Animator>().SetBool("Moving", false);
                if (!_attackOnCooldown)
                {
                    Attack();
                }
                
            }
        }
        return GameManager.Instance.Grid.GetComponent<GlobalGrid>().TileLocation(transform.position, _gridPos);
    }

    /// <summary>
    /// Moves the Pirate
    /// </summary>
    private void Move()
    {
        if (!_moving && !_attacking)
        {
            var destination = CheckMove();
            if (transform.position.Equals(destination))
            {
                return;
            }
            else
            {
                GetComponent<Animator>().SetInteger("Direction", _dir);
                GetComponent<Animator>().SetBool("Moving", true);
                _moving = true;
                StartCoroutine(Move(destination));
            }
        }

    }

    public void TakeDamage(float damage)
    {
        // Damage sound here
        AkSoundEngine.PostEvent("Play_Pigeon_Hurt", this.gameObject);
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Starts the attack for the pirate and starts a timer for the next one
    /// </summary>
    private void Attack()
    {
        _attackOnCooldown = true;
        _attacking = true;
        GetComponent<Animator>().SetBool("Attack", true);
        StartCoroutine(AttackTimer());
    }

    /// <summary>
    /// Spawns a box at the current position that moves in the given direction
    /// </summary>
    /// <param name="dir">The direction the box should go</param>
    private void SpawnBox(Vector2 dir)
    {

    }

    public override void Die()
    {
        // Death sound here
        AkSoundEngine.PostEvent("Play_Pirate_Dead", this.gameObject);
        GameObject.Find("ScoreManager").GetComponent<Score>().AddScore(100);
        Destroy(gameObject);
    }

    public override void Awaken()
    {
        // would be funny if he made a "huh?" noise when he awoke
        AkSoundEngine.PostEvent("Play_Pirate_Noise", this.gameObject);
        _sleeping = false;
    }

    public override void Sleep()
    {
        _sleeping = true;
        StopAllCoroutines();
        _moving = false;
        _attacking = false;
        _attackOnCooldown = false;
        GetComponent<Animator>().SetBool("Attack", false);
        GetComponent<Animator>().SetBool("Moving", false);
    }

    private void ThrowBoxes()
    {
        // Kirk add sounds here for attack
        AkSoundEngine.PostEvent("Play_Tornado", this.gameObject); 
        if (_altDirAttack)
        {
            _altDirAttack = false;
            SpawnBox(new Vector2(5, 5));
            SpawnBox(new Vector2(-5, 5));
            SpawnBox(new Vector2(-5, -5));
            SpawnBox(new Vector2(5, -5));
        }
        else
        {
            _altDirAttack = true;
            SpawnBox(new Vector2(10, 0));
            SpawnBox(new Vector2(-10, 0));
            SpawnBox(new Vector2(0, 10));
            SpawnBox(new Vector2(0, -10));
        }
    }

    private IEnumerator Move(Vector2 move)
    {
        float timeElapsed = 0;
        float runTime = .5f;

        Vector3 inital = transform.position;
        Vector3 goal = new Vector3(move.x, move.y, 0);
        Debug.Log(inital + "" + goal);
        while (timeElapsed < runTime)
        {
            transform.position = Vector2.Lerp(inital, goal, timeElapsed / runTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = goal;
        _moving = false;
        yield break;
    }

    IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(3);
        GetComponent<Animator>().SetBool("Attack", false);
        _attacking = false;
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(6);
        _attackOnCooldown = false;
    }


    public override float GetHealthVariable()
    {
        return BalanceVariables.pirateEnemy["maxHealth"] * (0.5f + (Mathf.Sqrt(GameManager.Instance.getLevelNum()) / 2));
    }

    IEnumerator BoxAttack()
    {
        yield return new WaitForSeconds(1);
        ThrowBoxes();

    }

    public override void TakeDamage()
    {
        
    }
}
