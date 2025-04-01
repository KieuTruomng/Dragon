using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
    [SerializeField] private float speed; // Tốc độ di chuyển của viên đạn
    [SerializeField] private float resetTime; // Thời gian tự hủy nếu không chạm vào gì
    private float lifetime;
    private Animator anim;
    private BoxCollider2D coll;
    private bool hit; // Kiểm tra xem đạn đã trúng mục tiêu hay chưa

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Kích hoạt viên đạn
    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        coll.enabled = true;
    }

    private void Update()
    {
        if (hit) return;

        // Di chuyển viên đạn
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        // Kiểm tra thời gian tồn tại, nếu quá resetTime thì hủy viên đạn
        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        base.OnTriggerEnter2D(collision); // Gọi logic gây sát thương từ lớp cha
        coll.enabled = false;

        if (anim != null)
            anim.SetTrigger("explode"); // Nếu có animation, kích hoạt hiệu ứng nổ
        else
            gameObject.SetActive(false); // Nếu không, vô hiệu hóa đạn ngay lập tức
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
