using UnityEngine;

public class StreetThug : MonoBehaviour
{
    public Transform target;
     
	public float PunchCoolDownTimeInSeconds = 1f;
    public float desiredMinimumDistanceFromPlayer = 1.0f;
    public float desiredMaximumDistanceFromPlayer = 3.0f;
    public float MinimumPunchingDistance = 1.0f;
    public float runningSpeed = 2.0f;

    private Animator animator;
	private float punchCoolDownTimeStamp;
    private Rigidbody2D rigidBody;
    private bool isFacingRight = true;
    private SpriteRenderer SpriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("DoublePunch") || stateInfo.IsName("Uppercut"))
        {
            SpriteRenderer.sortingOrder = 2;
        }
        else {
            SpriteRenderer.sortingOrder = 0;            
        }

        if (target != null) {            
            var distanceFromPlayer = target.transform.position.x - transform.position.x;
            if (Mathf.Abs(distanceFromPlayer) > desiredMaximumDistanceFromPlayer) {  
                FaceTowardsTarget();
                float step = runningSpeed * Time.deltaTime;
                var targetPosition = target.position;
                targetPosition.y = transform.position.y;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);
                animator.SetBool("IsWalking", true);
            }
            else {
                FaceTowardsTarget();
                animator.SetBool("IsWalking", false);
                if (Mathf.Abs(distanceFromPlayer) <= MinimumPunchingDistance) {
                    PunchTarget();
                }
            }
        }
    }

	public bool CanPunch() {
		return punchCoolDownTimeStamp <= Time.time;
	}

    //TODO: Animate for taking damage

    void PunchTarget() {
        if (CanPunch()) {
		    punchCoolDownTimeStamp = Time.time + PunchCoolDownTimeInSeconds;

            var randy = Random.Range(0f, 1f);
            if (randy >= 0.5f) {
                animator.SetTrigger("IsPunchingUppercut");
            }
            else {
                animator.SetTrigger("IsPunchingDouble");
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