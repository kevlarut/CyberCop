using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackGardenStudios.HitboxStudioPro
{
    public struct ContactData
    {
        public HitboxFeeder MyHitbox;
        public HitboxFeeder TheirHitbox;

        public float Damage;
        public float PoiseDamage;
        /// <summary>
        /// Identifier of the hit effect this attack uses.
        /// </summary>
        public int fxID;
        /// <summary>
        /// Amount of force to receive from this attack
        /// </summary>
        public Vector2 Force;
        /// <summary>
        /// Intersection point between these two hitboxes. Place a hit effect at this location.
        /// </summary>
        public Vector2 Point;
    }
}