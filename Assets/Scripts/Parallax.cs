 using UnityEngine;
 using System.Collections;
 
 public class Parallax : MonoBehaviour {
     
    public float Multiplier = 0.1f;
    public Transform Target;

    private float x;

    void Start() {
        x = Target.position.x;
    }

    void Update () 
    {
        if (Target.position.x != x) {
            var newX = transform.position.x - (Target.position.x - x ) * Multiplier;
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            x = Target.position.x;
        }
    }
 }