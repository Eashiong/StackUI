using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StackUI;
namespace StackUI.Demo
{
    public class MainView : PageView
    {

    }

    public class MainPresenter : Presenter
    {
        MainView myView;
        //资源加载后调用
        public override void OnAssetLoaded()
        {
            base.OnAssetLoaded();
            Debug.Log(this.ID + "相关资源已加载");
        }
        //被打开的时候调用（重复打开不会调用它）
        public override void OnInit(object arg)
        {
            base.OnInit(arg);
            Debug.Log(this.ID +  "已打开");
            Debug.Log(arg as string);//输出 Hello
            myView = view as MainView;
            myView.backAction = () => Navigation.Pop(this);

        }
        //已经是打开状态下 重复打开时调用
        public override void OnReInit(object arg)
        {
            base.OnReInit(arg);
            Debug.Log(this.ID +  "已重复打开");
        }
        //关闭时候调用
        public override void OnClose()
        {
            base.OnClose();
            myView.backAction = null;
            UnityEngine.Debug.Log(this.ID + "已经关闭");
        }
        //释放资源前调用(如果Navigation.SetDontDestroyAsset，或注册的时候,标识了隐藏的时候销毁资源)
        //它在OnClose后调用
        public override void OnDispose()
        {
            base.OnDispose();
            UnityEngine.Debug.Log(this.ID + "已经彻底销毁");
        }
    }
}