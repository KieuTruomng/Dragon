using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float attackCooldown; // Thời gian hồi chiêu giữa các lần bắn
    [SerializeField] private Transform firePoint; // Vị trí bắn mũi tên
    [SerializeField] private GameObject[] arrows; // Danh sách mũi tên có sẵn để sử dụng
    private float cooldownTimer; // Bộ đếm thời gian hồi chiêu

    [Header("SFX")]
    [SerializeField] private AudioClip arrowSound; // Âm thanh khi bắn mũi tên

    private void Attack()
    {
        cooldownTimer = 0; // Reset thời gian hồi chiêu

        SoundManager.instance.PlaySound(arrowSound); // Phát âm thanh bắn
        arrows[FindArrow()].transform.position = firePoint.position; // Đặt vị trí bắn
        arrows[FindArrow()].GetComponent<EnemyProjectile>().ActivateProjectile(); // Kích hoạt mũi tên
    }

    private int FindArrow()
    {
        // Tìm một mũi tên chưa được kích hoạt để tái sử dụng
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)
                return i;
        }
        return 0; // Nếu không tìm thấy, mặc định lấy mũi tên đầu tiên
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime; // Tăng bộ đếm thời gian

        if (cooldownTimer >= attackCooldown) // Nếu hết thời gian hồi chiêu, bắn mũi tên
            Attack();
    }
}
