using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StackUI;
namespace StackUI.Demo
{

    public class SplashView : View
    {
        public UIText uiText;
    }

    public class SplashPresenter:Presenter
    {
        private SplashView myView;
        public override void OnInit(object arg)
        {
            base.OnInit(arg);
            myView = view as SplashView;
            myView.StartCoroutine(Timer(arg as string));

        }
        private IEnumerator Timer(string nextPage)
        {
            int time = 3;
            while(time >=0)
            {
                myView.uiText.text = time.ToString();
                yield return new WaitForSeconds(1);
                time = time - 1;
            }
            //下一个页面点后退的时候不应该出现闪屏页
            Navigation.PopAndPush(nextPage);
        }
    }

}