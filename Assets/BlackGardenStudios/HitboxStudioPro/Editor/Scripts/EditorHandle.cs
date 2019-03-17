using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BlackGardenStudios.HitboxStudioPro
{
    public class EditorHandle
    {
        /// <summary>
        ///Multiply cursor delta by this to apply it to the collider rect.
        ///When dragging a left anchor we mul by 1, 0, 0, -1
        ///That decreases X while increasing width so the right anchor stays in place.
        ///Right anchors can have x=0, y=0, because they just add/subtract width and height from the rect.
        /// </summary>
        public Vector4 delta { get; internal set; }
        /// <summary>
        ///Center of the handle
        /// </summary>
        public Vector2 center { get; internal set; }
        /// <summary>
        /// Use the generated rect to detect if the cursor is over the handle
        /// </summary>
        public Rect rect { get; internal set; }
        /// <summary>
        /// Custom handle action for gizmos
        /// </summary>
        public Action<Vector2> action;

        static readonly private int SIZE = 8;
        static readonly private int RADIUS = SIZE / 2;

        internal EditorHandle()
        {
            center = Vector2.zero;
            delta = Vector4.zero;
            rect = new Rect();
        }

        internal EditorHandle(Vector4 mul)
        {
            delta = mul;
        }

        internal void Set(Vector2 position, Vector4 mul)
        {
            delta = mul;
            Set(position);
        }

        internal void Set(Vector2 position)
        {
            center = position;
            rect = new Rect(position.x - RADIUS, position.y - RADIUS, SIZE, SIZE);
        }

        public bool PointIntersect(Vector2 point)
        {
            return point.x >= rect.xMin && point.x <= rect.xMax && point.y >= rect.yMin && point.y <= rect.yMax;
        }

        public Vector4 GetDragRect(Vector2 mouse)
        {
            Vector2 diff = mouse - center;

            return new Vector4(diff.x * delta.x, diff.y * delta.y, diff.x * delta.z, diff.y * delta.w);
        }

        internal bool Draw(Vector2 position, Vector2 mouse, MouseCursor cursor)
        {
            bool hover = false;

            Set(position);

            var outerColor = (hover = PointIntersect(mouse)) ? new Color(1f, 0f, 0f, 1f) : new Color(0f, 0f, 0f, 1f);
            var innerColor = new Color(1f, 1f, 1f, 1f);

            if (Event.current.type == EventType.Repaint)
            {
                EditorGUI.DrawRect(rect, outerColor);
                EditorGUI.DrawRect(new Rect(position.x - RADIUS / 2, position.y - RADIUS / 2, SIZE / 2, SIZE / 2), innerColor);
                EditorGUIUtility.AddCursorRect(rect, cursor);
            }

            return hover;
        }
    }
}