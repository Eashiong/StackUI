
#if UNITY_2019_4_OR_NEWER

using UnityEditor;

namespace StackUI.Editor
{
    [FilePath("ProjectSettings/StackUISettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class StackUIProjectSettings : ScriptableSingleton<StackUIProjectSettings>
    {
        private const string DefaultSettingAssetPath = "Assets/StackUI/Asset/StackUISetting.asset";
        public UISettingData defaultUISettingData;

        [InitializeOnLoadMethod]
        private static void EnsureDefaultSettingData()
        {
            var settings = instance;
            if (settings.defaultUISettingData != null)
                return;

            var defaultAsset = AssetDatabase.LoadAssetAtPath<UISettingData>(DefaultSettingAssetPath);
            if (defaultAsset == null)
                return;

            settings.defaultUISettingData = defaultAsset;
            settings.Save();
        }

        public void Save()
        {
            Save(true);
        }
    }
}
#endif