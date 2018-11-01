 using UnityEngine;
 using System.Collections;
 
 public class Pacer : MonoBehaviour {
     
     public float LeftBoundaryDistance = 1f;
     public float RightBoundaryDistance = 1f;
     public float Speed = 1f;

    private bool isFacingRight = true;
    private Vector2 RightBoundary;
    private Vector2 LeftBoundary;
 
    void Start() {
        LeftBoundary = new Vector2(transform.position.x - LeftBoundaryDistance, transform.position.y);
        RightBoundary = new Vector2(transform.position.x + RightBoundaryDistance, transform.position.y);
    }

    void Update () 
    {
        if (isFacingRight) {
            if (transform.position.x != RightBoundary.x) {
                transform.position = Vector2.MoveTowards(transform.position, RightBoundary, Time.deltaTime * Speed);
            }
            else {
                FlipFacing();
            }            
        }
        else {
            if (transform.position.x != LeftBoundary.x) {
                transform.position = Vector2.MoveTowards(transform.position, LeftBoundary, Time.deltaTime * Speed);
            }
            else {
                FlipFacing();
            }            
        }
     }

    void FlipFacing()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }
 }