using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	public float CoolDownPeriodInSeconds = 1f;
    public float bulletForce = 5f;
    public GameObject bullet;
    public Transform bulletSpawn;

	private float coolDownTimeStamp;

	void Start () {	
	}
	
	void Update () {
	}

	public bool Shoot(bool isFacingRight) {
		if (coolDownTimeStamp <= Time.time) {
			coolDownTimeStamp = Time.time + CoolDownPeriodInSeconds;

			var bulletInstance = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
			var forceDirection = isFacingRight ? Vector2.right : Vector2.left;
			var bulletRigidBody2d = bulletInstance.GetComponent<Rigidbody2D>();

			if (!isFacingRight)
			{
				bulletInstance.transform.localScale = new Vector2(-bulletInstance.transform.localScale.x, bulletInstance.transform.localScale.y);
			}

			bulletInstance.GetComponent<Rigidbody2D>().AddForce(forceDirection * bulletForce);
			return true;
		}
		else {
			return false;
		}
	}
}
