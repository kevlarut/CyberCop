 using UnityEngine;
 using System.Collections;
 
 public class NPC : MonoBehaviour {
     
    public Transform target;
    private bool isFacingRight = true;
 
    void Update () 
    {
        if (target)
        {
            var camera = GetComponent<Camera>();

            var targetVector = target.position;

            if (!isFacingRight && target.position.x > transform.position.x) {
                FlipFacing();
            }
            else if (isFacingRight && target.position.x < transform.position.x) {
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