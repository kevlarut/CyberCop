using UnityEngine;

public class KnifeThrower : MonoBehaviour
{
    public Gun gun;
    public Transform target;
     
    public float MinimumShootingDistance = 3.0f;

    private Animator animator;
    private Rigidbody2D rigidBody;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {        
        if (target != null) {            
            var distanceFromPlayer = target.transform.position.x - transform.position.x;
            if (Mathf.Abs(distanceFromPlayer) <= MinimumShootingDistance) {
                if (gun.CanShoot()) {                                    
                    animator.SetBool("IsShooting", true);
                }
            }
        }
    }
}