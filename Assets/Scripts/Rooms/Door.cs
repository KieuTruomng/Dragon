using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom; // Phòng trước
    [SerializeField] private Transform nextRoom; // Phòng tiếp theo
    [SerializeField] private CameraController cam; // Tham chiếu đến CameraController

    private void Awake()
    {
        cam = Camera.main.GetComponent<CameraController>(); // Lấy CameraController từ camera chính
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") // Kiểm tra nếu người chơi chạm vào cửa
        {
            if (collision.transform.position.x < transform.position.x) // Nếu người chơi từ trái đi sang phải
            {
                cam.MoveToNewRoom(nextRoom); // Di chuyển camera đến phòng tiếp theo
                nextRoom.GetComponent<Room>().ActivateRoom(true); // Kích hoạt phòng mới
                previousRoom.GetComponent<Room>().ActivateRoom(false); // Vô hiệu hóa phòng cũ
            }
            else // Nếu người chơi từ phải đi sang trái
            {
                cam.MoveToNewRoom(previousRoom); // Di chuyển camera về phòng trước
                previousRoom.GetComponent<Room>().ActivateRoom(true); // Kích hoạt phòng trước
                nextRoom.GetComponent<Room>().ActivateRoom(false); // Vô hiệu hóa phòng mới
            }
        }
    }
}
