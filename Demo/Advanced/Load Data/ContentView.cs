using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StackUI;

namespace StackUI.Demo
{
    public class ContentView : PageView
    {
        public GameObject prefab;

        public List<Button> allImages {get;set;} = new List<Button>();

        public override void OnShow()
        {
            base.OnShow();
            Debug.Log(go.name +  "被打开了");
        }
        public override void OnClose()
        {
            base.OnClose();
            Debug.Log(go.name + "已关闭");
        }

    }
}
