using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StackUI;

namespace StackUI.Demo
{
    public class SimplePresenter : Presenter
    {
        private SimpleView myView;
        //新建 或者重新打开都会调用
        public override void OnInit(object arg)
        {
            base.OnInit(arg);
            myView = view as SimpleView;

            myView.update = JumpPage;
        }
        //关闭时调用
        //注意 关闭只是隐藏了物体，并不会删除
        public override void OnClose()
        {
            base.OnClose();
        }

        private void JumpPage()
        {
            if(Input.GetKey(KeyCode.Space))
            {
                //传递参数"ContentPresenter"给ContentPresenter,倒计时结束后当其跳转到ContentPresenter
                Navigation.Push("SplashPresenter","ContentPresenter");
            }
                
        }
    }
}
