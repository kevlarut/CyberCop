using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour {

	public float amplitude = 0.5f;
	public float hoverSpeed = 1f;

	private Vector2 tempPosition;
	
	void Start() {
		tempPosition = transform.position;		
	}

	void FixedUpdate () {
		tempPosition.y = Mathf.Sin(Time.realtimeSinceStartup * hoverSpeed) * amplitude;
		transform.position = tempPosition;
	}
}
