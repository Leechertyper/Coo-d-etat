using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the constants for the floor offsets (SUBJECT TO CHANGE, I JUST PICKED THESE NUMBERS BECAUSE THEY WORKED)
/// </summary>
public static class FloorConstants
{
    public const float VerticalRoomOffset = 29.0f;
    public const float HorizontalRoomOffset = 53.0f;
    
    public const float TransitionSpeed = 0.25f;

    public const float VerticalPlayerOffset = 8f;
    public const float HorizontalPlayerOffset = 8f;
}

[Serializable]

public class Floor : MonoBehaviour
{
    private List<List<Room>> _rooms;

    [SerializeField] private GameObject roomPrefab;

    [SerializeField] private CameraController camController;

    private Tuple<int, int> _floorDimensions;

    private Tuple<int, int> _playerLocation;


    private void Start()
    {
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
        for (var i = 0; i < c; i++)
        {
            _rooms.Add(new List<Room>());
        }

        for (var i = 0; i < r; i++)
        {
            for (var j = 0; j < c; j++)
            {
                var newRoom = Instantiate(roomPrefab, transform);
                newRoom.transform.position = new Vector3(newRoom.transform.position.x + FloorConstants.HorizontalRoomOffset * j, newRoom.transform.position.y - FloorConstants.VerticalRoomOffset*i);
                _rooms[i].Add(newRoom.GetComponent<Room>());
            }
        }

        _playerLocation = new Tuple<int, int>(0, 0);
        _rooms[0][0].gameObject.SetActive(true);
        _floorDimensions = new Tuple<int, int>(r, c);
    }

    /// <summary>
    /// Moves the player and camera to the room above the current room
    /// </summary>
    /// <param name="player">The game object that is tagged player (this is auto detected by the door script)</param>
    public void MoveUp(GameObject player)
    {
        if (_playerLocation.Item2 == 0) return;
        var newPlayerLocation = new Vector3(player.transform.position.x, player.transform.position.y + FloorConstants.VerticalPlayerOffset);
        player.transform.position = newPlayerLocation;
        camController.MoveUp();
        _playerLocation = new Tuple<int, int>(_playerLocation.Item1, _playerLocation.Item2 - 1);
    }

    /// <summary>
    /// Moves the player and camera to the room below the current room
    /// </summary>
    /// <param name="player">The game object that is tagged player (this is auto detected by the door script)</param>
    public void MoveDown(GameObject player)
    {
        if (_playerLocation.Item2 == _floorDimensions.Item2) return;
        var newPlayerLocation = new Vector3(player.transform.position.x, player.transform.position.y - FloorConstants.VerticalPlayerOffset);
        player.transform.position = newPlayerLocation;
        camController.MoveDown();
        _playerLocation = new Tuple<int, int>(_playerLocation.Item1, _playerLocation.Item2 + 1);
    }

    /// <summary>
    /// Moves the player and camera to the room right of the current room
    /// </summary>
    /// <param name="player">The game object that is tagged player (this is auto detected by the door script)</param>
    public void MoveRight(GameObject player)
    {
        if (_playerLocation.Item1 == _floorDimensions.Item1) return;
        var newPlayerLocation = new Vector3(player.transform.position.x + FloorConstants.HorizontalPlayerOffset, player.transform.position.y);
        camController.MoveRight();
        player.transform.position = newPlayerLocation;
        _playerLocation = new Tuple<int, int>(_playerLocation.Item1 + 1, _playerLocation.Item2);
    }

    /// <summary>
    /// Moves the player and camera to the room left of the current room
    /// </summary>
    /// <param name="player">The game object that is tagged player (this is auto detected by the door script)</param>
    public void MoveLeft(GameObject player)
    {
        if (_playerLocation.Item1 == 0) return;
        var newPlayerLocation = new Vector3(player.transform.position.x - FloorConstants.HorizontalPlayerOffset, player.transform.position.y);
        player.transform.position = newPlayerLocation;
        camController.MoveLeft();
        _playerLocation = new Tuple<int, int>(_playerLocation.Item1 - 1, _playerLocation.Item2);
    }
    
    
}
