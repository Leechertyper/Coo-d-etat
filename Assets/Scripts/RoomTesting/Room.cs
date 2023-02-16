using System;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Serializable]
    internal class EnemyObject
    {
        [SerializeField] private DroneBoss boss;
        [SerializeField] private List<DroneAI> enemies;

        public void Awaken()
        {
            if (enemies.Count > 0)
            {
                foreach (var enemy in enemies)
                {
                    enemy.Awaken();
                }
            }
            if (boss)
            {
                boss.Awaken();
            }
        }

        public void Sleep()
        {
            if (enemies.Count > 0)
            {
                foreach (var enemy in enemies)
                {
                    enemy.Sleep();
                }
            }
            if (boss)
            {
                boss.Sleep();
            }
        }
    }

    private Floor myFloor;
    
    [SerializeField] private EnemyObject enemies;

    private void Start()
    {
        myFloor = GetComponentInParent<Floor>();
        enemies.Sleep();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        Debug.Log("ENTERED NEW ROOM");
        enemies.Awaken();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        Debug.Log("LEFT OLD ROOM");
        enemies.Sleep();
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
