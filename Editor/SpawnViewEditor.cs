/*
    物体创建编辑器
*/

using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace StackUI.Tool
{
    public class SpawnViewEditor
    {

        [MenuItem("GameObject/StackUI/Simple View", false, 0)]
        static void CreateSimpleUI()
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(UISettingData.CurrentData.simpleUITemplatePath + ".prefab");
            GameObject go = GameObject.Instantiate(prefab);
            go.name = prefab.name;
            Selection.activeTransform = go.transform;

        }

        
        [MenuItem("GameObject/StackUI/Page View", false, 0)]
        static void CreatePage()
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(UISettingData.CurrentData.pageTemplatePath + ".prefab");
            GameObject go = GameObject.Instantiate(prefab);
            go.name = prefab.name;
            Selection.activeTransform = go.transform;


        }
        [MenuItem("GameObject/StackUI/Popup View", false, 0)]
        static void CreatePopup()
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(UISettingData.CurrentData.popupTemplatePath + ".prefab");
            GameObject go = GameObject.Instantiate(prefab);
            go.name = prefab.name;
            Selection.activeTransform = go.transform;

        }
        [MenuItem("GameObject/StackUI/Custom View", false, 0)]
        static void CreateCustom()
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(UISettingData.CurrentData.customTemplatePath + ".prefab");
            GameObject go = GameObject.Instantiate(prefab);
            go.name = prefab.name;
            Selection.activeTransform = go.transform;

        }

        
        [MenuItem("GameObject/StackUI/Image")]
        static void CreatImage()
        {
           
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject("Image", typeof(Image));
                go.GetComponent<Image>().raycastTarget = false;
                go.transform.SetParent(Selection.activeTransform, false);
                go.layer = LayerMask.NameToLayer("UI");
                Selection.activeGameObject = go;

            }
            else
            {
                Debug.Log("需要在画布钟创建!");
            }
                
           
        }
        [MenuItem("GameObject/StackUI/Text")]
        static void CreatText()
        {
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject("Text", typeof(Text));
                go.GetComponent<Text>().raycastTarget = false;
                go.GetComponent<Text>().supportRichText = false;
                go.transform.SetParent(Selection.activeTransform, false);
                go.layer = LayerMask.NameToLayer("UI");
                Selection.activeGameObject = go;
            }
            else
            {
                Debug.Log("需要在画布钟创建!");
            }
        }
        
        [MenuItem("GameObject/StackUI/Text Button")]
        static void CreateTextButton()
        {
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                Transform p = Selection.activeTransform;
                RectTransform button = CreateButton(p);
                UIButton btn = button.gameObject.GetComponent<UIButton>();



                RectTransform showTarget = new GameObject("ShowTarget").AddComponent<RectTransform>();
                showTarget.gameObject.layer = LayerMask.NameToLayer("UI");
                showTarget.SetParent(button, false);
                var text = showTarget.gameObject.AddComponent<Text>();
                text.text = "Button";
                text.raycastTarget = false;
                text.supportRichText = false;
                text.color = Color.black;
                text.fontSize = 24;
                text.alignment = TextAnchor.MiddleCenter;

                showTarget.anchorMin = Vector2.zero;
                showTarget.anchorMax = Vector2.one;
                showTarget.anchoredPosition = Vector2.zero;
                showTarget.sizeDelta = Vector2.zero;

                btn.showTarget = text;
                btn.targetGraphic = text;
                
            }
            

        }
        [MenuItem("GameObject/StackUI/Image Button")]
        static void CreateImageButton()
        {
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                Transform p = Selection.activeTransform;
                RectTransform button = CreateButton(p);
                UIButton btn = button.gameObject.GetComponent<UIButton>();


                RectTransform showTarget = new GameObject("ShowTarget").AddComponent<RectTransform>();
                showTarget.gameObject.layer = LayerMask.NameToLayer("UI");
                showTarget.SetParent(button, false);
                var image = showTarget.gameObject.AddComponent<Image>();
                image.raycastTarget = false;
                showTarget.anchorMin = Vector2.zero;
                showTarget.anchorMax = Vector2.one;
                showTarget.anchoredPosition = Vector2.zero;
                showTarget.sizeDelta = Vector2.zero;

                btn.showTarget = image;
                btn.targetGraphic = image;

                Selection.activeGameObject = button.gameObject;
            }
            else
            {
                Debug.Log("需要在画布钟创建!");
            }
        }
        private static RectTransform CreateButton(Transform parent)
        {
            RectTransform button = new GameObject("UIButton").AddComponent<RectTransform>();
            button.gameObject.layer = LayerMask.NameToLayer("UI");
            button.SetParent(parent, false);
            button.sizeDelta = new Vector2(160, 50);
            var raycasetTarget = button.gameObject.AddComponent<Image>();
            raycasetTarget.sprite = null;
            raycasetTarget.color = new Color(1,1,1,0);
            UIButton btn = button.gameObject.AddComponent<UIButton>();
            btn.enabled = true;
            btn.interval = 0.2f;
            btn.transition = Selectable.Transition.ColorTint;
            return button;
        }
    }
}
