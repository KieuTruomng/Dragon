using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] protected float damage; // Lượng sát thương gây ra khi chạm vào người chơi

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        // Nếu va chạm với người chơi, gây sát thương
        if (collision.tag == "Player")
            collision.GetComponent<Health>()?.TakeDamage(damage);
    }
}
