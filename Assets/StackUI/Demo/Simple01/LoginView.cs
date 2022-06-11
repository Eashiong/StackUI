
using StackUI;
public class LoginView : View
{
    public UIButton loginBtn;
}
public class LoginPresenter:Presenter
{
    private LoginView myView;
    public override void OnInit(object arg)
    {
        base.OnInit(arg);
        myView =  view as LoginView;
        myView.loginBtn.AddListener(()=> Navigation.Push("MainPresenter","Hello"));
    }
    public override void OnClose()
    {
        base.OnClose();
        //手动释放可以提高性能
        myView.loginBtn.RemoveAllListeners();

    }
}
