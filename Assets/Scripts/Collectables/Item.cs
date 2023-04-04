using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemEffect itemEffect;
    [SerializeField] private GameObject _particleSystem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        Destroy(Instantiate(_particleSystem, transform.position, transform.rotation), 0.5f);
        Destroy(gameObject);
        itemEffect.Apply(collision.gameObject);
    }
}
