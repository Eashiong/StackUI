/*
    模块的加载，后退，缓存，初始化
    需要调用AddTable函数配置路由
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
namespace StackUI
{

    public static partial class Navigation
    {
        private static Dictionary<string, RouteBuilder> table = new Dictionary<string, RouteBuilder>();
        private const object emptyObj = default(object);
        private static Stack<RouteBuilder> uiLayer = new Stack<RouteBuilder>();
        private static Dictionary<string, RouteBuilder> winds = new Dictionary<string, RouteBuilder>();

        public static string GetID<T>() where T:BasePresenter => IDManager.GetID<T>();
        public static string GetID(Type t) => IDManager.GetID(t);

        public static System.Type GetType(string id) => IDManager.GetType(id);



        /// <summary>
        /// 注册页面
        /// </summary>
        /// <typeparam name="T">视图和逻辑的粘合剂</typeparam>
        /// <param name="viewName">资源名</param>
        /// <param name="dontDestroy">界面关闭时不销毁物体</param>
        /// <param name="loader">自定义资源加载器</param>

        public static void AddTable<T>(string viewName,
                                        bool dontDestroy = true,
                                        Func<string, GameObject> loader = null,
                                        Action<AssetRemoveHandlerArgs> assetRemoveHandler = null) 
                                        where T:BasePresenter
        {
            System.Type t = typeof(T);
            RouteBuilder builder = new RouteBuilder(t, viewName,loader,assetRemoveHandler).SetDontDestroy(dontDestroy);
            table[builder.id] = builder;
        }


        /// <summary>
        /// 压入一个页面到屏幕前 这将打开一个页面
        /// </summary>
        /// <param name="id">页面ID</param>
        /// <param name="arg">页面参数</param>
        public static BasePresenter Push(string id, object arg = emptyObj)
        {
            if (!table.ContainsKey(id))
            {
                Debug.LogError("StackUI:没有注册页面:" + id);
                return null;
            }
            if (uiLayer.Count > 0)
            {
                var old = uiLayer.Peek();
                old.Close();
            }
            var cur = table[id];
            uiLayer.Push(cur);
            if(!cur.Build(arg))
            {
                uiLayer.Pop();
                return null;
            }
            return cur.Presenter;
        }
        

        /// <summary>
        /// 压入一个页面到屏幕前 这将打开一个页面
        /// </summary>
        /// <param name="arg">页面参数</param>
        /// <typeparam name="T">页面类型</typeparam>
        public static T Push<T>(object arg = emptyObj) where T:BasePresenter
        {
            return Push(GetID<T>(),arg) as T;

        }
        

        /// <summary>
        /// 将当前屏幕页面移除，效果将显示上一页
        /// </summary>
        /// <param name="arg">页面参数</param>
        public static void Pop(object arg = emptyObj)
        {
            if(!CanPop())
            {
                return;
            }

            var old = uiLayer.Pop();
            old.Close();
            var builder = uiLayer.Peek();
            builder.Build(arg);

        }
        




#region 组合操作方法组

        /// <summary>
        /// 持续把当前屏幕页面移除(Pop)，直到符合条件为止
        /// <para>使用场景举例：结算完成后连续返回，直到回到“大厅页”为止，再触发大厅页刷新</para>
        /// </summary>
        /// <param name="until">判断页面名满足条件 若空什么也不会发生</param>
        /// <param name="arg">页面参数</param>
        public static void PopUntil(System.Func<string, bool> until, object arg = emptyObj)
        {
            if( until == null)
                return;

            int count = uiLayer.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                var builder = uiLayer.Peek();
                bool result = until(builder.id);
                if (result)
                {
                    builder.Build(arg);
                    return;
                }
                else
                {
                    if(!CanPop())
                    {
                        Debug.LogError("StackUI:无法关闭仅存的一个页面");
                        return;
                    }
                    var old = uiLayer.Pop();
                    old.Close();
                }
            }
            Debug.LogError("StackUI:PopUntil操作失败：条件似乎永远都不会返回真");
        }
       

        /// <summary>
        /// <para>把当前页面从屏幕移除 并压入一个新页面 效果等同于拿新页面置换当前页</para>
        /// <para>旧页面从路径中彻底移除，效果是后续操作如果新页面Pop后退时候会跳过这个页面，跳转到上上次页面</para>
        /// <para>使用场景举例：登录成功后可替换登录页为主页，不保留登录页历史，防止返回到登录页</para>
        /// </summary>
        /// <param name="id">页面ID</param>
        /// <param name="arg">页面参数</param>
        /// <returns>新页面实例</returns>
        public static BasePresenter PopAndPush(string id, object arg = emptyObj)
        {
            if (!table.ContainsKey(id))
            {
                Debug.LogError("StackUI:没有注册页面:" + id);
                return null;
            }
            if (uiLayer.Count > 0)
            {
                var old = uiLayer.Pop();
                old.Close();
            }
            var cur = table[id];
            uiLayer.Push(cur);
            cur.Build(arg);
            return cur.Presenter;
        }

        

        /// <summary>
        /// <para>把当前页面从屏幕移除 并压入一个新页面 效果等同于拿新页面置换当前页</para>
        /// <para>旧页面从路径中彻底移除，效果是后续操作如果新页面Pop后退时候会跳过这个页面，跳转到上上次页面</para>
        /// <para>使用场景举例：登录成功后可替换登录页为主页，不保留登录页历史，防止返回到登录页</para>
        /// </summary>
        /// <typeparam name="T">页面类型</typeparam>
        /// <param name="arg">页面参数</param>
        /// <returns>新页面实例</returns>
        public static T PopAndPush<T>(object arg = emptyObj) where T:BasePresenter
        {
            return PopAndPush(GetID<T>(),arg) as T;
        }

        

        /// <summary>
        /// 打开一个页面，然后将之前的所有的页面移除
        /// </summary>
        /// <param name="id">页面ID</param>
        /// <param name="arg">页面参数</param>
        /// <returns>新页面实例</returns>
        public static BasePresenter PushAndRemoveAll(string id, object arg = emptyObj)
        {
            Clear();
            return Push(id, arg);

        }

        

        /// <summary>
        /// 打开一个页面，然后将之前的所有的页面移除
        /// </summary>
        /// <typeparam name="T">页面类型</typeparam>
        /// <param name="arg">页面参数</param>
        /// <returns>新页面实例</returns>
        public static T PushAndRemoveAll<T>(object arg = emptyObj) where T:BasePresenter
        {
            return PushAndRemoveAll(GetID<T>(),arg) as T;
        }

        


        /// <summary>
        /// 将之前的所有的页面移除(Pop)，直到符合条件为止,然后打开一个页面（Push）
        /// <para>使用场景举例：从多层购买流程中跳到“支付结果页”，并移除中间步骤直到“商品详情页”或“首页”</para>
        /// </summary>
        /// <param name="id">页面ID</param>
        /// <param name="until">返回真时，不再移除. 若空则生命也不会发生</param>
        /// <param name="arg">页面参数</param>
        /// <returns>新页面实例</returns>
        public static BasePresenter PushAndRemoveUntil(string id, System.Func<string, bool> until, object arg = emptyObj)
        {
            if (!table.ContainsKey(id))
            {
                Debug.LogError("StackUI:没有注册页面:" + id);
                return null;
            }
            if(until == null)
                return CurrentInstance();

            if(CurrentInstanceID() == id)
            {
                return CurrentInstance();
            }

            int count = uiLayer.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                var builder = uiLayer.Peek();               
                bool result = until(builder.id);
                builder.Close();
                if (result)
                {
                    var cur = table[id];
                    uiLayer.Push(table[id]);
                    cur.Build(arg);
                    return cur.Presenter;
                }
                else
                {
                    
                    uiLayer.Pop();
                }
            }
            if(until(""))
            {
                var cur = table[id];
                uiLayer.Push(table[id]);
                cur.Build(arg);
                return cur.Presenter;
            }
            else
            {
                Debug.LogError("StackUI:操作失败：条件似乎永远都不会返回真");
                return null;
            }

        }

        

        /// <summary>
        /// 将之前的所有的页面移除(Pop)，直到符合条件为止,然后打开一个页面（Push）
        /// <para>使用场景举例：从多层购买流程中跳到“支付结果页”，并移除中间步骤直到“商品详情页”或“首页”</para>
        /// </summary>
        /// <typeparam name="T">页面类型</typeparam>
        /// <param name="until">返回真时，不再移除</param>
        /// <param name="arg">页面参数</param>
        /// <returns>新页面实例</returns>
        public static T PushAndRemoveUntil<T>(System.Func<string, bool> until, object arg = emptyObj) where T:BasePresenter
        {
            return PushAndRemoveUntil(GetID<T>(),until,arg) as T;
        }

        
#endregion



        /// <summary>
        /// 删除所有页面和窗口
        /// <para>使用场景举例：退出登录前或请回到主页前，可统一关闭所有页面和窗口，避免残留UI，获得一个清晰干净的页面状态和缓存</para>
        /// </summary>
        public static void Clear()
        {
            if (uiLayer.Count > 0)
            {
                var top = uiLayer.Peek();
                top.Close();
            }
            uiLayer.Clear();

            foreach(var win in winds)
            {
                win.Value.Close();
            }
            winds.Clear();

        }
        

        /// <summary>
        /// 标识页面/窗口关闭时候是否删除资源
        /// <para> 框架默认页面关闭时不删除资源，除非使用本方法有意修改 </para>
        /// <para> 使用场景举例：对高频弹出的“商城页”开启缓存（true），对低频复杂页关闭缓存（false）节省内存 </para>
        /// </summary>
        /// <param name="id">页面ID</param>
        public static void SetDontDestroyAsset(string id,bool dontDestroy)
        {
            if(!table.ContainsKey(id))
            {
                Debug.LogError("StackUI:没有注册页面:" + id);
            }
            else
            {
                table[id].SetDontDestroy(dontDestroy);
            }
        }
        /// <summary>
        /// 标识页面/窗口关闭时候是否删除资源
        /// <para> 框架默认页面关闭时不删除资源，除非使用本方法有意修改 </para>
        /// <para> 使用场景举例：对高频弹出的“商城页”开启缓存（true），对低频复杂页关闭缓存（false）节省内存 </para>
        /// </summary>
        /// <param name="id">页面ID</param>
        /// <typeparam name="T">页面类型</typeparam>
        public static void SetDontDestroyAsset<T>(bool dontDestroy) where T:BasePresenter
        {
            SetDontDestroyAsset(GetID<T>(),dontDestroy);
        }


        /// <summary>
        /// 设置页面/窗口加载时使用的资源
        /// <para> 框架默认使用 <see cref="AddTable"/>注册时候的资源，除非使用本方法有意修改 </para>
        /// <para>在下次页面推到屏幕前时生效</para>
        /// <para> 使用场景举例：可以动态修改对应资源实现换肤，比如节日时间点到达时使用本方法修改页面使用的资源为节日资源 </para>
        /// </summary>
        /// <param name="id">页面ID</param>
        /// <param name="newName">资源名</param>
        public static void SetAssetName(string id,string newName)
        {
            if(!table.ContainsKey(id))
            {
                Debug.LogError("StackUI:没有注册页面:" + id);
            }
            else
            {
                table[id].SetNewAssetName(newName);
            }
        }
        /// <summary>
        /// 设置页面/窗口加载时使用的资源
        /// <para> 框架默认使用 <see cref="AddTable"/>注册时候的资源，除非使用本方法有意修改 </para>
        /// <para>在下次页面推到屏幕前时生效</para>
        /// <para> 使用场景举例：可以动态修改对应资源实现换肤，比如节日时间点到达时使用本方法修改页面使用的资源为节日资源 </para>
        /// </summary>
        /// <param name="newName">资源名</param>
        /// <typeparam name="T">页面类型</typeparam>
        public static void SetAssetName<T>(string newName) where T:BasePresenter
        {
            SetAssetName(GetID<T>(),newName);
        }


        /// <summary>
        /// 获取页面/窗口 的资源名
        /// </summary>
        /// <param name="id">页面/窗口 id</param>
        public static string GetAssetName(string id)
        {
            if(!table.ContainsKey(id))
            {
                Debug.LogError("StackUI:没有注册页面:" + id);
                return "";
            }
            else
            {
                return table[id].viewName;
            }
        }

        /// <summary>
        /// 获取页面/窗口 的资源名
        /// </summary>
        /// <param name="id">页面/窗口 id</param>
        public static string GetAssetName<T>() where T:BasePresenter
        {
            return GetAssetName(GetID<T>());
        }

        /// <summary>
        /// 当前页面是否能关闭(Pop)?
        /// <para>不适用于窗口</para>
        /// <para> 使用场景举例：判断当前页面是否能后退，来控制返回按钮或左滑操作是否可用 </para>
        /// </summary>
        public static bool CanPop() => uiLayer.Count > 1;

		/// <summary>
        /// 当前页面实例
        /// <para>不适用于窗口</para>
        /// <para> 使用场景举例：埋点系统获取当前页面 id，用于 统计停留时长统计 </para>
        /// </summary>
        public static BasePresenter CurrentInstance()
        {
            if(uiLayer.Count == 0)
                return null;
            return uiLayer.Peek().Presenter;
        }
        /// <summary>
        /// 当前类型的页面是否在屏幕前
         /// <para>不适用于窗口</para>
        /// <para> 使用场景举例：埋点系统获取当前页面 id，用于 统计停留时长统计 </para>
        /// </summary>
        public static bool IsCurrent<T>() where T:BasePresenter
        {
            if(uiLayer.Count == 0)
                return false;
            return uiLayer.Peek().Presenter.id == GetID<T>();
        }

        /// <summary>
        /// 当前页面实例ID
        /// <para>不适用于窗口</para>
        /// <para> 使用场景举例：埋点系统获取当前页面 id，用于 统计停留时长统计 </para>
        /// </summary>

        public static string CurrentInstanceID()
        {
            if(uiLayer.Count == 0)
                return null;
            return uiLayer.Peek().Presenter.id;
        }


#region 窗口操作

        /// <summary>
        /// 显示一个窗口 叠在页面上 不影响页面主栈
        /// </summary>
        /// <param name="id">窗口ID</param>
        /// <param name="arg">窗口参数</param>
        /// <returns>窗口</returns>
        public static BasePresenter ShowWin(string id,object arg = emptyObj)
        {
            if (!table.ContainsKey(id))
            {
                Debug.LogError("StackUI:没有注册窗口:" + id);
                return null;
            }

            var builder = table[id];
            if(!builder.Build(arg))
            {
                return null;
            }
            if (!winds.ContainsKey(id))
                winds.Add(id,builder);
            return builder.Presenter;
        }

        

        /// <summary>
        /// 显示一个窗口 叠在页面上 不影响页面主栈
        /// </summary>
        /// <param name="arg">窗口参数</param>
        /// <typeparam name="T">窗口类型</typeparam>
        /// <returns>窗口</returns>
        public static T ShowWin<T>(object arg = emptyObj) where T:BasePresenter
        {
            return ShowWin(GetID<T>(),arg) as T;
        }

        


        /// <summary>
        /// 显示一个窗口 叠在页面上 不影响页面主栈
        /// <para> 如果窗口已经存在了 根据ifExistDoReinit指示要不要触发ReInit函数</para>
        /// <para> 可如果窗口不存在 按正常流程显示一个窗口 </para>
        /// <para> 使用场景举例： 背包弹窗已开时，若收到新道具事件，传 ifExistDoReinit=true 强制刷新内容 </para>
        /// </summary>
        /// <param name="ifExistDoReinit">如果窗口已在当前屏幕，且ifExistDoReinit为true 会触发Reinit 函数 如果ifExistDoReinit为false 不会触发任何函数</param>
        /// <param name="arg">窗口参数</param>
        /// <typeparam name="T">窗口类型</typeparam>
        /// <returns>窗口</returns>
        public static BasePresenter ShowWin(string id,bool ifExistDoReinit,object arg = emptyObj)
        {
            //窗口存在 但是需要刷新
            if(ExistWin(id))
            {
                if(ifExistDoReinit)
                    return ShowWin(id,arg);
            }
            else
            {
                //窗口不存在
                return ShowWin(id,arg);
            }
            return GetWin(id);
        }

       

        /// <summary>
        /// 显示一个窗口 如果窗口已经存在了 指示要不要触发ReInit函数
        /// <para> 可如果窗口不存在 按正常流程显示一个窗口 </para>
        /// </summary>
        /// <param name="ifExistDoReinit">如果窗口已在当前屏幕，且ifExistDoReinit为true 会触发Reinit 函数 如果ifExistDoReinit为false 不会触发任何函数</param>
        /// <param name="arg">窗口参数</param>
        /// <typeparam name="T">窗口类型</typeparam>
        /// <returns>窗口</returns>
        public static T ShowWin<T>(bool ifExistDoReinit,object arg = emptyObj) where T:BasePresenter
        {
            return ShowWin(GetID<T>(),ifExistDoReinit,arg) as T;
        }


       

        /// <summary>
        /// 隐藏窗口
        /// </summary>
        public static void HideWin(BasePresenter p,bool destroyAsset = false)
        {
            HideWin(p.id,destroyAsset);
        }
        /// <summary>
        /// 隐藏窗口
        /// </summary>
        /// <param name="destroyAsset">是否强制删除资源缓存</param>
        public static void HideWin(string id,bool destroyAsset = false)
        {
            if (winds.ContainsKey(id))
            {
                winds[id].Close(destroyAsset);
                winds.Remove(id);
            }
                
        }
        /// <summary>
        ///  隐藏窗口
        /// </summary>
        /// <param name="destroyAsset">是否强制删除资源缓存</param>
        /// <typeparam name="T">窗口类型</typeparam>
        public static void HideWin<T>(bool destroyAsset = false) where T:BasePresenter
        {
            HideWin(GetID<T>(),destroyAsset);
        }

        /// <summary>
        /// 当前屏幕是否存在窗口
        /// </summary>
        /// <param name="id">窗口 ID</param>
        public static bool ExistWin(string id)
        {
            return winds.ContainsKey(id);
        }
        /// <summary>
        /// 当前屏幕是否存在窗口
        /// </summary>
        /// <param name="id">窗口 ID</param>
        public static bool ExistWin<T>() where T:BasePresenter
        {
            return ExistWin(GetID<T>());
        }

        /// <summary>
        /// 获取窗口实例
        /// </summary>
        /// <param name="id">窗口ID</param>
        public static BasePresenter GetWin(string id)
        {
            return ExistWin(id) ? winds[id].Presenter : null;
        }
        /// <summary>
        /// 获取窗口实例
        /// </summary>
        public static T GetWin<T>() where T:BasePresenter
        {
            string id = GetID<T>();
            return ExistWin(id) ? winds[id].Presenter as T : null;
        }

#endregion
   
    }

}
