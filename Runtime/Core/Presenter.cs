//Presenter基类
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StackUI
{

    public class Presenter:Presenter<View>  { }

    public class Presenter<TView> :BasePresenter where TView :View
    {
        public new TView view 
        { 
            get
            {
                return base.view as TView;
            }
            set
            {
                base.view = value;
            }
        }
    }


    public class BasePresenter
    {

        internal BasePresenter() { }

        /// <summary>
        /// 界面实例ID
        /// </summary>
        /// <value></value>
        public string id { get; internal set; }

        

        //生命周期 重写他们实现业务逻辑

        /// <summary>
        /// 当资源被载入/重新载入时调用
        /// </summary>
        public virtual void OnAssetLoaded() {}

        /// <summary>
        /// 当界面被实例化到场景时调用 
        /// 发生在Awake之后Start之前
        /// </summary>
        /// <param name="arg"></param>
        public virtual void OnInit(object arg) {  }
        /// <summary>
        /// 当屏幕已经是该界面 反复打开时候调用
        /// </summary>
        /// <param name="arg"></param>
        public virtual void OnReInit(object arg) {  }

        /// <summary>
        /// 当界面退出时候调用 
        /// 界面如果被隐藏（发生在隐藏之前调用）  
        /// 界面如果被销毁（发生在销毁前调用）
        /// </summary>
        public virtual void OnClose() {}

        /// <summary>
        /// 当界面退出并资源被销毁时候调用
        /// 发生在销毁前调用
        /// </summary>
        public virtual void OnDispose() {}


        /// <summary>
        /// 当前可见状态
        /// </summary>
        /// <value></value>
        public bool enable { get; internal set; }

        internal View view { get;  set; }


        internal void AssetLoaded()
        {
            OnAssetLoaded();
        }

        internal void Init(object arg)
        {
            bool b = enable;
            enable = true;
            if(!b)
                view.OnShow();
            else
                UnityEngine.Debug.LogWarning("重复初始化,id:" + id);
            OnInit(arg);
        }

        internal void ReInit(object arg)
        {
            OnReInit(arg);
        }

        internal void Close()
        {
            bool b = enable;
            enable = false;
            if(b)
                view.OnClose();
            else
                UnityEngine.Debug.LogWarning("重复隐藏,id:" + id);
            ClearAllListens();

            OnClose();
        }
        
        internal void Dispose()
        {
            ClearAllListens();
            OnDispose();
        }

#region 协程
        public Coroutine StartCoroutine(string methodName)  
        {
            return view.StartCoroutine(methodName);
        }
        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return view.StartCoroutine(routine);
        }
        public void StopCoroutine(IEnumerator routine)
        {
            view.StopCoroutine(routine);
        }
        public void StopCoroutine(Coroutine coroutine)
        {
            view.StopCoroutine(coroutine);
        }
        public void StopAllCoroutines()
        {
            view.StopAllCoroutines();
        }
#endregion
        
#region 事件注册与自动回收
        private readonly List<(Action bind, Action Unbind)> genericActions = new List<(Action bind, Action Unbind)>();
        private readonly List<IUnityEventListener> genericUnityEventListeners = new List<IUnityEventListener>();


        public void DoAction(Action bind, Action Unbind)
        {
            bind?.Invoke();
            genericActions.Add((bind,Unbind));
        }

        public void ListenUnity(UnityEvent unityEvent,UnityAction action)
        {
            genericUnityEventListeners.Add(new UnityEventListener(unityEvent, action));
        }

        public void ListenUnity<T0>(UnityEvent<T0> unityEvent, UnityAction<T0> action)
        {
            genericUnityEventListeners.Add(new UnityEventListener<T0>(unityEvent, action));
        }

        public void ListenUnity<T0, T1>(UnityEvent<T0, T1> unityEvent, UnityAction<T0, T1> action)
        {
            genericUnityEventListeners.Add(new UnityEventListener<T0, T1>(unityEvent, action));
        }

        public void ListenUnity<T0, T1, T2>(UnityEvent<T0, T1, T2> unityEvent, UnityAction<T0, T1, T2> action)
        {
            genericUnityEventListeners.Add(new UnityEventListener<T0, T1, T2>(unityEvent, action));
        }

        public void ListenUnity<T0, T1, T2, T3>(UnityEvent<T0, T1, T2, T3> unityEvent, UnityAction<T0, T1, T2, T3> action)
        {
            genericUnityEventListeners.Add(new UnityEventListener<T0, T1, T2, T3>(unityEvent, action));
        }


        private void ClearAllListens()
        {
            foreach (var actions in genericActions)
            {
                actions.Unbind?.Invoke();
            }
            genericActions.Clear();

            foreach (var listener in genericUnityEventListeners)
            {
                listener.RemoveListener();
            }
            genericUnityEventListeners.Clear();
        }

        private interface IUnityEventListener
        {
            void RemoveListener();
        }

        private readonly struct UnityEventListener : IUnityEventListener
        {
            private readonly UnityEvent unityEvent;
            private readonly UnityAction action;
            public UnityEventListener(UnityEvent unityEvent, UnityAction action)
            {
                this.unityEvent = unityEvent;
                this.action = action;
            }
            public void RemoveListener()
            {
                unityEvent.RemoveListener(action);
            }
        }
        private readonly struct UnityEventListener<T0> : IUnityEventListener
        {
            private readonly UnityEvent<T0> unityEvent;
            private readonly UnityAction<T0> action;

            public UnityEventListener(UnityEvent<T0> unityEvent, UnityAction<T0> action)
            {
                this.unityEvent = unityEvent;
                this.action = action;
            }

            public void RemoveListener()
            {
                unityEvent.RemoveListener(action);
            }
        }

        private readonly struct UnityEventListener<T0, T1> : IUnityEventListener
        {
            private readonly UnityEvent<T0, T1> unityEvent;
            private readonly UnityAction<T0, T1> action;

            public UnityEventListener(UnityEvent<T0, T1> unityEvent, UnityAction<T0, T1> action)
            {
                this.unityEvent = unityEvent;
                this.action = action;
            }

            public void RemoveListener()
            {
                unityEvent.RemoveListener(action);
            }
        }

        private readonly struct UnityEventListener<T0, T1, T2> : IUnityEventListener
        {
            private readonly UnityEvent<T0, T1, T2> unityEvent;
            private readonly UnityAction<T0, T1, T2> action;

            public UnityEventListener(UnityEvent<T0, T1, T2> unityEvent, UnityAction<T0, T1, T2> action)
            {
                this.unityEvent = unityEvent;
                this.action = action;
            }

            public void RemoveListener()
            {
                unityEvent.RemoveListener(action);
            }
        }

        private readonly struct UnityEventListener<T0, T1, T2, T3> : IUnityEventListener
        {
            private readonly UnityEvent<T0, T1, T2, T3> unityEvent;
            private readonly UnityAction<T0, T1, T2, T3> action;

            public UnityEventListener(UnityEvent<T0, T1, T2, T3> unityEvent, UnityAction<T0, T1, T2, T3> action)
            {
                this.unityEvent = unityEvent;
                this.action = action;
            }

            public void RemoveListener()
            {
                unityEvent.RemoveListener(action);
            }
        }

    #endregion
    
    }

   
}