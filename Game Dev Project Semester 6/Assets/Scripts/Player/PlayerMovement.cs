using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
    private PlayerAttack att;
    private PlayerController playerController;

    // Component variables
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D coll;
    private Vector2 moveInput;
    private float mobileInputX = 0f;

    // Movements variables
    [Header("Player Movements")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;


    // Knockback variables
    public float knockbackForce;
    public float knockbackCounter;
    public float knockbackTotalTime;
    public bool knockbackFromRight;
    private bool isInvicible = false;


    // Crouch variables
    private bool isCrouching = false;
    private Vector2 standingColliderSize;
    private Vector2 standingColliderOffset;


    // Dodge / Rolling variables
    private bool isRolling;


    public enum MovementsState { idle, run, jump, fall, attack1, attack2, die, hit, roll }

    [Header("Jump Settings")]
    [SerializeField] private LayerMask jumpableGround;

    public bool isJumping = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        playerController = new PlayerController();
        att = GetComponent<PlayerAttack>();
    }

    private void OnEnable()
    {
        playerController.Enable();
        playerController.Movements.Move.performed += OnMove;
        playerController.Movements.Move.canceled += OnMoveCancel;
        playerController.Movements.Jump.performed += OnJump;
        playerController.Movements.Roll.performed += OnRoll;
        playerController.Attacks.BasicAttack.performed += OnAttack;
    }

    private void OnDisable()
    {
        playerController.Disable();
        playerController.Movements.Move.performed -= OnMove;
        playerController.Movements.Move.canceled -= OnMoveCancel;
        playerController.Movements.Jump.performed -= OnJump;
        playerController.Movements.Roll.performed -= OnRoll;
        playerController.Attacks.BasicAttack.performed -= OnAttack;

    }

    // Attack methods
    private void OnAttack(InputAction.CallbackContext context)
    {
        att.DoAttack1();
    }

    public void mobileAttack()
    {
        att.DoAttack1();
    }

    private void OnMove(InputAction.CallbackContext ctx) => moveInput = ctx.ReadValue<Vector2>();
    private void OnMoveCancel(InputAction.CallbackContext ctx) => moveInput = Vector2.zero;

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (isGrounded() && !isCrouching)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void OnRoll(InputAction.CallbackContext ctx)
    {
        anim.SetInteger("state", (int)MovementsState.roll);
    }


    private void Jump()
    {
        if (isGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isJumping = true;
        }
    }

    void Update()
    {
        if (Application.isMobilePlatform)
        {
            moveInput = new Vector2(mobileInputX, 0f);
        }
        else
        {
            moveInput = playerController.Movements.Move.ReadValue<Vector2>();
        }
        if (!att.IsAttacking)
        {
            if (knockbackCounter <= 0)
            {
                Vector2 targetVelocity = new Vector2((moveInput.x + mobileInputX) * moveSpeed, rb.linearVelocity.y);
                rb.linearVelocity = targetVelocity;
            }
            else
            {
                if (knockbackFromRight == true)
                {
                    rb.linearVelocity = new Vector2(-knockbackForce, 1);
                }
                if (knockbackFromRight == false)
                {
                    rb.linearVelocity = new Vector2(knockbackForce, 1);
                }
                knockbackCounter -= Time.deltaTime;
            }

        }
        else
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (att.IsAttacking)
        {
            return;
        }

        MovementsState state;

        float horizontal = moveInput.x != 0 ? moveInput.x : mobileInputX;

        // Walk and idle animation
        if (horizontal > 0f)
        {
            state = MovementsState.run;
            sprite.flipX = false;
        }
        else if (horizontal < 0f)
        {
            state = MovementsState.run;
            sprite.flipX = true;
        }
        else
        {
            state = MovementsState.idle;
        }

        // Jump animation
        if (rb.linearVelocity.y > 0.1f)
        {
            state = MovementsState.jump;
        }
        else if (rb.linearVelocity.y < -0.1f)
        {
            state = MovementsState.fall;
        }

        // Knockback animation
        if (knockbackCounter > 0)
        {
            state = MovementsState.hit;
        }

        anim.SetInteger("state", (int)state);
    }

    public bool isGrounded()
    {
        isJumping = false;
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

    public void moveRight(bool isPressed)
    {
        if (isPressed)
            mobileInputX = 1f;
        else if (mobileInputX == 1f)
            mobileInputX = 0f;
    }

    public void moveLeft(bool isPressed)
    {
        if (isPressed)
            mobileInputX = -1f;
        else if (mobileInputX == -1f)
            mobileInputX = 0f;
    }

    public void mobileJump()
    {
        if (isGrounded())
            Jump();
    }
}