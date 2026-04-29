using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StackUI;

namespace StackUI.Demo.Base
{

    public class Base : MonoBehaviour
    {
        void Start()
        {
            //注册所有页面 页面先注册 才能使用 
            Navigation.AddTable<LifeCyclePresenter>("StackUIDemo/Base/LifeCycleView");
            Navigation.AddTable<SimplePresenter>("StackUIDemo/Base/SimpleView");
            Navigation.AddTable<MultipleExamples.ConfirmDialogPresenter>("StackUIDemo/MultipleExamples/ConfirmDialogView");
            Navigation.AddTable<SkinPresenter>("StackUIDemo/Base/SkinView-White");

            //自定义加载资源的方式 并指示关闭时删除资源（不缓存） 并指示如何移除资源
            Navigation.AddTable<AssetLoaderPresenter>("CustomLoaderAsset",false,CustomLoader,CustomAssetRemoveHandler);



            //第一个页面
            Navigation.Push("SimplePresenter");
        }



        private GameObject CustomLoader(string sourceName)
        {
            //比如从本地读取
            //return GameObject.Instantiate(Resources.Load<GameObject>(sourceName));
            //比如从AB包
            //var assetBundle = AssetBundle.LoadFromFile(abfilePath);
            //return assetBundle.LoadAsset<GameObject>(sourceName);

            //这里返回一个空物体
            return new GameObject(sourceName).AddComponent<AssetLoaderView>().gameObject;
        }
        private void CustomAssetRemoveHandler(AssetRemoveHandlerArgs args)
        {
            Debug.Log($"自定义资源清理逻辑: id:{args.id}, viewName:{args.viewName}, asset:{args.asset} 被删除");

            //比如清理AB包
            //assetBundle.Unload(false);

            UnityEngine.GameObject.Destroy(args.asset);
        }
    }
}