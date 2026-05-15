using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
namespace StackUI
{
    /// <summary>
    /// 资源移除处理参数
    /// </summary>
    public struct AssetRemoveHandlerArgs
    {
        /// <summary>
        /// 被移除的资源名
        /// </summary>
        public string viewName;
        /// <summary>
        /// 被移除页面/窗口的 唯一ID
        /// </summary>
        public string id;
        /// <summary>
        /// 被移除资源对象
        /// </summary>
        public GameObject asset;
    }
    /// <summary>
    /// 页面创建器
    /// </summary>
    internal class RouteBuilder
    {
        /// <summary>
        /// 页面
        /// </summary>
        /// <value></value>
        internal BasePresenter Presenter { get; private set; }
        //Presenter type
        private Type t;
        //资源名
        internal string viewName {get;private set; }
        //唯一ID
        internal string id;

        private bool isDirty = false;

        //资源创建器
        private Func<string, Task<GameObject>> loaderAsync;

        private Func<string, GameObject> loader;

        //界面关闭时不删除资源
        private bool dontDestroy;

        internal Func<AssetRemoveHandlerArgs,Task> assetRemoveHandlerAsync;
        internal Action<AssetRemoveHandlerArgs> assetRemoveHandler;

        /// <param name="t">页面class type</param>
        /// <param name="viewName">页面名、资源名</param>
        /// <param name="builder">物体创建器</param>
        internal RouteBuilder( System.Type t, 
                                string viewName, 
                                Func<string, Task<GameObject>> loaderAsync = null,
                                Func<AssetRemoveHandlerArgs,Task> assetRemoveHandlerAsync = null)
        {
            id = IDManager.Register(t);
            this.viewName = viewName;
            this.t = t;
            this.dontDestroy = true;
            this.assetRemoveHandlerAsync = assetRemoveHandlerAsync ?? DefaultAssetRemoveHandlerAsync;
            this.loaderAsync = loaderAsync ?? DefaultLoaderAsync;
            
        }



        /// <param name="t">页面class type</param>
        /// <param name="viewName">页面名、资源名</param>
        /// <param name="builder">物体创建器</param>
        internal RouteBuilder(System.Type t, string viewName, Func<string, GameObject> loader = null,Action<AssetRemoveHandlerArgs> assetRemoveHandler = null)
        {
            id = IDManager.Register(t);
            this.viewName = viewName;
            this.t = t;
            this.dontDestroy = true;
            this.assetRemoveHandler = assetRemoveHandler ?? DefaultAssetRemoveHandler;
            this.loader = loader ?? DefaultLoader;
            
        }
        private void DefaultAssetRemoveHandler(AssetRemoveHandlerArgs args)
        {
            UnityEngine.GameObject.Destroy(args.asset);
        }
        internal async Task DefaultAssetRemoveHandlerAsync(AssetRemoveHandlerArgs args)
        {
            UnityEngine.GameObject.Destroy(args.asset);
            await Task.CompletedTask;
        }
        private static async Task<GameObject> DefaultLoaderAsync(string viewName)
        {
            var request = Resources.LoadAsync<GameObject>(viewName);
            var prefab = await ToTask<GameObject>(request);
            if (prefab == null)
            {
                Debug.LogError("StackUI:找不到资源，请检查资源:" + viewName);
                return null;
            }
            var go = GameObject.Instantiate(prefab);
            return go;
        }

 
        private static GameObject DefaultLoader(string viewName)
        {
            var prefab = Resources.Load<GameObject>(viewName);
            if (prefab == null)
            {
                Debug.LogError("StackUI:找不到资源，请检查资源:" + viewName);
                return null;
            }
            var go = GameObject.Instantiate(prefab);
            return go;
        }

        internal RouteBuilder SetDontDestroy(bool dontDestroy)
        {
            this.dontDestroy = dontDestroy;
            return this;
        }
        internal void SetNewAssetName(string name)
        {
            isDirty = viewName != name;
            viewName = name;

            if(isDirty)
            {
                if(Presenter != null && !Presenter.enable && Presenter.view != null)
                {
                    Presenter.Dispose();
                    
                    assetRemoveHandler(new AssetRemoveHandlerArgs{
                        id = id,
                        viewName = viewName,
                        asset = Presenter.view.gameObject
                    });
                }
                
            }
        }
        internal async Task SetNewAssetNameAsync(string name)
        {
            isDirty = viewName != name;
            viewName = name;

            if(isDirty)
            {
                if(Presenter != null && !Presenter.enable && Presenter.view != null)
                {
                    Presenter.Dispose();
                    
                    await assetRemoveHandlerAsync(new AssetRemoveHandlerArgs{
                        id = id,
                        viewName = viewName,
                        asset = Presenter.view.gameObject
                    });
                }
                
            }
        }

