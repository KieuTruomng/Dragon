using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen; // Giao diện khi thua
    [SerializeField] private AudioClip gameOverSound; // Âm thanh khi thua

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen; // Giao diện tạm dừng

    [Header("Win UI")]
    [SerializeField] private GameObject winScreen; // Giao diện khi thắng

    private void Awake()
    {
        HideAllUI(); // Ẩn tất cả UI khi game bắt đầu
    }

    private void Update()
    {
        // Nhấn phím ESC để tạm dừng (trừ khi đang ở màn hình Game Over hoặc Win)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gameOverScreen.activeInHierarchy && !winScreen.activeInHierarchy)
            {
                TogglePause();
            }
        }
    }

    #region UI Control
    // Ẩn toàn bộ UI
    private void HideAllUI()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        winScreen?.SetActive(false); // Kiểm tra nếu có UI Win thì ẩn luôn
    }

    // Hiển thị UI được truyền vào, nhưng nếu không phải màn pause thì ẩn UI khác
    public void ShowUI(GameObject ui)
    {
        if (ui != pauseScreen) 
        {
            HideAllUI();
        }

        if (ui != null)
        {
            ui.SetActive(true);
        }
    }
    #endregion

    #region Game Over
    // Hiển thị màn hình Game Over và dừng thời gian
    public void GameOver()
    {
        ShowUI(gameOverScreen);
        SoundManager.instance.PlaySound(gameOverSound);
        Time.timeScale = 0;
    }

    // Chơi lại level hiện tại
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Chạy lại nhạc nền sau khi restart
        SoundManager.instance.PlayMusic();
    }

    // Quay về menu chính
    [System.Obsolete]
    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);

        // Đặt lại menu chính khi quay về menu
        MenuManager menuManager = FindObjectOfType<MenuManager>();
        if (menuManager != null)
        {
            menuManager.ResetMenu();
        }
    }

    // Thoát game
    public void Quit()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Dừng game khi đang chạy trong Unity Editor
        #endif
    }
    #endregion

    #region Pause
    // Chuyển đổi trạng thái pause (bật/tắt)
    public void TogglePause()
    {
        bool isPaused = !pauseScreen.activeInHierarchy;
        Debug.Log("Pause Toggled: " + isPaused);

        ShowUI(isPaused ? pauseScreen : null); // Hiển thị hoặc tắt màn hình pause
        Time.timeScale = isPaused ? 0 : 1; // Dừng hoặc tiếp tục game
        Debug.Log("Time Scale: " + Time.timeScale);
    }
    #endregion

    #region Win
    // Hiển thị màn hình chiến thắng và dừng game
    public void WinGame()
    {
        ShowUI(winScreen);
        
        // Dừng nhạc nền trước khi phát âm thanh chiến thắng
        SoundManager.instance.StopMusic();

        Time.timeScale = 0; // Dừng game khi thắng
    }
    #endregion

    // Chuyển sang màn tiếp theo nếu có, nếu không thì quay về menu chính
    public void NextLevel()
    {
        Time.timeScale = 1;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
            SoundManager.instance.PlayMusic(); 
        }
        else
        {
            Debug.Log("Đã hoàn thành tất cả level!");
            SceneManager.LoadScene(0); // Quay lại menu chính
            SoundManager.instance.PlayMusic(); 
        }
    }

    // Mở màn hình chọn level trong menu chính
    public void OpenSelectLevel()
    {
        Time.timeScale = 1;
        PlayerPrefs.SetInt("OpenSelectLevel", 1); // Lưu trạng thái cần mở SelectLevelUI
        SceneManager.LoadScene("_Menu"); // Load scene menu
        SoundManager.instance.PlayMusic();
    }

    // Điều chỉnh âm lượng hiệu ứng âm thanh
    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.1f);
    }

    // Điều chỉnh âm lượng nhạc nền
    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.1f);
    }
}
