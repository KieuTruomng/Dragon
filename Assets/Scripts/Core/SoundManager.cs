using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; } // Singleton quản lý âm thanh
    private AudioSource soundSource; // Nguồn phát hiệu ứng âm thanh
    private AudioSource musicSource; // Nguồn phát nhạc nền

    private void Awake()
    {
        // Lấy thành phần AudioSource từ GameObject hiện tại
        soundSource = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();

        // Đảm bảo chỉ có một SoundManager tồn tại xuyên suốt game
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Không bị hủy khi chuyển scene
        }
        else if (instance != null && instance != this)
        {
            Destroy(gameObject); // Hủy đối tượng nếu đã có một SoundManager khác tồn tại
        }

        // Gán giá trị âm lượng mặc định
        ChangeMusicVolume(0);
        ChangeSoundVolume(0);
    }

    // Phát hiệu ứng âm thanh (không ảnh hưởng đến nhạc nền)
    public void PlaySound(AudioClip _sound)
    {
        soundSource.PlayOneShot(_sound);
    }

    // Thay đổi âm lượng hiệu ứng âm thanh
    public void ChangeSoundVolume(float _change)
    {
        ChangeSourceVolume(1, "soundVolume", _change, soundSource);
    }

    // Thay đổi âm lượng nhạc nền
    public void ChangeMusicVolume(float _change)
    {
        ChangeSourceVolume(0.3f, "musicVolume", _change, musicSource);
    }

    // Hàm thay đổi âm lượng chung cho cả hiệu ứng và nhạc nền
    private void ChangeSourceVolume(float baseVolume, string volumeName, float change, AudioSource source)
    {
        // Lấy giá trị âm lượng hiện tại từ PlayerPrefs (mặc định là 1 nếu chưa có)
        float currentVolume = PlayerPrefs.GetFloat(volumeName, 1);
        currentVolume += change; // Thay đổi âm lượng

        // Giới hạn âm lượng trong khoảng từ 0 đến 1
        if (currentVolume > 1)
            currentVolume = 0;
        else if (currentVolume < 0)
            currentVolume = 1;

        // Gán giá trị âm lượng mới
        float finalVolume = currentVolume * baseVolume;
        source.volume = finalVolume;

        // Lưu lại giá trị âm lượng vào PlayerPrefs
        PlayerPrefs.SetFloat(volumeName, currentVolume);
    }

    // Dừng phát nhạc nền
    public void StopMusic()
    {
        musicSource.Stop();
    }

    // Bắt đầu phát nhạc nền
    public void PlayMusic()
    {
        musicSource.Play();
    }
}
