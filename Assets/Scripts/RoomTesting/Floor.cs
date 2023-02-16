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
}

public class Floor : MonoBehaviour
{
    private List<List<Room>> _rooms;

    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject topRightCornerRoomPrefab;
    [SerializeField] private GameObject bottomLeftCornerRoomPrefab;
    [SerializeField] private GameObject leftColumnRoomPrefab;
    [SerializeField] private GameObject rightColumnRoomPrefab;
    [SerializeField] private GameObject bottomRowRoomPrefab;
    [SerializeField] private GameObject topRowRoomPrefab;
    [SerializeField] private GameObject startRoomPrefab;
    [SerializeField] private GameObject endRoomPrefab;

    private CameraController _camController;
    

    private void Start()
    {
        _camController = Camera.main.GetComponent<CameraController>();
        SpawnRooms(3, 3);
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
                //DONT LOOK ITS HORRIBLE BUT IT WORKS
                var roomToSpawn = roomPrefab;

                if (i == 0 && j == 0)
                {
                    roomToSpawn = leftColumnRoomPrefab;
                }
                else if (i == 0 && j == c - 1)
                {
                    roomToSpawn = topRightCornerRoomPrefab;
                }
                else if (i == 0)
                {
                    roomToSpawn = topRowRoomPrefab;
                }
                else if (j == 0 && i == r - 1)
                {
                    roomToSpawn = bottomLeftCornerRoomPrefab;
                }
                else if (j == c - 1)
                {
                    roomToSpawn = rightColumnRoomPrefab;
                }
                else if (j == 0)
                {
                    roomToSpawn = leftColumnRoomPrefab;
                }
                else if (j == c - 1 && i == r - 1)
                {
                    roomToSpawn = rightColumnRoomPrefab;
                }
                else if (i == r - 1)
                {
                    roomToSpawn = bottomRowRoomPrefab;
                }
                
                
                
                var newRoom = Instantiate(roomToSpawn, transform);
                newRoom.transform.position = new Vector3(newRoom.transform.position.x + FloorConstants.HorizontalRoomOffset * j, newRoom.transform.position.y - FloorConstants.VerticalRoomOffset*i);
                _rooms[i].Add(newRoom.GetComponent<Room>());
            }
        }
        
        var endRoom = Instantiate(endRoomPrefab,transform);
        endRoom.transform.position = new Vector3(endRoom.transform.position.x + FloorConstants.HorizontalRoomOffset * (c-1),
            endRoom.transform.position.y - FloorConstants.VerticalRoomOffset * r);
        GameManager.Instance.SetEndRoomPos(new Vector2(endRoom.transform.position.x,endRoom.transform.position.y));
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
