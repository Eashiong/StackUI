using StackUI;

namespace StackUI.Demo.MultipleExamples
{
    /// <summary>
    /// 主页视图：两个入口按钮。请在 prefab 上把对应按钮拖到字段。
    /// </summary>
    public class HomeView : View
    {
        public UIButton goodsBtn;
        public UIButton settingsBtn;
    }

    /// <summary>
    /// 主页：两个按钮分别 Push 到商品页和设置页。
    /// </summary>
    public class HomePresenter : Presenter<HomeView>
    {
        public override void OnInit(object arg)
        {
            ListenUnity(view.goodsBtn.onClick, () => Navigation.Push<GoodsPresenter>());
            ListenUnity(view.settingsBtn.onClick, () => Navigation.Push<SettingsPresenter>());
        }
    }
}
