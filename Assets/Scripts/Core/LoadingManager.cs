using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance; // Singleton để quản lý duy nhất một LoadingManager trong game

    void Awake()
    {
        // Kiểm tra nếu chưa có instance thì gán nó vào biến instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Không hủy đối tượng khi chuyển scene
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Nếu đã có một instance khác, hủy object này để tránh trùng lặp
            return;
        }
    }

    // Hàm load màn chơi theo chỉ số levelIndex
    public void LoadLevel(int levelIndex)
    {
        Debug.Log($"Attempting to load level: {levelIndex}"); // In ra console để debug

        // Kiểm tra nếu levelIndex hợp lệ (trong phạm vi số lượng scene có sẵn)
        if (levelIndex < 0 || levelIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError($"Invalid Level Index: {levelIndex}"); // Báo lỗi nếu index không hợp lệ
            return;
        }

        Time.timeScale = 1; // Đảm bảo game không bị pause khi load màn mới

        // Lưu lại level hiện tại vào PlayerPrefs để có thể tải lại sau
        PlayerPrefs.SetInt("currentLevel", levelIndex);
        PlayerPrefs.Save();

        // Chuyển sang màn chơi mới
        SceneManager.LoadScene(levelIndex);
    }
}
