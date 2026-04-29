using StackUI;

namespace StackUI.Demo.MultipleExamples
{
    /// <summary>
    /// 设置页视图：退出登录按钮 + 返回按钮。
    /// </summary>
    public class SettingsView : View
    {
        public UIButton logoutBtn;
        public UIButton backBtn;
    }

    /// <summary>
    /// 设置页：
    /// - 返回按钮：Pop（回到主页）。
    /// - 退出登录：清空整个栈并跳到登录页 —— 用 PushAndRemoveUntil 实现，
    ///   until 在主页时为真，框架会移除其上所有页面，再用登录页替换主页。
    /// </summary>
    public class SettingsPresenter : Presenter<SettingsView>
    {
        public override void OnInit(object arg)
        {
            ListenUnity(view.backBtn.onClick, () => Navigation.Pop());

            ListenUnity(view.logoutBtn.onClick, () =>
            {
                //移除所有页面知道当前页面为空为止
                Navigation.PushAndRemoveUntil<LoginPresenter>(id => id == "");

                //或者使用Clear方法，清空整个栈并跳到登录页
                // Navigation.Clear();
                // Navigation.Push<LoginPresenter>();
            });
        }
    }
}
