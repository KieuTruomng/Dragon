using UnityEngine;

public class Enemy_Sideways : MonoBehaviour
{
    [SerializeField] private float movementDistance; // Khoảng cách di chuyển từ vị trí ban đầu
    [SerializeField] private float speed; // Tốc độ di chuyển
    [SerializeField] private float damage; // Lượng sát thương gây ra khi va chạm với người chơi
    private bool movingLeft; // Kiểm tra hướng di chuyển
    private float leftEdge; // Biên trái
    private float rightEdge; // Biên phải

    private void Awake()
    {
        // Xác định giới hạn trái và phải dựa trên vị trí ban đầu của enemy
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
    }

    private void Update()
    {
        // Di chuyển sang trái
        if (movingLeft)
        {
            if (transform.position.x > leftEdge)
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
                movingLeft = false; // Đổi hướng sang phải
        }
        else // Di chuyển sang phải
        {
            if (transform.position.x < rightEdge)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
                movingLeft = true; // Đổi hướng sang trái
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Gây sát thương khi chạm vào người chơi
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
