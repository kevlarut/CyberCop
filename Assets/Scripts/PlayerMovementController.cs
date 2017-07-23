using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovementController : MonoBehaviour
{
    public float jumpForce = 150f;
    public float runningSpeed = 2.0f;
    public Gun Gun;

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

        if (Input.GetButton("Fire1") || Input.GetKeyDown(KeyCode.Space))
        {
            if (Gun.Shoot(isFacingRight)) {                
                animator.SetBool("IsShooting", true);
            }
        }

        RespawnIfPlayerHasFallenToHisDoom();
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

    void RespawnIfPlayerHasFallenToHisDoom() {
        if (transform.position.y < -3) {
            transform.position = new Vector3(0, 1, 0);
            rigidBody.velocity = new Vector3(0, 0, 0);
        }
    }

    void OnDestroy() {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
}