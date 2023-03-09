using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGrid : MonoBehaviour
{
    // the size of a room prefab
    [Header("The size of a room")]
    [SerializeField] private Vector2Int roomSize = new Vector2Int(15, 7);

    // the offset of the rooms
    [Header("The offset between each room")]
    [SerializeField] private int roomOffset = 1;

    // the amount of rooms in the floor in vector form
    [Header("The dimesions of the floor")]
    [SerializeField] private Vector2Int floorDimesions = new Vector2Int(3, 3);

    // the scale of the tiles
    [Header("The scaling of the tiles")]
    [SerializeField] private int tileScale = 3;

    // the global game grid array
    private GlobalTile[,] _grid;

    // the size of the grid
    private Vector2Int _size;

    // the center of the rooms
    private List<List<int>> _roomCenters;

    // the current offset
    private Vector2Int _currentOffset = new Vector2Int(0,0);


    /// <summary>
    /// Global Tile Class - Holds data for each tile in the global grid
    /// </summary>
    public class GlobalTile : MonoBehaviour
    {
        [Header("The current position")]
        public Vector2 position;

        [Header("Can the player move onto this tile")]
        public bool blocked = false;

        /// <summary>
        /// Global tile constructor
        /// </summary>
        /// <param name="position">the position of the tile in the world</param>
        /// <param name="blocked">whether or not this tile is player accessible</param>
        public GlobalTile(Vector2 position, bool blocked = false)
        {
            this.position = position;
            this.blocked = blocked;
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        // calculating the total size of the grid
        // start with the amount of rooms and the size of each one
        _size.x = (roomSize.x * floorDimesions.x) + (roomOffset * (floorDimesions.x - 1));
        _size.y = (roomSize.y * floorDimesions.y) + (roomOffset * (floorDimesions.y - 1));

        // initialize the grid
        _grid = new GlobalTile[_size.x, _size.y];

        // sets the interator to the top left corner of the parent
        Vector2 iterationPosition = new Vector2(transform.position.x - _size.x/2, transform.position.y - _size.y/2);

        // iterates through the width of the grid
        for (int i = 0; i < _size.x; i++)
        {
            // iterates through the length of the grid
            for (int j = 0; j < _size.y; j++)
            {
                // creates a new tile at pos (i, j) in the grid
                _grid[i, j] = new GlobalTile(iterationPosition);

                // iterates the y pos
                iterationPosition.y += tileScale;

            }
            // resets the x position of the iterator to the first column and moves down a row
            iterationPosition = new Vector2(iterationPosition.x += tileScale, transform.position.y);
        }

        // go through and find the centers of the rooms
        for (int i = 8; i < _size.x; i += 9)
        {
            for(int j = 6; j < _size.y; j += 10)
            {
                // add the coordinates into a temp list
                List<int> _t = new List<int>();
                _t.Add(i);
                _t.Add(j);

                // add the temp list into the room centers list
                _roomCenters.Add(_t);

                // add an offset value to j
                j++;
            }

            // add an offset value to i
            i++;
        }

        // now go through each room center and add the boundaries
        foreach(List<int> curList in _roomCenters)
        {
            // get the starting locations at the top left
            int xIter = curList[0];
            int yIter = curList[1];

            // run through the new 17x10 area - im only checking for the top wall and the offset space at the bottom
            for(int i = xIter - 8; i < xIter + 8; i++)
            {
                for(int j = yIter - 4; j < yIter + 4; j++)
                {
                    // if the current position is on the outside, block that tile
                    if (i == 0 || j == 0 || i == 16 || j == 10)
                    {
                        _grid[i, j].blocked = true;
                    }
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
