using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBossGrid : MonoBehaviour
{
    // The size of the room (assuming rooms are perfect squares)
    [SerializeField] private int roomDimensions;

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

    // The tile grid
    private Tile[,] _grid;

    // the current attackPattern in use
    private AttackPattern _currentAttack;

    // the index of the current attack in the attacks list
    private int _currentAttackIndex;

    private bool _isAttacking = false;



    /// <summary>
    /// A tile that stores positional information and an activity status
    /// </summary>
    public class Tile
    {
        private int _x;
        private int _y;
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
            _x = (int)pos.x;
            _y = (int)pos.y;
        }

        /// <summary>
        /// Gets the x position of the tile
        /// </summary>
        /// <returns>The x position of the tile</returns>
        public int GetX()
        {
            return _x;
        }

        /// <summary>
        /// Gets the y position of the tile
        /// </summary>
        /// <returns>The tiles y position</returns>
        public int GetY()
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
            _x = (int)newPos.x;
            _y = (int)newPos.y;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Vector2 iterationPosition = gridStartPosition;
        _grid = new Tile[roomDimensions, roomDimensions];
        for (int i = 0; i < roomDimensions; i++)
        {
            for (int j = 0; j < roomDimensions; j++)
            {
                _grid[i, j] = new Tile(iterationPosition);
                Instantiate(floorTile);
                floorTile.transform.position = _grid[i, j].GetPosAsVector();
                iterationPosition.x += gridStep;

            }
            iterationPosition = new Vector2(gridStartPosition.x, iterationPosition.y += gridStep);
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
            _isAttacking = false;
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
            _isAttacking = false;
        }
    }

    IEnumerator DelayNextAttack(float attackDelay)
    {
        yield return new WaitForSeconds(attackDelay);
        NextBombAttack();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isAttacking)
        {
            _isAttacking = true;
            StartBombAttack();
        }
    }
}
