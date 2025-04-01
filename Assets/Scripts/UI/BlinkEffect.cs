using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlinkEffect : MonoBehaviour
{
    public Text buttonText; // Tham chiếu đến Text của nút Start
    public Color color1 = Color.red;
    public Color color2 = Color.yellow;
    public float blinkSpeed = 1.5f; // Tăng thời gian để màu chuyển dần

    private void Start()
    {
        if (buttonText == null)
            buttonText = GetComponent<Text>();

        StartCoroutine(BlinkTextSmooth());
    }

    IEnumerator BlinkTextSmooth()
    {
        float t = 0;
        bool isIncreasing = true;

        while (true)
        {
            t += (isIncreasing ? Time.deltaTime : -Time.deltaTime) / blinkSpeed;
            buttonText.color = Color.Lerp(color1, color2, t);

            if (t >= 1) isIncreasing = false;
            if (t <= 0) isIncreasing = true;

            yield return null; // Đảm bảo cập nhật mỗi frame
        }
    }
}
