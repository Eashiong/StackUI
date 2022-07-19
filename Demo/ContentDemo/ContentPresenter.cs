using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StackUI;

namespace StackUI.Demo
{
    public class ContentPresenter : Presenter
    {

        private ContentView myView;
        private ContentData serverData;

        public override void OnInit(object arg)
        {
            base.OnInit(arg);
            myView = view as ContentView;
            //注册页面后退事件
            //后退的时候 把当前页面关闭。系统会自动把上一次打开的页面打开
            myView.backAction = () => Navigation.Pop(this);
            var loading = Navigation.ShowWin("LoadingPresenter");
            ContentData.GetServerData((serverData) =>
            {
                for (int i = 0; i < serverData.datas.Count; i++)
                {
                    ContentData.ContentInfo info = serverData.datas[i];
                    UIButton btn = GameObject.Instantiate(myView.prefab, myView.prefab.transform.parent).GetComponent<UIButton>();
                    btn.gameObject.SetActive(true);
                    (btn.showTarget as UIText).text = info.name;
                    myView.allImages.Add(btn);

                    btn.AddListener(() =>
                    {
                        //点击按钮 在新页面去展示这个按钮代表的资产详情
                        Navigation.Push("ThreeDPresenter", info);
                    });
                }
                Navigation.HideWin(loading);
            });



        }



        public override void OnClose()
        {
            base.OnClose();
            foreach (var item in myView.allImages)
            {
                Object.Destroy(item.gameObject);
            }
            myView.allImages.Clear();
        }
    }
}
