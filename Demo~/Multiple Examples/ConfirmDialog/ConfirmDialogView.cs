using System;
using StackUI;
using UnityEngine.UI;

namespace StackUI.Demo.MultipleExamples
{
    /// <summary>
    /// 弹窗显示参数：业务方传入提示语和确认回调。
    /// </summary>
    public class ConfirmDialogArg
    {
        public string message;
        public Action onConfirm;
        public Action onCancel;
    }

    /// <summary>
    /// 通用确认弹窗视图：消息文字 + 确定/取消两个按钮。
    /// </summary>
    public class ConfirmDialogView : View
    {
        public Text messageText;
        public UIButton confirmBtn;
        public UIButton cancelBtn;
    }


    public class ConfirmDialogPresenter : Presenter<ConfirmDialogView>
    {
        private Action _onConfirm;
        private Action _onCancel;

        public override void OnInit(object arg)
        {
            Bind(arg);
            ListenUnity(view.confirmBtn.onClick, () =>
            {
                var cb = _onConfirm;
                Navigation.HideWin<ConfirmDialogPresenter>();
                cb?.Invoke();
            });
            ListenUnity(view.cancelBtn.onClick, () => {
                var cb = _onCancel;
                Navigation.HideWin<ConfirmDialogPresenter>();
                cb?.Invoke();
            });
        }

        public override void OnReInit(object arg)
        {
            Bind(arg);
        }

        private void Bind(object arg)
        {
            if (arg is ConfirmDialogArg data)
            {
                view.messageText.text = data.message;
                _onConfirm = data.onConfirm;
                _onCancel = data.onCancel;
            }
        }
    }
}
