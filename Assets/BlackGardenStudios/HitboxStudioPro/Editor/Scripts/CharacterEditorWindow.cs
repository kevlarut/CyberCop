using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace BlackGardenStudios.HitboxStudioPro
{
    public class CharacterEditorWindow : EditorWindow
    {
        GameObject target;
        float toolbarWidth = 300f;
        float previewZoom = 2f;
        Vector2 previewPan = Vector2.zero;
        int previewPaletteIndex = -1;
        Vector2 editorScroll;
        bool showColliders = true;
        bool showGizmos = false;
        bool showMove;
        bool showAnimation;
        bool showAttackData;
        bool showFrameData;
        bool showEvents;

        bool showCharacterEditor;
        bool showPaletteEditor;
        static Texture2D backgroundImage;
        static Texture2D editorBackgroundImage;

        private static readonly float MINZOOM = 1f;
        private static readonly float MAXZOOM = 8f;

        private HitBoxManagerInspector m_HitboxInspector = null;

        [MenuItem("Window/Black Garden Studios/Character Editor", priority = 1000)]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            CharacterEditorWindow window = (CharacterEditorWindow)EditorWindow.GetWindow(typeof(CharacterEditorWindow));
            window.titleContent = new GUIContent("Character Editor");
            window.Show();
        }

        void OnGUI()
        {
            wantsMouseMove = true;
            var currentEvent = Event.current;

            if (currentEvent.type == EventType.MouseDrag ||
                currentEvent.type == EventType.MouseDown ||
                currentEvent.type == EventType.MouseMove)
                Repaint();

            if (editorBackgroundImage == null)
                editorBackgroundImage = Resources.Load<Texture2D>("GrayCheckerBackground");

            if (backgroundImage == null)
                backgroundImage = MakeTex(1, 1, new Color(0.1f, 0.1f, 0.1f, 1f));

            GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), backgroundImage, ScaleMode.StretchToFill);

            //Render the preview
            SpriteRenderer renderer = null;
            HitboxManager hitboxManager = target == null ? null : hitboxManager = target.GetComponentInChildren<HitboxManager>();

            if (hitboxManager != null && (renderer = target.GetComponentInChildren<SpriteRenderer>()) != null)
            {
                var character = target.GetComponentInChildren<ICharacter>();
                if(m_HitboxInspector == null)
                    m_HitboxInspector = (HitBoxManagerInspector)Editor.CreateEditor(hitboxManager);

                var palette = character == null ? null : character.ActivePalette;

                RenderSprite(renderer, palette, previewPaletteIndex);
            }

            EditorGUILayout.BeginHorizontal();

            var color = GUI.backgroundColor;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(toolbarWidth));
            GUILayout.Label("Select Character", EditorStyles.boldLabel);
            var previoustarget = target;
            target = (GameObject)EditorGUILayout.ObjectField(target, typeof(GameObject), false);
            if(previoustarget != target)
            {
                m_HitboxInspector = null;
                Repaint();
                return;
            }
            EditorGUILayout.Separator();
            editorScroll = EditorGUILayout.BeginScrollView(editorScroll, GUILayout.Width(toolbarWidth));
            if (target != null && m_HitboxInspector != null)
            {
                RenderInspector(m_HitboxInspector);
            }

            showCharacterEditor = EditorGUILayout.Foldout(showCharacterEditor, new GUIContent("Character Settings"), true);

            if (showCharacterEditor && target != null)
            {
                ICharacter player = target.GetComponentInChildren<ICharacter>();

                if (player != null)
                {
                    Editor.CreateEditor((Object)player).OnInspectorGUI();
                }
            }

            showPaletteEditor = EditorGUILayout.Foldout(showPaletteEditor, new GUIContent("Palette Editor", 
                "View, modify, and create palettes for characters here."), true);

            if(currentEvent.type == EventType.MouseMove)
                previewPaletteIndex = -1;

            if (target != null && hitboxManager != null && showPaletteEditor)
            {
                var character = target.GetComponentInChildren<ICharacter>();

                if (character != null)
                {
                    var group = character.PaletteGroup;

                    if (group != null && group.Palettes.Length > 0)
                    {
                        var serialized = new SerializedObject((Object)character);
                        var list = new List<SpritePalette>(group.Palettes);
                        var names = list.Select((SpritePalette sp) => sp.Name);
                        var serializedpalette = serialized.FindProperty("m_ActivePalette");
                        var palette = character.ActivePalette;

                        palette = group.Palettes[EditorGUILayout.Popup(Mathf.Max(0, list.IndexOf(character.ActivePalette)), names.ToArray())];
                        serializedpalette.objectReferenceValue = palette;
                        serialized.ApplyModifiedProperties();

                        if (palette != null)
                        {
                            SpritePaletteInspector paletteInspector = (SpritePaletteInspector)Editor.CreateEditor(palette);

                            paletteInspector.OnInspectorGUI();

                            if (currentEvent.type == EventType.MouseMove)
                                previewPaletteIndex = paletteInspector.MouseOverIndex;
                        }
                    }
                }
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            showColliders = GUILayout.Toggle(showColliders && !showGizmos, "Edit Hitboxes");
            showGizmos = GUILayout.Toggle(showGizmos && !showColliders, "Edit Gizmos");
            if (GUILayout.Button("Settings", GUILayout.Width(100f)))
                SettingsEditorWindow.Init();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Zoom", EditorStyles.boldLabel, GUILayout.MaxWidth(40));
            previewZoom = Mathf.Round(Mathf.Clamp(GUILayout.HorizontalSlider(previewZoom, MINZOOM, MAXZOOM, GUILayout.MaxWidth(100)), MINZOOM, MAXZOOM));

            if (currentEvent.type == EventType.ScrollWheel)
            {
                var delta = (currentEvent.delta.y * -1f / Mathf.Abs(currentEvent.delta.y));

                previewZoom = Mathf.Clamp(previewZoom + delta, MINZOOM, MAXZOOM);
                Repaint();
            }
            else if (currentEvent.type == EventType.MouseDrag && currentEvent.button == 2)
            {
                var delta = currentEvent.delta;

                previewPan += delta;
            }

            if (GUILayout.Button("Reset View", GUILayout.MaxWidth(80)))
            {
                previewZoom = 2f;
                previewPan = Vector2.zero;
                Repaint();
            }

            if (m_HitboxInspector != null)
                m_HitboxInspector.DrawEditorTimeline(new Vector2(toolbarWidth + 50 + 240f, 45f),
                    position.width - toolbarWidth - 320,
                    Event.current.mousePosition);

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private void RenderInspector(HitBoxManagerInspector inspector)
        {
            inspector.showColliders = showColliders;
            inspector.showAnimation = showAnimation;
            inspector.showAttackData = showAttackData;
            inspector.showFrameData = showFrameData;
            inspector.showMove = showMove;
            inspector.showEvents = showEvents;

            inspector.OnInspectorGUI();

            showColliders = inspector.showColliders;
            showAnimation = inspector.showAnimation;
            showAttackData = inspector.showAttackData;
            showFrameData = inspector.showFrameData;
            showMove = inspector.showMove;
            showEvents = inspector.showEvents;
        }

        private float spriteWidth;
        private float spriteHeight;

        private void RenderSprite(SpriteRenderer renderer, SpritePalette palette, int highlightIndex = -1)
        {
            if (renderer == null || renderer.sprite == null) return;

            var scale = previewZoom;
            var pivot = renderer.sprite.pivot * scale;
            spriteWidth = renderer.sprite.rect.width * scale;
            spriteHeight = renderer.sprite.rect.height * scale;
            var pos = new Vector2(
                toolbarWidth + ((position.width - toolbarWidth) / 2f) - (spriteWidth / 2f),
                position.height / 2f - (spriteHeight / 2f));

            if (Event.current.type == EventType.Repaint)
                DrawTextureGUI(pos + previewPan, renderer.sprite, 
                    new Vector2(spriteWidth, spriteHeight), palette, highlightIndex);

            var normalizedY = pivot.y / spriteHeight;
            normalizedY = 1f - normalizedY;
            pivot.y = normalizedY * spriteHeight;

            var mouse = Event.current.mousePosition;

            if (showColliders)
                m_HitboxInspector.DrawColliderRects(pos + pivot + previewPan, pivot, previewPan, mouse, scale);
            else if (showGizmos)
                m_HitboxInspector.DrawEditorGizmos(pos + pivot + previewPan, pivot, mouse, scale);
        }

        static private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }

        static private void DrawTexture(Vector2 position, Sprite sprite, Vector2 size)
        {
            Rect spriteRect = new Rect(sprite.rect.x / sprite.texture.width, sprite.rect.y / sprite.texture.height,
                                       sprite.rect.width / sprite.texture.width, sprite.rect.height / sprite.texture.height);
            Vector2 actualSize = size;

            actualSize.y *= (sprite.rect.height / sprite.rect.width);
            Graphics.DrawTexture(new Rect(position.x, position.y + (size.y - actualSize.y) / 2, actualSize.x, actualSize.y), sprite.texture, spriteRect, 0, 0, 0, 0);
        }

        static private Texture2D m_BufferTexture;

        static private void DrawTextureGUI(Vector2 position, Sprite sprite, 
            Vector2 size, SpritePalette palette, int highlightIndex = -1)
        {
            Rect spriteRect = new Rect(sprite.rect.x / sprite.texture.width, sprite.rect.y / sprite.texture.height,
                                       sprite.rect.width / sprite.texture.width, sprite.rect.height / sprite.texture.height);
            Vector2 actualSize = size;

            var checkerWidth = Mathf.CeilToInt(size.x / 32f);
            var checkerHeight = Mathf.CeilToInt(size.y / 32f);

            for (int x = 0; x < checkerWidth; x++)
                for (int y = 0; y < checkerHeight; y++)
                    GUI.DrawTexture(new Rect(position.x + x * 32f, position.y + y * 32f, 32f, 32f), editorBackgroundImage);

            if (palette == null)
                GUI.DrawTextureWithTexCoords(new Rect(position.x, position.y, actualSize.x, actualSize.y), sprite.texture, spriteRect);
            else
            {
                CopySpriteToTexture(sprite, palette, highlightIndex);
                GUI.DrawTexture(new Rect(position.x, position.y, actualSize.x, actualSize.y), m_BufferTexture);
            }
        }

        //If we don't have a palette assigned don't even bother with this function
        static private void CopySpriteToTexture(Sprite sprite, SpritePalette palette, int highlightIndex = -1)
        {
            var source = sprite.texture;
            var rect = sprite.rect;
            int width = (int)rect.width;
            int height = (int)rect.height;

            if (m_BufferTexture == null)
            {
                m_BufferTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
                m_BufferTexture.filterMode = FilterMode.Point;
                m_BufferTexture.wrapMode = TextureWrapMode.Clamp;
            }
            else if (m_BufferTexture.width != width || m_BufferTexture.height != height)
            {
                m_BufferTexture.Resize(width, height);
                m_BufferTexture.Apply();
            }
           
            var inPixels = source.GetPixels(Mathf.FloorToInt(rect.x), Mathf.FloorToInt(rect.y), width, height);
            var outPixels = new Color[width * height];

            for (int i = 0; i < width * height; i++)
            {
                var index = Mathf.Clamp(Mathf.RoundToInt(inPixels[i].r * 255f), 0, palette.Colors.Length - 1);

                outPixels[i] = index == highlightIndex ? new Color(1f, 0f, 1f, 1f) : palette.Colors[index];
                outPixels[i].a = inPixels[i].a;
            }

            m_BufferTexture.SetPixels(outPixels);
            m_BufferTexture.Apply();
        }
    }
}