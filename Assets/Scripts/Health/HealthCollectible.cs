using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [SerializeField] private float healthValue; // Giá trị máu được hồi khi nhặt vật phẩm
    [SerializeField] private AudioClip pickupSound; // Âm thanh khi nhặt vật phẩm

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem đối tượng va chạm có phải là người chơi không
        if (collision.tag == "Player")
        {
            SoundManager.instance.PlaySound(pickupSound); // Phát âm thanh nhặt vật phẩm
            collision.GetComponent<Health>().AddHealth(healthValue); // Hồi máu cho người chơi
            gameObject.SetActive(false); // Ẩn vật phẩm sau khi nhặt
        }
    }
}
