using UnityEngine;
 using System.Collections;
 
 public class AnimationAutoDestroy : MonoBehaviour {
     void Start () {
         Destroy (gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length); 
     }
 }