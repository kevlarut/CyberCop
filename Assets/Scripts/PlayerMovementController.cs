using UnityEngine;

public class PlayerMovementController : MonoBehaviour {

    public float jumpFactor = 300;
    public float speedFactor = 5.0f;

    private bool grounded = true;
    private bool isFacingRight = true;

    private Animator animator;
    private Rigidbody2D rigidBody;
    
    void Start () {

        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
    }
	
	void Update () {

        var walkingInput = Input.GetAxisRaw("Horizontal");        
        var jumpingInput = Input.GetAxisRaw("Vertical");

        var isJumping = !grounded;
        var isRunning = !isJumping && walkingInput != 0;

        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsJumping", isJumping);

        rigidBody.velocity = new Vector2(walkingInput * speedFactor, rigidBody.velocity.y);

        if (jumpingInput > 0 && grounded)
        {
            rigidBody.AddForce(Vector2.up * jumpFactor);
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