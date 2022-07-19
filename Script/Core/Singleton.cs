//单例模板类
using System;
using UnityEngine;
namespace StackUI
{

    /// <summary>
    /// 单例模板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : Singleton<T>
    {
        private static T _Ins { get; set;}
        public static T Ins
        {
            get
            {
                if (_Ins == null)
                {
                    _Ins = Activator.CreateInstance<T>();
                }
                return _Ins;
            }
        }
        protected Singleton() { this.OnInit(); }

        protected virtual void OnInit() { }
        /// <summary>
        /// 提前生成实例
        /// </summary>
        public void StartUp() { }

        public void DisposeSingleton()
        {

            OnDispose();
            _Ins = null;
            

        }
        protected virtual void OnDispose()
        {

        }

    }
    /// <summary>
    /// Unity 单例模板 使用MonoBehaviour
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _Ins;
        public static T Ins
        {
            get
            {
                if (_Ins == null)
                {

                    _Ins = new GameObject(typeof(T).Name).AddComponent<T>();
                    DontDestroyOnLoad(_Ins);
                    _Ins.OnInit();
                }
                return _Ins;
            }
        }
        protected virtual void OnInit() { }
        /// <summary>
        /// 提前生成实例
        /// </summary>
        public void StartUp() { }

        public void DisposeSingleton()
        {
            GameObject.Destroy(this.gameObject);
            _Ins = null;
        }

    }
}

