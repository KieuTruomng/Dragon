using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private RectTransform arrow; // Mũi tên chỉ thị vị trí hiện tại trong menu
    [SerializeField] private RectTransform[] mainMenuButtons; // Các nút trong menu chính
    [SerializeField] private RectTransform[] levelButtons; // Các nút chọn màn chơi
    [SerializeField] private GameObject selectLevelUI; // Giao diện chọn màn chơi
    [SerializeField] private GameObject mainMenuUI; // Giao diện menu chính

    [Header("Audio Clips")]
    [SerializeField] private AudioClip changeSound; // Âm thanh khi di chuyển trong menu
    [SerializeField] private AudioClip interactSound; // Âm thanh khi chọn menu

    private int currentPosition = 0; // Vị trí hiện tại của menu
    private bool isSelectingLevel = false; // Kiểm tra xem có đang ở màn hình chọn level không

    private void Awake()
    {
        mainMenuUI.SetActive(true); // Hiển thị menu chính khi game khởi động
        selectLevelUI.SetActive(false); // Ẩn giao diện chọn màn chơi
        ChangePosition(0); // Đặt lại vị trí mũi tên
    }

    private void Start()
    {
        // Kiểm tra xem có cần mở giao diện chọn level ngay từ đầu không
        if (PlayerPrefs.GetInt("OpenSelectLevel", 0) == 1)
        {
            OpenSelectLevelUI();
            PlayerPrefs.SetInt("OpenSelectLevel", 0); // Reset lại giá trị này
        }
    }

    public void OpenSelectLevelUI()
    {
        selectLevelUI.SetActive(true); // Hiển thị giao diện chọn màn chơi
    }

    private void Update()
    {
        // Xử lý di chuyển trong menu bằng phím mũi tên
        if (Input.GetKeyDown(KeyCode.UpArrow))
            ChangePosition(-1);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            ChangePosition(1);

        // Xử lý khi người chơi chọn một tùy chọn bằng phím Enter hoặc nút "Submit"
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit"))
            Interact();
    }

    // Thay đổi vị trí của mũi tên chỉ thị trong menu
    public void ChangePosition(int _change)
    {
        currentPosition += _change;

        if (_change != 0)
            SoundManager.instance.PlaySound(changeSound); // Phát âm thanh khi di chuyển menu

        // Xác định xem đang ở menu chính hay menu chọn level
        RectTransform[] buttons = isSelectingLevel ? levelButtons : mainMenuButtons;

        // Xử lý vòng lặp khi di chuyển qua danh sách các nút menu
        if (currentPosition < 0)
            currentPosition = buttons.Length - 1;
        else if (currentPosition >= buttons.Length)
            currentPosition = 0;

        AssignPosition(buttons); // Cập nhật vị trí mũi tên
    }

    // Đặt vị trí mũi tên theo nút được chọn
    private void AssignPosition(RectTransform[] buttons)
    {
        if (arrow != null && buttons.Length > 0)
            arrow.position = new Vector3(arrow.position.x, buttons[currentPosition].position.y);
    }

    // Quay lại menu chính từ menu chọn level
    public void GoToMainMenu()
    {
        isSelectingLevel = false;  // Đặt lại trạng thái menu chính
        selectLevelUI.SetActive(false);  // Ẩn màn hình chọn level
        mainMenuUI.SetActive(true);  // Hiển thị lại menu chính
        currentPosition = 0;  // Đặt lại vị trí menu
        AssignPosition(mainMenuButtons); // Đặt lại vị trí mũi tên
    }

    // Xử lý khi người chơi chọn một tùy chọn trong menu
    public void Interact()
    {
        SoundManager.instance.PlaySound(interactSound); // Phát âm thanh khi nhấn chọn

        if (isSelectingLevel) // Nếu đang ở giao diện chọn màn chơi
        {
            int selectedLevel = currentPosition + 1; // Lấy số level dựa vào vị trí
            Debug.Log("Selected Level: " + selectedLevel);
            PlayLevel(selectedLevel); // Gọi hàm để chơi level tương ứng
        }
        else
        {
            if (currentPosition == 0) // Nếu chọn mục "Chơi"
            {
                isSelectingLevel = true;
                mainMenuUI.SetActive(false); // Ẩn menu chính
                selectLevelUI.SetActive(true); // Hiển thị giao diện chọn level
                currentPosition = 0; // Reset vị trí
                AssignPosition(levelButtons);
            }
        }
    }

    // Thoát game
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Dừng game trong Unity Editor
        #else
        Application.Quit(); // Thoát game khi build
        #endif
    }

    // Reset menu về trạng thái ban đầu
    public void ResetMenu()
    {
        isSelectingLevel = false;  
        mainMenuUI.SetActive(true);
        selectLevelUI.SetActive(false);
        currentPosition = 0;
        AssignPosition(mainMenuButtons);
    }

    // Hàm để chuyển sang màn chơi được chọn
    public void PlayLevel(int levelIndex)
    {
        Debug.Log($"Starting Level {levelIndex}");

        PlayerPrefs.SetInt("currentLevel", levelIndex); // Lưu màn chơi hiện tại
        PlayerPrefs.Save(); // Đảm bảo dữ liệu được lưu lại

        if (LoadingManager.instance != null) // Kiểm tra xem có LoadingManager không
        {
            LoadingManager.instance.LoadLevel(levelIndex); // Chuyển sang màn chơi
            SoundManager.instance.PlayMusic(); // Phát nhạc nền
        }
        else
        {
            Debug.LogError("LoadingManager instance is NULL!"); // Báo lỗi nếu thiếu LoadingManager
        }
    }

    // Điều chỉnh âm lượng hiệu ứng âm thanh
    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }

    // Điều chỉnh âm lượng nhạc nền
    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.2f);
    }
}
