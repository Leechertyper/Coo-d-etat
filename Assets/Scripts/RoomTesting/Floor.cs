using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the constants for the floor offsets (SUBJECT TO CHANGE, I JUST PICKED THESE NUMBERS BECAUSE THEY WORKED)
/// </summary>
public static class FloorConstants
{
    public const float VerticalRoomOffset = 35.5f;
    public const float HorizontalRoomOffset = 69.5f;
    
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
    
    private List<List<Room>> _rooms;

    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject startRoomPrefab;
    [SerializeField] private GameObject endRoomPrefab;

    private CameraController _camController;
    
    
    [SerializeField] private Camera miniMapCamera;

    private float _horizontalGap = 0;
    private float _verticalGap = 0;
    private bool _areGapsCalculated = false;
    

    private void Start()
    {
        _camController = Camera.main.GetComponent<CameraController>();
        _floorXDimension = 3;
        _floorYDimension = 3;
        SpawnRooms(_floorXDimension, _floorYDimension);
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
            _rooms.Add(new List<Room>());
        }

        var startRoom = Instantiate(startRoomPrefab, transform);
        startRoom.transform.position = new Vector3(transform.position.x,
            transform.position.y + FloorConstants.VerticalRoomOffset);
        
        _camController.MoveCameraToStart(startRoom.transform);

        for (var i = 0; i < r; i++)
        {
            for (var j = 0; j < c; j++)
            {
                var newRoom = Instantiate(roomPrefab, transform);
                var newRoomScript = newRoom.GetComponent<Room>();
                newRoomScript.InitializeRoom(i,j,_floorXDimension,_floorYDimension);
                newRoom.transform.position =
                    new Vector3(newRoom.transform.position.x + FloorConstants.HorizontalRoomOffset * j,
                        newRoom.transform.position.y - FloorConstants.VerticalRoomOffset * i);
                _rooms[i].Add(newRoomScript);
            }
        }

        var endRoom = Instantiate(endRoomPrefab,transform);
        endRoom.transform.position = new Vector3(endRoom.transform.position.x + FloorConstants.HorizontalRoomOffset * (c-1),
            endRoom.transform.position.y - FloorConstants.VerticalRoomOffset * r);
        //GameManager.Instance.SetEndRoomPos(new Vector2(endRoom.transform.position.x,endRoom.transform.position.y));
        
        
        //MINIMAP STUFF
        
        //CALCULATING GAPS
        var roomOne = _rooms[0][0].transform;           //  1   -   2   -   3
        var roomTwo = _rooms[0][1].transform;           //  4   -   5   -   6
        var roomFour = _rooms[1][0].transform;          //  7   -   8   -   9
        float newXCoord = 0;
        float newYCoord = 0;

        //TODO FIX THIS
        _horizontalGap = roomTwo.transform.position.x - roomOne.transform.position.x;
        _verticalGap = roomFour.transform.position.y - roomOne.transform.position.y;

        

        if (_floorXDimension % 2 == 0)
        {
            //even
            
        }
        else
        {
            //odd
            var placeholderNum = Mathf.Floor(_floorXDimension/2f);
            Debug.Log("X: " + placeholderNum);
            newXCoord = FloorConstants.RoomSizeX / 2 * placeholderNum +
                            _horizontalGap * placeholderNum;
        }
        
        if (_floorYDimension % 2 == 0)
        {
            
        }
        else
        {
            var placeholderNum = Mathf.Floor(_floorXDimension/2f);
            Debug.Log("Y: " + placeholderNum);
            newYCoord = FloorConstants.RoomSizeY / 2 * placeholderNum +
                            _verticalGap * placeholderNum;
        }

        var miniMapCamPos = new Vector3(newXCoord, newYCoord, -10);
        miniMapCamera.transform.position = miniMapCamPos;
    }

    /// <summary>
    /// Moves the player and camera to the room above the current room
    /// </summary>
    /// <param name="player">The game object that is tagged player (this is auto detected by the door script)</param>
    public void MoveUp(GameObject player)
    {
        if (_camController.IsMoving) return;
        var newPlayerLocation = new Vector3(player.transform.position.x, player.transform.position.y + FloorConstants.VerticalPlayerOffset);
        player.transform.position = newPlayerLocation;
        _camController.MoveUp();
    }

    /// <summary>
    /// Moves the player and camera to the room below the current room
    /// </summary>
    /// <param name="player">The game object that is tagged player (this is auto detected by the door script)</param>
    public void MoveDown(GameObject player)
    {
        if (_camController.IsMoving) return;
        var newPlayerLocation = new Vector3(player.transform.position.x, player.transform.position.y - FloorConstants.VerticalPlayerOffset);
        player.transform.position = newPlayerLocation;
        _camController.MoveDown();
    }

    /// <summary>
    /// Moves the player and camera to the room right of the current room
    /// </summary>
    /// <param name="player">The game object that is tagged player (this is auto detected by the door script)</param>
    public void MoveRight(GameObject player)
    {
        if (_camController.IsMoving) return;
        var newPlayerLocation = new Vector3(player.transform.position.x + FloorConstants.HorizontalPlayerOffset, player.transform.position.y);
        _camController.MoveRight();
        player.transform.position = newPlayerLocation;
    }

    /// <summary>
    /// Moves the player and camera to the room left of the current room
    /// </summary>
    /// <param name="player">The game object that is tagged player (this is auto detected by the door script)</param>
    public void MoveLeft(GameObject player)
    {
        if (_camController.IsMoving) return;
        var newPlayerLocation = new Vector3(player.transform.position.x - FloorConstants.HorizontalPlayerOffset, player.transform.position.y);
        player.transform.position = newPlayerLocation;
        _camController.MoveLeft();
    }
    
    
}
