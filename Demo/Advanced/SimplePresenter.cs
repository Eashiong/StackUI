using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StackUI;

namespace StackUI.Demo
{
    public class SimplePresenter : Presenter<SimpleView>
    {
        //新建 或者重新打开都会调用
        public override void OnInit(object arg)
        {
            view.update = JumpPage;
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
