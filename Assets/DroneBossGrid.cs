using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    // The tile grid
    private Tile[,] _grid;



    /**
     * Tile Class
     * 
     * Used for storing Grid coordinates and any other information that the grid may need a tile to hold
     * 
     */
    public class Tile
    {
        private int _x;
        private int _y;
        private bool _active = false;

        /**
         * Tile
         * Initializes a new Tile
         * 
         * pos - the position of the tile in the worldspace
         */
        public Tile(Vector2 pos)
        {
            _x = (int) pos.x;
            _y = (int) pos.y;
        }

        /**
         * GetX
         * 
         * return - the x position of the tile
         */
        public int GetX()
        {
            return _x;
        }

        /**
         * GetY
         * 
         * return - the y position of the tile
         */
        public int GetY()
        {
            return _y;
        }

        /**
         * SetX
         * 
         * newX - the updated x position of the tile
         */
        public void SetX(int newX)
        {
            _x = newX;
        }

        /**
         * SetY
         * 
         * newY - the updated y position of the tile
         */
        public void SetY(int newY)
        {
            _y = newY;
        }

        /**
         * GetPosAsVector
         * 
         * return - the position of the tile as a Vector2
         */
        public Vector2 GetPosAsVector()
        {
            return new Vector2(_x, _y);
        }

        /**
         * SetPosAsVector
         * 
         * newPos - the new x and y positions of the tile as a Vector2
         */
        public void SetPosAsVector(Vector2 newPos)
        {
            _x = (int) newPos.x;
            _y = (int) newPos.y;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Vector2 iterationPosition = gridStartPosition;
        _grid = new Tile[roomDimensions, roomDimensions];
        for(int i = 0; i < roomDimensions; i++)
        {
            for(int j = 0; j < roomDimensions; j++)
            {
                _grid[i, j] = new Tile(iterationPosition);
                Instantiate(floorTile);
                floorTile.transform.position = _grid[i, j].GetPosAsVector();
                Debug.Log(floorTile.transform.position);
                iterationPosition.x += gridStep;

            }
            iterationPosition = new Vector2(gridStartPosition.x, iterationPosition.y += gridStep);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
