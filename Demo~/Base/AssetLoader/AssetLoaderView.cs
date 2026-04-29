using System;
using System.Collections;
using System.Collections.Generic;
using StackUI;
using UnityEngine;

namespace StackUI.Demo.Base
{
    public class AssetLoaderView : View
    {
        private GameObject cube;
        private Camera renderCamera;

        public void Build()
        {
            // 创建相机
            var cameraGo = new GameObject("RenderCamera");
            cameraGo.transform.SetParent(go.transform, false);
            renderCamera = cameraGo.AddComponent<Camera>();

            // 创建方块
            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = "Cube";
            cube.transform.SetParent(go.transform, false);
            cube.transform.localPosition = new Vector3(0f, 0f, 5f);
            cube.transform.localScale = Vector3.one;

            // 创建 3D 文本
            var textGo = new GameObject("Tip3DText");
            textGo.transform.SetParent(go.transform, false);
            textGo.transform.localPosition = new Vector3(0f, 1.2f, 5f);
            textGo.transform.localScale = Vector3.one * 0.4f;
            var textMesh = textGo.AddComponent<TextMesh>();
            textMesh.text = "按空格返回上一页";
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            textMesh.characterSize = 0.1f;
            textMesh.fontSize = 60;
            textMesh.color = Color.white;

            // 创建灯光 
            GameObject lightGo = new GameObject("Light");
            lightGo.transform.SetParent(go.transform, false);
            lightGo.transform.localPosition = new Vector3(0f, 0, 1f);
            lightGo.transform.localScale = Vector3.one * 0.4f;
            var light = lightGo.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = Color.white;
            light.intensity = 5f;
        }

        public override void OnShow()
        {
            base.OnShow();
            StartCoroutine(RotateCubeCoroutine());
        }

        private IEnumerator RotateCubeCoroutine()
        {
            while (cube != null)
            {
                cube.transform.Rotate(Vector3.up, 90f * Time.deltaTime, Space.Self);
                yield return null;
            }
        }
    }

    public class AssetLoaderPresenter : Presenter<AssetLoaderView>
    {
        public override void OnAssetLoaded()
        {
            view.Build();
        }

        public override void OnInit(object arg)
        {
            base.OnInit(arg);
            view.update = () => {
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    Navigation.Pop();
                }
            };
        }
    }
}
