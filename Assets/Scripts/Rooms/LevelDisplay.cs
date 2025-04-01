using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelDisplay : MonoBehaviour
{
    public Text levelText; // Hiển thị số level (nếu dùng TextMeshPro, đổi thành TMP_Text)
    public float displayTime = 2f; // Thời gian hiển thị trước khi bắt đầu mờ dần
    public float fadeDuration = 1f; // Thời gian để hoàn tất hiệu ứng mờ

    private void Start()
    {
        int levelIndex = PlayerPrefs.GetInt("currentLevel", 1); // Lấy level hiện tại từ PlayerPrefs
        levelText.text = "Level " + levelIndex; // Cập nhật nội dung hiển thị
        StartCoroutine(FadeOutText()); // Bắt đầu hiệu ứng mờ dần
    }

    private IEnumerator FadeOutText()
    {
        yield return new WaitForSeconds(displayTime); // Đợi một khoảng thời gian trước khi bắt đầu fade

        float elapsedTime = 0f;
        Color textColor = levelText.color;

        while (elapsedTime < fadeDuration) // Thực hiện hiệu ứng fade
        {
            elapsedTime += Time.deltaTime;
            textColor.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration); // Giảm dần độ trong suốt
            levelText.color = textColor;
            yield return null;
        }

        levelText.gameObject.SetActive(false); // Ẩn text sau khi fade xong
    }
}
