using System.Collections;
using UnityEngine;
using StackUI;
using UnityEngine.UI;

namespace StackUI.Demo
{
    public class LifeCycle01View : View
    {
        public Text text;
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
    public class LifeCyclePresenter01 : Presenter<LifeCycle01View>
    {
        public override void OnAssetLoaded()
        {
            Debug.Log($"{id} -> OnAssetLoaded");
        }

        public override void OnInit(object arg)
        {
            Debug.Log($"{id} -> OnInit, arg: {arg}");
            view.text.text = arg.ToString();
        }

        public override void OnReInit(object arg)
        {
            Debug.Log($"{id} -> OnReInit, arg: {arg}");
            view.text.text = arg.ToString();
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