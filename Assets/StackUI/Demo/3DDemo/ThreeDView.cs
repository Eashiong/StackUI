using System.Collections;
using System.Collections.Generic;
using StackUI;
using UnityEngine;
namespace Demo
{
    public class ThreeDView : PageView
    {
        public UIText cubeName;
        public GameObject dynamicCube { get; set; }
        public float speed { get; set; }

        //view 可以负责一切画面表现部分。如：动画、外观、过度效果等

        protected override void Update()
        {
            if (dynamicCube)
            {
                dynamicCube.transform.Rotate(Vector3.up * Time.deltaTime * speed);
            }
        }
        public override void OnShow()
        {
            base.OnShow();
            Debug.Log(go.name + "被打开了");
        }
        public override void OnClose()
        {
            base.OnClose();
            Debug.Log(go.name + "已关闭");
        }
    }
}