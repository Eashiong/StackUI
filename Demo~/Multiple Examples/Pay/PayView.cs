using StackUI;

namespace StackUI.Demo.MultipleExamples
{
    public class PayView : PageView
    {
        public UIButton homeBtn;
    }

    /// <summary>
    /// 支付成功页：用户已经走完"主页→商品→购买"流程，
    /// 这里需要一次性跳过中间的商品页，直接回到主页 —— 用 PopUntil 实现。
    /// </summary>
    public class PayPresenter : Presenter<PayView>
    {
        public override void OnInit(object arg)
        {
            ListenUnity(view.homeBtn.onClick, () =>
            {
                Navigation.PopUntil(id => id == nameof(HomePresenter));
                
            });
        }
    }
}
