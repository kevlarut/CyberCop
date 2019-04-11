using UnityEngine;
using BlackGardenStudios.HitboxStudioPro;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class StreetThug : MonoBehaviour
{
    public Transform Target;
    public Transform GroundDetection;
     
	public float PunchCoolDownTimeInSeconds = 1f;
    public float desiredMinimumDistanceFromPlayer = 1.0f;
    public float desiredMaximumDistanceFromPlayer = 3.0f;
    public float MinimumPunchingDistance = 1.0f;
    public float runningSpeed = 2.0f;

    private Animator _animator;
	private float punchCoolDownTimeStamp;
    private bool isFacingRight = true;
    private SpriteRenderer SpriteRenderer;
    private float SightRange = 2.0f;

    private float groundDetectionDistance = 1f;

    void Start()
    {
        _animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("DoublePunch") || stateInfo.IsName("Uppercut"))
        {
            SpriteRenderer.sortingOrder = 2;
        }
        else {
            SpriteRenderer.sortingOrder = 0;            
        }

        if (Target != null) {
            var distanceFromPlayer = Mathf.Abs(Target.transform.position.x - transform.position.x);
            var canSeePlayer = distanceFromPlayer <= SightRange;

            if (canSeePlayer) {
                Debug.Log("I can see the player.  Distance is " + distanceFromPlayer);
                if (distanceFromPlayer > desiredMaximumDistanceFromPlayer) {  
                    FaceTowardsTarget();

                    var groundRaycastHit = Physics2D.Raycast(GroundDetection.position, Vector2.down, groundDetectionDistance);
                    if (groundRaycastHit.collider == false || groundRaycastHit.collider.tag != "Platform") {
                        // We can't move there, it's a vacuum
                    }
                    else {     
                        float step = runningSpeed * Time.deltaTime;
                        var targetPosition = Target.position;
                        targetPosition.y = transform.position.y;
                        transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);
                        _animator.SetBool("IsWalking", true);
                    }
                }
                else {
                    FaceTowardsTarget();
                    _animator.SetBool("IsWalking", false);
                    if (distanceFromPlayer <= MinimumPunchingDistance) {
                        PunchTarget();
                    }
                }
            }
        }
    }

	public bool CanPunch() {
		return punchCoolDownTimeStamp <= Time.time;
	}

    void PunchTarget() {
        if (CanPunch()) {
		    punchCoolDownTimeStamp = Time.time + PunchCoolDownTimeInSeconds;

            var randy = Random.Range(0f, 1f);
            if (randy >= 0.5f) {
                _animator.SetTrigger("IsPunchingUppercut");
            }
            else {
                _animator.SetTrigger("IsPunchingDouble");
            }
        }
    }
    
    void FaceAwayFromTarget() {
        if (Target.position.x > transform.position.x && isFacingRight) {
            FlipFacing();
        }
        else if (Target.position.x < transform.position.x && !isFacingRight) {
            FlipFacing();
        }
    }

    void FaceTowardsTarget() {
        if (Target.position.x > transform.position.x && !isFacingRight) {
            FlipFacing();
        }
        else if (Target.position.x < transform.position.x && isFacingRight) {
            FlipFacing();
        }
    }

    void FlipFacing()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }
}