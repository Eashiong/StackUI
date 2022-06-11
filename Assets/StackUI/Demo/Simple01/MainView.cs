using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StackUI;
public class MainView : PageView
{
    
}

public class MainPresenter:Presenter
{
    MainView myView;
    public override void OnInit(object arg)
    {
        base.OnInit(arg);
        Debug.Log(arg as string);//输出 Hello
        myView = view as MainView;
        myView.backAction = ()=> Navigation.Pop(this);

    }
    public override void OnClose()
    {
        base.OnClose();
        myView.backAction = null;
    }
}
