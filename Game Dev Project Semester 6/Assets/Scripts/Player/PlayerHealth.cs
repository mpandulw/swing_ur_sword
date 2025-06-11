using System;
using UnityEngine;
using static PlayerMovements;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health")]
    public int health; // Current player health
    public int maxHealth; // Max player health


    private PlayerMovements playerMovements; // Import PlayerMovements class
    private Animator anim;
    public GameObject DiePanel;

    private void Awake()
    {
        playerMovements = GetComponent<PlayerMovements>();
        anim = GetComponent<Animator>();

        if (playerMovements == null)
        {
            Debug.LogError("Couldn't find PlayerMovements component in this game object");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DiePanel.SetActive(false);
        health = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {

            Die();
        }
        else
        {

        }
    }

    private void Die()
    {
        playerMovements.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f; // Just in case you use rotation
        }

        anim.SetInteger("state", (int)MovementsState.die);
        DiePanel.SetActive(true);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Death")
        {
            health = 0;
            Die();
        }
    }
}
