 using UnityEngine;
 using System.Collections;
 
 public class FollowHorizontally : MonoBehaviour {
     
    public float Offset = 0f;
    public Transform Target;
 
    void Update () 
    {
        if (Target)
        {
            var targetVector = Target.position;
            var destinationX = Target.position.x + Offset;
            transform.position = new Vector3(destinationX, transform.position.y, transform.position.z);        

         }     
     }
 }