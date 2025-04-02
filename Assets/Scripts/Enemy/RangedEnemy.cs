using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown; // Thời gian hồi chiêu khi tấn công
    [SerializeField] private float range; // Phạm vi tấn công
    [SerializeField] private int damage; // Lượng sát thương gây ra

    [Header("Ranged Attack")]
    [SerializeField] private Transform firepoint; // Vị trí bắn đạn
    [SerializeField] private GameObject[] fireballs; // Danh sách đạn có thể sử dụng

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance; // Khoảng cách phát hiện người chơi
    [SerializeField] private BoxCollider2D boxCollider; // Collider dùng để kiểm tra va chạm

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer; // Layer của người chơi để phát hiện

    private float cooldownTimer = Mathf.Infinity; // Bộ đếm thời gian hồi chiêu

    [Header("Fireball Sound")]
    [SerializeField] private AudioClip fireballSound; // Âm thanh khi bắn đạn

    // Tham chiếu đến các thành phần khác
    private Animator anim; // Dùng để điều khiển animation
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
                anim.SetTrigger("rangedAttack"); // Gọi animation tấn công
            }
        }

        // Nếu có EnemyPatrol, vô hiệu hóa khi thấy người chơi để dừng tuần tra
        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }

    private void RangedAttack()
    {
        // Phát âm thanh khi bắn đạn
        SoundManager.instance.PlaySound(fireballSound);

        cooldownTimer = 0; // Reset thời gian hồi chiêu

        // Lấy một viên đạn chưa được sử dụng trong mảng fireballs
        int fireballIndex = FindFireball();
        fireballs[fireballIndex].transform.position = firepoint.position; // Đặt vị trí bắn
        fireballs[fireballIndex].GetComponent<EnemyProjectile>().ActivateProjectile(); // Kích hoạt viên đạn
        
    }

    private int FindFireball()
    {
        // Tìm viên đạn chưa được kích hoạt để tái sử dụng
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0; // Nếu tất cả đạn đều đang được sử dụng, dùng viên đầu tiên
    }

    private bool PlayerInSight()
    {
        // Dùng BoxCast để phát hiện người chơi trong phạm vi
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

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
}
