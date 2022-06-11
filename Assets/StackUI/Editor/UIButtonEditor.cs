/*
    按钮序列化
*/
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
namespace StackUI.Tool
{


    [CustomEditor(typeof(UIButton), true)]
    [CanEditMultipleObjects]
    public class UIButtonEditor : ButtonEditor
    {
        private SerializedProperty m_interval;
        private SerializedProperty m_showTarget;


        protected override void OnEnable()
        {
            base.OnEnable();
            m_interval = serializedObject.FindProperty("interval");
            m_showTarget = serializedObject.FindProperty("showTarget");

        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_interval, new GUIContent("重复点击有效间隔"));
            EditorGUILayout.PropertyField(m_showTarget, new GUIContent("按钮显示图层"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
