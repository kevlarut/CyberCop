using UnityEngine;

namespace Assets.Scripts
{
    public class Pheasant : MonoBehaviour
    {
        public float Speed;

        // Use this for initialization
        void Start()
        {            
            var y = Random.Range(-1f, 0f);
            transform.Translate(3.5f, y, 0);
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(Speed * Time.deltaTime * -1, 0, 0);
            
            if (transform.position.x < -4)
            {
                Destroy(gameObject);
            }
        }
    }
}