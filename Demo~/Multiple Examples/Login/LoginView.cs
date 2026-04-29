using StackUI;

namespace StackUI.Demo.MultipleExamples
{
    /// <summary>
    /// 登录页视图：仅一个登录按钮。请在 prefab 上挂载并把 loginBtn 拖到该字段。
    /// </summary>
    public class LoginView : View
    {
        public UIButton loginBtn;
    }

    /// <summary>
    /// 登录页：点登录按钮后用 PopAndPush 跳到主页。
    /// 这样栈里登录页会被替换掉，主页 Pop 不会再回到登录页（防止用户误返）。
    /// </summary>
    public class LoginPresenter : Presenter<LoginView>
    {
        public override void OnInit(object arg)
        {
            ListenUnity(view.loginBtn.onClick, () =>
            {
                Navigation.PopAndPush<HomePresenter>();
            });
        }
    }
}
