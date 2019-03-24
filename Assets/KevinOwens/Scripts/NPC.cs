 using UnityEngine;
 using System.Collections;
 using PixelCrushers;
 
 public class NPC : MonoBehaviour {
     
    public Transform target;
    private bool isFacingRight = true;
 
    void Update () 
    {
        if (target)
        {
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

        var alwaysFaceCameras = GetComponentsInChildren<AlwaysFaceCamera>();
        foreach (var alwaysFaceCamera in alwaysFaceCameras) {
            alwaysFaceCamera.rotate180 = !isFacingRight;
        }
    }
 }