using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
namespace StackUI.Tool
{
    public class UISettingWindow : EditorWindow
    {
        [MenuItem("StackUI/Setting %#u")]
        public static void OpenWindow()
        {
            UISettingWindow window = EditorWindow.GetWindow(typeof(UISettingWindow), true, "StackUI设置", true) as UISettingWindow;
            window.Show();
        }

        private UISettingData uISettingData;
        bool isDirty = false;

        private void OnEnable()
        {
            uISettingData = UISettingData.Load();
        }

        public void OnGUI()
        {
            GUILayout.BeginVertical();

            GUILayout.Space(4);

            GenPrefabSetting(ref isDirty);

            GUISeparator();

            // GenCodeSetting(ref isDirty);

            // GUISeparator();

            using (new EditorGUI.DisabledScope(!isDirty))
            {
                if (GUILayout.Button("保存"))
                {
                    uISettingData.Save();
                    isDirty = false;
                }
            }


            GUILayout.EndVertical();
        }



        
        private void GenCodeSetting(ref bool isDirty)
        {
            GUILayout.Label("生成代码路径配置", EditorStyles.whiteLargeLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Label("生成路径", GUILayout.Width(100));
            string codeGenPath = GUILayout.TextField(uISettingData.codeGenPath);
            if (codeGenPath != uISettingData.codeGenPath)
                isDirty = true;
            uISettingData.codeGenPath = codeGenPath;
            GUILayout.EndHorizontal();

            GUILayout.Space(3);
            GUILayout.BeginHorizontal();
            GUILayout.Label("创建子目录", GUILayout.Width(100));
            bool createSubDir = EditorGUILayout.Toggle(uISettingData.createSubDir, GUILayout.Width(100));
            if (createSubDir != uISettingData.createSubDir)
                isDirty = true;
            uISettingData.createSubDir = createSubDir;
            GUILayout.EndHorizontal();
        }
        private void GenPrefabSetting(ref bool isDirty)
        {
            GUILayout.Label("预设模板路径配置", EditorStyles.whiteLargeLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Label("基础UI模板", GUILayout.Width(100));
            string simpleUITemplatePath = GUILayout.TextField(uISettingData.simpleUITemplatePath);
            if (simpleUITemplatePath != uISettingData.simpleUITemplatePath)
                isDirty = true;
            uISettingData.simpleUITemplatePath = simpleUITemplatePath;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("带返回按钮UI模板", GUILayout.Width(100));
            string pageTemplatePath = GUILayout.TextField(uISettingData.pageTemplatePath);
            if (pageTemplatePath != uISettingData.pageTemplatePath)
                isDirty = true;
            uISettingData.pageTemplatePath = pageTemplatePath;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("弹窗UI模板", GUILayout.Width(100));
            string popupTemplatePath = GUILayout.TextField(uISettingData.popupTemplatePath);
            if (popupTemplatePath != uISettingData.popupTemplatePath)
                isDirty = true;
            uISettingData.popupTemplatePath = popupTemplatePath;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("自定义模板", GUILayout.Width(100));
            string customTemplatePath = GUILayout.TextField(uISettingData.customTemplatePath);
            if (customTemplatePath != uISettingData.customTemplatePath)
                isDirty = true;
            uISettingData.customTemplatePath = customTemplatePath;
            GUILayout.EndHorizontal();

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



}
