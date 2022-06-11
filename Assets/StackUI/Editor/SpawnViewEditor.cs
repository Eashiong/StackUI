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

        [MenuItem("GameObject/MVP/Simple", false, 0)]
        static void CreateSimple()
        {
            var go = new GameObject("SimpleView");
            Selection.activeGameObject = go;
        }
        [MenuItem("GameObject/MVP/Simple UI", false, 0)]
        static void CreateSimplePage()
        {
            Transform root = new GameObject("SimplePageView").transform;
            Transform cam = SpawnCamera();
            cam.SetParent(root, false);

            Transform canvas = SpawnCanvas(cam.GetComponent<Camera>());
            canvas.SetParent(root, false);
            Selection.activeTransform = canvas;


        }
        [MenuItem("GameObject/MVP/Return UI", false, 0)]
        static void CreatePage()
        {
            Transform root = new GameObject("PageView").transform;
            Transform cam = SpawnCamera();
            cam.SetParent(root, false);

            Transform canvas = SpawnCanvas(cam.GetComponent<Camera>());
            canvas.SetParent(root, false);

            SpawnBack(canvas);
            Selection.activeTransform = canvas;

        }

        static Transform SpawnCamera()
        {
            Camera cam = new GameObject("Camera").AddComponent<Camera>();
            cam.clearFlags = CameraClearFlags.Depth;
            cam.cullingMask = LayerMask.GetMask("UI");
            cam.orthographic = true;
            cam.farClipPlane = 100;
            cam.depth = 1;
            //cam.allowDynamicResolution = true;
            return cam.transform;
        }
        static Transform SpawnCanvas(Camera cam)
        {
            Canvas canvas = new GameObject("Canvas").AddComponent<Canvas>();
            if (cam)
            {
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = cam;
            }
            else
            {
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }


            var scaler = canvas.gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(750,1624);
            scaler.matchWidthOrHeight = 1;
            canvas.gameObject.AddComponent<GraphicRaycaster>();

            canvas.gameObject.layer = LayerMask.NameToLayer("UI");


            return canvas.transform;
        }
        static Transform SpawnBack(Transform canvas)
        {
            RectTransform back = new GameObject("BackBtn").AddComponent<RectTransform>();
            back.gameObject.layer = LayerMask.NameToLayer("UI");
            back.SetParent(canvas, false);
            var raycasetTarget = back.gameObject.AddComponent<UIRawImage>();
            raycasetTarget.texture = null;
            raycasetTarget.color = new Color(1,1,1,0);
            back.pivot = new Vector2(0, 1);
            back.anchorMin = new Vector2(0, 1);
            back.anchorMax = new Vector2(0, 1);
            back.anchoredPosition = new Vector2(0,-88);
            back.sizeDelta = new Vector2(200, 88);
            UIButton btn = back.gameObject.AddComponent<UIButton>();
            btn.transition = Selectable.Transition.None;



            RectTransform showTarget = new GameObject("ShowTarget").AddComponent<RectTransform>();
            showTarget.gameObject.layer = LayerMask.NameToLayer("UI");
            showTarget.SetParent(back, false);
            var ima = showTarget.gameObject.AddComponent<UIImage>();
            ima.sprite = null;
            ima.raycastTarget = false;

            showTarget.anchorMin = new Vector2(0.5f, 0.5f);
            showTarget.anchorMax = new Vector2(0.5f, 0.5f);
            showTarget.anchoredPosition = new Vector2(0, 0);
            showTarget.sizeDelta = new Vector2(42, 42);
            var icon = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Script/MVP/CommonSprite/back.png",typeof(Sprite)) as Sprite;
            ima.sprite = icon ;
            btn.showTarget = ima;
            btn.interval = 0.5f;





            return back.transform;
        }

        [MenuItem("GameObject/UI/Image")]
        static void CreatImage()
        {
            GameObject go = new GameObject("UIImage", typeof(UIImage));
            go.GetComponent<UIImage>().raycastTarget = false;
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
                go.transform.SetParent(Selection.activeTransform, false);
            else
                go.transform.SetParent(CheckCanvas(), false);
            go.layer = LayerMask.NameToLayer("UI");
            Selection.activeGameObject = go;
        }
        [MenuItem("GameObject/UI/Text")]
        static void CreatText()
        {
            GameObject go = new GameObject("UIText", typeof(UIText));
            go.GetComponent<UIText>().raycastTarget = false;
            go.GetComponent<UIText>().supportRichText = false;
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
                go.transform.SetParent(Selection.activeTransform, false);
            else
                go.transform.SetParent(CheckCanvas(), false);
            go.layer = LayerMask.NameToLayer("UI");
            Selection.activeGameObject = go;
        }
        [MenuItem("GameObject/UI/Raw Image")]
        static void CreatRawImage()
        {
            GameObject go = new GameObject("UIRawImage", typeof(UIRawImage));
            go.GetComponent<UIRawImage>().raycastTarget = false;
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
                go.transform.SetParent(Selection.activeTransform);

            else
                go.transform.SetParent(CheckCanvas());
            go.layer = LayerMask.NameToLayer("UI");
            Selection.activeGameObject = go;
        }
        [MenuItem("GameObject/UI/Button")]
        static void CreateButton()
        {
            Transform p;
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
                p = Selection.activeTransform;
            else
                p = CheckCanvas();

            RectTransform button = new GameObject("UIButton").AddComponent<RectTransform>();
            button.gameObject.layer = LayerMask.NameToLayer("UI");
            button.SetParent(p, false);
            button.sizeDelta = new Vector2(160, 50);
            var raycasetTarget = button.gameObject.AddComponent<UIRawImage>();
            raycasetTarget.texture = null;
            UIButton btn = button.gameObject.AddComponent<UIButton>();
            btn.enabled = true;
            btn.transition = Selectable.Transition.None;



            RectTransform showTarget = new GameObject("ShowTarget").AddComponent<RectTransform>();
            showTarget.gameObject.layer = LayerMask.NameToLayer("UI");
            showTarget.SetParent(button, false);
            var text = showTarget.gameObject.AddComponent<UIText>();
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
            btn.interval = 0.2f;

            Selection.activeGameObject = button.gameObject;
        }



        private static Transform CheckCanvas()
        {
            var canvas = Object.FindObjectOfType<Canvas>();
            Transform trans;
            if (!canvas)
                trans = SpawnCanvas(null);
            else
                trans = canvas.transform;
            if (!Object.FindObjectOfType<EventSystem>())
            {
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }
            return trans;



        }
    }
}
