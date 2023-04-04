using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the constants for the floor offsets (SUBJECT TO CHANGE, I JUST PICKED THESE NUMBERS BECAUSE THEY WORKED)
/// </summary>
public static class FloorConstants
{
    public const float VerticalRoomOffset = 36f;
    public const float HorizontalRoomOffset = 69f;
    
    public const float TransitionSpeed = 0.25f;

    public const float VerticalPlayerOffset = 17f;
    public const float HorizontalPlayerOffset = 27f;

    public const float RoomSizeX = 48f;
    public const float RoomSizeY = 12f;
}

public class Floor : MonoBehaviour
{

    private int _floorXDimension;
    private int _floorYDimension;
    public List<List<Room>> _rooms;
    public Room.RoomType currentRoomType;
    public Room.FloorType currentFloorType;
    public Vector2Int currentRoom;


    [SerializeField] private List<GameObject> mainRooms;
    [SerializeField] private List<GameObject> endRooms;
    private int roomNum;

    private CameraController _camController;
    
    
    [SerializeField] private Camera miniMapCamera;

    private float _horizontalGap = 0;
    private float _verticalGap = 0;
    private bool _areGapsCalculated = false;


    [SerializeField] private List<GameObject> spawnableEnemies;

    [SerializeField] private List<GameObject> spawnableInteractables;
    [SerializeField] private GameObject charger;
    [SerializeField] private GameObject boss;

    private Room _bossRoomScript;
    
    
    
    

    private void Start()
    {
        _camController = Camera.main.GetComponent<CameraController>();
        _floorXDimension = 3;
        _floorYDimension = 3;
        roomNum = GameManager.Instance.getRoomNum() - 1;
        Debug.Log("I AM HERE, FLOOR START");
        SpawnRooms(_floorXDimension, _floorYDimension);
    }

    public void TESTING()
    {
        _camController = Camera.main.GetComponent<CameraController>();
        _floorXDimension = 3;
        _floorYDimension = 3;
        Debug.Log("I AM HERE, FLOOR TESTING");
        SpawnRooms(_floorXDimension, _floorYDimension);
    }

    /// <summary>
    /// Waits until the main grid has been generated before finishing the rooms
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForGrid()
    {
        Debug.Log("I AM HERE, WAITING FOR GRID");
        while (!GameManager.Instance.Grid.gridGenerated)
        {
            yield return null;
        }
        Debug.Log("I AM HERE, GRID IS OK");
        FinishRoomSetup();
    }
    /// <summary>
    /// Spawns a grid of rooms (r x c)
    /// </summary>
    /// <param name="r"> Number of Rows</param>
    /// <param name="c"> Number of Columns</param>
    private void SpawnRooms(int r, int c)
    {
        _rooms = new List<List<Room>>();
        for (var _ = 0; _ < c; _++)
        {
            _rooms.Add(new List<Room>()); // initializes the rooms sub-arrays
        }
        Debug.Log("I AM HERE, FLOOR SPAWN ROOMS"+ _rooms.Count);
        

        _rooms.Add(new List<Room>());// adds an extra list on the bottom for the end room
        for (var i = 0; i < r; i++)
        {
            for (var j = 0; j < c; j++)
            {
                var newRoom = Instantiate(mainRooms[roomNum], transform);
                var newRoomScript = newRoom.GetComponent<Room>();
                newRoomScript.InitializeRoom(i,j,_floorXDimension,_floorYDimension);
                newRoom.transform.position =
                    new Vector3(newRoom.transform.position.x + FloorConstants.HorizontalRoomOffset * j,
                        newRoom.transform.position.y - FloorConstants.VerticalRoomOffset * i);//places the rooms in the correct position on the grid
                _rooms[i].Add(newRoomScript);
            }
        }
        _rooms[0][0].SetRoomType(Room.RoomType.Start); // sets the start room to start type so no enemies spawn in it
        StartCoroutine(WaitForGrid());
        
    }


