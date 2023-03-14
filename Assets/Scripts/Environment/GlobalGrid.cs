using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGrid : MonoBehaviour
{
    private class Mover
    {
        // if the movement is done
        public bool isDone = false;

        /// <summary>
        /// Moves an entity from one location to another over time
        /// </summary>
        /// <param name="entity">the entity that will be moved</param>
        /// <param name="endValue">the location after the lerp</param>
        /// <param name="duration">how long it takes in seconds</param>
        /// <returns></returns>
        public IEnumerator LerpFunction(GameObject entity, Vector2 endValue, float duration)
        {
            float time = 0;
            Vector2 startValue = entity.transform.position;
            while (time < duration)
            {
                entity.transform.position = Vector2.Lerp(startValue, endValue, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            entity.transform.position = endValue;
            isDone = true;
        }
    }

    /// <summary>
    /// Global Tile Class - Holds data for each tile in the global grid
    /// </summary>
    public class GlobalTile
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

    // the size of a room prefab
    [Header("The size of a room")]
    [SerializeField] private Vector2Int roomSize = new Vector2Int(17, 10);

    // the offset of the rooms
    [Header("The offset between each room")]
    [SerializeField] private Vector2Int roomOffset = new Vector2Int(6,2);

    // the amount of rooms in the floor in vector form
    [Header("The dimesions of the floor")]
    [SerializeField] private Vector2Int floorDimesions = new Vector2Int(3, 3);

    // the scale of the tiles
    [Header("The scaling of the tiles")]
    [SerializeField] private int tileScale = 3;

    // if you want the grid to be printed out
    [Header("Whether or not to print out the grid")]
    [SerializeField] private bool printGrid;

    // the global game grid array
    private GlobalTile[,] _grid;

    // the size of the grid
    private Vector2Int _size;

    // the center of the rooms
    private List<List<int>> _roomCenters = new List<List<int>>();

    // the current offset
    private Vector2Int _currentOffset = new Vector2Int(0,0);



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
        Vector2 startPos = new Vector2(transform.position.x - ((roomSize.x - 1) / 2) * tileScale, transform.position.y + (2 * tileScale) + ((roomSize.y - 1) / 2) * tileScale);
        // sets the interator to the center of the parent
        Vector2 iterationPosition = startPos;

        // iterates through the width of the grid
        for (int i = 0; i < _size.x; i++)
        {
            // iterates through the length of the grid
            for (int j = 0; j < _size.y; j++)
            {
                // creates a new tile at pos (i, j) in the grid
                _grid[i, j] = new GlobalTile (iterationPosition);

                // iterates the y pos
                iterationPosition.y -= tileScale;

            }
            // resets the x position of the iterator to the first column and moves down a row
            iterationPosition = new Vector2(iterationPosition.x += tileScale, startPos.y);
        }

        // go through and find the centers of the rooms
        // for y I know the offset is different, we need to shift the full size plus 1 because our room is placed under the room
        for (int i = roomSize.y / 2 + 1; i < _size.y; i++)
        {
            // since the rooms x isnt even, I need to floor the result to make it get the exact center 17 / 2 = 8.5 -> floor => 8
            for(int j = Mathf.FloorToInt(roomSize.x/2); j < _size.x; j++)
            {
                // add the coordinates into a temp list
                List<int> _t = new List<int>();
                _t.Add(j);
                _t.Add(i);


                // add the temp list into the room centers list
                _roomCenters.Add(_t);

                // mark the tile as center
                _grid[j, i].center = true;

                // add an offset value to j
                j += roomOffset.x + roomSize.x - 1;
            }

            // add an offset value to i
            i += roomOffset.y + roomSize.y - 1;
        }
        int index = 0;
        // now go through each room center and add the boundaries
        foreach (List<int> curList in _roomCenters)
        {
            // get the starting locations at the top left
            int xIter = curList[0];
            int yIter = curList[1];

            // run through the new 17x10 area - im only checking for the top wall and the offset space at the bottom
            for (int i = xIter - 8; i <= xIter + 8; i++)
            {
                for (int j = yIter - 4; j <= yIter + 4; j++)
                {
                    // if the current position is on the outside, block that tile
                    if (i == xIter - 8 || j == yIter - 4 || i == xIter + 8 || j == yIter + 4)
                    {
                        _grid[i, j].blocked = true;
                    }

                }
            }
            // for each room I need to place doors based on where it is
            // top left corner
            if (index == 0)
            {
                _grid[xIter + 8, yIter].blocked = false;
                _grid[xIter + 8, yIter].door = true;
                _grid[xIter, yIter + 4].blocked = false;
                _grid[xIter, yIter + 4].door = true;
            }
            // top right corner
            else if (index == floorDimesions.x - 1)
            {
                _grid[xIter - 8, yIter].blocked = false;
                _grid[xIter - 8, yIter].door = true;
                _grid[xIter, yIter + 4].blocked = false;
                _grid[xIter, yIter + 4].door = true;
            }
            // bottom left corner
            else if (index == floorDimesions.x * floorDimesions.y - (floorDimesions.x))
            {
                _grid[xIter + 8, yIter].blocked = false;
                _grid[xIter + 8, yIter].door = true;
                _grid[xIter, yIter - 4].blocked = false;
                _grid[xIter, yIter - 4].door = true;
            }
            // bottom right corner
            else if (index == floorDimesions.x * floorDimesions.y - 1)
            {
                _grid[xIter - 8, yIter].blocked = false;
                _grid[xIter - 8, yIter].door = true;
                _grid[xIter, yIter - 4].blocked = false;
                _grid[xIter, yIter - 4].door = true;
            }
            // anywhere on the top
            else if (index < floorDimesions.x)
            {
                _grid[xIter, yIter + 4].blocked = false;
                _grid[xIter, yIter + 4].door = true;
                _grid[xIter + 8, yIter].blocked = false;
                _grid[xIter + 8, yIter].door = true;
                _grid[xIter - 8, yIter].blocked = false;
                _grid[xIter - 8, yIter].door = true;
            }
            // anywhere on the bottom
            else if(index >= floorDimesions.x * floorDimesions.y - floorDimesions.x)
            {
                _grid[xIter, yIter - 4].blocked = false;
                _grid[xIter, yIter - 4].door = true;
                _grid[xIter + 8, yIter].blocked = false;
                _grid[xIter + 8, yIter].door = true;
                _grid[xIter - 8, yIter].blocked = false;
                _grid[xIter - 8, yIter].door = true;
            }
            // this is anywhere on the left side
            else if ((index + 1) % 4 == 0)
            {
                _grid[xIter + 8, yIter].blocked = false;
                _grid[xIter + 8, yIter].door = true;
                _grid[xIter, yIter + 4].blocked = false;
                _grid[xIter, yIter + 4].door = true;
                _grid[xIter, yIter - 4].blocked = false;
                _grid[xIter, yIter - 4].door = true;
            }
            // anywhere on the right
            else if ((index + 1) % 3 == 0)
            {
                _grid[xIter - 8, yIter].blocked = false;
                _grid[xIter - 8, yIter].door = true;
                _grid[xIter, yIter + 4].blocked = false;
                _grid[xIter, yIter + 4].door = true;
                _grid[xIter, yIter - 4].blocked = false;
                _grid[xIter, yIter - 4].door = true;
            }
            // else its in the middle
            else
            {
                _grid[xIter - 8, yIter].blocked = false;
                _grid[xIter - 8, yIter].door = true;
                _grid[xIter + 8, yIter].blocked = false;
                _grid[xIter + 8, yIter].door = true;
                _grid[xIter, yIter + 4].blocked = false;
                _grid[xIter, yIter + 4].door = true;
                _grid[xIter, yIter - 4].blocked = false;
                _grid[xIter, yIter - 4].door = true;
            }

            index++;
        }
        Debug.Log(GetRoomsAsString());
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// creates a string of the rooms and their status
    /// </summary>
    /// <returns>the string to print</returns>
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
                        returnString += "[D] ";
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

    /// <summary>
    /// Moves an entity to tile in the grid, if the move is not possible, then it will not make that move.
    /// </summary>
    /// <param name="entity">the entity to be moved</param>
    /// <param name="currentPosition">the current tile the entity is on</param>
    /// <param name="destination">the tile to move to</param>
    /// <param name="diagonalAllowed">if the entity is allowed to travel diagonally</param>
    /// <returns>the new current position of this entity in the grid</returns>
    public Vector2Int MoveToTile(GameObject entity, Vector2Int currentPosition, Vector2Int destination, bool diagonalAllowed = false)
    {
        Mover mover = new Mover();
        // check the destination tile
        if (!_grid[destination.x, destination.y].blocked)
        {
            StartCoroutine(mover.LerpFunction(entity, _grid[destination.x, destination.y].position, 2f));
            while (!mover.isDone)
            {
                // do nothing just waiting
            }
            return destination;
        } 
        // if its blocked
        else
        {
            // just return where they started
            return currentPosition;
        }
    }


    public Vector2 TileLocation(Vector3 currentPosition, Vector2Int destination)
    {
        // check the destination tile
        if (!_grid[destination.x, destination.y].blocked)
        {
            return _grid[destination.x, destination.y].position;
        }
        // if its blocked
        else
        {
            // just return where they started
            return currentPosition;
        }
    }

    public Vector2 GetTile(Vector3 pos, out Vector2Int gridPos)
    {
        float bestDis = 10000;
        Vector2 worldPos = new Vector2(0, 0);
        gridPos = new Vector2Int(0, 0);
        for(int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
               if(Vector2.Distance(_grid[x, y].position, pos) < bestDis)
                {
                    bestDis = Vector2.Distance(_grid[x, y].position, pos);
                    worldPos = _grid[x, y].position;
                    gridPos = new Vector2Int(x, y);
                }
            }
        }
        return worldPos;
    }


    /// <summary>
    /// Instantiates and places the given item in the room representing the index
    /// </summary>
    /// <param name="item">the item to be placed</param>
    /// <param name="roomIndex">the index of the room </param>
    public void PlaceIteminRoom(GameObject item, int roomIndex)
    {
        // grab the room being checked
        List<int> room = _roomCenters[roomIndex];
        // grab the center coordinates
        Vector2Int roomCoordinates = new Vector2Int(room[0], room[1]);
        // init empty list
        List<Vector2Int> freeTiles = new List<Vector2Int>();
        // get the top row, if there is a door above, dont include it
        for(int i = roomCoordinates.x - Mathf.FloorToInt(roomSize.x/2) - 1; i < roomCoordinates.x + Mathf.FloorToInt(roomSize.x / 2) - 1; i++)
        {
            if(!_grid[i, (roomCoordinates.y - roomSize.y/2) - 1].door)
            {
                freeTiles.Add(new Vector2Int(i, (roomCoordinates.y - roomSize.y / 2) - 2));
            }
        }
        int randomNum = Random.Range(0, freeTiles.Count);
        GameObject newItem = Instantiate(item);
        newItem.transform.position = _grid[freeTiles[randomNum].x, freeTiles[randomNum].y].position;
    }

    /// <summary>
    /// Instantiates and places an enemy in the given indexed room
    /// </summary>
    /// <param name="enemy">the enemy to be placed</param>
    /// <param name="roomIndex">the index of the room </param>
    public void PlaceEnemyinRoom(GameObject enemy, int roomIndex)
    {
        // grab the room being checked
        List<int> room = _roomCenters[roomIndex];
        // grab the center coordinates
        Vector2Int roomCoordinates = new Vector2Int(room[0], room[1]);
        // init empty list
        List<Vector2Int> freeTiles = new List<Vector2Int>();
        // get the top row, if there is a door above, dont include it
        for (int i = roomCoordinates.x - Mathf.FloorToInt(roomSize.x / 2) - 2; i < roomCoordinates.x + Mathf.FloorToInt(roomSize.x / 2) - 2; i++)
        {
            for(int j = roomCoordinates.y - Mathf.FloorToInt(roomSize.y/2) - 2; j < roomCoordinates.y + Mathf.FloorToInt(roomSize.y / 2) - 2; j++)
            {
                freeTiles.Add(new Vector2Int(i, j));
            }

        }
        int randomNum = Random.Range(0, freeTiles.Count);
        GameObject newEnemy = Instantiate(enemy);
        newEnemy.transform.position = _grid[freeTiles[randomNum].x, freeTiles[randomNum].y].position;
    }
}
