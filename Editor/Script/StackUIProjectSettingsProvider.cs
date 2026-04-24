
#if UNITY_2019_4_OR_NEWER
using UnityEditor;
using UnityEngine;

namespace StackUI.Editor
{
    public class StackUIProjectSettingsProvider : SettingsProvider
    {
        public const string SettingsPath = "Project/StackUI";
        private SerializedObject settings;
        private SerializedProperty defaultDataProp;

        public StackUIProjectSettingsProvider(string path, SettingsScope scope) : base(path, scope)
        {
        }

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            return new StackUIProjectSettingsProvider(SettingsPath, SettingsScope.Project)
            {
                label = "StackUI"
            };
        }

        public override void OnActivate(string searchContext, UnityEngine.UIElements.VisualElement rootElement)
        {
            settings = new SerializedObject(StackUIProjectSettings.instance);
            defaultDataProp = settings.FindProperty("defaultUISettingData");
        }

        public override void OnGUI(string searchContext)
        {
            if (settings == null)
            {
                settings = new SerializedObject(StackUIProjectSettings.instance);
                defaultDataProp = settings.FindProperty("defaultUISettingData");
            }

            settings.Update();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(defaultDataProp, new GUIContent("Default UI Setting Data"));
            if (EditorGUI.EndChangeCheck())
            {
                settings.ApplyModifiedPropertiesWithoutUndo();
                StackUIProjectSettings.instance.Save();
            }
            else
            {
                settings.ApplyModifiedPropertiesWithoutUndo();
            }

            if (defaultDataProp.objectReferenceValue == null)
                EditorGUILayout.HelpBox("请指定默认的 UISettingData 资源。", MessageType.Info);
        }

    }
}
#endif