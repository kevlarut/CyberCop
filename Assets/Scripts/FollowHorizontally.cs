 using UnityEngine;
 using System.Collections;
 
 public class FollowHorizontally : MonoBehaviour {
     
    public Transform target;
    public float Offset = 0f;

    private Vector3 velocity = Vector3.zero;
 
    void Update () 
    {
        if (target)
        {
            var targetVector = target.position;
            var destinationX = target.position.x + Offset;
            transform.position = new Vector3(destinationX, transform.position.y, transform.position.z);        

         }     
     }
 }