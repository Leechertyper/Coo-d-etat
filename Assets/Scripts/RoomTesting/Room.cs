using System;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Floor myFloor;
    
    [SerializeField] private GameObject topWallPiece;
    [SerializeField] private GameObject topDoorCollider;
    [SerializeField] private GameObject bottomWallPiece;
    [SerializeField] private GameObject bottomDoorCollider;
    [SerializeField] private GameObject rightWallPiece;
    [SerializeField] private GameObject rightDoorCollider;
    [SerializeField] private GameObject leftWallPiece;
    [SerializeField] private GameObject leftDoorCollider;

    [SerializeField] private SpriteRenderer miniMapIcon;
    
    [Serializable]
    public enum RoomType
    {
        Normal,
        Enemy,
        Key,
        Boss
    }

    private void Start()
    {
        myFloor = GetComponentInParent<Floor>();
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
    
    
    
    /// <summary>
    /// Initializes everything to do with the room, but for now it just sets up the doors
    /// </summary>
    /// <param name="x">X coordinate of this room</param>
    /// <param name="y">Y coordinate of this room</param>
    /// <param name="rows">number of rows in the current floor</param>
    /// <param name="columns">number of columns in the current floor</param>
    public void InitializeRoom(int x, int y, int rows, int columns)
    {
        SetUpDoors(x,y,rows,columns);
    }

    private void SetUpDoors(int x, int y, int rows, int columns)
    {
        if (x == 0 && y == 0)
        {
            //first room, needs to have door for starting room
            DisableLeftDoor();
            return;
        }
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
        myFloor.MoveUp(player);
    }
    public void BottomDoor(GameObject player)
    {
        myFloor.MoveDown(player);
    }
    public void LeftDoor(GameObject player)
    {
        myFloor.MoveLeft(player);
    }
    public void RightDoor(GameObject player)
    {
        myFloor.MoveRight(player);
    }
    
    
}
