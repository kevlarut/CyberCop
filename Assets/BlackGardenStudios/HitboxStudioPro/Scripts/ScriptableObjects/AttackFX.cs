using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackGardenStudios.HitboxStudioPro
{
    [CreateAssetMenu()]
    public class AttackFX : ScriptableObject
    {
        public GameObject[] Effects;
        public int uniqueID;
    }
}