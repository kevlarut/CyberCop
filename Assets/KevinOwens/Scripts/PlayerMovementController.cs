﻿using UnityEngine;
using UnityEngine.SceneManagement;
using BlackGardenStudios.HitboxStudioPro;

public class PlayerMovementController : MonoBehaviour
{
    public float PunchCoolDownTimeInSeconds = 1f;
    public float jumpForce = 150f;
    public float runningSpeed = 2.0f;
    public Gun Gun;

    private float punchCoolDownTimeStamp;
    private bool grounded = true;
    private bool isFacingRight = true;
    private bool isTalking = false;

    private Animator animator;
    private SpriteRenderer SpriteRenderer;
    private Rigidbody2D rigidBody;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate() {

        if (isTalking) {
            return;
        }

        var maxVelocity = 2.5f;
        var walkingInput = Input.GetAxisRaw("Horizontal");
        var movement = new Vector2(walkingInput, 0f);

        var forceModifier = 1f;
        if (!grounded) {
            forceModifier = 0.25f;
        }

        rigidBody.AddForce(movement * runningSpeed * forceModifier);
        
        Vector3 clampVel = rigidBody.velocity;
        clampVel.x = Mathf.Clamp(clampVel.x, -maxVelocity, maxVelocity);
        rigidBody.velocity = clampVel;

        var jumpingInput = Input.GetAxisRaw("Vertical");

        var isJumping = !grounded && rigidBody.velocity.y > 0.1f;
        var isRunning = !isJumping && walkingInput != 0;

        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsJumping", isJumping);

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

        if (isTalking) {
            return;
        }

        if (Input.GetButton("Fire1"))
        {
            if (Gun.CanShoot() && Gun.Shoot(isFacingRight)) {                
                animator.SetBool("IsShooting", true);
            }
        }

        if (Input.GetButton("Fire2"))
        {             
            if (CanPunch()) {
                PunchTarget();
            }
        }
        
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Punching"))
        {
            SpriteRenderer.sortingOrder = 3;
        }
        else {
            SpriteRenderer.sortingOrder = 1;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // if (collision.gameObject.tag != "Enemy") 
        // {
        //     grounded = true;
        // }
        if (rigidBody.velocity.y < 0.1f) {
            grounded = true;
        }
    }

    public bool GetIsFacingRight() {
        return isFacingRight;
    }

    void FlipFacing()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }
    public void OnDeath() {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }

    void OnConversationEnd() {
        isTalking = false;
    }

    void OnConversationStart() {
        isTalking = true;
        rigidBody.velocity = new Vector2(0, 0);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsJumping", false);
    }

    void StopTalking() {
        isTalking = false;
    }

    
	public bool CanPunch() {
		return punchCoolDownTimeStamp <= Time.time;
	}

    void PunchTarget() {
        if (CanPunch()) {
		    punchCoolDownTimeStamp = Time.time + PunchCoolDownTimeInSeconds;
            animator.SetTrigger("IsPunching");
        }
    }
}