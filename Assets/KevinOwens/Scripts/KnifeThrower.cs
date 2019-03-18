using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Gun))]
public class KnifeThrower : MonoBehaviour
{
    public Gun Gun;
    public Transform Target;
     
    public float Range = 3.0f;

    private Animator _animator;

    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {        
        if (Target != null) {            
            var distanceFromPlayer = Target.transform.position.x - transform.position.x;
            if (Mathf.Abs(distanceFromPlayer) <= Range) {
                if (Gun.CanShoot()) {                                    
                    _animator.SetBool("IsShooting", true);
                }
            }
        }
    }
}