 using UnityEngine;
 using System.Collections;
 
 public class Parallax : MonoBehaviour {
     
     public bool IsSecondCopy = false;
    public float Multiplier = 0.1f;
    public Transform Target;

    private float previousX;

    void Start() {
        previousX = Target.position.x;

        if (IsSecondCopy) {
            var newX = transform.position.x + GetLength();
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }

    void Update () 
    {
        if (Target.position.x != previousX) {
            var newX = transform.position.x - (Target.position.x - previousX) * Multiplier;

            var wrapLength = GetLength() * (IsSecondCopy ? 2 : 1);
            if (transform.position.x < Target.position.x - wrapLength) {
                newX = Target.position.x + wrapLength;
            }
            else if (transform.position.x > Target.position.x + wrapLength) {
                newX = Target.position.x - wrapLength;
            }

            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            previousX = Target.position.x;
        }
    }

    float GetLength() {
        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        return spriteRenderer.bounds.size.x;
    }
 }