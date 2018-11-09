using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void OnDamageTaken() {
        _damageTaken++;        
        if (_damageTaken >= MaxHitPoints) {
            Instantiate(Explosion, _rigidBody.transform.position, Quaternion.identity);

            var playerMovementController = GetComponent<PlayerMovementController>();
            if (playerMovementController != null) {
                playerMovementController.OnDeath();
            }
            else {
       		    Destroy(gameObject);
            }
        }
        else {
            var pingInstance = Instantiate(Ping, _rigidBody.transform.position, Quaternion.identity);	
            pingInstance.transform.parent = gameObject.transform;
            var bulletForce = 10f;
            _rigidBody.AddForce(Vector2.right * bulletForce);
        }
    }
}
