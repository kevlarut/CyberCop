 using UnityEngine;
 using System.Collections;
 
 public class FollowCamera : MonoBehaviour {
     
    public float dampTime = 0.15f;
    public Transform target;

    private Vector3 velocity = Vector3.zero;
    private bool isTalking = false;
 
    void Update () 
    {
        if (target)
        {
            if (isTalking) {
                var camera = GetComponent<Camera>();

                var targetVector = target.position;

                Vector3 point = camera.WorldToViewportPoint(targetVector);
                Vector3 delta = targetVector - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
                Vector3 destination = transform.position + delta;
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            }
            else {
                var camera = GetComponent<Camera>();

                var targetVector = target.position;
                var player = target.GetComponent<PlayerMovementController>();
                if (player != null) {
                    var lookAheadDistance = 1f;
                    var destinationX = 0f;
                    if (player.GetIsFacingRight()) {
                        destinationX = target.position.x + lookAheadDistance;
                    }
                    else {
                        destinationX = target.position.x - lookAheadDistance;
                    }
                    targetVector = new Vector3(destinationX, target.position.y, target.position.z);
                }

                Vector3 point = camera.WorldToViewportPoint(targetVector);
                Vector3 delta = targetVector - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
                Vector3 destination = transform.position + delta;
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            }
         }     
     }

     void StartConversation() 
     {
        isTalking = true;
    }

     void EndConversation() 
     {
         isTalking = false;
     }
 }