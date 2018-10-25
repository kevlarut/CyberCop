using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public float horizontalForce = 10f;
    public Transform target;
     
    public float distanceWithinWhichToFollowTheTarget = 10.0f;
    public float desiredDistanceFromTarget = 0.5f;
    
    private Rigidbody2D _rigidBody;
    private bool _isFacingRight = false;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {        
        if (target != null) {
            if (target.position.x > transform.position.x && !_isFacingRight) {
                FlipFacing();
            }
            else if (target.position.x < transform.position.x && _isFacingRight) {
                FlipFacing();
            }
            
            var destinationX = 0f;
            if (_isFacingRight) {
                destinationX = target.position.x - desiredDistanceFromTarget;
            }
            else {
                destinationX = target.position.x + desiredDistanceFromTarget;
            }
            var desiredYRelativeToTarget = 0.25f;
            var destination = new Vector3(destinationX, target.position.y + desiredYRelativeToTarget, target.position.z);

            _rigidBody.AddForce((destination - transform.position).normalized * horizontalForce * Time.smoothDeltaTime);
        }
    }
    
    void FlipFacing()
    {
        _isFacingRight = !_isFacingRight;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }
}