using UnityEngine;

public class Room : MonoBehaviour
{
    private Floor myFloor;

    private void Start()
    {
        myFloor = GetComponentInParent<Floor>();
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
