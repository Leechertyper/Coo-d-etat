using System;
using UnityEngine;

public class RoomDoor : MonoBehaviour
{
    [Serializable]
    private enum DoorPosition
    {
        Top,
        Bottom,
        Left,
        Right
    }
    
    private Room _myRoom;
    [SerializeField] private DoorPosition doorPos;
    [SerializeField] private bool locked;
    [SerializeField] private GameObject lockedDoorTilemap;
    [SerializeField] private Collider2D doorCollider;

    private void Start()
    {
        _myRoom = GetComponentInParent<Room>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player") || locked) return;

        switch (doorPos)
        {
            case DoorPosition.Top:
                col.gameObject.GetComponent<PlayerMovement>().TopDoor();
                _myRoom.TopDoor(col.gameObject);
                break;
            case DoorPosition.Bottom:
                col.gameObject.GetComponent<PlayerMovement>().BottomDoor();
                _myRoom.BottomDoor(col.gameObject);
                break;
            case DoorPosition.Left:
                col.gameObject.GetComponent<PlayerMovement>().LeftDoor();
                _myRoom.LeftDoor(col.gameObject);
                break;
            case DoorPosition.Right:
                col.gameObject.GetComponent<PlayerMovement>().RightDoor();
                _myRoom.RightDoor(col.gameObject);
                break;
        }
    }


    public void LockDoor()
    {
        if (!gameObject.activeSelf) return;
        locked = true;
        lockedDoorTilemap.SetActive(true);
        doorCollider.isTrigger = false;
    }

    public void UnlockDoor()
    {
        if (!gameObject.activeSelf) return;
        locked = false;
        lockedDoorTilemap.SetActive(false);
        doorCollider.isTrigger = true;
    }
}
