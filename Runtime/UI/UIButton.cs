
/*
    按钮封装
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
namespace StackUI
{
    [System.Serializable]
    public class UIButton : Button
    {
        /// <summary>
        /// 按钮显示图标、文字
        /// </summary>
        public Graphic showTarget;


        /// <summary>
        /// 间隔时间 单位毫秒
        /// </summary>
        [SerializeField]
        [Tooltip("单位秒，用于防止暴力重复点击")]
        public float interval = 0.2f;


        protected override void Awake()
        {
            base.Awake();
            this.onClick.AddListener(OnClick);
            if (showTarget && showTarget.gameObject != this.gameObject)
                showTarget.raycastTarget = false;
        }
        private float timer;
        protected void Update()
        {
            if (this.interactable == false)
            {
                timer = timer + Time.deltaTime;
                if (timer >= interval)
                {
                    this.interactable = true;
                    timer = 0;
                }
            }
        }
        private void OnClick()
        {
            if (interval > 0)
                this.interactable = false;
        }


        public void AddListener(UnityAction action)
        {
            this.onClick.RemoveAllListeners();
            this.onClick.AddListener(action);

        } 
        public void RemoveAllListeners() => this.onClick.RemoveAllListeners();
        public void RemoveListener(UnityAction action) => this.onClick.RemoveListener(action);


    }
}
