using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerMovements mov;
    private Animator anim;
    private Rigidbody2D rb;

    // Attack var
    private bool isAtt = false;
    public float attDuration = 0.4f;
    public int attDamage = 1;
    private enum AttackType { Attack1, Attack2 }

    public GameObject attackPos;
    public float rad;
    public LayerMask enemies;

    // Getter and setter variable isAttacking
    public bool IsAttacking => isAtt;

    void Awake()
    {
        mov = GetComponent<PlayerMovements>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void DoAttack1()
    {
        if (mov.isGrounded() && !mov.isJumping)
        {
            isAtt = true;
            anim.SetInteger("state", (int)PlayerMovements.MovementsState.attack1);
            Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPos.transform.position, rad, enemies);
            foreach (Collider2D enemyGameObject in enemy)
            {
                Debug.Log("Hit");
                enemyGameObject.GetComponent<EnemyHealth>().TakeDamage(attDamage);
            }
            rb.linearVelocity = Vector2.zero;

            Invoke(nameof(EndAttack), attDuration);
        }
    }

    private void EndAttack()
    {
        isAtt = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPos.transform.position, rad);
    }
}