using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
    [Header("Player Movements")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private PlayerController playerController;
    private BoxCollider2D coll;

    private Vector2 moveInput;
    private bool isCrouching = false;
    private Vector2 standingColliderSize;
    private Vector2 standingColliderOffset;

    private float mobileInputX = 0f;

    private enum MovementsState { idle, run, jump, fall }

    [Header("Jump Settings")]
    [SerializeField] private LayerMask jumpableGround;
    private bool isJumping = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        playerController = new PlayerController();

        // Store original collider values
        standingColliderSize = coll.size;
        standingColliderOffset = coll.offset;
    }

    private void OnEnable()
    {
        playerController.Enable();
        playerController.Movements.Move.performed += OnMove;
        playerController.Movements.Move.canceled += OnMoveCancel;
        playerController.Movements.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        playerController.Disable();
        playerController.Movements.Move.performed -= OnMove;
        playerController.Movements.Move.canceled -= OnMoveCancel;
        playerController.Movements.Jump.performed -= OnJump;
    }

    private void OnMove(InputAction.CallbackContext ctx) => moveInput = ctx.ReadValue<Vector2>();
    private void OnMoveCancel(InputAction.CallbackContext ctx) => moveInput = Vector2.zero;

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (isGrounded() && !isCrouching) // Can't jump while crouching
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void Jump()
    {
        // Cek ulang grounded saat ini, dan jangan gunakan isJumping (karena bisa delay)
        if (isGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isJumping = true;
        }
    }

    private void FixedUpdate()
    {
        if (Application.isMobilePlatform)
        {
            moveInput = new Vector2(mobileInputX, 0f);
        }
        else
        {
            moveInput = playerController.Movements.Move.ReadValue<Vector2>();
        }

        Vector2 targetVelocity = new Vector2((moveInput.x + mobileInputX) * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = targetVelocity;
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        MovementsState state;

        if (moveInput.x > 0f)
        {
            state = MovementsState.run;
            sprite.flipX = false;
        }
        else if (moveInput.x < 0f)
        {
            state = MovementsState.run;
            sprite.flipX = true;
        }
        else
        {
            state = MovementsState.idle;
        }

        if (rb.linearVelocity.y > 0.1f)
        {
            state = MovementsState.jump;
        }
        else if (rb.linearVelocity.y < -0.1f)
        {
            state = MovementsState.fall;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool isGrounded()
    {
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