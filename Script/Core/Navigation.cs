/*
    模块的加载，后退，缓存，初始化
    需要调用AddTable函数配置路由
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace StackUI
{

    public static class Navigation
    {
        private static Dictionary<string, IRouteBuilder> table = new Dictionary<string, IRouteBuilder>();
        private const object emptyObj = default(object);
        private static Stack<IRouteBuilder> uiLayer = new Stack<IRouteBuilder>();
        private static Dictionary<string, IRouteBuilder> winds = new Dictionary<string, IRouteBuilder>();

        /// <summary>
        /// 注册页面
        /// </summary>
        /// <param name="id">唯一ID</param>
        /// <param name="viewName">资源名</param>
        /// <param name="dontDestroy">界面关闭时不销毁物体</param>
        /// <typeparam name="T">presenter，即为主持人，他是视图和逻辑的粘合剂</typeparam>
        public static void AddTable<T>(string id,string viewName,bool dontDestroy = true,Func<string, GameObject> loader = null)
        {
            System.Type t = typeof(T);
            IRouteBuilder builder = new RouteBuilder(t, viewName,loader).SetDontDestroy(dontDestroy) as IRouteBuilder;
            IRouteBuilder b = builder as IRouteBuilder;
            table.Add(b.ID, b);
        }

        /// <summary>
        /// 打开一个页面
        /// </summary>
        /// <param name="id">页面ID</param>
        /// <param name="arg">页面参数</param>
        public static void Push(string id, object arg = emptyObj)
        {
            if (!table.ContainsKey(id))
            {
                throw new System.Exception("没有注册页面:" + id);
            }
            if (uiLayer.Count > 0)
            {
                var old = uiLayer.Peek();
                old.Close();
            }
            var cur = table[id];
            uiLayer.Push(cur);
            cur.Build(arg);
        }
        /// <summary>
        /// 与当前页面置换
        /// 关闭当前页面后，不显示上一页，而是立即打开一个新页面
        /// </summary>
        /// <param name="id">页面ID</param>
        /// <param name="arg">页面参数</param>
        public static void PopAndPush(string id, object arg = emptyObj)
        {
            if (!table.ContainsKey(id))
            {
                throw new System.Exception("没有注册页面:" + id);
            }
            if (uiLayer.Count > 0)
            {
                var old = uiLayer.Pop();
                old.Close();
            }
            var cur = table[id];
            uiLayer.Push(cur);
            cur.Build(arg);
        }
        /// <summary>
        /// 打开一个页面，然后将之前的所有的页面移除
        /// </summary>
        /// <param name="id">页面ID</param>
        /// <param name="arg">页面参数</param>
        public static void PushAndRemoveAll(string id, object arg = emptyObj)
        {
            Clear();
            Push(id, arg);

        }
        /// <summary>
        ///  打开一个页面，然后将之前的所有的页面移除，直到符合条件为止
        /// </summary>
        /// <param name="id">页面ID</param>
        /// <param name="until">返回真时，不再移除</param>
        /// <param name="arg">页面参数</param>
        public static void PushAndRemoveUntil(string id, System.Func<string, bool> until, object arg = emptyObj)
        {
            if (!table.ContainsKey(id))
            {
                throw new System.Exception("没有注册页面:" + id);
            }
            int count = uiLayer.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                var builder = uiLayer.Peek();
                builder.Close();
                bool result = until(builder.ID);
                if (result)
                {
                    var cur = table[id];
                    uiLayer.Push(table[id]);
                    cur.Build(arg);
                    return;
                }
                else
                {
                    uiLayer.Pop();
                }
            }
            throw new System.Exception("操作失败：条件似乎永远都不会返回真");

        }

        /// <summary>
        /// 关闭一个页面，并显示上一页
        /// </summary>
        /// <param name="p">当前页</param>
        /// <param name="arg">页面参数</param>
        public static void Pop(Presenter p, object arg = emptyObj)
        {
            if(!CanPop())
            {
                Debug.LogError("无法关闭仅存的一个页面");
                return;
            }
            var builder = uiLayer.Peek();
            if (!builder.ID.Equals((p as IPresenter).id))
            {
                Debug.LogError("参数不是栈顶元素");
                return;
            }
            var old = uiLayer.Pop();
            old.Close();
            builder = uiLayer.Peek();
            builder.Build(arg);

        }
        /// <summary>
        /// 关闭一个页面，并显示上一页
        /// </summary>
        /// <param name="id">当前页</param>
        /// <param name="arg">页面参数</param>
        public static void Pop(string id, object obj = emptyObj)
        {
            if(!CanPop())
            {
                Debug.LogError("无法关闭仅存的一个页面");
                return;
            }
            var builder = uiLayer.Peek();
            if (!builder.ID.Equals(id))
            {
                Debug.LogError("参数不是栈顶元素");
                return;
            }
            var old = uiLayer.Pop();
            old.Close();
            builder = uiLayer.Peek();
            builder.Build(obj);

        }

        /// <summary>
        /// 删除所有页面
        /// </summary>
        public static void Clear()
        {
            if (uiLayer.Count > 0)
            {
                var top = uiLayer.Peek();
                top.Close();
            }
            uiLayer.Clear();

        }
        /// <summary>
        /// 持续关闭页面，直到符合条件为止
        /// </summary>
        /// <param name="until">判断页面名满足条件</param>
        /// <param name="arg"></param>
        public static void PopUntil(System.Func<string, bool> until, object arg = emptyObj)
        {

            int count = uiLayer.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                if(!CanPop())
                {
                    Debug.LogError("无法关闭仅存的一个页面");
                    return;
                }
                var builder = uiLayer.Peek();
                bool result = until(builder.ID);
                if (result)
                {
                    builder.Build(arg);
                    return;
                }
                else
                {
                    uiLayer.Pop();
                }
            }
            throw new System.Exception("操作失败：条件似乎永远都不会返回真");
        }

        /// <summary>
        /// 当前页面是否能关闭?
        /// </summary>
        /// <returns></returns>
        public static bool CanPop() => uiLayer.Count > 1;

		/// <summary>
        /// 当前页面
        /// </summary>
        public static IPresenter Current()
        {
            return (uiLayer.Peek() as RouteBuilder).Presenter;
        }

        /// <summary>
        /// 叠加显示一个窗口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public static IPresenter ShowWin(string id,object arg = emptyObj)
        {
            if (!table.ContainsKey(id))
            {
                throw new System.Exception("没有注册页面:" + id);
            }

            var builder = table[id];
            builder.Build(arg);
            if (!winds.ContainsKey(id))
                winds.Add(id,builder);
            return (builder as RouteBuilder).Presenter;
        }
        /// <summary>
        /// 隐藏窗口
        /// </summary>
        public static void HideWin(IPresenter p)
        {
            HideWin(p.id);
        }
        /// <summary>
        /// 隐藏窗口
        /// </summary>
        public static void HideWin(string id)
        {
            if (winds.ContainsKey(id))
            {
                winds[id].Close();
                winds.Remove(id);
            }
                
        }
    }
    public interface IRouteBuilder
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="arg"></param>
        void Build(object arg);
        /// <summary>
        /// 关闭
        /// </summary>
        void Close();
        /// <summary>
        /// 唯一ID标识
        /// </summary>
        /// <value></value>
        string ID { get; set; }
    }

    /// <summary>
    /// 页面创建器
    /// </summary>
    public class RouteBuilder : IRouteBuilder
    {
        /// <summary>
        /// 页面
        /// </summary>
        /// <value></value>
        public IPresenter Presenter { get; private set; }
        //Presenter type
        private Type t;
        //资源名
        private string viewName;
        //唯一ID
        private string id;
        string IRouteBuilder.ID { get => id; set => id = value; }

        //已打开?
        private bool IsActive;
        //资源创建器
        private Func<string, GameObject> loader;
        //界面关闭时不删除资源
        private bool dontDestroy;

        /// <param name="t">页面class type</param>
        /// <param name="viewName">页面名、资源名</param>
        /// <param name="builder">物体创建器</param>
        public RouteBuilder(System.Type t, string viewName, Func<string, GameObject> loader = null)
        {
            id = t.Name;
            this.viewName = viewName;
            this.t = t;
            this.dontDestroy = true;
            if (loader != null)
            {
                this.loader = loader;
            }
            else
            {
                this.loader = DefaultLoader;
            }
        }
        public RouteBuilder SetDontDestroy(bool dontDestroy)
        {
            this.dontDestroy = dontDestroy;
            return this;
        }

        void IRouteBuilder.Build(object arg)
        {
            IsActive = true;
            if (Presenter == null)
            {

                Presenter = System.Activator.CreateInstance(t) as IPresenter;
                Presenter.id = id;
                var go = loader(this.viewName);
                Presenter.view = go.GetComponent<View>();
            }
            Presenter.OnInit(arg);

        }


        void IRouteBuilder.Close()
        {
            if (IsActive)
            {
                IsActive = false;
                Presenter.OnClose();
            }
            if (!dontDestroy)
            {
                UnityEngine.GameObject.Destroy(Presenter.view.gameObject);
                Presenter = null;
            }

        }
        private static GameObject DefaultLoader(string viewName)
        {
            var prefab = Resources.Load<GameObject>(viewName);
            if (prefab == null)
            {
                throw new System.Exception("找不到资源，请检查拼写:" + viewName);
            }
            var go = GameObject.Instantiate(prefab);
            return go;
        }

    }

}
