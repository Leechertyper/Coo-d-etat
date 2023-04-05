using UnityEngine;

public class GOTONEXTROOM : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            GameManager.Instance.GoToNextFloor();
        }
    }
}
