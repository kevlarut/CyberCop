using UnityEngine;

public class CyberDeathBall : MonoBehaviour
{
    public float hoverForce = 1f;
    public float devianceTolerance = 0.25f;
    public float maximumVelocity = 1f;
    public GameObject Explosion;

    private Animator animator;
    private Rigidbody2D rigidBody;

    private float originalY;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        originalY = rigidBody.transform.position.y;
    }

    void Update()
    {
        if (rigidBody.transform.position.y < originalY - devianceTolerance && rigidBody.velocity.magnitude < maximumVelocity)
        {
            rigidBody.AddForce(Vector2.up * hoverForce);
        }
    }

    void OnDestroy() {
        var explosionInstance = Instantiate(Explosion, rigidBody.transform.position, Quaternion.identity);	
    }
}