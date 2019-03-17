using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BlackGardenStudios.HitboxStudioPro
{
    public class SettingsEditorWindow : EditorWindow
    {
        TextAsset m_TypeTemplate;
        TextAsset m_EventTemplate;

        Vector2 m_Scroll;
        bool m_TypeFoldout = true;
        bool m_EventFoldout = false;
        bool m_TypeChange = false;
        bool m_EventChange = false;

        List<TypeEntry> m_EventEntries;
        List<TypeEntry> m_TypeEntries;
        int m_MaxHitboxes;

        GUIContent m_ToolbarPlusIcon;
        GUIContent m_ToolbarMinusIcon;

        internal class TypeEntry
        {
            internal string Label;
            internal int Value;
            internal Color Color;
        }

        //Whenever we add new types we'll pull from this list.
        static readonly Color[] m_DefaultHitboxColors = new Color[]
{
            new Color(0f,       1f,         1f,     1f), //Cyan
            new Color(1f,       0.125f,     0f,     1f), //Red
            new Color(0f,       0f,         1f,     1f), //Blue
            new Color(1f,       0.5f,       0f,     1f), //Orange
            new Color(1f,       1f,         0f,     1f), //Yellow
            new Color(0.75f,    0f,         1f,     1f), //Purple
            new Color(0f,       1f,         0f,     1f), //Green
            new Color(1f,       0f,         0.75f,  1f), //Pink
            new Color(0.5f,     1f,         0f,     1f), //Lime
            new Color(0f,       1f,         0.5f,   1f)  //Teal
};

        static public void Init()
        {
            // Get existing open window or if none, make a new one:
            SettingsEditorWindow window = (SettingsEditorWindow)GetWindow(typeof(SettingsEditorWindow),true, "Hitbox Studio Pro Settings", true);
            window.minSize = new Vector2(350, 350);
            window.Show();
        }

        void OnEnable()
        {
            //if (EditorGUIUtility.isProSkin)
            m_ToolbarPlusIcon = EditorGUIUtility.IconContent("Toolbar Plus");
            m_ToolbarMinusIcon = EditorGUIUtility.IconContent("Toolbar Minus");

            m_TypeTemplate = Resources.Load<TextAsset>("Generation/HitboxType");
            m_EventTemplate = Resources.Load<TextAsset>("Generation/FrameEvent");

            RefreshTypes();
            RefreshEvents();
        }

        void RefreshTypes()
        {
            var typeLabels = Enum.GetNames(typeof(HitboxType));
            var typeValues = Enum.GetValues(typeof(HitboxType));

            m_TypeEntries = new List<TypeEntry>();

            for (int i = 0; i < typeLabels.Length; i++)
            {
                m_TypeEntries.Add(new TypeEntry
                {
                    Label = typeLabels[i],
                    Color = HitboxSettings.COLOR((HitboxType)typeValues.GetValue(i)),
                    Value = (int)typeValues.GetValue(i)
                });
            }

            m_MaxHitboxes = HitboxSettings.MAX_HITBOXES;
        }

        void RefreshEvents()
        {
            var eventLabels = Enum.GetNames(typeof(FrameEvent));
            var eventValues = Enum.GetValues(typeof(FrameEvent));

            m_EventEntries = new List<TypeEntry>();

            for (int i = 0; i < eventLabels.Length; i++)
            {
                m_EventEntries.Add(new TypeEntry
                {
                    Label = eventLabels[i],
                    Value = (int)eventValues.GetValue(i)
                });
            }
        }

        void SaveTypes()
        {
            var template = m_TypeTemplate.text;
            var entries = "";
            var colors = "";

            for (int i = 0; i < m_TypeEntries.Count; i++)
            {
                if (string.IsNullOrEmpty(m_TypeEntries[i].Label)) { m_TypeEntries.RemoveAt(i); i--; continue; }

                var entry = (i > 0 ? ", \n        " : "        ") + m_TypeEntries[i].Label + " = " + m_TypeEntries[i].Value;
                var color = m_TypeEntries[i].Color;
                var switchColor = (i > 0 ? "\n                " : "                ") + "case HitboxType." + m_TypeEntries[i].Label 
                                    + ": return new Color(" + color.r + "f, " + color.g +"f, " + color.b +"f, 1f);";
                entries += entry;
                colors += switchColor;
            }

            template = template.Replace("[ENTRIES]", entries);
            template = template.Replace("[MAX_HITBOXES]", m_MaxHitboxes.ToString());
            template = template.Replace("[COLOR_SWITCH]", colors);

            File.WriteAllText(GetHitboxStudioDataPath() + "HitboxType.cs", template);

            AssetDatabase.Refresh();
        }

        void SaveEvents()
        {
            var template = m_EventTemplate.text;
            var entries = "";

            for (int i = 0; i < m_EventEntries.Count; i++)
            {
                if (string.IsNullOrEmpty(m_EventEntries[i].Label)) { m_EventEntries.RemoveAt(i); i--; continue; }

                var entry = (i > 0 ? ", \n        " : "        ") + m_EventEntries[i].Label + " = " + m_EventEntries[i].Value;
                entries += entry;
            }

            File.WriteAllText(GetHitboxStudioDataPath() + "FrameEvent.cs", template.Replace("[ENTRIES]", entries));

            AssetDatabase.Refresh();
        }

        string GetHitboxStudioDataPath()
        {
            var dataPath = Application.dataPath;
            var scriptPath = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this));

            dataPath = dataPath.Substring(0, dataPath.LastIndexOf("Assets"));
            scriptPath = scriptPath.Substring(0, scriptPath.LastIndexOf("Editor/Scripts/Settings"));

            return dataPath + scriptPath + "Scripts/Data/";
        }

        void OnGUI()
        {
            m_Scroll = EditorGUILayout.BeginScrollView(m_Scroll);
            RenderEvents();
            RenderTypes();
            EditorGUILayout.EndScrollView();
        }

        void RenderStrings(string label, List<TypeEntry> list, ref bool foldout, ref bool changes, Action apply, Action refresh, Action ui = null, bool color = false)
        {
            foldout = EditorGUILayout.Foldout(foldout, label);

            if (foldout)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                if (ui != null)
                    ui();
                for (int i = 0; i < list.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    int value = list[i].Value;
                    EditorGUILayout.LabelField(value.ToString(), GUILayout.MaxWidth(24f));
                    GUI.SetNextControlName(i.ToString());
                    var originalLabel = list[i].Label;
                    list[i].Label = EditorGUILayout.TextField(list[i].Label).ToUpper();

                    if (!originalLabel.Equals(list[i].Label))
                        changes = true;

                    if (color == true)
                    {
                        var originalColor = list[i].Color;
                        //make sure color alpha = 1f
#if UNITY_2018_2_OR_NEWER
                        list[i].Color = EditorGUILayout.ColorField(new GUIContent(""), list[i].Color, true, false, false, GUILayout.Width(32));
#else
                        list[i].Color = EditorGUILayout.ColorField(new GUIContent(""), list[i].Color, true, false, false, null, GUILayout.Width(32));
#endif
                        list[i].Color.a = 1f;

                        if (originalColor != list[i].Color)
                            changes = true;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(m_ToolbarPlusIcon, GUILayout.Width(32f)))
                {
                    var value = Enumerable.Range(0, list.Count + 1).Except(list.Select((TypeEntry e) => e.Value)).Min();
                    list.Add(new TypeEntry
                    {
                        Label = "",
                        Value = value,
                        Color = m_DefaultHitboxColors[value % m_DefaultHitboxColors.Length]
                    });
                    list.Sort((TypeEntry a, TypeEntry b) => a.Value - b.Value);
                    changes = true;
                }
                if (GUILayout.Button(m_ToolbarMinusIcon, GUILayout.Width(32f)))
                {
                    var strIndex = GUI.GetNameOfFocusedControl();

                    if (!string.IsNullOrEmpty(strIndex))
                        list.RemoveAt(Convert.ToInt32(strIndex));

                    GUI.FocusControl("");
                    changes = true;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginHorizontal();
                GUI.enabled = changes;
                if (GUILayout.Button("Apply", GUILayout.Width(64f))) { changes = false; GUI.FocusControl(""); apply(); }
                if (GUILayout.Button("Cancel", GUILayout.Width(64f))) { changes = false; GUI.FocusControl(""); refresh(); }
                GUI.enabled = true;
                EditorGUILayout.EndHorizontal();
            }
        }

        void RenderTypes()
        {
            RenderStrings("Hitbox Configuration", m_TypeEntries, ref m_TypeFoldout, ref m_TypeChange, SaveTypes, RefreshTypes,
                () =>
                {
                    var count = m_MaxHitboxes;

                    m_MaxHitboxes = Mathf.Max(1, EditorGUILayout.IntField("Maximum Hitboxes", m_MaxHitboxes));

                    if (count != m_MaxHitboxes)
                        m_TypeChange = true;
                }, true);

            if (m_TypeFoldout)
                Editor.CreateEditor(HitboxCollisionMatrix.Instance).OnInspectorGUI();
        }

        void RenderEvents()
        {
            RenderStrings("Animation Events", m_EventEntries, ref m_EventFoldout, ref m_EventChange, SaveEvents, RefreshEvents);
        }
    }
}