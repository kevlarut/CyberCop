using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D col)
    {
		var target = col.gameObject;
     	if(target.tag == "Enemy")
     	{		
       		Destroy(target);
			Destroy(gameObject);
     	}
	}
}
