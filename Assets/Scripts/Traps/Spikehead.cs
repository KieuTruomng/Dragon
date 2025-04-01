using UnityEngine;

public class Spikehead : EnemyDamage
{
    [Header("Thuộc tính của SpikeHead")]
    [SerializeField] private float speed; // Tốc độ di chuyển khi tấn công
    [SerializeField] private float range; // Phạm vi tấn công (khoảng cách phát hiện người chơi)
    [SerializeField] private float checkDelay; // Khoảng thời gian giữa các lần kiểm tra người chơi
    [SerializeField] private LayerMask playerLayer; // Layer của người chơi để SpikeHead có thể nhận diện

    private Vector3[] directions = new Vector3[4]; // Mảng chứa 4 hướng mà SpikeHead có thể di chuyển
    private Vector3 destination; // Hướng di chuyển hiện tại khi phát hiện người chơi
    private float checkTimer; // Bộ đếm thời gian để kiểm tra người chơi
    private bool attacking; // Biến kiểm tra xem SpikeHead có đang tấn công hay không

    [Header("Âm thanh")]
    [SerializeField] private AudioClip impactSound; // Âm thanh khi va chạm

    private void OnEnable()
    {
        Stop(); // Khi SpikeHead được kích hoạt, nó sẽ dừng lại ở vị trí hiện tại
    }

    private void Update()
    {
        // Nếu đang tấn công, di chuyển về phía người chơi theo hướng đã chọn
        if (attacking)
            transform.Translate(destination * Time.deltaTime * speed);
        else
        {
            // Nếu không tấn công, kiểm tra xem có thể phát hiện người chơi không
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay)
                CheckForPlayer();
        }
    }

    private void CheckForPlayer()
    {
        CalculateDirections(); // Tính toán 4 hướng tấn công

        // Kiểm tra xem có phát hiện người chơi trong 4 hướng không
        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i], Color.red); // Vẽ tia debug để kiểm tra trong Unity
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

            // Nếu phát hiện người chơi và chưa tấn công
            if (hit.collider != null && !attacking)
            {
                attacking = true; // Bắt đầu tấn công
                destination = directions[i]; // Di chuyển về hướng phát hiện người chơi
                checkTimer = 0; // Reset thời gian kiểm tra
            }
        }
    }

    private void CalculateDirections()
    {
        // Thiết lập 4 hướng di chuyển có thể có của SpikeHead
        directions[0] = transform.right * range;  // Hướng phải
        directions[1] = -transform.right * range; // Hướng trái
        directions[2] = transform.up * range;     // Hướng lên
        directions[3] = -transform.up * range;    // Hướng xuống
    }

    private void Stop()
    {
        destination = transform.position; // Đặt điểm đến là vị trí hiện tại để dừng lại
        attacking = false; // Đánh dấu không còn tấn công
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.instance.PlaySound(impactSound); // Phát âm thanh va chạm
        base.OnTriggerEnter2D(collision); // Gọi hàm xử lý va chạm của lớp cha (EnemyDamage)
        Stop(); // Dừng lại khi va chạm với vật thể khác
    }
}
