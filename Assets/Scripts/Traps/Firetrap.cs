using UnityEngine;
using System.Collections;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage; // Sát thương khi bẫy kích hoạt

    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay; // Độ trễ trước khi bẫy kích hoạt
    [SerializeField] private float activeTime; // Thời gian bẫy hoạt động
    private Animator anim;
    private SpriteRenderer spriteRend;

    [Header("SFX")]
    [SerializeField] private AudioClip firetrapSound; // Âm thanh khi bẫy kích hoạt

    private bool triggered; // Đánh dấu khi bẫy được kích hoạt
    private bool active; // Kiểm tra xem bẫy có đang hoạt động không

    private Health playerHealth; // Lưu trữ tham chiếu đến máu của người chơi

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Nếu người chơi đang trong vùng bẫy và bẫy đang hoạt động, gây sát thương liên tục
        if (playerHealth != null && active)
            playerHealth.TakeDamage(damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerHealth = collision.GetComponent<Health>();

            if (!triggered)
                StartCoroutine(ActivateFiretrap()); // Bắt đầu quá trình kích hoạt bẫy

            if (active)
                collision.GetComponent<Health>().TakeDamage(damage); // Gây sát thương ngay nếu bẫy đang hoạt động
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            playerHealth = null; // Xóa tham chiếu khi người chơi rời khỏi bẫy
    }

    private IEnumerator ActivateFiretrap()
    {
        // Đổi màu sprite sang đỏ để cảnh báo người chơi và kích hoạt bẫy
        triggered = true;
        spriteRend.color = Color.red;

        // Chờ một khoảng thời gian trước khi bẫy hoạt động
        yield return new WaitForSeconds(activationDelay);
        SoundManager.instance.PlaySound(firetrapSound);
        spriteRend.color = Color.white; // Đổi lại màu gốc

        active = true;
        anim.SetBool("activated", true); // Bật animation bẫy hoạt động

        // Sau khi hết thời gian hoạt động, tắt bẫy và reset trạng thái
        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
        anim.SetBool("activated", false);
    }
}
