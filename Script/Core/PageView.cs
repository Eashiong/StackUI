/*
    页面视图提供UI一些基本功能：返回，画布 UI相机 标题栏，页面滑动等

*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace StackUI
{

    /// <summary>
    /// 页面视图
    /// </summary>
    public class PageView : View
    {
        [SerializeField]
        protected UIButton backBtn;
        /// <summary>
        /// 页面后退
        /// </summary>
        public UnityAction backAction { get; set; }

        /// <summary>
        /// 画布
        /// </summary>
        /// <value></value>
        public Canvas canvas { get; private set; }

        /// <summary>
        /// UI相机
        /// </summary>
        /// <value></value>
        public Camera uiCam { get; private set; }

        protected virtual void Awake()
        {
            backBtn?.AddListener(() => { backAction?.Invoke(); });

            //如果camera canvas无法根据下面路径查找得到，可以子类重写awake自行find
            uiCam = transform.Find("Camera")?.GetComponent<Camera>();
            canvas = transform.Find("Canvas")?.GetComponent<Canvas>();


        }
    }
}
