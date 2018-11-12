using UnityEngine;

namespace Assets.Scripts
{
    public class WaterTexture : MonoBehaviour
    {
        public float scrollSpeed = 0.1f;
        Renderer rend;

        void Start()
        {
            rend = GetComponent<Renderer> ();
        }

        void Update()
        {
            float offset = Time.time * scrollSpeed;
            rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        }
    }
}