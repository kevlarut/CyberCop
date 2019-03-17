using System;
using System.Diagnostics;

using UnityEditor;
using UnityEngine;

using Debug = UnityEngine.Debug;

namespace BlackGardenStudios.HitboxStudioPro.Help
{
    [HelpURL("https://makaka.org/category/docs")]
    [InitializeOnLoad]
    public class PublisherWelcomeScreen : EditorWindow
    {
        private static PublisherWelcomeScreen window;
        private static Vector2 windowsSize = new Vector2(500f, 325f);
        private Vector2 scrollPosition;

        private static string windowHeaderText = "Black Garden Studios Support";
        private string copyright = "© Copyright " + DateTime.Now.Year + " Black Garden Studios ";

        private const string isShowAtStartEditorPrefs = "WelcomeScreenShowAtStart";
        private static bool isShowAtStart = true;

        private static bool isInited;

        private static GUIStyle headerStyle;
        private static GUIStyle copyrightStyle;
        
        private static Texture2D docsIcon;
        private static Texture2D rateIcon;
        private static Texture2D supportIcon;
        private static Texture2D twitterIcon;

        static PublisherWelcomeScreen()
        {
            EditorApplication.update -= GetShowAtStart;
            EditorApplication.update += GetShowAtStart;
        }

        private void OnGUI()
        {
            if (!isInited)
            {
                Init();
            }

            if (GUI.Button(new Rect(0f, 0f, windowsSize.x, 58f), "", headerStyle))
                Process.Start("http://blackgarden.games");

            GUILayoutUtility.GetRect(position.width, 70f);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            if (DrawButton(docsIcon, "Guide", "View our online user guide"))
                Process.Start("http://blackgarden.games/hitbox-studio-user-guide/");

            if (DrawButton(supportIcon, "Support", "If the guide doesn't cover it contact us for support"))
                Process.Start("mailto:support@blackgarden.games?subject=Asset Support");

            if (DrawButton(rateIcon, "Rate and Review", "Let everyone know what you think about this tool"))
                Process.Start("http://u3d.as/1rZk");

            if (DrawButton(twitterIcon, "Twitter", "Latest news on our projects"))
                Process.Start("https://www.twitter.com/BlackGardendev/");

            /*//if (DrawButton(allOurAssetsIcon, "Black Garden Studio", "Our unity asset publisher page")) 
            //	Process.Start("https://assetstore.unity.com/publishers/17426");*/



            EditorGUILayout.EndScrollView();
            EditorGUILayout.LabelField(copyright, copyrightStyle);
        }

        private static bool Init()
        {
            try
            {
                headerStyle = new GUIStyle();
                headerStyle.normal.background = Resources.Load("HeaderLogo") as Texture2D;
                headerStyle.normal.textColor = Color.white;
                headerStyle.fontStyle = FontStyle.Bold;
                headerStyle.padding = new RectOffset(340, 0, 27, 0);
                headerStyle.margin = new RectOffset(0, 0, 0, 0);

                copyrightStyle = new GUIStyle();
                copyrightStyle.alignment = TextAnchor.MiddleRight;

                docsIcon = Resources.Load("Docs") as Texture2D;
                rateIcon = Resources.Load("Rate") as Texture2D;
                supportIcon = Resources.Load("Support") as Texture2D;
                twitterIcon = Resources.Load("Twitter") as Texture2D;

                isInited = true;
            }
            catch (Exception e)
            {
                Debug.Log("WELCOME SCREEN INIT: " + e);
                return false;
            }

            return true;
        }

        private static bool DrawButton(Texture2D icon, string title = "", string description = "")
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(34f);
            GUILayout.Box(icon, GUIStyle.none, GUILayout.MaxWidth(48f), GUILayout.MaxHeight(48f));
            GUILayout.Space(10f);

            GUILayout.BeginVertical();

            GUILayout.Space(1f);
            GUILayout.Label(title, EditorStyles.boldLabel);
            GUILayout.Label(description);

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            Rect rect = GUILayoutUtility.GetLastRect();
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);

            GUILayout.Space(10f);

            return Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition);
        }


        private static void GetShowAtStart()
        {
            EditorApplication.update -= GetShowAtStart;

            isShowAtStart = EditorPrefs.GetBool(isShowAtStartEditorPrefs, true);

            if (isShowAtStart)
            {
                EditorApplication.update -= OpenAtStartup;
                EditorApplication.update += OpenAtStartup;
            }
        }

        private static void OpenAtStartup()
        {
            if (isInited && Init())
            {
                OpenWindow();

                EditorApplication.update -= OpenAtStartup;
            }
        }

        [MenuItem("Window/Black Garden Studios/Help", false, 1001)]
        public static void OpenWindow()
        {
            if (window == null)
            {
                window = GetWindow<PublisherWelcomeScreen>(true, windowHeaderText, true);
                window.maxSize = window.minSize = windowsSize;
            }
        }

        private void OnEnable()
        {
            window = this;
        }

        private void OnDestroy()
        {
            window = null;

            EditorPrefs.SetBool(isShowAtStartEditorPrefs, false);
        }
    }
}