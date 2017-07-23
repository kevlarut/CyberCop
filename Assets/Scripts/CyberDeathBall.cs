using UnityEngine;

public class CyberDeathBall : MonoBehaviour
{
    public float hoverAmplitude = 1f;
    public float hoverSpeed = 10f;
    public float hoverForce = 10f;
    public float horizontalForce = 10f;
    public GameObject explosion;
    public Gun gun;
    public Transform player;
     
    public float distanceWithinWhichToFollowThePlayer = 10.0f;
    public float desiredDistanceFromPlayer = 0.5f;
    
    private Animator _animator;
    private Rigidbody2D _rigidBody;
    private float _originalY;
    private bool _isFacingRight = false;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _originalY = _rigidBody.transform.position.y;
    }

    void Update()
    {
        var desiredY = _originalY + hoverAmplitude * Mathf.Sin(hoverSpeed * Time.time);
        var verticalSpaceToMakeUp = desiredY - _rigidBody.transform.position.y;
        if (verticalSpaceToMakeUp > 0)
        {
            _rigidBody.AddForce(Vector2.up * verticalSpaceToMakeUp * hoverForce);
        }
        
        if (player.position.x > transform.position.x && !_isFacingRight) {
            FlipFacing();
        }
        else if (player.position.x < transform.position.x && _isFacingRight) {
            FlipFacing();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (gun.Shoot(_isFacingRight)) {                
                _animator.SetBool("IsShooting", true);
            }
        }

        var distance = Vector3.Distance(transform.position, player.position);
        if (distance < distanceWithinWhichToFollowThePlayer && distance > desiredDistanceFromPlayer) {
            _rigidBody.AddForce((player.transform.position - transform.position).normalized * horizontalForce * Time.smoothDeltaTime);
        }
    }

    void OnDestroy() {
        Instantiate(explosion, _rigidBody.transform.position, Quaternion.identity);
    }
    
    void FlipFacing()
    {
        _isFacingRight = !_isFacingRight;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }
}