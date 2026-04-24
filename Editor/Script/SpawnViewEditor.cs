/*
    物体创建编辑器
*/

using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace StackUI.Editor
{
    public class SpawnViewEditor
    {

        private static bool CHECK()
        {

            if(SettingData == null)
            {
#if UNITY_2019_4_OR_NEWER
                Debug.LogError("StackUI: 未配置全局模板，请把StackUI/Asset/StackUISetting.asset 放入Project Setting/StackUI/Default UI Setting Data。");
                Debug.LogError("StackUI: 或使用菜单`Assets/StackUI/Create Setting Data`创建一个");
#else
                Debug.LogError("StackUI: 找不到:" + DefaultSettingAssetPath);
#endif
                return false;
            }

            return true;
        }

        private const string DefaultSettingAssetPath = "Assets/StackUI/Asset/StackUISetting.asset";
        private static UISettingData _settingData;
        private static UISettingData SettingData
        {
            get
            {
                if(_settingData == null)
                {
#if UNITY_2019_4_OR_NEWER
                    _settingData = StackUIProjectSettings.instance.defaultUISettingData;
#else
                    _settingData = AssetDatabase.LoadAssetAtPath<UISettingData>(DefaultSettingAssetPath);
#endif
                }
                return _settingData;
            }
        }


        [MenuItem("GameObject/StackUI/Simple View", false, 0)]
        static void CreateSimpleUI()
        {
            if(!CHECK())
                return;
            SpawnTemplate(SettingData.simpleUITemplatePath, "基础UI模板");


        }

        
        [MenuItem("GameObject/StackUI/Page View", false, 0)]
        static void CreatePage()
        {
            if(!CHECK())
                return;
                
            SpawnTemplate(SettingData.pageTemplatePath, "带返回按钮UI模板");

        }
        [MenuItem("GameObject/StackUI/Popup View", false, 0)]
        static void CreatePopup()
        {
            if(!CHECK())
                return;
                
            SpawnTemplate(SettingData.popupTemplatePath, "弹窗UI模板");

        }

        [MenuItem("GameObject/StackUI/Text Button", false, 0)]
        static void CreateTextButton()
        {
            if(!CHECK())
                return;
                
            GameObject btn = SpawnTemplate(SettingData.TextButtonTemplatePath, "文本按钮");
            if(btn)
            {
                btn.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                btn.transform.localScale = Vector3.one;
            }
        }

         [MenuItem("GameObject/StackUI/Image Button", false, 0)]
        static void CreateImageButton()
        {
            if(!CHECK())
                return;
                
            GameObject btn = SpawnTemplate(SettingData.ImageButtonTemplatePath, "图片按钮");
            if(btn)
            {
                btn.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                btn.transform.localScale = Vector3.one;
            }

        }

        private static GameObject SpawnTemplate(GameObject prefab, string label)
        {
            if (prefab == null)
            {
                Debug.LogError($"StackUI: `{SettingData.name}.asset` 没有配置相应模板，无法创建对象");
                return null;
            }

            GameObject go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if (go == null)
            {
                go = GameObject.Instantiate(prefab);
            }
            go.name = prefab.name;
            go.transform.SetParent(Selection.activeTransform);
            Selection.activeTransform = go.transform;

            PrefabUtility.UnpackPrefabInstance(go,PrefabUnpackMode.Completely,InteractionMode.AutomatedAction);
            return go;
        }
    }
}
