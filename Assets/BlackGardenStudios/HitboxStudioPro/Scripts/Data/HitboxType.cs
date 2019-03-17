using System;
using UnityEngine;

namespace BlackGardenStudios.HitboxStudioPro
{
    [Serializable]
    public enum HitboxType
    {
        TRIGGER = 0, 
        HURT = 1, 
        GUARD = 2, 
        ARMOR = 3, 
        GRAB = 4, 
        TECH = 5
    }

    public class HitboxSettings
    {
        static public readonly int MAX_HITBOXES = 10;

        static public Color COLOR(HitboxType type)
        {
            switch(type)
            {
                case HitboxType.TRIGGER: return new Color(0f, 1f, 1f, 1f);
                case HitboxType.HURT: return new Color(1f, 0.125f, 0f, 1f);
                case HitboxType.GUARD: return new Color(0f, 0f, 1f, 1f);
                case HitboxType.ARMOR: return new Color(1f, 0.5f, 0f, 1f);
                case HitboxType.GRAB: return new Color(1f, 1f, 0f, 1f);
                case HitboxType.TECH: return new Color(0.75f, 0f, 1f, 1f);
            }

            return Color.black;
        }
    }
}