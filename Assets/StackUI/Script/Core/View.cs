
/*

 简单的一个视图
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StackUI
{


    public class View : MonoBehaviour
    {
        public Action update { get; set; }

        /// <summary>
        /// 根物体
        /// </summary>
        /// <value></value>
        public GameObject go => this.gameObject;
        

        protected virtual void Update()
        {
            update?.Invoke();
        }
        /// <summary>
        /// 视图隐藏时
        /// </summary>
        public virtual void OnClose()
        {
            go.SetActive(false);
        }
        /// <summary>
        /// 视图被显示时
        /// </summary>
        public virtual void OnShow()
        {
            go.SetActive(true);
        }
    }
}