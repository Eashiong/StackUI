
/*
    按钮封装
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using UnityEngine.EventSystems;


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
        private float _lastClickTime = -999f;


        private bool CanPass()
        {
            if (interval <= 0f) return true;
            var now = Time.unscaledTime; // 不受 Time.timeScale 影响
            if (now - _lastClickTime < interval) return false;
            _lastClickTime = now;
            return true;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (CanPass())
                {
                    base.OnPointerClick(eventData);
                }
            }
        }
        public override void OnSubmit(BaseEventData eventData)
        {
            if (CanPass())
            {
                base.OnSubmit(eventData);
            }
        }






        public void AddListener(UnityAction action)
        {
            this.onClick.AddListener(action);

        } 
        public void RemoveAllListeners() => this.onClick.RemoveAllListeners();
        public void RemoveListener(UnityAction action) => this.onClick.RemoveListener(action);


    }

   
    
}
