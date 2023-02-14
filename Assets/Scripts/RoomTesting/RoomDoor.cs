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
                _myRoom.TopDoor(col.gameObject);
                break;
            case DoorPosition.Bottom:
                _myRoom.BottomDoor(col.gameObject);
                break;
            case DoorPosition.Left:
                _myRoom.LeftDoor(col.gameObject);
                break;
            case DoorPosition.Right:
                _myRoom.RightDoor(col.gameObject);
                break;
        }
    }
}
