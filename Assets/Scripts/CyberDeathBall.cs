using UnityEngine;

public class CyberDeathBall : MonoBehaviour
{
    public float hoverForce = 1f;
    public float devianceTolerance = 0.25f;
    public float maximumVelocity = 1f;
    public float MaxHitPoints = 3f;
    public GameObject Explosion;
    public GameObject Ping;

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
        if (_rigidBody.transform.position.y < _originalY - devianceTolerance && _rigidBody.velocity.magnitude < maximumVelocity)
        {
            _rigidBody.AddForce(Vector2.up * hoverForce);
        }
    }

    public void OnDamageTaken() {
        _damageTaken++;        
        if (_damageTaken >= MaxHitPoints) {
       		Destroy(gameObject);
        }
        else {
            var pingInstance = Instantiate(Ping, _rigidBody.transform.position, Quaternion.identity);	
            pingInstance.transform.parent = gameObject.transform;
            var bulletForce = 10f;
            _rigidBody.AddForce(Vector2.right * bulletForce);
        }
    }

    void OnDestroy() {
        var explosionInstance = Instantiate(Explosion, _rigidBody.transform.position, Quaternion.identity);	
    }
}