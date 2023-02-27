using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] float jumpForce = 1;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;
    bool falling;
    float wallJumpCooldown;
    float horizontalInput;
    Rigidbody2D body;
    Animator animator;
    CapsuleCollider2D boxCollider;

    private Vector2 input;
    [Header("SFX")]
    [SerializeField] AudioClip jumpSound;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);

        //flipping sprite direction based on movement
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(5, 5, 5);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-5, 5, 5);

        if (body.velocity.y <= 0.05f) falling = true;
        else falling = false;

        //wall jump logic
        if (wallJumpCooldown > 0.2f)
        {
            if (OnWall() && !IsGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 3;

            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
                if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) { }
                    SoundManager.instance.PlaySound(jumpSound);
            }
        }
        else wallJumpCooldown += Time.deltaTime;

        //set animator param
        animator.SetBool("run", horizontalInput != 0);
        animator.SetBool("grounded", IsGrounded());
        animator.SetBool("falling", falling);
    }

    public void OnMove(InputValue inputValue)
    {
        input = inputValue.Get<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }
    void Jump()
    {
        if (IsGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            animator.SetTrigger("jump");

        }
        else if (OnWall() && !IsGrounded())
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            wallJumpCooldown = 0;
        }
    }

    bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    bool OnWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool CanAttack()
    {
        return horizontalInput == 0 && IsGrounded() && !OnWall();
    }

}
