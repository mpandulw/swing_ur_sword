using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("Enemy attack")]
    public int damage; // How much the game object will damage the player

    [Header("Player Object")]
    public GameObject player;
    private PlayerMovements playerMovements; // PlayerMovements script
    private PlayerHealth playerHealth; // PlayerHealth script

    void Awake()
    {
        playerMovements = player.GetComponent<PlayerMovements>();
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHealth.TakeDamage(damage);
            playerMovements.knockbackCounter = playerMovements.knockbackTotalTime;
            if (collision.transform.position.x <= transform.position.x)
            {
                playerMovements.knockbackFromRight = true;
            }
            if (collision.transform.position.x > transform.position.x)
            {
                playerMovements.knockbackFromRight = false;
            }
        }
    }
}
