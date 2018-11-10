using UnityEngine;

public class Punk : MonoBehaviour
{
    public Gun gun;
    public Transform target;
     
    public float desiredMinimumDistanceFromPlayer = 1.0f;
    public float desiredMaximumDistanceFromPlayer = 3.0f;
    public float MinimumShootingDistance = 3.0f;
    public float runningSpeed = 2.0f;

    private Animator animator;
    private Rigidbody2D rigidBody;
    private bool isFacingRight = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {        
        if (target != null) {            
            var distanceFromPlayer = target.transform.position.x - transform.position.x;
            if (Mathf.Abs(distanceFromPlayer) > desiredMaximumDistanceFromPlayer) {  
                FaceTowardsTarget();
                float step = runningSpeed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, target.position, step);
                animator.SetBool("IsRunning", true);
            }
            else {
                FaceTowardsTarget();
                animator.SetBool("IsRunning", false);
                if (Mathf.Abs(distanceFromPlayer) <= MinimumShootingDistance) {
                    if (gun.Shoot(isFacingRight)) {                
                        animator.SetBool("IsShooting", true);
                    }
                }
            }
        }
    }
    
    void FaceAwayFromTarget() {
        if (target.position.x > transform.position.x && isFacingRight) {
            FlipFacing();
        }
        else if (target.position.x < transform.position.x && !isFacingRight) {
            FlipFacing();
        }
    }

    void FaceTowardsTarget() {
        if (target.position.x > transform.position.x && !isFacingRight) {
            FlipFacing();
        }
        else if (target.position.x < transform.position.x && isFacingRight) {
            FlipFacing();
        }
    }

    void FlipFacing()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }
}