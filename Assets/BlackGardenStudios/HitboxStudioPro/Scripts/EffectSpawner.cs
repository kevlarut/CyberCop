using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BlackGardenStudios.HitboxStudioPro
{
    public class EffectSpawner : MonoBehaviour
    {
        AttackFX[] m_Effects;
        Dictionary<int, AttackFX> m_EffectDict = new Dictionary<int, AttackFX>(12);

        static private EffectSpawner instance
        {
            get
            {
                if (m_Instance == null)
                {
                    new GameObject("Effect Spawner", typeof(EffectSpawner));
                }

                return m_Instance;
            }
            set
            {
                m_Instance = value;
            }
        }

        static private EffectSpawner m_Instance;

        static public void PlayHitEffect(int uid, Vector3 point, int order, bool flipx)
        {
            instance._PlayHitEffect(uid, point, order, flipx);
        }

        public void _PlayHitEffect(int uid, Vector3 point, int order, bool flipx)
        {
            AttackFX pool;

            if (m_EffectDict.TryGetValue(uid, out pool) && pool.Effects != null && pool.Effects.Length > 0)
            {
                GameObject effect = pool.Effects[Random.Range(0, pool.Effects.Length)];
                var go = Instantiate(effect, point, Quaternion.identity);
                var renderer = go.GetComponent<SpriteRenderer>();

                renderer.flipX = flipx;
                renderer.sortingOrder = order;
                StartCoroutine(DestroyEffect(go));
            }
        }

        private WaitForSeconds m_Wait = new WaitForSeconds(1f);

        private IEnumerator DestroyEffect(GameObject go)
        {
            yield return m_Wait;
            Destroy(go);
        }

        void Awake()
        {
            if (m_Instance == null)
            {
                m_Instance = this;
                m_Effects = Resources.LoadAll<AttackFX>("Effects/");

                for (int i = 0; i < m_Effects.Length; i++)
                    m_EffectDict.Add(m_Effects[i].uniqueID, m_Effects[i]);

            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        static public AttackFX[] GetPools()
        {
            var list = Resources.LoadAll<AttackFX>("Effects/").ToList();
            list.Sort((AttackFX a, AttackFX b) => b.uniqueID - a.uniqueID);
            return list.ToArray();
        }

        void OnDestroy()
        {
            if (m_Instance == this)
            {
                m_Instance = null;
            }
        }
    }
}