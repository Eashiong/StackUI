using UnityEngine;
using StackUI;

namespace StackUI.Demo.MultipleExamples
{
    /// <summary>
    /// Base 演示入口。把本组件挂到场景任意物体上，运行后自动登录页起步：
    ///
    ///   登录页(LoginView)
    ///     └─登录(PopAndPush)─► 主页(HomeView)
    ///           ├─逛商城(Push)──► 商品页(GoodsView)
    ///           │                   ├─返回(Pop)─────► 主页
    ///           │                   └─购买(ShowWin)─► 确认弹窗(ConfirmDialog)
    ///           │                                       └─确定(Push)─► 支付成功(PaySuccessView)
    ///           │                                                         └─返回首页(PopUntil)─► 主页
    ///           └─设置(Push)──► 设置页(SettingsView)
    ///                              └─退出登录(PushAndRemoveUntil)─► 登录页
    /// </summary>
    public class MultipleExamples : MonoBehaviour
    {
        private void Start()
        {
            Navigation.AddTable<LoginPresenter>("StackUIDemo/MultipleExamples/LoginView");
            Navigation.AddTable<HomePresenter>("StackUIDemo/MultipleExamples/HomeView");
            Navigation.AddTable<GoodsPresenter>("StackUIDemo/MultipleExamples/GoodsView");
            Navigation.AddTable<PayPresenter>("StackUIDemo/MultipleExamples/PayView");
            Navigation.AddTable<SettingsPresenter>("StackUIDemo/MultipleExamples/SettingsView");
            Navigation.AddTable<ConfirmDialogPresenter>("StackUIDemo/MultipleExamples/ConfirmDialogView");
            Navigation.AddTable<ShoppingCartPresenter>("StackUIDemo/MultipleExamples/ShoppingCartView");

            Navigation.Push<LoginPresenter>();
        }
    }
}
