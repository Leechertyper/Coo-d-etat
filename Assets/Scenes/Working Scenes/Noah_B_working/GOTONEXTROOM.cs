using UnityEngine;

public class GOTONEXTROOM : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        GameManager.Instance.GoToNextFloor();
    }
}
