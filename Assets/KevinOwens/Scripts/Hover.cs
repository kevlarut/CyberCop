using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour {

	public float amplitude = 0.5f;
	public float hoverSpeed = 1f;

	private Vector2 tempPosition;
	private float startingY = 0f;
	
	void Start() {
		tempPosition = transform.position;
		startingY = transform.position.y;
	}

	void FixedUpdate () {
		tempPosition.y = startingY + Mathf.Sin(Time.realtimeSinceStartup * hoverSpeed) * amplitude;
		transform.position = tempPosition;
	}
}
