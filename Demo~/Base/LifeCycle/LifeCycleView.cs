using System.Collections;
using UnityEngine;
using StackUI;
using UnityEngine.UI;

namespace StackUI.Demo.Base
{
    public class LifeCycleView : PageView
    {
        public override void OnShow()
        {
            base.OnShow();
            Debug.Log($"{go} -> 已显示");
        }
        public override void OnClose()
        {
            base.OnClose();
            Debug.Log($"{go} -> 已隐藏");
        }

    }
    public class LifeCyclePresenter : Presenter<LifeCycleView>
    {
        public override void OnAssetLoaded()
        {
            Debug.Log($"{id} -> OnAssetLoaded");
            
        }

        public override void OnInit(object arg)
        {
            Debug.Log($"{id} -> OnInit, arg: {arg}");
            view.backAction = ()=> Navigation.Pop();
            view.update = () => {
                //每帧更新一次
            };
            
        }

        public override void OnReInit(object arg)
        {
            Debug.Log($"{id} -> OnReInit, arg: {arg}");
        }

        public override void OnClose()
        {
            Debug.Log($"{id} -> OnClose");
        }

        public override void OnDispose()
        {
            Debug.Log($"{id} -> OnDispose");
        }
    }
}