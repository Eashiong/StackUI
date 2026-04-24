using UnityEditor;
using UnityEngine;
namespace StackUI.Editor
{
    [CreateAssetMenu(fileName = "StackUISetting", menuName = "StackUI/Create Setting Data")]
    public class UISettingData : ScriptableObject
    {

        public GameObject simpleUITemplatePath;
        public GameObject pageTemplatePath;
        public GameObject popupTemplatePath;


        public GameObject TextButtonTemplatePath;
        public GameObject ImageButtonTemplatePath;
        

    }

}