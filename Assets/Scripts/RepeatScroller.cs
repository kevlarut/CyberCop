using UnityEngine;

namespace Assets.Scripts.Scene
{
    public class RepeatScroller : MonoBehaviour
    {
        public float Length;
        public float Speed;

        private Vector2 startPosition;

        void Start()
        {
            startPosition = transform.position;
        }
        
        void Update()
        {
            float newPosition = Mathf.Repeat(Time.time * Speed, Length);
            transform.position = startPosition + Vector2.left * newPosition;
        }        
    }
}