    private void FinishRoomSetup()
    {   
        Debug.Log("Finsihing Room Setup");
        while (true)
        {
            var randomX = Random.Range(0, 3);
            var randomY = Random.Range(0, 3);
            var bossRoom = _rooms[randomX][randomY];//randomly pick a room to be the boss room
            if (bossRoom.roomHasBeenInitialized) continue; // if room is already initialized then re-pick
            bossRoom.SetRoomType(Room.RoomType.Boss);//sets chosen room to boss room

            _bossRoomScript = bossRoom;
            GameManager.Instance.Grid.PlaceBossinRoom(boss, _floorXDimension * randomX + randomY);//places the boss in the center of the boss room
            break;
        }
        
        
        while (true)//same as above but for the charger room
        {
            var randomX = Random.Range(0, 3);
            var randomY = Random.Range(0, 3);
            var chargerRoom = _rooms[randomX][randomY];
            if (chargerRoom.roomHasBeenInitialized) continue;
            chargerRoom.SetRoomType(Room.RoomType.Charger);
            
            
            GameManager.Instance.Grid.PlaceIteminRoom(charger, _floorXDimension* randomX + randomY);
            break;
        }


        for (var i = 0; i < _floorXDimension; i++)//fills the rest of the rooms with enemies
        {
            for (var j = 0; j < _rooms[i].Count; j++)
            {
                int tempRandom = Random.Range(0, 100);
                int tempInterations = 0;
                //There is a 60% chance that a room will have 2 interactables, 25% chance that it will have 3, and 15% chance that it will have 1
                if(tempRandom < 60)
                {
                    tempInterations = 2;
                }
                else if(tempInterations < 85)
                {
                    tempInterations = 3;
                }
                else
                {
                    tempInterations = 1;
                }

        
                    
                    List<GameObject> tempList = new List<GameObject>();
                    for (int k = 0; k < tempInterations; k++)
                    {  
                        tempList.Add(spawnableInteractables[Random.Range(0, spawnableInteractables.Count)]);
                    }
                    GameManager.Instance.Grid.PlaceInteractableObjectinRoom(tempList, _floorXDimension * i + j, tempInterations);
                


                if (_rooms[i][j].roomHasBeenInitialized) continue;
                _rooms[i][j].SetRoomType(Room.RoomType.Enemy);
                var amountOfEnemiesInRoom = Random.Range(2, 3);

                for (var k = 0; k < amountOfEnemiesInRoom; k++)
                {
                    var randomEnemy = spawnableEnemies[Random.Range(0, spawnableEnemies.Count)];
            
                    var returnEnemy = GameManager.Instance.Grid.PlaceEnemyinRoom(randomEnemy,_floorXDimension*i + j);
                    _rooms[i][j].SpawnEnemy(returnEnemy.GetComponent<Enemy>()); 
                }
            }
        }




        _camController.MoveCameraToStart(_rooms[0][0].transform);//moves camera to the first room
        currentRoomType = _rooms[0][0].roomType;
        currentFloorType = _rooms[0][0].floorType;
        currentRoom = new Vector2Int(0, 0);
        changeTheme();
        var endRoom = Instantiate(endRooms[roomNum],transform);
        endRoom.transform.position = new Vector3(endRoom.transform.position.x + FloorConstants.HorizontalRoomOffset * (_floorXDimension-1),
            endRoom.transform.position.y - FloorConstants.VerticalRoomOffset * _floorYDimension);
        var endRoomScript = endRoom.GetComponent<Room>();
        endRoomScript.SetRoomType(Room.RoomType.EndRoom);

        _rooms[_floorXDimension].Add(null);
        _rooms[_floorXDimension].Add(null);//adds a couple nulls because lists dont allow for inserting at random indexes
        _rooms[_floorXDimension].Add(endRoomScript);
        _rooms[_floorXDimension-1][_floorYDimension-1].SetBottomDoorLocked(true);//locks the end room
        GameManager.Instance.SetEndRoomPos(new Vector2(endRoom.transform.position.x,endRoom.transform.position.y));
        var middleRoom = _rooms[1][1].transform.position;
        var miniMapCamPos = new Vector3(middleRoom.x,middleRoom.y, -10);//sets the minimap camera to be centered on the middle room ONLY WORKS ON 3x3
        miniMapCamera.transform.position = miniMapCamPos;
    }
    
