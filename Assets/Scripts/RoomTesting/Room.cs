using System;
using System.Collections;
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


    [SerializeField] private RoomDoor topDoor;
    [SerializeField] private RoomDoor bottomDoor;
    [SerializeField] private RoomDoor leftDoor;
    [SerializeField] private RoomDoor rightDoor;
    

    private bool _hasBeenCleared;
    public RoomType roomType { get; private set; }
    [SerializeField] public FloorType floorType;
    public bool roomHasBeenInitialized;

    private List<Enemy> _enemies = new List<Enemy>();

    public enum RoomType
    {
        Start,
        Enemy,
        Key,
        Boss,
        Charger,
        EndRoom
    }

    public enum FloorType
    {
        City,
        Lab,
        Park,
        Pier
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        EnemiesAwake();
        if (roomType != RoomType.Boss) return;
        LockAllDoors();
        StartCoroutine(WaitForPlayerToGrabKeycard());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        EnemiesSleep();
    }

    private void EnemiesAwake()
    {
        List<Enemy> removalList = new List<Enemy>();
        if (_enemies == null) return;
        foreach (var enemy in _enemies)
        {
            if (enemy == null)
            {
                removalList.Add(enemy);
            }
            else
            {
                enemy.Awaken();
            }
        }
        removeEnemies(removalList);
    }
    private void EnemiesSleep()
    {
        List<Enemy> removalList = new List<Enemy>();
        if (_enemies == null) return;
        foreach (var enemy in _enemies)
        {
            if(enemy == null)
            {
                removalList.Add(enemy);
            }
            else
            {
                enemy.Sleep();
            }
        }
        removeEnemies(removalList);
    }
    
    private void removeEnemies(List<Enemy> removeList)
    {
        foreach(var enemy in removeList)
        {
            _enemies.Remove(enemy);
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

    private IEnumerator WaitForPlayerToGrabKeycard()
    {
        while (!GameManager.Instance.GetPlayerObject().GetComponent<Player>()._hasKey)
        {
            yield return null;
        }
        
        UnlockAllDoors();
        _myFloor.UnlockLastRoom();
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

    private void SetTopDoorLocked(bool locked)
    {
        Debug.Log("Locking Top Door");
        if (locked)
        {
            topDoor.LockDoor();
        }
        else
        {
            topDoor.UnlockDoor();
        }
        
    }

    public void SetBottomDoorLocked(bool locked)
    {
        Debug.Log("Locking Bottom Door");
        if (locked)
        {
            bottomDoor.LockDoor();
        }
        else
        {
            bottomDoor.UnlockDoor();
        }
    }

    private void SetLeftDoorLocked(bool locked)
    {
        Debug.Log("Locking Left Door");
        if (locked)
        {
            leftDoor.LockDoor();
        }
        else
        {
            leftDoor.UnlockDoor();
        }
    }

    private void SetRightDoorLocked(bool locked)
    {
        Debug.Log("Locking Right Door");
        if (locked)
        {
            rightDoor.LockDoor();
        }
        else
        {
            rightDoor.UnlockDoor();
        }
    }


    private void LockAllDoors()
    {
        
        SetBottomDoorLocked(true);
        SetLeftDoorLocked(true);
        SetRightDoorLocked(true);
        SetTopDoorLocked(true);
    }

    private void UnlockAllDoors()
    {
        
        SetBottomDoorLocked(false);
        SetLeftDoorLocked(false);
        SetRightDoorLocked(false);
        SetTopDoorLocked(false);
    }
    

}
