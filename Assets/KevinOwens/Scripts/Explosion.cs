using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class Explosion : MonoBehaviour {

	public float Intensity = 1f;

    private Light _light;
	private float startTime; 

	void Start () {
        _light = GetComponent<Light>();

		startTime = Time.time;

		var mainCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0];
		mainCamera.GetComponent<Shaker>().TriggerShake();
	}
	
	void Update () {
		_light.intensity = Mathf.Pow((Intensity / (Time.time - startTime)), 1.75f);
	}
}
