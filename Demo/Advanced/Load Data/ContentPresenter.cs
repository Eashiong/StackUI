using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StackUI;

namespace StackUI.Demo
{
    public class ContentPresenter : Presenter<ContentView>
    {
        private ContentData serverData;

        public override void OnInit(object arg)
        {
            //注册页面后退事件
            //后退的时候 把当前页面关闭。系统会自动把上一次打开的页面打开
            view.backAction = () => Navigation.Pop(this);
            Navigation.ShowWin("LoadingPresenter");
            ContentData.GetServerData((serverData) =>
            {
                for (int i = 0; i < serverData.datas.Count; i++)
                {
                    ContentData.ContentInfo info = serverData.datas[i];
                    UIButton btn = GameObject.Instantiate(view.prefab, view.prefab.transform.parent).GetComponent<UIButton>();
                    btn.gameObject.SetActive(true);
                    (btn.showTarget as UIText).text = info.name;
                    view.allImages.Add(btn);

                    btn.AddListener(() =>
                    {
                        //点击按钮 在新页面去展示这个按钮代表的资产详情
                        Navigation.Push("ThreeDPresenter", info);
                    });
                }
                Navigation.HideWin("LoadingPresenter");
            });



        }



        public override void OnClose()
        {
            foreach (var item in view.allImages)
            {
                Object.Destroy(item.gameObject);
            }
            view.allImages.Clear();
        }
    }
}
