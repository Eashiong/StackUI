using System;
using System.Collections.Generic;

namespace StackUI
{
    /// <summary>
    /// ID 管理器
    /// </summary>
    internal static class IDManager
    {
        /// <summary>
        /// 获取类型 T 的 ID
        /// </summary>
        internal static string GetID<T>() where T:BasePresenter
        {
            return typeof(T).FullName;
        }
        /// <summary>
        /// 获取类型 T 的 ID
        /// </summary>
        internal static string GetID(Type t)
        {
            return t.FullName;
        }

        private static Dictionary<string,Type> idMap = new Dictionary<string, Type>();
        internal static string Register<T>() where T:BasePresenter
        {
            return Register(typeof(T));
        }
        internal static string Register(Type t)
        {
            string id = GetID(t);
            string newID = id;
            int index = 0;
            while(idMap.ContainsKey(newID))
                newID = id + "_" + index;
            idMap[newID] = t;
            return newID;
        }
        internal static Type GetType(string id)
        {
            return idMap[id];
        }
    }
}