using UnityEngine;
using UnityEngine.SceneManagement;

public class DieMenu : MonoBehaviour
{
    [Header("Respawn Point")]
    public Vector2 respawnPoint;
    public GameObject player;

    private PlayerMovements playerMovements;
    private PlayerHealth playerHealth;

    void Awake()
    {
        playerHealth = player.GetComponent<PlayerHealth>();
        playerMovements = player.GetComponent<PlayerMovements>();
    }

    public void Respawn()
    {
        playerHealth.health = playerHealth.maxHealth;
        playerMovements.enabled = true;
        player.transform.position = respawnPoint;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        gameObject.SetActive(false); // this hides the die panel
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
