using UnityEngine;

public class KnifeThrower : MonoBehaviour
{
    public Gun gun;
    public Transform target;
     
    public float MinimumShootingDistance = 3.0f;

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
            FaceTowardsTarget();
            if (Mathf.Abs(distanceFromPlayer) <= MinimumShootingDistance) {
                if (gun.CanShoot()) {                
                    animator.SetBool("IsShooting", true);
                }
            }
        }
    }

    public void Shoot() {
        gun.Shoot(isFacingRight);
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