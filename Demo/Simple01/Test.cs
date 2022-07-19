using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StackUI;
namespace StackUI.Demo
{
    public class Test: MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            //注册类、ID、和资源名
            //建议ID和类名一致
            Navigation.AddTable<LoginPresenter>("LoginPresenter","LoginView");
            Navigation.AddTable<MainPresenter>("MainPresenter","MainView");

            //传入ID  打开登录页
            Navigation.Push("LoginPresenter");
            
        }

    }
}
