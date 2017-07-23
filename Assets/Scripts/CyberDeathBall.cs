using UnityEngine;

public class CyberDeathBall : MonoBehaviour
{
    public float hoverForce = 1f;
    public float MaxHitPoints = 3f;
    public GameObject Explosion;
    public Gun Gun;

    private Animator _animator;
    private Rigidbody2D _rigidBody;
    private float _damageTaken;
    private float _originalY;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _originalY = _rigidBody.transform.position.y;
    }

    void Update()
    {
        var verticalSpaceToMakeUp = _originalY - _rigidBody.transform.position.y;
        if (verticalSpaceToMakeUp > 0)
        {
            _rigidBody.AddForce(Vector2.up * hoverForce * verticalSpaceToMakeUp);
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var isFacingRight = false;
            if (Gun.Shoot(isFacingRight)) {                
                _animator.SetBool("IsShooting", true);
            }
        }
    }

    void OnDestroy() {
        var explosionInstance = Instantiate(Explosion, _rigidBody.transform.position, Quaternion.identity);	
    }
}