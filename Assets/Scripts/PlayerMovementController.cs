using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float jumpForce = 150f;
    public float runningSpeed = 2.0f;
    public float bulletForce = 300f;
    public GameObject bullet;
    public Transform bulletSpawn;

    private bool grounded = true;
    private bool isFacingRight = true;

    private Animator animator;
    private Rigidbody2D rigidBody;

    void Start()
    {

        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        var walkingInput = Input.GetAxisRaw("Horizontal");
        var jumpingInput = Input.GetAxisRaw("Vertical");

        var isJumping = !grounded;
        var isRunning = !isJumping && walkingInput != 0;

        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsJumping", isJumping);

        rigidBody.velocity = new Vector2(walkingInput * runningSpeed, rigidBody.velocity.y);

        if (jumpingInput > 0 && grounded)
        {
            rigidBody.AddForce(Vector2.up * jumpForce);
            grounded = false;
        }

        if (walkingInput > 0 && !isFacingRight)
        {
            FlipFacing();
        }
        else if (walkingInput < 0 && isFacingRight)
        {
            FlipFacing();
        }

        if (Input.GetButton("Fire1") || Input.GetKeyDown("space"))
        {
            animator.SetBool("IsShooting", true);

            var bulletInstance = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
            var forceDirection = isFacingRight ? Vector2.right : Vector2.left;
            var bulletRigidBody2d = bulletInstance.GetComponent<Rigidbody2D>();

            if (!isFacingRight)
            {
                bulletInstance.transform.localScale = new Vector2(-bulletInstance.transform.localScale.x, bulletInstance.transform.localScale.y);
            }

            bulletInstance.GetComponent<Rigidbody2D>().AddForce(forceDirection * bulletForce);
        }
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        grounded = true;
    }

    void FlipFacing()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }
}