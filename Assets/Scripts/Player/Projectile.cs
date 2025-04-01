using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed; // Tốc độ bay của viên đạn
    private float direction; // Hướng bay (trái/phải)
    private bool hit; // Kiểm tra đạn đã va chạm chưa
    private float lifetime; // Đếm thời gian tồn tại

    private Animator anim;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>(); // Lấy Animator của đạn
        boxCollider = GetComponent<BoxCollider2D>(); // Lấy Collider
    }

    private void Update()
    {
        if (hit) return; // Nếu đã va chạm thì không di chuyển nữa
        
        float movementSpeed = speed * Time.deltaTime * direction; 
        transform.Translate(movementSpeed, 0, 0); // Di chuyển theo hướng đã thiết lập

        lifetime += Time.deltaTime;
        if (lifetime > 5) gameObject.SetActive(false); // Tự hủy sau 5 giây nếu không va chạm
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true; // Đánh dấu đạn đã va chạm
        boxCollider.enabled = false; // Tắt collider để tránh va chạm tiếp
        anim.SetTrigger("explode"); // Kích hoạt animation nổ

        if (collision.tag == "Enemy") // Nếu va chạm với kẻ địch, gây sát thương
            collision.GetComponent<Health>()?.TakeDamage(1);
    }

    public void SetDirection(float _direction)
    {
        lifetime = 0; // Reset thời gian tồn tại
        direction = _direction; // Thiết lập hướng bay
        gameObject.SetActive(true); // Kích hoạt đạn
        hit = false; // Đặt lại trạng thái chưa va chạm
        boxCollider.enabled = true; // Bật lại collider

        // Đảo hướng đạn nếu cần để đảm bảo hướng bắn chính xác
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false); // Ẩn đạn khi animation nổ kết thúc
    }
}
