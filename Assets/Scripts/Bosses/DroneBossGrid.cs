using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBossGrid : MonoBehaviour
{
    // The size of the room (assuming rooms are perfect squares)
    [SerializeField] private Vector2 roomDimensions;

    // The size of the tiles in the grid
    [SerializeField] private int gridStep;

    // The starting position of the grid
    [SerializeField] private Vector2 gridStartPosition;

    // ----TEMP---- this is to show the grid in the worldspace
    [SerializeField] private GameObject floorTile;

    // The Patterns for a bomb attack, uses a arraylist the first parameter is list of tile indexes and the second is the attacks spawn weight
    [Header("The air attack for the enemy, takes two parameters. First is the index of the tiles to be affected and the second is the attacks weight")]
    [SerializeField] private AttackPattern[] attacks;

    // the object to spawn
    [Header("The attack object prefab")]
    [SerializeField] private GameObject targetObject;

    [SerializeField] Canvas bossLabel;

    [SerializeField] Canvas healthBars;

    [SerializeField] Canvas winText;

    // The tile grid
    private Tile[,] _grid;

    // the current attackPattern in use
    private AttackPattern _currentAttack;

    // the index of the current attack in the attacks list
    private int _currentAttackIndex;

    public bool isAttacking = false;


    /// <summary>
    /// A 2D array of Tiles
    /// </summary>
    public Tile[,] Grid { get => _grid; set => _grid = value; }



    /// <summary>
    /// A tile that stores positional information and an activity status
    /// </summary>
    public class Tile
    {
        private float _x;
        private float _y;
        private bool _active = false;

        /// <summary>
        /// Whether or not the tile is currently being used
        /// </summary>
        public bool Active { get => _active; set => _active = value; }

        /// <summary>
        /// The tile constructor
        /// </summary>
        /// <param name="pos">The tiles initial position</param>
        public Tile(Vector2 pos)
        {
            _x = pos.x;
            _y = pos.y;
        }

        /// <summary>
        /// Gets the x position of the tile
        /// </summary>
        /// <returns>The x position of the tile</returns>
        public float GetX()
        {
            return _x;
        }

        /// <summary>
        /// Gets the y position of the tile
        /// </summary>
        /// <returns>The tiles y position</returns>
        public float GetY()
        {
            return _y;
        }

        /// <summary>
        /// Sets the x position of the tile
        /// </summary>
        /// <param name="newX">The new x position of the tile</param>
        public void SetX(int newX)
        {
            _x = newX;
        }

        /// <summary>
        /// Sets the y positon of the tile
        /// </summary>
        /// <param name="newY">The new y position</param>
        public void SetY(int newY)
        {
            _y = newY;
        }

        /// <summary>
        /// Returns a Vector2 containing the tiles x and y position
        /// </summary>
        /// <returns>A Vector2 containing the tiles x and y position</returns>
        public Vector2 GetPosAsVector()
        {
            return new Vector2(_x, _y);
        }

        /// <summary>
        /// Sets the position of tile by taking in a Vector2
        /// </summary>
        /// <param name="newPos">A Vector2 containing the new x and y position of the tile</param>
        public void SetPosAsVector(Vector2 newPos)
        {
            _x = newPos.x;
            _y = newPos.y;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.Instance)
        {
            gridStartPosition.x = 116.5f;
            gridStartPosition.y = -145.5f;
        }
        else
        {
            gridStartPosition.x = gridStartPosition.x*gridStep;
            gridStartPosition.y = gridStartPosition.y*gridStep;
        }
        // sets the interator to the start position of the grid
        Vector2 iterationPosition = new Vector2(gridStartPosition.x, gridStartPosition.y);
        // initializes the grid
        _grid = new Tile[(int) roomDimensions.x, (int) roomDimensions.y];
        // iterates through the width of the grid
        for (int i = 0; i < (int) roomDimensions.x; i++)
        {
            // iterates through the length of the grid
            for (int j = 0; j < (int) roomDimensions.y; j++)
            {
                // creates a new tile at pos (i, j) in the grid
                _grid[i, j] = new Tile(iterationPosition);
                // ***TEMP*** creates a floor tile object at the tiles position
                //Instantiate(floorTile);
                // ***TEMP*** sets the floor tiles position to the tile position at (i, j)
                //floorTile.transform.position = _grid[i, j].GetPosAsVector();
                // changes the current x position of the tiles by gridstep to the next column
                iterationPosition.y += gridStep;

            }
            // resets the x position of the iterator to the first column and moves down a row
            iterationPosition = new Vector2(iterationPosition.x += gridStep, gridStartPosition.y);
        }
    }

    public void StartBombAttack()
    {
        _currentAttackIndex = (int) Mathf.Round(Random.Range(0, attacks.Length));
        _currentAttack = attacks[_currentAttackIndex];
        for(int i = 0; i < _currentAttack.Positions.Length; i++)
        {
            GameObject newAttackTarget = Instantiate(targetObject);
            Vector2 pos2D = _grid[(int)_currentAttack.Positions[i].x, (int)_currentAttack.Positions[i].y].GetPosAsVector();
            newAttackTarget.transform.position = new Vector3(pos2D.x, pos2D.y, -2);
        }

        if (_currentAttack.HasFollowingAttack)
        {
            StartCoroutine(DelayNextAttack(_currentAttack.NextAttackDelay));
        }
        else
        {
            Debug.Log("Attack Finished");
            isAttacking = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entity Entered");
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.GetChild(0).GetComponent<DroneBoss>().Awaken();
        }
    }

    public void NextBombAttack()
    {
        _currentAttack = _currentAttack.NextAttack;
        for (int i = 0; i < _currentAttack.Positions.Length; i++)
        {
            GameObject newAttackTarget = Instantiate(targetObject);
            Vector2 pos2D = _grid[(int)_currentAttack.Positions[i].x, (int)_currentAttack.Positions[i].y].GetPosAsVector();
            newAttackTarget.transform.position = new Vector3(pos2D.x, pos2D.y, -2);
        }

        if (_currentAttack.HasFollowingAttack)
        {
            StartCoroutine(DelayNextAttack(_currentAttack.NextAttackDelay));
        }
        else
        {
            Debug.Log("Attack Finished");
            isAttacking = false;
        }
    }

    IEnumerator DelayNextAttack(float attackDelay)
    {
        yield return new WaitForSeconds(attackDelay);
        NextBombAttack();
    }

}
