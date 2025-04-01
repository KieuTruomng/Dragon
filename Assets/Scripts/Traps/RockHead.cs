using UnityEngine;
using System.Collections;

public class RockHead : MonoBehaviour
{
    // Các biến có thể chỉnh sửa từ Inspector trong Unity
    [SerializeField] private float fallSpeed = 5f;  // Tốc độ rơi (thực tế không dùng)
    [SerializeField] private float detectionRange = 5f; // Khoảng cách phát hiện người chơi
    [SerializeField] private float damage = 20f; // Lượng sát thương gây ra khi trúng người chơi
    [SerializeField] private float fallDelay = 1f; // ⏳ Thời gian trễ trước khi đá rơi
    [SerializeField] private float shakeIntensity = 0.1f; // Độ rung khi cảnh báo trước khi rơi
    [SerializeField] private float shakeDuration = 0.5f; // Thời gian rung trước khi rơi

    private bool isFalling = false; // Biến kiểm tra xem đá có đang rơi không
    private bool hasLanded = false; // Biến kiểm tra xem đá đã chạm đất chưa
    private Rigidbody2D rb; // Tham chiếu đến Rigidbody2D của RockHead
    private Vector3 originalPosition; // Lưu vị trí ban đầu để reset lại

    [Header("SFX & Effects")] // Hiển thị nhóm này trong Unity Inspector
    [SerializeField] private AudioClip fallSound; // Âm thanh khi đá rơi
    [SerializeField] private AudioClip landSound; // Âm thanh khi đá chạm đất
    [SerializeField] private GameObject dustEffect; // Hiệu ứng bụi khi đá chạm đất

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Lấy component Rigidbody2D
        rb.gravityScale = 0; // Tắt trọng lực ban đầu để đá không tự rơi
        originalPosition = transform.position; // Lưu vị trí ban đầu của đá
        StartCoroutine(CheckForPlayer()); // Bắt đầu kiểm tra người chơi liên tục
    }

    private IEnumerator CheckForPlayer()
    {
        while (!isFalling) // Nếu đá chưa rơi, kiểm tra người chơi liên tục
        {
            yield return new WaitForSeconds(0.2f); // Giảm tần suất kiểm tra để tối ưu hiệu suất
            DetectPlayer();
        }
    }

    private void DetectPlayer()
    {
        // Tạo một tia Raycast từ vị trí của RockHead, hướng thẳng xuống, với độ dài detectionRange
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, detectionRange, LayerMask.GetMask("Player"));

        // Nếu tia Raycast chạm vào người chơi và đá chưa rơi
        if (hit.collider != null && !isFalling)
        {
            isFalling = true; // Đánh dấu đá sẽ rơi
            StartCoroutine(WarningBeforeFall()); // Gọi hàm rung cảnh báo trước khi rơi
        }
    }

    private IEnumerator WarningBeforeFall()
    {
        // Rung nhẹ để cảnh báo trước khi đá rơi
        float elapsedTime = 0f;
        while (elapsedTime < shakeDuration)
        {
            transform.position = originalPosition + (Vector3)(Random.insideUnitCircle * shakeIntensity);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition; // Đưa đá về vị trí cũ sau khi rung xong

        yield return new WaitForSeconds(fallDelay); // ⏳ Chờ trước khi đá rơi thực sự
        
        SoundManager.instance.PlaySound(fallSound); // Phát âm thanh rơi
        rb.gravityScale = 3; // Kích hoạt trọng lực để đá rơi xuống
    }

    [System.Obsolete] // Cảnh báo lỗi thời nhưng không ảnh hưởng đến game
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") && !hasLanded)
        {
            hasLanded = true;
            
            // Hiển thị hiệu ứng vỡ tại vị trí viên đá
            GameObject effect = Instantiate(dustEffect, transform.position, Quaternion.identity);
            
            // Ẩn viên đá thay vì hủy ngay lập tức
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;

            // Phát âm thanh vỡ
            SoundManager.instance.PlaySound(landSound);

            // Hủy hiệu ứng vỡ sau 2 giây và xóa viên đá
            Destroy(effect, 0.5f);
            Destroy(gameObject, 1f);
        }
        // Nếu đá chạm vào Player
    if (collision.CompareTag("Player"))
    {
        Health playerHealth = collision.GetComponent<Health>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage); // Gây sát thương lên Player
        }
    }

    }

    [System.Obsolete]
    private IEnumerator ResetRockHead()
    {
        rb.gravityScale = 0; // Tắt trọng lực để đá không rơi tiếp
        rb.velocity = Vector2.zero; // Dừng mọi chuyển động
        yield return new WaitForSeconds(1.5f); // Chờ một chút trước khi reset lại

        isFalling = false; // Reset trạng thái rơi
        hasLanded = false; // Reset trạng thái chạm đất
        transform.position = originalPosition; // Đưa đá về vị trí ban đầu
        gameObject.SetActive(true); // Kích hoạt lại đá để có thể rơi lại
    }
}
