using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private GameObject pickupEffect; // Hiệu ứng VFX khi thu thập
    [SerializeField] private AudioClip pickupSound;   // Âm thanh thu thập
    [SerializeField] private float destroyDelay = 0.5f; // Độ trễ xóa item

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Chỉ Player mới nhặt được
        {
            // Hiển thị hiệu ứng thu thập (nếu có)
            if (pickupEffect != null)
            {
                GameObject effect = Instantiate(pickupEffect, transform.position, Quaternion.identity);
                Destroy(effect, 1f); // Xóa hiệu ứng sau 1 giây
            }

            // Phát âm thanh tại vị trí nhân vật
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, other.transform.position);
            }

            // Ẩn item ngay lập tức, nhưng xóa sau delay để tránh lỗi
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject, destroyDelay);
        }
    }
}
