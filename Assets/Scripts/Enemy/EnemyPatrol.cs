using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header ("Patrol Points")]
    [SerializeField] private Transform leftEdge;  // Điểm ranh giới bên trái
    [SerializeField] private Transform rightEdge; // Điểm ranh giới bên phải

    [Header("Enemy")]
    [SerializeField] private Transform enemy; // Đối tượng enemy cần di chuyển

    [Header("Movement parameters")]
    [SerializeField] private float speed; // Tốc độ di chuyển của enemy
    private Vector3 initScale; // Kích thước ban đầu của enemy
    private bool movingLeft; // Biến kiểm tra hướng di chuyển

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration; // Thời gian dừng lại khi đến mép
    private float idleTimer; // Bộ đếm thời gian dừng

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim; // Animator để điều khiển hoạt ảnh

    private void Awake()
    {
        // Lưu lại kích thước ban đầu của enemy
        initScale = enemy.localScale;
    }

    private void OnDisable()
    {
        // Khi enemy bị vô hiệu hóa, đặt trạng thái "moving" trong animator về false
        anim.SetBool("moving", false);
    }

    private void Update()
    {
        // Kiểm tra hướng di chuyển
        if (movingLeft)
        {
            // Nếu enemy chưa đến ranh giới bên trái, tiếp tục di chuyển trái
            if (enemy.position.x >= leftEdge.position.x)
                MoveInDirection(-1);
            else
                DirectionChange(); // Nếu đã đến biên trái, đổi hướng
        }
        else
        {
            // Nếu enemy chưa đến ranh giới bên phải, tiếp tục di chuyển phải
            if (enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
                DirectionChange(); // Nếu đã đến biên phải, đổi hướng
        }
    }

    private void DirectionChange()
    {
        anim.SetBool("moving", false); // Tắt animation di chuyển
        idleTimer += Time.deltaTime; // Bắt đầu đếm thời gian chờ

        if(idleTimer > idleDuration) // Nếu đã chờ đủ thời gian
            movingLeft = !movingLeft; // Đổi hướng di chuyển
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0; // Reset thời gian chờ
        anim.SetBool("moving", true); // Bật animation di chuyển

        // Quay enemy theo hướng di chuyển
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction,
            initScale.y, initScale.z);

        // Di chuyển enemy theo hướng xác định
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
            enemy.position.y, enemy.position.z);
    }
}