    /// <summary>
    /// Moves the player and camera to the room above the current room
    /// </summary>
    /// <param name="player">The game object that is tagged player (this is auto detected by the door script)</param>
    public void MoveUp(GameObject player)
    {
        if (_camController.IsMoving) return;
/*        var newPlayerLocation = new Vector3(player.transform.position.x, player.transform.position.y + FloorConstants.VerticalPlayerOffset);
        Debug.Log("Changing position = " + player.transform.position.y);
        player.transform.position = newPlayerLocation;*/
        _camController.MoveUp();
        currentRoom += new Vector2Int(-1, 0);
        currentRoomType = _rooms[currentRoom.x][0].roomType;
        changeTheme();
        Debug.Log(currentRoom);
        Debug.Log(currentRoomType);
    }

    /// <summary>
    /// Moves the player and camera to the room below the current room
    /// </summary>
    /// <param name="player">The game object that is tagged player (this is auto detected by the door script)</param>
    public void MoveDown(GameObject player)
    {
        if (_camController.IsMoving) return;
/*        var newPlayerLocation = new Vector3(player.transform.position.x, player.transform.position.y - FloorConstants.VerticalPlayerOffset);
        Debug.Log("Changing position = " + player.transform.position.y);
        player.transform.position = newPlayerLocation;*/
        _camController.MoveDown();
        currentRoom += new Vector2Int(1, 0);
        currentRoomType = _rooms[currentRoom.x][currentRoom.y].roomType;
        changeTheme();
        Debug.Log(currentRoom);
        Debug.Log(currentRoomType);
    }

    /// <summary>
    /// Moves the player and camera to the room right of the current room
    /// </summary>
    /// <param name="player">The game object that is tagged player (this is auto detected by the door script)</param>
    public void MoveRight(GameObject player)
    {
        if (_camController.IsMoving) return;
        /*var newPlayerLocation = new Vector3(player.transform.position.x + FloorConstants.HorizontalPlayerOffset, player.transform.position.y);
        Debug.Log("Changing position = " + player.transform.position.x);
        player.transform.position = newPlayerLocation;*/
        _camController.MoveRight();
        currentRoom += new Vector2Int(0, 1);
        currentRoomType = _rooms[currentRoom.x][currentRoom.y].roomType;
        changeTheme();
        Debug.Log(currentRoom);
        Debug.Log(currentRoomType);
    }

    /// <summary>
    /// Moves the player and camera to the room left of the current room
    /// </summary>
    /// <param name="player">The game object that is tagged player (this is auto detected by the door script)</param>
    public void MoveLeft(GameObject player)
    {
        if (_camController.IsMoving) return;
        /*var newPlayerLocation = new Vector3(player.transform.position.x - FloorConstants.HorizontalPlayerOffset, player.transform.position.y);
        Debug.Log("Changing position = " + player.transform.position.x);
        player.transform.position = newPlayerLocation;*/
        _camController.MoveLeft();
        currentRoom += new Vector2Int(0, -1);
        currentRoomType = _rooms[currentRoom.x][currentRoom.y].roomType;
        changeTheme();
        Debug.Log(currentRoom);
        Debug.Log(currentRoomType);
    }

    public void changeTheme()
    {
        switch (currentRoomType)
        {
            case Room.RoomType.Start:
                AkSoundEngine.SetState("Music_State", "Start_Room");
                break;
            case Room.RoomType.Boss:
                AkSoundEngine.SetState("Music_State", "Boss_Room");
                break;
            case Room.RoomType.Charger:
                AkSoundEngine.SetState("Music_State", "Rest_Room");
                break;
            case Room.RoomType.Enemy:
                AkSoundEngine.SetState("Music_State", "Normal_Room");
                break;
            case Room.RoomType.EndRoom:
                AkSoundEngine.SetState("Music_State", "End_Room");
                break;
            default:
                AkSoundEngine.SetState("Music_State", "None");
                break;
        }
    }

    public void UnlockLastRoom()
    {
        _rooms[_floorXDimension-1][_floorYDimension-1].SetBottomDoorLocked(false);
    }
}
