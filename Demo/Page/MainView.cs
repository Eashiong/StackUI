using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StackUI;
namespace StackUI.Demo
{
    public class MainView : PageView
    {

    }

    public class MainPresenter : Presenter<MainView>
    {
        
        //被打开的时候调用（重复打开不会调用它）
        public override void OnInit(object arg)
        {
            view.backAction = () => Navigation.Pop(this);

        }
       
    }
}