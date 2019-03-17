using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BlackGardenStudios.HitboxStudioPro
{
    public static class ListExtensions
    {
        static public bool TryUniqueAdd<T>(this List<T> list, T item)
        {
            if (list.Contains(item))
                return false;
            else
            {
                list.Add(item);
                return true;
            }
        }
    }
}