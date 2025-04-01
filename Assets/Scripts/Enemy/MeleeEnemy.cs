using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown; // Thời gian hồi chiêu khi tấn công
    [SerializeField] private float range; // Phạm vi tấn công
    [SerializeField] private int damage; // Lượng sát thương gây ra

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance; // Khoảng cách phát hiện người chơi
    [SerializeField] private BoxCollider2D boxCollider; // Collider dùng để kiểm tra va chạm

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer; // Layer của người chơi để phát hiện

    private float cooldownTimer = Mathf.Infinity; // Bộ đếm thời gian hồi chiêu

    // Tham chiếu đến các thành phần khác
    private Animator anim; // Dùng để điều khiển animation
    private Health playerHealth; // Lưu trạng thái máu của người chơi
    private EnemyPatrol enemyPatrol; // Dùng để dừng tuần tra khi tấn công

    private void Awake()
    {
        anim = GetComponent<Animator>(); // Lấy component Animator
        enemyPatrol = GetComponentInParent<EnemyPatrol>(); // Lấy EnemyPatrol từ cha (nếu có)
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime; // Tăng bộ đếm thời gian hồi chiêu theo thời gian

        // Kiểm tra nếu phát hiện người chơi
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown) // Nếu đã sẵn sàng tấn công
            {
                cooldownTimer = 0; // Reset thời gian hồi chiêu
                anim.SetTrigger("meleeAttack"); // Gọi animation tấn công
            }
        }

        // Nếu có EnemyPatrol, vô hiệu hóa khi thấy người chơi để dừng tuần tra
        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }

    private bool PlayerInSight()
    {
        // Dùng BoxCast để phát hiện người chơi trong phạm vi
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>(); // Lấy component máu của người chơi

        return hit.collider != null; // Trả về true nếu phát hiện người chơi
    }

    private void OnDrawGizmos()
    {
        // Hiển thị phạm vi tấn công bằng Gizmos để dễ debug
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        // Gây sát thương nếu người chơi vẫn còn trong tầm đánh
        if (PlayerInSight())
            playerHealth.TakeDamage(damage);
    }
}