        //创建一个Ui 并触发生命周期初始化相关函数
        //如果第一次创建 触发AssetLoaded
        //如果未激活 触发 Init 不触发 ReInit
        //如果已激活 触发 ReInit 不触发 Init
        internal async Task<bool> BuildAsync(object arg)
        {
            if (Presenter == null)
            {
                Presenter = System.Activator.CreateInstance(t) as BasePresenter;
                Presenter.id = id;
                bool result = await LoadAssetAsync();
                if(!result)
                {
                    return false;
                }
                
            }
            else if(isDirty)
            {
                if(Presenter.view != null)
                {
                    await assetRemoveHandlerAsync(new AssetRemoveHandlerArgs{
                        id = id,
                        viewName = viewName,
                        asset = Presenter.view.gameObject
                    });
                }
                bool result = await LoadAssetAsync();
                if(!result)
                {
                    return false;
                }
            }

            
            if(Presenter.enable == false)
                Presenter.Init(arg);
            else
                Presenter.ReInit(arg);

            return true;
            
        }
        internal bool Build(object arg)
        {
            if (Presenter == null)
            {
                Presenter = System.Activator.CreateInstance(t) as BasePresenter;
                Presenter.id = id;
                bool result = LoadAsset();
                if(!result)
                {
                    return false;
                }
                
            }
            else if(isDirty)
            {
                if(Presenter.view != null)
                {
                    assetRemoveHandler(new AssetRemoveHandlerArgs{
                        id = id,
                        viewName = viewName,
                        asset = Presenter.view.gameObject
                    });
                }
                bool result = LoadAsset();
                if(!result)
                {
                    return false;
                }
            }

            
            if(Presenter.enable == false)
                Presenter.Init(arg);
            else
                Presenter.ReInit(arg);

            return true;
            
        }
        
        private async Task<bool> LoadAssetAsync()
        {
            var go = await loaderAsync(this.viewName);
            if(go == null)
            {
                Presenter = null;
                Debug.LogError($"StackUI:无法构建UI，因为无法加载资源，请检测对应的资源{this.viewName}");
                return false;
            }
            Presenter.view = go.GetComponent<View>();
            if(Presenter.view == null)
            {
                Presenter = null;
                Debug.LogError($"StackUI:无法构建UI，因为{go}缺少View组件，请检测对应的资源{this.viewName}");
                await assetRemoveHandlerAsync(new AssetRemoveHandlerArgs{
                        id = id,
                        viewName = viewName,
                        asset = go
                    });
                return false;
            }
            isDirty = false;
            Presenter.AssetLoaded();
            return true;
        }
        private bool LoadAsset()
        {
            var go = loader(this.viewName);
            if(go == null)
            {
                Presenter = null;
                Debug.LogError($"StackUI:无法构建UI，因为无法加载资源，请检测对应的资源{this.viewName}");
                return false;
            }
            Presenter.view = go.GetComponent<View>();
            if(Presenter.view == null)
            {
                Presenter = null;
                Debug.LogError($"StackUI:无法构建UI，因为{go}缺少View组件，请检测对应的资源{this.viewName}");
                assetRemoveHandler(new AssetRemoveHandlerArgs{
                        id = id,
                        viewName = viewName,
                        asset = go
                    });
                return false;
            }
            isDirty = false;
            Presenter.AssetLoaded();
            return true;
        }


        internal async Task CloseAsync(bool forceDestroy = false)
        {
            if(Presenter == null)
            {
                Debug.LogError("StackUI:Presenter实例不能为空");
                return;
            }
            if (Presenter.enable)
            {
                try { Presenter.Close(); } catch(System.Exception e) {Debug.LogError(e);};
            }
            if (forceDestroy || !dontDestroy || isDirty)
            {
                Presenter.Dispose();
                await assetRemoveHandlerAsync(new AssetRemoveHandlerArgs{
                        id = id,
                        viewName = viewName,
                        asset = Presenter.view.gameObject
                    });
                Presenter = null;
            }

        }
        internal void Close(bool forceDestroy = false)
        {
            if(Presenter == null)
            {
                Debug.LogError("StackUI:Presenter实例不能为空");
                return;
            }
            if (Presenter.enable)
            {
                try { Presenter.Close(); } catch(System.Exception e) {Debug.LogError(e);};
            }
            if (forceDestroy || !dontDestroy || isDirty)
            {
                Presenter.Dispose();
                assetRemoveHandler(new AssetRemoveHandlerArgs{
                        id = id,
                        viewName = viewName,
                        asset = Presenter.view.gameObject
                    });
                Presenter = null;
            }

        }


        private static async Task<T> ToTask<T>(ResourceRequest request) where T:UnityEngine.Object
        {
            while (!request.isDone)
                await Task.Yield();
            return request.asset as T;
        }

    }
}