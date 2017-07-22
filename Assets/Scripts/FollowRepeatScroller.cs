using UnityEngine;

namespace Assets.Scripts.Scene
{
    public class FollowRepeatScroller : MonoBehaviour
    {
        public float Length;
        public float Speed;
     	public Transform Target;

        private Vector3 startPosition;
		private float distance = 0f;
		private float destinationX;

        void Start()
        {
            startPosition = transform.position;
			destinationX = startPosition.x - Length;
        }
        
        void Update()
        {
			distance += Speed;
            transform.position = startPosition + Vector3.left * distance;

			if (transform.position.x < destinationX) {				
				distance = 0;
				startPosition = new Vector3(Target.position.x + Length, startPosition.y, startPosition.z);
				destinationX = Target.position.x - Length;
			}
        }        
    }
}