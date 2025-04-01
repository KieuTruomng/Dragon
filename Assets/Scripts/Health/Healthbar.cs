using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Health playerHealth; // Tham chiếu đến script Health của người chơi
    [SerializeField] private Image totalhealthBar; // Thanh máu tổng (biểu thị lượng máu tối đa)
    [SerializeField] private Image currenthealthBar; // Thanh máu hiện tại (biểu thị lượng máu còn lại)

    private void Start()
    {
        totalhealthBar.fillAmount = playerHealth.currentHealth / 10; // Thiết lập thanh máu tổng ban đầu
    }

    private void Update()
    {
        currenthealthBar.fillAmount = playerHealth.currentHealth / 10; // Cập nhật thanh máu khi bị mất máu hoặc hồi máu
    }
}
