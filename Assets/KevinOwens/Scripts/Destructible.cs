using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Destructible : MonoBehaviour {

    public float MaxHitPoints = 3f;
    public GameObject Ping;
    public GameObject Explosion;
	
    private float _damageTaken;
    private Rigidbody2D _rigidBody;
	
	void Start () {	
        _rigidBody = GetComponent<Rigidbody2D>();	
	}
	
	void Update () {		
	}

    public void OnDamageTaken(float damage, Vector2 point, bool shouldExplodeOnDeath = true) {
        var position = _rigidBody ? _rigidBody.transform.position : transform.position;
        if (point.x != 0 && point.y != 0) {
            position = new Vector3(point.x, point.y, 0f);
        }

        _damageTaken += damage;        
        if (_damageTaken >= MaxHitPoints) {
            if (shouldExplodeOnDeath) {
                Instantiate(Explosion, position, Quaternion.identity);
            }

            var playerMovementController = GetComponent<PlayerMovementController>();
            if (playerMovementController != null) {
                playerMovementController.OnDeath();
            }
            else {
       		    Destroy(gameObject);
            }
        }
        else {
            var pingInstance = Instantiate(Ping, position, Quaternion.identity);	
            pingInstance.transform.parent = gameObject.transform;
            var bulletForce = 10f;
            _rigidBody.AddForce(Vector2.right * bulletForce);
        }
    }
}
