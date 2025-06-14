using System;
using UnityEngine;
using static PlayerMovements;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health")]
    public int health;
    public int maxHealth;

    private PlayerMovements playerMovements;
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
            rb.angularVelocity = 0f;
        }

        BoxCollider2D coll = GetComponent<BoxCollider2D>();
        coll.size = new Vector2(0.5f, 0.8f);

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
