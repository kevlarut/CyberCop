using UnityEngine;

namespace Assets.Scripts.Scene
{
    public class RepeatScroller : MonoBehaviour
    {
        private static int RIGHT = 0;
        private static int LEFT = 1;

        public float Length = 10;
        public float Speed = 1;
        public int Direction = RepeatScroller.LEFT;

        private Vector3 startPosition;

        void Start()
        {
            startPosition = transform.position;
        }
        
        void Update()
        {
            float newPosition = Mathf.Repeat(Time.time * Speed, Length);
            transform.position = startPosition + (Direction == RepeatScroller.LEFT ? Vector3.left : Vector3.right) * newPosition;
        }        
    }
}