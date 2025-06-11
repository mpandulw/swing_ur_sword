using UnityEngine;
using UnityEngine.UI;

public class DisplayHearts : MonoBehaviour
{
    [Header("Sprite")]
    public Sprite heart;
    public Sprite emptyHeart;
    public Image[] hearts;
    private PlayerHealth playerHealth;
    private int currentHealth;
    private int maxHealth;

    void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        currentHealth = playerHealth.health;
        maxHealth = playerHealth.maxHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = heart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }

        }
    }
}
