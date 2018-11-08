using UnityEngine;

namespace Assets.Scripts
{
    public class Spawner : MonoBehaviour
    {
        public float MinSpawnDelay = 5;
        public float MaxSpawnDelay = 10;

        private float _currentSpawnDelay;

        public GameObject GameObject;

        void Start()
        {
            _currentSpawnDelay = Random.Range(MinSpawnDelay, MaxSpawnDelay);
        }

        void Update()
        {
            _currentSpawnDelay -= Time.deltaTime;

            if (_currentSpawnDelay <= 0)
            {
                _currentSpawnDelay = Random.Range(MinSpawnDelay, MaxSpawnDelay);
                var createdObject = Instantiate(GameObject);
                createdObject.transform.Translate(transform.position.x, transform.position.y, 0);
            }
        }
    }
}