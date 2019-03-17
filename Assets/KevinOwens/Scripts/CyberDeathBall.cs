using UnityEngine;

public class CyberDeathBall : MonoBehaviour
{
    public float horizontalForce = 10f;
    public Gun gun;
    public Transform player;
    public float listeningRange = 2f;
    public float MinimumShootingDistance = 1f;
     
    public float distanceWithinWhichToFollowThePlayer = 10.0f;
    public float desiredDistanceFromPlayer = 0.5f;
    
    private Animator _animator;
    private Rigidbody2D _rigidBody;
    private bool _isFacingRight = false;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {        
        var distanceFromPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (player != null && IsPlayerWithinSightRange()) {
            if (player.position.x > transform.position.x && !_isFacingRight) {
                FlipFacing();
            }
            else if (player.position.x < transform.position.x && _isFacingRight) {
                FlipFacing();
            }
            
            var destinationX = 0f;
            if (_isFacingRight) {
                destinationX = player.position.x - desiredDistanceFromPlayer;
            }
            else {
                destinationX = player.position.x + desiredDistanceFromPlayer;
            }
            var desiredYRelativeToPlayer = 0.25f;
            var y = player.position.y + desiredYRelativeToPlayer;
            var destination = new Vector3(destinationX, y, player.position.z);

            _rigidBody.AddForce((destination - transform.position).normalized * horizontalForce * Time.smoothDeltaTime);

            // Shoot the player if he's close enough.
            if (GetComponent<Renderer>().isVisible && gun.CanShoot() && Mathf.Abs(distanceFromPlayer) <= MinimumShootingDistance) {
                if (gun.Shoot(_isFacingRight)) {                
                    _animator.SetBool("IsShooting", true);
                }
            }
        }
    }
    
    bool IsPlayerWithinSightRange() {
        return Vector2.Distance(transform.position, player.transform.position) <= listeningRange;
    }

    void FlipFacing()
    {
        _isFacingRight = !_isFacingRight;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }
}