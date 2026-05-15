/*
    异步方法
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

        /// <summary>
        /// 注册页面
        /// </summary>
        /// <typeparam name="T">视图和逻辑的粘合剂</typeparam>
        /// <param name="viewName">资源名</param>
        /// <param name="dontDestroy">界面关闭时不销毁物体</param>
        /// <param name="loader">自定义资源加载器</param>

        public static void AddTableWithAsync<T>(string viewName,
                                        bool dontDestroy = true,
                                        Func<string, Task<GameObject>> loader = null,
                                        Func<AssetRemoveHandlerArgs,Task> assetRemoveHandler = null) 
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
        public static async Task PushAsync(string id, object arg = emptyObj)
        {
            if (!table.ContainsKey(id))
            {
                Debug.LogError("StackUI:没有注册页面:" + id);
                return;
            }
            if (uiLayer.Count > 0)
            {
                var old = uiLayer.Peek();
                await old.CloseAsync();
            }
            var cur = table[id];
            uiLayer.Push(cur);
            if(!await cur.BuildAsync(arg))
            {
                uiLayer.Pop();
            }
        }
        /// <summary>
        /// 压入一个页面到屏幕前 这将打开一个页面
        /// </summary>
        /// <param name="arg">页面参数</param>
        /// <typeparam name="T">页面类型</typeparam>
        public static async Task PushAsync<T>(object arg = emptyObj) where T:BasePresenter
        {
            System.Type t = typeof(T);
            await PushAsync(GetID<T>(),arg);
        }

        /// <summary>
        /// 将当前屏幕页面移除，效果将显示上一页
        /// </summary>
        /// <param name="arg">页面参数</param>
        public static async Task PopAsync(object arg = emptyObj)
        {
            if(!CanPop())
            {
                return;
            }

            var old = uiLayer.Pop();
            await old.CloseAsync();
            var builder = uiLayer.Peek();
            await builder.BuildAsync(arg);

        }

         /// <summary>
        /// 持续把当前屏幕页面移除(Pop)，直到符合条件为止
        /// <para>使用场景举例：结算完成后连续返回，直到回到“大厅页”为止，再触发大厅页刷新</para>
        /// </summary>
        /// <param name="until">判断页面名满足条件 若空什么也不会发生</param>
        /// <param name="arg">页面参数</param>
        public static async Task PopUntilAsync(System.Func<string, bool> until, object arg = emptyObj)
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
                    await builder.BuildAsync(arg);
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
                    await old.CloseAsync();
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
        public static async Task PopAndPushAsync(string id, object arg = emptyObj)
        {
            if (!table.ContainsKey(id))
            {
                Debug.LogError("StackUI:没有注册页面:" + id);
                return;
            }
            if (uiLayer.Count > 0)
            {
                var old = uiLayer.Pop();
                await old.CloseAsync();
            }
            var cur = table[id];
            uiLayer.Push(cur);
            if(!await cur.BuildAsync(arg))
            {
                uiLayer.Pop();
            }
        }

        /// <summary>
        /// <para>把当前页面从屏幕移除 并压入一个新页面 效果等同于拿新页面置换当前页</para>
        /// <para>旧页面从路径中彻底移除，效果是后续操作如果新页面Pop后退时候会跳过这个页面，跳转到上上次页面</para>
        /// <para>使用场景举例：登录成功后可替换登录页为主页，不保留登录页历史，防止返回到登录页</para>
        /// </summary>
        /// <typeparam name="T">页面类型</typeparam>
        /// <param name="arg">页面参数</param>
        public static async Task PopAndPushAsync<T>(object arg = emptyObj) where T:BasePresenter
        {
            await PopAndPushAsync(GetID<T>(),arg);
        }

        /// <summary>
        /// 打开一个页面，然后将之前的所有的页面移除
        /// </summary>
        /// <param name="id">页面ID</param>
        /// <param name="arg">页面参数</param>
        public static async Task PushAndRemoveAllAsync(string id, object arg = emptyObj)
        {
            Clear();
            await PushAsync(id, arg);

        }

        /// <summary>
        /// 打开一个页面，然后将之前的所有的页面移除
        /// </summary>
        /// <typeparam name="T">页面类型</typeparam>
        /// <param name="arg">页面参数</param>
        public static async Task PushAndRemoveAllAsync<T>(object arg = emptyObj) where T:BasePresenter
        {
            await PushAndRemoveAllAsync(GetID<T>(),arg);
        }


        /// <summary>
        /// 将之前的所有的页面移除(Pop)，直到符合条件为止,然后打开一个页面（Push）
        /// <para>使用场景举例：从多层购买流程中跳到“支付结果页”，并移除中间步骤直到“商品详情页”或“首页”</para>
        /// </summary>
        /// <param name="id">页面ID</param>
        /// <param name="until">返回真时，不再移除. 若空则生命也不会发生</param>
        /// <param name="arg">页面参数</param>
        public static async Task PushAndRemoveUntilAsync(string id, System.Func<string, bool> until, object arg = emptyObj)
        {
            if (!table.ContainsKey(id))
            {
                Debug.LogError("StackUI:没有注册页面:" + id);
                return;
            }
            if(until == null)
                return;

            if(CurrentInstanceID() == id)
            {
                return;
            }

            int count = uiLayer.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                var builder = uiLayer.Peek();               
                bool result = until(builder.id);
                await builder.CloseAsync();
                if (result)
                {
                    var cur = table[id];
                    uiLayer.Push(table[id]);
                    await cur.BuildAsync(arg);
                    return;
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
                await cur.BuildAsync(arg);
            }
            else
            {
                Debug.LogError("StackUI:操作失败：条件似乎永远都不会返回真");
            }

        }


        /// <summary>
        /// 将之前的所有的页面移除(Pop)，直到符合条件为止,然后打开一个页面（Push）
        /// <para>使用场景举例：从多层购买流程中跳到“支付结果页”，并移除中间步骤直到“商品详情页”或“首页”</para>
        /// </summary>
        /// <typeparam name="T">页面类型</typeparam>
        /// <param name="until">返回真时，不再移除</param>
        /// <param name="arg">页面参数</param>
        public static async Task PushAndRemoveUntilAsync<T>(System.Func<string, bool> until, object arg = emptyObj) where T:BasePresenter
        {
            await PushAndRemoveUntilAsync(GetID<T>(),until,arg);
        }


        /// <summary>
        /// 显示一个窗口 叠在页面上 不影响页面主栈
        /// </summary>
        /// <param name="id">窗口ID</param>
        /// <param name="arg">窗口参数</param>
        /// <returns>窗口</returns>
        public static async Task<BasePresenter> ShowWinAsync(string id,object arg = emptyObj)
        {
            if (!table.ContainsKey(id))
            {
                Debug.LogError("StackUI:没有注册窗口:" + id);
                return null;
            }

            var builder = table[id];
            if(!await builder.BuildAsync(arg))
            {
                return null;
            }
            if (!winds.ContainsKey(id))
                winds.Add(id,builder);
            return builder.Presenter;
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
        public static async Task<BasePresenter> ShowWinAsync(string id,bool ifExistDoReinit,object arg = emptyObj)
        {
            //窗口存在 但是需要刷新
            if(ExistWin(id))
            {
                if(ifExistDoReinit)
                    return await ShowWinAsync(id,arg);
            }
            else
            {
                //窗口不存在
                return await ShowWinAsync(id,arg);
            }
            return GetWin(id);
        }

        /// <summary>
        /// 显示一个窗口 叠在页面上 不影响页面主栈
        /// </summary>
        /// <param name="arg">窗口参数</param>
        /// <typeparam name="T">窗口类型</typeparam>
        /// <returns>窗口</returns>
        public static async Task<T> ShowWinAsync<T>(object arg = emptyObj) where T:BasePresenter
        {
            return await ShowWinAsync(GetID<T>(),arg) as T;
        }

        /// <summary>
        /// 显示一个窗口 如果窗口已经存在了 指示要不要触发ReInit函数
        /// <para> 可如果窗口不存在 按正常流程显示一个窗口 </para>
        /// </summary>
        /// <param name="ifExistDoReinit">如果窗口已在当前屏幕，且ifExistDoReinit为true 会触发Reinit 函数 如果ifExistDoReinit为false 不会触发任何函数</param>
        /// <param name="arg">窗口参数</param>
        /// <typeparam name="T">窗口类型</typeparam>
        /// <returns>窗口</returns>
        public static async Task<T> ShowWinAsync<T>(bool ifExistDoReinit,object arg = emptyObj) where T:BasePresenter
        {
            return await ShowWinAsync(GetID<T>(),ifExistDoReinit,arg) as T;
        }


        /// <summary>
        /// 删除所有页面和窗口
        /// <para>使用场景举例：退出登录前或请回到主页前，可统一关闭所有页面和窗口，避免残留UI，获得一个清晰干净的页面状态和缓存</para>
        /// </summary>
        public static async Task ClearAsync()
        {
            if (uiLayer.Count > 0)
            {
                var top = uiLayer.Peek();
                await top.CloseAsync();
            }
            uiLayer.Clear();

            foreach(var win in winds)
            {
                await win.Value.CloseAsync();
            }
            winds.Clear();

        }
    }
}