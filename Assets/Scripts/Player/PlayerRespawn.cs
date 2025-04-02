using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpoint;
    private Transform currentCheckpoint;
    private Health playerHealth;
    private UIManager uiManager;

    [System.Obsolete]
    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void RespawnCheck()
{
    Debug.Log("Respawn Check - Checkpoint: " + (currentCheckpoint != null ? currentCheckpoint.position.ToString() : "NULL"));

    if (currentCheckpoint == null) 
    {
        uiManager.GameOver();
        return;
    }

    playerHealth.Respawn();
    transform.position = currentCheckpoint.position;
    Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform;
            SoundManager.instance.PlaySound(checkpoint);
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("activate");
        }
    }
}