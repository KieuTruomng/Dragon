using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private RectTransform[] buttons; // Danh sách các nút có thể chọn
    [SerializeField] private AudioClip changeSound; // Âm thanh khi thay đổi vị trí
    [SerializeField] private AudioClip interactSound; // Âm thanh khi chọn một tùy chọn

    private RectTransform arrow; // Mũi tên chọn vị trí hiện tại
    private int currentPosition; // Vị trí hiện tại của mũi tên trong danh sách nút

    private void Awake()
    {
        arrow = GetComponent<RectTransform>(); // Lấy RectTransform của mũi tên
    }

    private void OnEnable()
    {
        currentPosition = 0; // Đặt lại vị trí đầu tiên khi menu được bật lên
        ChangePosition(0); // Cập nhật vị trí mũi tên
    }

    private void Update()
    {
        // Di chuyển mũi tên lên xuống khi nhấn các phím mũi tên hoặc phím W/S
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            ChangePosition(-1);
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            ChangePosition(1);

        // Xác nhận lựa chọn khi nhấn Enter hoặc phím E
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E))
            Interact();
    }

    // Thay đổi vị trí của mũi tên khi di chuyển qua các tùy chọn
    private void ChangePosition(int _change)
    {
        currentPosition += _change; // Cập nhật vị trí

        if (_change != 0) 
            SoundManager.instance.PlaySound(changeSound); // Phát âm thanh khi thay đổi vị trí

        // Xử lý vòng lặp khi di chuyển qua các tùy chọn
        if (currentPosition < 0)
            currentPosition = buttons.Length - 1;
        else if (currentPosition >= buttons.Length)
            currentPosition = 0;

        AssignPosition(); // Cập nhật vị trí của mũi tên trên giao diện
    }

    // Cập nhật vị trí của mũi tên theo vị trí của nút đang chọn
    private void AssignPosition()
    {
        arrow.position = new Vector3(arrow.position.x, buttons[currentPosition].position.y);
    }

    // Xử lý khi người chơi chọn một tùy chọn
    private void Interact()
    {
        SoundManager.instance.PlaySound(interactSound); // Phát âm thanh khi chọn

        // Gọi sự kiện onClick của nút hiện tại
        buttons[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
}
