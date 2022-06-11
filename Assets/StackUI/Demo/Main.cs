using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StackUI;

namespace Demo
{

    public class Main : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            //注册所有页面 页面先注册 才能使用 
            Navigation.AddTable<SplashPresenter>("SplashPresenter","SplashView");
            Navigation.AddTable<SimplePresenter>("SimplePresenter","SimpleView");
            Navigation.AddTable<ContentPresenter>("ContentPresenter","ContentView");
            Navigation.AddTable<ThreeDPresenter>("ThreeDPresenter","ThreeDView");

            //也自定义加载资源的方式 并指示页面关闭时删除物体
            //默认从Resources下读取，并页面关闭的时候物体隐藏
            Navigation.AddTable<LoadingPresenter>("LoadingPresenter","LoadingWin", false,CustomLoader);

            //第一个页面
            Navigation.Push("SimplePresenter");
        }

        private GameObject CustomLoader(string sourceName)
        {
            var prefab = Resources.Load<GameObject>(sourceName);
            if (prefab == null)
            {
                throw new System.Exception("找不到资源，请检查拼写:" + sourceName);
            }
            var go = GameObject.Instantiate(prefab);
            go.name = sourceName;
            return go;
        }
    }
}