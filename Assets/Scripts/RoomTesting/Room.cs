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
                    enemy.enabled = true;
                }
            }
            if (boss)
            {
                boss.Sleep();
            }
        }

        public void Sleep()
        {
            if (enemies.Count > 0)
            {
                foreach (var enemy in enemies)
                {
                    enemy.enabled = false;
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

    private void OnCollisionEnter2D (Collision2D other)
    {
        enemies.Awaken();
    }

    private void OnCollisionExit2D(Collision2D other)
    {
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
