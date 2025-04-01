using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies; // Danh sách kẻ địch trong phòng
    private Vector3[] initialPosition; // Lưu vị trí ban đầu của từng kẻ địch

    private void Awake()
    {
        // Lưu vị trí ban đầu của kẻ địch
        initialPosition = new Vector3[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
                initialPosition[i] = enemies[i].transform.position;
        }

        // Nếu không phải phòng đầu tiên, tắt phòng đi
        if (transform.GetSiblingIndex() != 0)
            ActivateRoom(false);
    }

    public void ActivateRoom(bool _status)
    {
        // Bật/tắt kẻ địch trong phòng và reset vị trí của chúng
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].SetActive(_status);
                enemies[i].transform.position = initialPosition[i]; // Đưa kẻ địch về vị trí ban đầu
            }
        }
    }
}
