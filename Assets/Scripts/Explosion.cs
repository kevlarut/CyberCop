using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	public float Intensity = 1f;

    private Light light;
	private float startTime; 

	void Start () {
        light = GetComponent<Light>();

		startTime = Time.time;
	}
	
	void Update () {
		light.intensity = Mathf.Pow((Intensity / (Time.time - startTime)), 1.75f);
	}
}
