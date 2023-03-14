using System;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Floor _myFloor;
    
    [SerializeField] private GameObject topWallPiece;
    [SerializeField] private GameObject topDoorCollider;
    [SerializeField] private GameObject bottomWallPiece;
    [SerializeField] private GameObject bottomDoorCollider;
    [SerializeField] private GameObject rightWallPiece;
    [SerializeField] private GameObject rightDoorCollider;
    [SerializeField] private GameObject leftWallPiece;
    [SerializeField] private GameObject leftDoorCollider;

    [SerializeField] private GameObject chargerRoomSprite;
    [SerializeField] private GameObject keyRoomSprite;
    [SerializeField] private GameObject bossRoomSprite;

    private bool _hasBeenCleared;
    public RoomType roomType { get; private set; }
    public bool roomHasBeenInitialized;

    private List<Enemy> _enemies = new List<Enemy>();

    public enum RoomType
    {
        Start,
        Enemy,
        Key,
        Boss,
        Charger,
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        EnemiesAwake();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        EnemiesSleep();
    }

    private void EnemiesAwake()
    {
        if (_enemies == null) return;
        foreach (var enemy in _enemies)
        {
            enemy.Awaken();
        }
    }
    private void EnemiesSleep()
    {
        if (_enemies == null) return;
        foreach (var enemy in _enemies)
        {
            enemy.Sleep();
        }
    }
    
    public void SetRoomType(RoomType roomType)
    {
        this.roomType = roomType;
        switch (this.roomType)
        {
            case RoomType.Boss:
                bossRoomSprite.SetActive(true);
                break;
            case RoomType.Charger:
                chargerRoomSprite.SetActive(true);
                break;
            case RoomType.Key:
                keyRoomSprite.SetActive(true);
                break;
        }

        roomHasBeenInitialized = true;
    }
    
    private void Start()
    {
        _myFloor = GetComponentInParent<Floor>();
    }

    private void DisableLeftDoor()
    {
        leftWallPiece.SetActive(true);
        leftDoorCollider.SetActive(false);
    }
    private void DisableRightDoor()
    {
        rightWallPiece.SetActive(true);
        rightDoorCollider.SetActive(false);
    }
    private void DisableTopDoor()
    {
        topWallPiece.SetActive(true);
        topDoorCollider.SetActive(false);
    }
    private void DisableBottomDoor()
    {
        bottomWallPiece.SetActive(true);
        bottomDoorCollider.SetActive(false);
    }

    public void SpawnEnemy(Enemy newEnemy)
    {
        _enemies.Add(newEnemy);
    }
    
    /// <summary>
    /// Initializes everything to do with the room, but for now it just sets up the doors
    /// </summary>
    /// <param name="x">X coordinate of this room</param>
    /// <param name="y">Y coordinate of this room</param>
    /// <param name="rows">number of rows in the current floor</param>
    /// <param name="columns">number of columns in the current floor</param>
    public void InitializeRoom(int x, int y, int rows, int columns)
    {
        SetUpDoors(y,x,rows,columns);
    }

    private void SetUpDoors(int x, int y, int rows, int columns)
    {
        if (y == columns - 1 && x == rows - 1)
        {
            //last room
            DisableRightDoor();
            return;
        }
        if (x == 0)
        {
            DisableLeftDoor();
        }
        if (x == rows - 1)
        {
            DisableRightDoor();
        }
        if (y == 0)
        {
            DisableTopDoor();
        }
        if (y == columns - 1)
        {
            DisableBottomDoor();
        }
    }
    
    public void TopDoor(GameObject player)
    {
        _myFloor.MoveUp(player);
    }
    public void BottomDoor(GameObject player)
    {
        _myFloor.MoveDown(player);
    }
    public void LeftDoor(GameObject player)
    {
        _myFloor.MoveLeft(player);
    }
    public void RightDoor(GameObject player)
    {
        _myFloor.MoveRight(player);
    }
    
}
