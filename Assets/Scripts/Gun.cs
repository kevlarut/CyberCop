﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	public float CoolDownPeriodInSeconds = 1f;
    public float bulletForce = 5f;
	public float Torque = 0f;
    public GameObject bullet;
    public Transform bulletSpawn;
	public string TargetTag = "Enemy";

	private float coolDownTimeStamp;

	void Start () {	
	}
	
	void Update () {
	}

	public bool Shoot(bool isFacingRight) {
		if (coolDownTimeStamp <= Time.time) {
			coolDownTimeStamp = Time.time + CoolDownPeriodInSeconds;

			var bulletInstance = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
			bulletInstance.GetComponent<Bullet>().TargetTag = TargetTag;
			var forceDirection = isFacingRight ? Vector2.right : Vector2.left;

			if (!isFacingRight)
			{
				bulletInstance.transform.localScale = new Vector2(-bulletInstance.transform.localScale.x, bulletInstance.transform.localScale.y);
			}

			var rigidBody = bulletInstance.GetComponent<Rigidbody2D>();
			rigidBody.AddForce(forceDirection * bulletForce);
			//if (Torque > 0f) {
        		rigidBody.AddTorque(Torque);
			//}
			return true;
		}
		else {
			return false;
		}
	}
}
