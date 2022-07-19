
/*
    View组件系列化
    属性绑定工具
*/
using UnityEngine;
using UnityEditor;
using System.Linq;


namespace StackUI.Tool
{
    [CustomEditor(typeof(View), true)]
    public class AutoBindObject : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var myScript = target as View;
            GUILayout.Space(10);
            serializedObject.Update();
            EditorGUILayout.HelpBox("脚本名和gameObject名相同时，可以按键绑定(不分大小写)", MessageType.Info);
            if (GUILayout.Button("绑定引用"))
            {
                Transform[] childs = myScript.gameObject.GetComponentsInChildren<Transform>(true);
                var prop = serializedObject.GetIterator();
                while (prop.Next(true))
                {
                    var items = childs.Where(item => item.gameObject.name.ToLower() == prop.propertyPath.ToLower()).ToList();
                    if (items.Count > 1)
                    {
                        Debug.LogError(string.Format("发现{0}个{1}，无法安全的绑定，请取一个有意义的名字", items.Count, prop.propertyPath));
                    }
                    else if (items.Count == 1)
                    {
                        prop.objectReferenceValue = items[0].gameObject;
                    }
                }

                serializedObject.ApplyModifiedProperties();



            }

        }

    }
}
