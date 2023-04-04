using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorchPirate : Enemy
{
    public PirateHealth hp;
    [SerializeField] private GameObject projectile;
    private float currentHealth;
    private bool _attacking = false;
    private bool _moving = false;
    private Vector2Int _gridPos;
    private GameObject _player;
    private int _dir = 0;
    private bool _attackOnCooldown = false;
    private bool _altDirAttack = false;
    private bool _sleeping = false;
    private GlobalGrid _grid;


    // Start is called before the first frame update
    void Start()
    {
        _grid = GameManager.Instance.Grid.GetComponent<GlobalGrid>();
        _player = GameManager.Instance.GetPlayerObject();
        _grid.GetTile(transform.position, out _gridPos);
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
    private Vector3 CheckMove()
    {
        if (!_moving && !_attacking)
        {
            // if it is to the down by more than 2 tiles
            if (_player.transform.position.y > transform.position.y + 6)// && _grid.TileOpen(_grid.GetTileFromPos(transform.position + (Vector3.up * 6))))
            {
                _dir = 0;
                GetComponent<Animator>().SetInteger("Direction", _dir);
                GetComponent<Animator>().SetBool("Moving", true);
                return transform.position + (Vector3.up * 3);
            }
            // if it is to the up by more than 2 tiles
            else if (_player.transform.position.y < transform.position.y - 6 )//&& _grid.TileOpen(_grid.GetTileFromPos(transform.position + (Vector3.down * 6))))
            {
                _dir = 1;
                GetComponent<Animator>().SetInteger("Direction", _dir);
                GetComponent<Animator>().SetBool("Moving", true);
                return transform.position + (Vector3.down * 3);
            }
            // right
            else if (_player.transform.position.x > transform.position.x + 6 && _grid.TileOpen(_grid.GetTileFromPos(transform.position + (Vector3.right * 6))))
            {
                _dir = 3;
                GetComponent<Animator>().SetInteger("Direction", _dir);
                GetComponent<Animator>().SetBool("Moving", true);
                return transform.position + (Vector3.right * 3);
            }
            // left
            else if (_player.transform.position.x < transform.position.x - 6 && _grid.TileOpen(_grid.GetTileFromPos(transform.position + (Vector3.left * 6))))
            {
                _dir = 2;
                GetComponent<Animator>().SetInteger("Direction", _dir);
                GetComponent<Animator>().SetBool("Moving", true);
                return transform.position + (Vector3.left * 3);
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
        return transform.position;
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
                _moving = true;
                StartCoroutine(Move(destination));
            }
        }

    }
    
    public void TakeDamage(float damage)
    {
        // Damage sound here
        AkSoundEngine.PostEvent("Play_Pirate_Noise", this.gameObject);
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
        StartCoroutine(BoxAttack());
    }

    /// <summary>
    /// Spawns a box at the current position that moves in the given direction
    /// </summary>
    /// <param name="dir">The direction the box should go</param>
    private void SpawnBox(int dir)
    {
        GameObject box = Instantiate(projectile);
        box.transform.position = transform.position;
        box.GetComponent<BoxLerp>().direction = dir;
        box.GetComponent<BoxLerp>().boxType = 1;

    }

    public override void Die()
    {
        // Death sound here
        AkSoundEngine.PostEvent("Play_Pirate_Dead", this.gameObject);
        GameObject.Find("ScoreManager").GetComponent<Score>().AddScore(100);
        StopAllCoroutines();
        this.enabled = false;
    }

    public override void TakeDamage()
    {

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
            SpawnBox(4);
            SpawnBox(5);
            SpawnBox(6);
            SpawnBox(7);
        }
        else
        {
            _altDirAttack = true;
            SpawnBox(0);
            SpawnBox(1);
            SpawnBox(2);
            SpawnBox(3);
        }
        if (_attacking)
        {
            StartCoroutine(BoxAttack());
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

<<<<<<< Updated upstream
    public override float GetHealthVariable()
    {
        return BalanceVariables.pirateEnemy["maxHealth"];
=======
    IEnumerator BoxAttack()
    {
        yield return new WaitForSeconds(1);
        ThrowBoxes();
>>>>>>> Stashed changes
    }
}
