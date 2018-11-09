﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovementController : MonoBehaviour
{
    public float jumpForce = 150f;
    public float runningSpeed = 2.0f;
    public Gun Gun;

    private bool grounded = true;
    private bool isFacingRight = true;
    private bool isTalking = false;

    private Animator animator;
    private Rigidbody2D rigidBody;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isTalking) {
            return;
        }

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

    public bool GetIsFacingRight() {
        return isFacingRight;
    }

    void FlipFacing()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    void RespawnIfPlayerHasFallenToHisDoom() {
        if (transform.position.y < -3) {
            OnDeath();
        }
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
}