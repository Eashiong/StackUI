using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StackUI;
namespace StackUI.Demo.Base
{
    
    public class SkinView : PageView
    {
        public Button red;
        public Button white;
    }

    public class SkinPresenter:Presenter<SkinView>
    {
        const string red = "StackUIDemo/Base/SkinView-Red";
        const string white = "StackUIDemo/Base/SkinView-White";
        public override void OnInit(object arg)
        {
            base.OnInit(arg);
            Debug.Log("SkinPresenter OnInit");
            view.backAction = ()=> Navigation.Pop();
            ListenUnity(view.white.onClick,()=>
            {
                if(Navigation.GetAssetName(this.id) == red)
                {
                    
                    Navigation.SetAssetName(this.id,white);
                    Navigation.PopAndPush(this.id);
                }

            });

            ListenUnity(view.red.onClick,()=>
            {
                if(Navigation.GetAssetName(this.id) == white)
                {
                    Navigation.SetAssetName(this.id,red);
                    Navigation.PopAndPush(this.id);
                }
            });
        }
        //修改资源会导致界面关闭的时候调用OnDispose 因为旧资源被销毁
        public override void OnDispose()
        {
            base.OnDispose();
            Debug.Log("SkinPresenter OnDispose");
        }
    }
}