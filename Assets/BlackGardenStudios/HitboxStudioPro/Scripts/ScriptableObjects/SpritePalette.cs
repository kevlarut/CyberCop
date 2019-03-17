using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackGardenStudios.HitboxStudioPro
{
    [CreateAssetMenu()]
    public class SpritePalette : ScriptableObject
    {
        public string Name;
        public Color[] Colors;
        public Texture2D Texture
        {
            get
            {
                if (m_Texture == null)
                {
                    m_Texture = new Texture2D(256, 1, TextureFormat.ARGB32, false);
                    m_Texture.filterMode = FilterMode.Point;
                    m_Texture.SetPixels(0, 0, Colors.Length, 1, Colors);
                    m_Texture.Apply();
                }

                return m_Texture;
            }
        }

        private Texture2D m_Texture;
    }
}