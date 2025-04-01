using UnityEngine;

public class MeleeEnemy_patrol : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown; // Thời gian hồi chiêu khi tấn công
    [SerializeField] private float range; // Phạm vi tấn công
    [SerializeField] private int damage; // Sát thương gây ra

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance; // Khoảng cách phát hiện người chơi
    [SerializeField] private BoxCollider2D boxCollider; // Collider dùng để kiểm tra va chạm

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer; // Layer của người chơi để phát hiện

    [Header("Patrol Parameters")]
    [SerializeField] private float patrolSpeed; // Tốc độ tuần tra
    [SerializeField] private float patrolRange; // Phạm vi tuần tra
    private Vector2 startPosition; // Vị trí ban đầu của enemy
    private bool movingRight = true; // Hướng di chuyển của enemy

    private float cooldownTimer = Mathf.Infinity; // Bộ đếm thời gian hồi chiêu

    private Animator anim; // Animator để điều khiển animation
    private Health playerHealth; // Lưu trạng thái máu của người chơi

    private void Awake()
    {
        anim = GetComponent<Animator>(); // Lấy Animator từ enemy
        startPosition = transform.position; // Lưu lại vị trí ban đầu
        Flip(); // Đảm bảo enemy luôn hướng về một phía khi bắt đầu
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime; // Tăng bộ đếm hồi chiêu theo thời gian

        if (PlayerInSight()) // Nếu phát hiện người chơi
        {
            if (cooldownTimer >= attackCooldown) // Kiểm tra nếu có thể tấn công
            {
                cooldownTimer = 0; // Reset thời gian hồi chiêu
                anim.SetTrigger("meleeAttack"); // Gọi animation tấn công
            }
        }
        else
        {
            Patrol(); // Nếu không thấy người chơi thì tiếp tục tuần tra
        }
    }

    private void Patrol()
    {
        // Xác định biên trái và phải của tuần tra
        float patrolEdgeRight = startPosition.x + patrolRange;
        float patrolEdgeLeft = startPosition.x - patrolRange;

        // Xác định vị trí mục tiêu tiếp theo
        Vector2 targetPosition = movingRight ?
            new Vector2(patrolEdgeRight, transform.position.y) :
            new Vector2(patrolEdgeLeft, transform.position.y);

        // Di chuyển enemy về vị trí mục tiêu
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, patrolSpeed * Time.deltaTime);

        // Kiểm tra nếu đã đến biên tuần tra thì đổi hướng
        if (transform.position.x >= patrolEdgeRight && movingRight)
        {
            movingRight = false;
            Flip();
        }
        else if (transform.position.x <= patrolEdgeLeft && !movingRight)
        {
            movingRight = true;
            Flip();
        }
    }

    private void Flip()
    {
        // Đảo hướng của enemy khi đến biên tuần tra
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private bool PlayerInSight()
    {
        if (boxCollider == null) return false; // Nếu không có BoxCollider thì bỏ qua

        // Xác định hướng nhìn của enemy
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        Vector2 center = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.center.y);

        // Kiểm tra va chạm bằng BoxCast (hộp dò tìm người chơi)
        RaycastHit2D hit = Physics2D.BoxCast(
            center + direction * range * colliderDistance,
            new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y),
            0, direction, 0, playerLayer);

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>(); // Lấy component máu của người chơi
            Debug.Log("Player detected!"); // Debug khi phát hiện người chơi
        }
        else
        {
            Debug.Log("Player not in sight."); // Debug khi không phát hiện người chơi
        }

        return hit.collider != null; // Trả về true nếu phát hiện người chơi
    }

    private void OnDrawGizmos()
    {
        if (boxCollider != null)
        {
            // Vẽ Gizmos hộp phát hiện người chơi để dễ debug
            Vector2 direction = movingRight ? Vector2.right : Vector2.left;
            Vector2 center = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.center.y);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(
                center + direction * range * transform.localScale.x * colliderDistance,
                new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
        }

        // Vẽ Gizmos phạm vi tuần tra
        Gizmos.color = Color.green;
        Gizmos.DrawLine(
            new Vector2(startPosition.x - patrolRange, transform.position.y),
            new Vector2(startPosition.x + patrolRange, transform.position.y));
    }

    private void DamagePlayer()
    {
        if (PlayerInSight()) // Nếu người chơi vẫn ở trong tầm nhìn
            playerHealth.TakeDamage(damage); // Gây sát thương
    }
}
