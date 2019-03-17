using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

namespace BlackGardenStudios.HitboxStudioPro
{
    [CustomEditor(typeof(HitboxCollisionMatrix))]
    public class HitboxCollisionMatrixInspector : Editor
    {
        private HitboxCollisionMatrix m_Matrix;
        private SerializedObject m_Object;

        public void OnEnable()
        {
            m_Matrix = (HitboxCollisionMatrix)target;
            m_Object = new SerializedObject(target);
        }

        public override void OnInspectorGUI()
        {
            if (m_Matrix.m_CollisionMatrix == null) return;
            var label = Enum.GetNames(typeof(HitboxType));
            var reverse = new List<string>(label.Reverse());
            var maxlen = label.Max((string s) => s.Length);
            var len = Mathf.RoundToInt(Mathf.Sqrt(m_Matrix.m_CollisionMatrix.Length));
            var matrix = m_Object.FindProperty("m_CollisionMatrix");
            var spacerHeight = maxlen * 8.5f;

            EditorGUILayout.BeginHorizontal(GUILayout.Height(spacerHeight));
            GUILayout.Space(132f);
            for (int i = 0; i < len; i++)
            {
                GUILayout.Space(28f);
                var rect = GUILayoutUtility.GetLastRect();
                DrawLabel(reverse[i], new Vector2(Mathf.Round(rect.xMin + 5f), Mathf.Round(rect.yMin + spacerHeight - reverse[i].Length * 8.5f)), 90f);
            }
            EditorGUILayout.EndHorizontal();
            for (int y = 0; y < len; y++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(120f);
                var rect = GUILayoutUtility.GetLastRect();

                DrawLabel(label[y], new Vector2(Mathf.Round(rect.xMin + 115 - label[y].Length * 8.45f), Mathf.Round(rect.yMin + 8f)));

                for (int x = 0; x < len - y; x++)
                {
                    var element = matrix.GetArrayElementAtIndex(x + y * len);
                    var value = element.enumValueIndex;
                    var bgColor = Color.grey;
                    bool same = x == len - y - 1;
                    string str = " ";

                    switch (value)
                    {
                        case 1:
                            str = "<";
                            bgColor = Color.cyan;
                            break;
                        case 2:
                            str = "^";
                            bgColor = Color.green;
                            break;
                        case 3:
                            str = "+";
                            bgColor = Color.magenta;
                            break;
                    }

                    var bgSave = GUI.backgroundColor;
                    GUI.backgroundColor = bgColor;
                    if (GUILayout.Button(str, GUILayout.Width(24f), GUILayout.Height(24f)))
                    {
                        switch (value)
                        {
                            default:
                            case 0:
                                element.enumValueIndex = same ? 3 : 1; break;
                            case 1:
                                element.enumValueIndex++; break;
                            case 2:
                                element.enumValueIndex++; break;
                            case 3:
                                element.enumValueIndex = 0; break;
                        }
                    }
                    GUI.backgroundColor = bgSave;

                }

                EditorGUILayout.EndHorizontal();
            }

            m_Object.ApplyModifiedProperties();
        }

        private void DrawLabel(string label, Vector2 position, float angle = 0f)
        {
            if (angle == 0f)
            {
                HitBoxManagerInspector.DrawLabel(label, position, 12, FontStyle.Normal, true);
            }
            else
            {
                var matrix = GUI.matrix;
                GUIUtility.RotateAroundPivot(angle, position);
                HitBoxManagerInspector.DrawLabel(label, position, 12, FontStyle.Normal, true);
                GUI.matrix = matrix;
            }
        }
    }
}