
using StackUI;
namespace StackUI.Demo
{
    public class LoginView : View
    {
        public UIButton loginBtn;
    }
    public class LoginPresenter : Presenter<LoginView>
    {
        public override void OnInit(object arg)
        {
            ListenUnity(view.loginBtn.onClick, () => Navigation.Push("MainPresenter", "Hello"));
        }
    }
}