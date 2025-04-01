using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ItemCollector : MonoBehaviour
{
    [Header("UI Elements")]
    public Text itemCountText; // Hiển thị số vật phẩm thu thập
    public Text timerText; // Hiển thị thời gian còn lại
    public GameObject gameOverUI; // UI Game Over
    public GameObject winUI; // UI Win
    [SerializeField] private AudioClip gameOverSound; // Âm thanh thua game
    [SerializeField] private AudioClip winSound; // Âm thanh thắng game

    [Header("Game Settings")]
    public float startTime = 30f; // Thời gian bắt đầu (có thể chỉnh trong Inspector)
    public int totalItems = 5; // Số vật phẩm cần thu thập để thắng

    private int collectedItems = 0; // Số vật phẩm đã thu thập
    private float timeRemaining; // Thời gian còn lại
    private bool isGameOver = false; // Trạng thái game kết thúc

    void Start()
    {
        timeRemaining = startTime; // Gán thời gian bắt đầu
        gameOverUI.SetActive(false); // Ẩn UI Game Over ban đầu
        winUI.SetActive(false); // Ẩn UI Win ban đầu
        UpdateUI();
    }

    void Update()
    {
        if (!isGameOver)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime; // Giảm thời gian theo thời gian thực
                timerText.text = $"Time: {timeRemaining:F1}s"; // Cập nhật UI thời gian
            }
            else
            {
                GameOver(); // Kết thúc game nếu hết thời gian
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Nếu va chạm với vật phẩm và game chưa kết thúc
        if (collision.CompareTag("Item") && !isGameOver)
        {
            Destroy(collision.gameObject); // Xóa vật phẩm
            collectedItems++; // Tăng số lượng vật phẩm thu thập
            UpdateUI();

            if (collectedItems >= totalItems)
            {
                WinGame(); // Thắng game nếu thu thập đủ vật phẩm
            }
        }
    }

    void UpdateUI()
    {
        itemCountText.text = $"Items: {collectedItems}/{totalItems}"; // Cập nhật UI vật phẩm
    }

    void GameOver()
    {
        isGameOver = true;
        gameOverUI.SetActive(true); // Hiện UI Game Over
        SoundManager.instance.PlaySound(gameOverSound); // Phát âm thanh thua game
        Time.timeScale = 0; // Dừng game
    }

    void WinGame()
    {
        isGameOver = true;
        winUI.SetActive(true); // Hiện UI Win
        SoundManager.instance.StopMusic(); // Tắt nhạc nền
        SoundManager.instance.PlaySound(winSound); // Phát âm thanh thắng game
        Time.timeScale = 0; // Dừng game
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Reset tốc độ game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Load lại scene hiện tại
    }
}
