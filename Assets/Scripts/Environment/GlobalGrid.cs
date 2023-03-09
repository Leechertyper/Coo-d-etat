using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGrid : MonoBehaviour
{
    // the size of a room prefab
    [Header("The size of a room")]
    [SerializeField] private Vector2Int roomSize = new Vector2Int(17, 10);

    // the offset of the rooms
    [Header("The offset between each room")]
    [SerializeField] private Vector2Int roomOffset = new Vector2Int(6,2);

    // the amount of rooms in the floor in vector form
    [Header("The dimesions of the floor")]
    [SerializeField] private Vector2Int floorDimesions = new Vector2Int(3, 5);

    // the scale of the tiles
    [Header("The scaling of the tiles")]
    [SerializeField] private int tileScale = 3;

    // temp for testing
    [SerializeField] private GameObject tileObj;

    // the global game grid array
    private GlobalTile[,] _grid;

    // the size of the grid
    private Vector2Int _size;

    // the center of the rooms
    private List<List<int>> _roomCenters = new List<List<int>>();

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

        [Header("Is this the center of a room?")]
        public bool center = false;

        [Header("Is this a door?")]
        public bool door = false;

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
        _size.x = (roomSize.x * floorDimesions.x) + (roomOffset.x * (floorDimesions.x - 1));
        // gets an extra one cause the room has an open bottom
        _size.y = (roomSize.y * floorDimesions.y) + (roomOffset.y * (floorDimesions.y - 1) + 2);

        // initialize the grid
        _grid = new GlobalTile[_size.x, _size.y];
        
        // get the start pos
        Vector2 startPos = new Vector2(transform.position.x - ((roomSize.x - 1) / 2) * 3, transform.position.y + (2 * 3) + ((roomSize.y - 1) / 2) * 3);
        // sets the interator to the center of the parent
        Vector2 iterationPosition = startPos;

        // iterates through the width of the grid
        for (int i = 0; i < _size.x; i++)
        {
            // iterates through the length of the grid
            for (int j = 0; j < _size.y; j++)
            {
                // creates a new tile at pos (i, j) in the grid
                _grid[i, j] = new GlobalTile(iterationPosition);

                // TEMP MAKE A TILE
                GameObject tempTile = Instantiate(tileObj);
                tempTile.transform.position = iterationPosition;
                // iterates the y pos
                iterationPosition.y -= tileScale;

            }
            // resets the x position of the iterator to the first column and moves down a row
            iterationPosition = new Vector2(iterationPosition.x += tileScale, startPos.y);
        }

        // go through and find the centers of the rooms
        for (int i = 8; i < _size.x; i++)
        {
            for(int j = 6; j < _size.y; j++)
            {
                // add the coordinates into a temp list
                List<int> _t = new List<int>();
                _t.Add(i);
                _t.Add(j);

                // add the temp list into the room centers list
                _roomCenters.Add(_t);

                // mark the tile as center
                _grid[i, j].center = true;

                // add an offset value to j
                j += roomOffset.y + roomSize.y - 1;
            }

            // add an offset value to i
            i += roomOffset.x + roomSize.x - 1;
        }
        int index = 0;
        // now go through each room center and add the boundaries
        foreach(List<int> curList in _roomCenters)
        {
            // get the starting locations at the top left
            int xIter = curList[0];
            int yIter = curList[1];

            // run through the new 17x10 area - im only checking for the top wall and the offset space at the bottom
            for(int i = xIter - 8; i <= xIter + 8; i++)
            {
                for(int j = yIter - 4; j <= yIter + 4; j++)
                {
                    // if the current position is on the outside, block that tile
                    if (i == xIter - 8 || j == yIter - 4 || i == xIter + 8 || j == yIter + 4)
                    {
                        _grid[i, j].blocked = true;
                    }

                }
            }
            switch(index)
            {
                case 0:
                    _grid[xIter, yIter - 4].blocked = false;
                    _grid[xIter, yIter - 4].door = true;
                    break;
                case 1:
                    _grid[xIter, yIter - 4].blocked = false;
                    _grid[xIter, yIter - 4].door = true;
                    _grid[xIter, yIter + 4].blocked = false;
                    _grid[xIter, yIter + 4].door = true;
                    _grid[xIter + 8, yIter].blocked = false;
                    _grid[xIter + 8, yIter].door = true;
                    break;
                case 2:
                    _grid[xIter, yIter - 4].blocked = false;
                    _grid[xIter, yIter - 4].door = true;
                    _grid[xIter, yIter + 4].blocked = false;
                    _grid[xIter, yIter + 4].door = true;
                    _grid[xIter + 8, yIter].blocked = false;
                    _grid[xIter + 8, yIter].door = true;
                    break;
                case 3:
                    _grid[xIter, yIter + 4].blocked = false;
                    _grid[xIter, yIter + 4].door = true;
                    _grid[xIter + 8, yIter].blocked = false;
                    _grid[xIter + 8, yIter].door = true;
                    break;
                case 5:
                    _grid[xIter - 8, yIter].blocked = false;
                    _grid[xIter - 8, yIter].door = true;
                    _grid[xIter, yIter + 4].blocked = false;
                    _grid[xIter, yIter + 4].door = true;
                    _grid[xIter + 8, yIter].blocked = false;
                    _grid[xIter + 8, yIter].door = true;
                    break;
                case 6:
                    _grid[xIter - 8, yIter].blocked = false;
                    _grid[xIter - 8, yIter].door = true;
                    _grid[xIter, yIter + 4].blocked = false;
                    _grid[xIter, yIter + 4].door = true;
                    _grid[xIter, yIter - 4].blocked = false;
                    _grid[xIter, yIter - 4].door = true;
                    _grid[xIter + 8, yIter].blocked = false;
                    _grid[xIter + 8, yIter].door = true;
                    break;
                case 7:
                    _grid[xIter - 8, yIter].blocked = false;
                    _grid[xIter - 8, yIter].door = true;
                    _grid[xIter, yIter - 4].blocked = false;
                    _grid[xIter, yIter - 4].door = true;
                    _grid[xIter + 8, yIter].blocked = false;
                    _grid[xIter + 8, yIter].door = true;
                    break;
                case 9:
                    _grid[xIter, yIter + 4].blocked = false;
                    _grid[xIter, yIter + 4].door = true;
                    _grid[xIter - 8, yIter].blocked = false;
                    _grid[xIter - 8, yIter].door = true;
                    break;
                case 10:
                    _grid[xIter, yIter - 4].blocked = false;
                    _grid[xIter, yIter - 4].door = true;
                    _grid[xIter - 8, yIter].blocked = false;
                    _grid[xIter - 8, yIter].door = true;
                    _grid[xIter, yIter + 4].blocked = false;
                    _grid[xIter, yIter + 4].door = true;
                    break;
                case 11:
                    _grid[xIter, yIter - 4].blocked = false;
                    _grid[xIter, yIter - 4].door = true;
                    _grid[xIter - 8, yIter].blocked = false;
                    _grid[xIter - 8, yIter].door = true;
                    _grid[xIter, yIter + 4].blocked = false;
                    _grid[xIter, yIter + 4].door = true;
                    break;
                case 12:
                    _grid[xIter, yIter - 4].blocked = false;
                    _grid[xIter, yIter - 4].door = true;
                    break;
            }



            index++;
        }

        Debug.Log(GetRoomsAsString());
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetRoomsAsString()
    {
        string returnString = "    X\nY    ";
        for (int j = 0; j < _grid.GetLength(1); j++){
            for (int i = 0; i < _grid.GetLength(0); i++)
            {
                if(j == 0)
                {
                    returnString += " "+ i.ToString() + "  ";
                }
                else
                {
                    if (_grid[i, j-1].blocked)
                    {
                        returnString += "[b] ";
                    }
                    else if (_grid[i, j-1].center)
                    {
                        returnString += "[c] ";
                    }
                    else if (_grid[i, j - 1].door)
                    {
                        returnString += "[d] ";
                    }
                    else
                    {
                        returnString += "[_] ";
                    }
                }

            }
            returnString += "\n" + j.ToString() + "    ";
        }
        return returnString;
    }
}
