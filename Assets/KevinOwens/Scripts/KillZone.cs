using UnityEngine;

namespace Assets.Scripts
{
    public class KillZone : MonoBehaviour
    {
        void OnCollisionEnter2D(Collision2D collision)
        {
            var target = collision.gameObject;
            var destructible = target.GetComponent<Destructible>();
            if (destructible != null) {
                destructible.OnDamageTaken(destructible.MaxHitPoints, new Vector2(0f, 0f), false);
            }
        }
    }
}