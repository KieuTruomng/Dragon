using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown; // Thời gian hồi chiêu
    [SerializeField] private Transform firePoint; // Vị trí bắn fireball
    [SerializeField] private GameObject[] fireballs; // Danh sách fireball có sẵn
    [SerializeField] private AudioClip fireballSound; // Âm thanh khi bắn

    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity; // Bộ đếm thời gian hồi chiêu

    private void Awake()
    {
        anim = GetComponent<Animator>(); // Lấy Animator của nhân vật
        playerMovement = GetComponent<PlayerMovement>(); // Lấy PlayerMovement
    }

    private void Update()
    {
        // Kiểm tra nếu nhấn chuột trái, hết thời gian hồi chiêu, có thể tấn công và game chưa dừng
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.canAttack()
            && Time.timeScale > 0)
            Attack();

        cooldownTimer += Time.deltaTime; // Tăng thời gian hồi chiêu
    }

    private void Attack()
    {
        SoundManager.instance.PlaySound(fireballSound); // Phát âm thanh bắn
        anim.SetTrigger("attack"); // Kích hoạt animation tấn công
        cooldownTimer = 0; // Reset thời gian hồi chiêu

        // Xác định fireball chưa sử dụng để bắn ra
        fireballs[FindFireball()].transform.position = firePoint.position;
        fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy) // Tìm fireball chưa được kích hoạt
                return i;
        }
        return 0; // Nếu tất cả fireball đang hoạt động, chọn fireball đầu tiên
    }
}
