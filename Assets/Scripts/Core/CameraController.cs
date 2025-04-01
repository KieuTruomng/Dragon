using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Điều khiển camera trong từng phòng (Room Camera)
    [SerializeField] private float speed; // Tốc độ chuyển đổi giữa các phòng
    private float currentPosX; // Vị trí X hiện tại của camera
    private Vector3 velocity = Vector3.zero; // Tốc độ dịch chuyển camera

    // Chế độ theo dõi nhân vật (Follow Player)
    [SerializeField] private Transform player; // Đối tượng Player để theo dõi
    [SerializeField] private float aheadDistance; // Khoảng cách camera đi trước khi nhân vật di chuyển
    [SerializeField] private float cameraSpeed; // Tốc độ di chuyển của camera
    private float lookAhead; // Giá trị để điều chỉnh vị trí camera khi nhân vật di chuyển

    private void Update()
    {
        // Chế độ chuyển phòng: Camera di chuyển mượt đến vị trí mới
        // transform.position = Vector3.SmoothDamp(transform.position, 
        //     new Vector3(currentPosX, transform.position.y, transform.position.z), 
        //     ref velocity, speed);

        // Chế độ theo dõi nhân vật (bị tắt bằng comment)
        transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);
    }

    // Hàm di chuyển camera đến phòng mới
    public void MoveToNewRoom(Transform _newRoom)
    {
        print("here"); // In ra console để debug
        currentPosX = _newRoom.position.x; // Cập nhật vị trí X mới của camera
    }
}
