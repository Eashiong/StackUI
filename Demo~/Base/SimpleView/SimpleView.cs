using System;
using System.Collections;
using System.Collections.Generic;
using StackUI;
using UnityEngine;

namespace StackUI.Demo.Base
{
    public class SimpleView : View
    {

        public UIButton lifecycleButton;
        public UIButton dialogButton;
        public UIButton skinButton;
        public UIButton assetLoaderButton;


    }

    public class SimplePresenter : Presenter<SimpleView>
    {
        //新建 或者重新打开都会调用
        public override void OnInit(object arg)
        {
            ListenUnity(view.lifecycleButton.onClick, () => Navigation.Push<LifeCyclePresenter>());

            ListenUnity(view.dialogButton.onClick, () =>
            {
                var arg = new StackUI.Demo.MultipleExamples.ConfirmDialogArg
                {
                    message = "Hello World",
                    onConfirm = () =>
                    {
                        Debug.Log("Confirm");
                    },
                    onCancel = () =>
                    {
                        Debug.Log("Cancel");
                    }
                };
                Navigation.ShowWin<StackUI.Demo.MultipleExamples.ConfirmDialogPresenter>(arg);

            });

            ListenUnity(view.skinButton.onClick, ()=> Navigation.Push<SkinPresenter>());

            ListenUnity(view.assetLoaderButton.onClick, ()=> Navigation.Push<AssetLoaderPresenter>());

        }
    }
}
