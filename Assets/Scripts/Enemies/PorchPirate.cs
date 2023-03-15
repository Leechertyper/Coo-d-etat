using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorchPirate : MonoBehaviour
{

    private GlobalGrid _grid = GameManager.Instance.Grid.GetComponent<GlobalGrid>();
    private GameObject _player = GameManager.Instance.GetPlayerObject();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(_player.transform.position, transform.position) < 9)
        {
            MoveAway();
        }
    }

    // will move away from the player and spin
    private void MoveAway()
    {
        // change the position

    }

    // at low health it will keep spinning until it dies
    private void Spin()
    {
        if(GetComponent<Health>().health <= 10)
        {

        }
    }

    // 
    private void Attack()
    {

    }

    private void Die()
    {
        GameObject.Find("ScoreManager").GetComponent<Score>().AddScore(100);
    }

    private IEnumerator MoveTime()
    {
        yield return new WaitForSeconds(5);
    }
}
