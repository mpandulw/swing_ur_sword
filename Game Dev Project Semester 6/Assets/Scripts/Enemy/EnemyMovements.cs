using System;
using System.Collections;
using UnityEngine;

public class EnemyMovements : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed;
    public int patrolDestination;
    public float idleDuration; // Duration for idle 
    public GameObject player; // Player game object
    private bool isWaiting; // Boolean for waiting state
    private Animator anim;

    private enum MovementsState
    {
        idle,
        run,
        attack
    }

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isWaiting)
        {
            MoveAndAnimate();
        }
    }

    private void MoveAndAnimate()
    {
        MovementsState state = MovementsState.idle;

        Transform targetPoint = patrolPoints[patrolDestination];

        // Check for player proximity first
        if (Vector2.Distance(transform.position, player.transform.position) < 5)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);

            // Flip sprite to face player
            if (player.transform.position.x > transform.position.x)
                transform.localScale = new Vector2(-1, 1);
            else
                transform.localScale = new Vector2(1, 1);

            state = MovementsState.run;
        }
        else
        {
            // Patrol logic
            transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

            // Flip sprite to face target
            if (targetPoint.position.x > transform.position.x)
                transform.localScale = new Vector2(-1, 1);
            else
                transform.localScale = new Vector2(1, 1);

            if (Vector2.Distance(transform.position, targetPoint.position) < 0.2f && !isWaiting)
            {
                StartCoroutine(WaitAtPatrolPoint());
                return;
            }

            state = MovementsState.run;
        }

        anim.SetInteger("state", (int)state);
    }


    private IEnumerator WaitAtPatrolPoint()
    {
        isWaiting = true;
        anim.SetInteger("state", (int)MovementsState.idle);

        yield return new WaitForSeconds(idleDuration);

        patrolDestination = (patrolDestination + 1) % patrolPoints.Length;
        isWaiting = false;
    }
}
