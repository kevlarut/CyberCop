using UnityEngine;

namespace Assets.Scripts
{
    public class Water : MonoBehaviour
    {
        public GameObject SplashGameObject;

        void OnCollisionEnter2D(Collision2D collision)
        {
            var target = collision.gameObject;
            var destructible = target.GetComponent<Destructible>();
            if (destructible != null) {
                var splash = Instantiate(SplashGameObject);
                splash.transform.Translate(target.transform.position.x, transform.position.y, 0);
                destructible.OnDamageTaken(destructible.MaxHitPoints, new Vector2(0f, 0f), false);
            }
        }
    }
}