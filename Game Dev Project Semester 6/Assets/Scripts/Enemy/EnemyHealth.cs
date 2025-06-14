using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private int currentHealth;
    public int maxHealth;
    private Animator anim;
    private BoxCollider2D coll;
    private EnemyDamage dmg;
    private EnemyMovements mov;

    void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        dmg = GetComponent<EnemyDamage>();
        mov = GetComponent<EnemyMovements>();
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        Debug.Log(currentHealth);
        anim.SetTrigger("attacked");
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        coll.size = new Vector2(0.5f, 1f);

        anim.SetBool("dead", true);

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        dmg.enabled = false;
        mov.enabled = false;

        // Destroy(gameObject, 2f);
    }
}