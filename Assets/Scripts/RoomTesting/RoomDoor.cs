using UnityEngine;

public class RoomDoor : MonoBehaviour
{
    [SerializeField] private bool locked;
    [SerializeField] private Sprite doorSpriteUnlocked;
    [SerializeField] private Sprite doorSpriteLocked;
    
    private SpriteRenderer _doorSpriteRenderer;

    private void Start()
    {
        _doorSpriteRenderer = GetComponent<SpriteRenderer>();
        _doorSpriteRenderer.sprite = locked ? doorSpriteLocked : doorSpriteUnlocked;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") || !locked)
        {
            //TODO: move player to next room
        }
    }
}
