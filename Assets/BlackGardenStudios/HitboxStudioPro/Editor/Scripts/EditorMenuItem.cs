using BlackGardenStudios;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BlackGardenStudios.HitboxStudioPro
{
    public class EditorMenuItem
    {
        public string label { get; private set; }
        FontStyle style;
        Rect rect, border;
        Action<int> action;
        int index;
        public float width { set { border.width = rect.width = value; } }

        static readonly Color bgColor = new Color(0.25f, 0.25f, 0.25f, 1f);
        static readonly Color highlightColor = new Color(0f, 0.6f, 1f, 1f);
        static readonly Color borderColor = new Color(0.125f, 0.125f, 0.125f, 1f);

        public EditorMenuItem(string text, int index, Action<int> func, Vector2 menuPosition, FontStyle style = FontStyle.Normal)
        {
            label = text;
            rect = new Rect(menuPosition.x, menuPosition.y + index * 16f, 120f, 16f);
            border = new Rect(menuPosition.x, menuPosition.y + index * 16f - 1, 120f, 1f);
            action = func;
            this.index = index;
            this.style = style;
        }

        public void Draw(Vector2 mouse)
        {
            var color = (label != null && rect.Contains(mouse)) ? highlightColor : bgColor;

            EditorGUI.DrawRect(rect, color);
            EditorGUI.DrawRect(border, borderColor);

            if (label != null)
                HitBoxManagerInspector.DrawLabel(label, rect.position, 12, style);
        }

        public void ProcessEvent(Vector2 mouse, EventType type, int button)
        {
            if (rect.Contains(mouse) && type == EventType.MouseDown && button == 0 && action != null)
                action.Invoke(index);
        }
    }
}