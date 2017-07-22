 using UnityEngine;
 using System.Collections;
 
 public class FollowCamera : MonoBehaviour {
     
     public float dampTime = 0.15f;
     public Transform Target;

     private Vector3 velocity = Vector3.zero;
 
     void Update () 
     {
         if (Target)
         {
             var camera = GetComponent<Camera>();
             Vector3 point = camera.WorldToViewportPoint(Target.position);
             Vector3 delta = Target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
             Vector3 destination = transform.position + delta;
             transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
         }     
     }
 }