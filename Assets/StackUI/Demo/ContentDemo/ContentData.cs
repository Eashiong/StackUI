using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StackUI;

namespace Demo
{
    public class ContentData
    {
        public class ContentInfo
        {
            public string id;
            public string name;
            public float speed;//旋转速度
            public Color color;//材质颜色
        }
        public List<ContentInfo> datas;


        //模拟服务器请求数据

        public static void GetServerData(System.Action<ContentData> callback)
        {
            Load(callback).Start();
            
        }

        private static IEnumerator Load(System.Action<ContentData> callback)
        {
            yield return new WaitForSeconds(1.5f);
            List<ContentInfo> _datas = new List<ContentInfo>();
            for (int i = 0; i < 20; i++)
            {
                _datas.Add(
                    new ContentInfo()
                    {
                        id = i.ToString(),
                        name = "cube " + i.ToString(),
                        color = Color.blue,
                        speed = i * 5

                    }
                );
            }
            ContentData data = new ContentData()
            {
                datas = _datas
            };
            callback?.Invoke(data);
        }
    }
}