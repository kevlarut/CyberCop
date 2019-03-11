using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
     
	public string TargetTag = "Enemy";

	 void Start () {
		 var timeToLive = 2f;
         Destroy(gameObject, timeToLive); 
     }

	void OnTriggerEnter2D(Collider2D col)
    {
		var target = col.gameObject;
		var isVisible = target.GetComponent<Renderer>() ? target.GetComponent<Renderer>().isVisible : target.GetComponentInChildren<Renderer>().isVisible;
     	if(isVisible && target.tag == TargetTag)
     	{
			target.GetComponent<Destructible>().OnDamageTaken(1f);
			Destroy(gameObject);
     	}
	}
}
