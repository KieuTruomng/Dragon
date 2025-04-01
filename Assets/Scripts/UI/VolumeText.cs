using UnityEngine;
using UnityEngine.UI;

public class VolumeText : MonoBehaviour
{
    [SerializeField] private string volumeName; // Tên key trong PlayerPrefs (vd: "SoundVolume" hoặc "MusicVolume")
    [SerializeField] private string textIntro; // Phần giới thiệu trước giá trị âm lượng (vd: "Âm thanh: " hoặc "Nhạc: ")
    private Text txt; // Biến lưu trữ đối tượng Text hiển thị trên UI

    private void Awake()
    {
        txt = GetComponent<Text>(); // Lấy component Text từ GameObject
    }

    private void Update()
    {
        UpdateVolume(); // Cập nhật giá trị âm lượng hiển thị liên tục
    }

    private void UpdateVolume()
    {
        float volumeValue = PlayerPrefs.GetFloat(volumeName) * 100; // Lấy giá trị âm lượng từ PlayerPrefs và nhân 100 để hiển thị %
        txt.text = textIntro + volumeValue.ToString("F0"); // Hiển thị giá trị dưới dạng số nguyên (F0: làm tròn)
    }
}
