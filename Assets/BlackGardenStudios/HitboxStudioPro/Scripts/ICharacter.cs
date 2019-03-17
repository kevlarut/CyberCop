using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackGardenStudios.HitboxStudioPro
{
    public interface ICharacter
    {
        SpritePalette ActivePalette { get; }
        SpritePaletteGroup PaletteGroup { get; }

        float Poise { get; set; }

        /// <summary>
        /// Is the characters facing direction flipped?
        /// </summary>
        bool FlipX { get; }

        /// <summary>
        /// Called when two valid hitboxes intersect.
        /// </summary>
        void HitboxContact(ContactData data);
    }

}
