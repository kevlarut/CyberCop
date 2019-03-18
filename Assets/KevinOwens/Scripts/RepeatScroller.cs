using UnityEngine;

namespace Assets.Scripts.Scene
{
    public class RepeatScroller : MonoBehaviour
    {
        public float Length = 10;
        public float Speed = 1;
        public Vector3 Direction = Vector3.left;

        private Vector3 startPosition;

        void Start()
        {
            startPosition = transform.position;
        }
        
        void Update()
        {
            float newPosition = Mathf.Repeat(Time.time * Speed, Length);
            transform.position = startPosition + Direction * newPosition;
        }        
    }
}