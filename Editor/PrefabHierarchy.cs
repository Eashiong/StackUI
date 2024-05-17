using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
namespace StackUI.Tool
{
    public class PrefabBindWin : EditorWindow
    {
        public static void OpenWindow()
        {
            PrefabBindWin window = EditorWindow.GetWindow(typeof(PrefabBindWin), false, "绑定设置", true) as PrefabBindWin;
            window.Show();
        }
        public static bool Exist()
        {
            return EditorWindow.GetWindow(typeof(PrefabBindWin), true, "绑定设置", true) != null;
        }
        public static void CloseWindow()
        {
            PrefabBindWin window = EditorWindow.GetWindow(typeof(PrefabBindWin), true, "绑定设置", true) as PrefabBindWin;
            window.Close();
        }
        int index = 0;
        public void OnGUI()
        {
            if (Selection.activeObject == null || (GameObject)Selection.activeObject == null)
                return;
            GameObject target = (GameObject)Selection.activeObject;
            
            target = EditorGUILayout.ObjectField("物体", target, typeof(GameObject), false) as GameObject;
            GUISeparator();
            var coms = ((GameObject)Selection.activeObject).GetComponents<Component>();
            Dictionary<Type, Component> comsDir = new Dictionary<Type, Component>();
            string[] comsNames = new string[coms.Length + 2];
            comsDir.Add(typeof(GameObject),target.transform);
            comsDir.Add(typeof(Transform),target.transform);
            comsNames[0] = typeof(GameObject).Name;
            comsNames[1] = typeof(Transform).Name;
            //string[] options = { "GameObject", "Transform", "RectTransform", };
            for (int i = 0; i < coms.Length; i++)
            {
                var com = coms[i];
                if (!comsDir.ContainsKey(com.GetType()))
                {
                    comsDir.Add(com.GetType(), com);
                }
                comsNames[i+2] = com.GetType().Name;
            }
            index = EditorGUILayout.Popup("Bind Type", index, comsNames);
        }
        static public void GUILine(Color color, float height = 2f)
        {
            Rect position = GUILayoutUtility.GetRect(0f, float.MaxValue, height, height, LineStyle);

            if (Event.current.type == EventType.Repaint)
            {
                Color orgColor = GUI.color;
                GUI.color = orgColor * color;
                LineStyle.Draw(position, false, false, false, false);
                GUI.color = orgColor;
            }
        }
        static public GUIStyle _LineStyle;
        static public GUIStyle LineStyle
        {
            get
            {
                if (_LineStyle == null)
                {
                    _LineStyle = new GUIStyle();
                    _LineStyle.normal.background = EditorGUIUtility.whiteTexture;
                    _LineStyle.stretchWidth = true;
                }

                return _LineStyle;
            }
        }
        void GUISeparator()
        {
            GUILayout.Space(4);
            if (EditorGUIUtility.isProSkin)
            {
                GUILine(new Color(.15f, .15f, .15f), 1);
                GUILine(new Color(.4f, .4f, .4f), 1);
            }
            else
            {
                GUILine(new Color(.3f, .3f, .3f), 1);
                GUILine(new Color(.9f, .9f, .9f), 1);
            }
            GUILayout.Space(4);
        }


    }
    public class PrefabHierarchy
    {
        [InitializeOnLoadMethod]
        private static void Init()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyGUI;
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;

        }



        static int index = 0;
        private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
            if (Selection.activeObject && Selection.activeObject.GetInstanceID() == instanceID && Selection.activeObject is GameObject)
            {
                selectionRect.x = selectionRect.x + selectionRect.width - 40;
                selectionRect.width = 40;
                if (GUI.Button(selectionRect, "Bind"))
                {

                    PrefabBindWin.OpenWindow();
                }

            }
        }
    }


    public class BindInfo
    {
        GameObject go;
        Type type;
    }
}
