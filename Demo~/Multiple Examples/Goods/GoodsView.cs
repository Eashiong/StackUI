using StackUI;

namespace StackUI.Demo.MultipleExamples
{
    public class GoodsData
    {
        public int count = 0;
        public float priceUnit = 10;
    }
    /// <summary>
    /// 商品页视图：购买按钮 + 返回按钮。
    /// </summary>
    public class GoodsView : View
    {
        public UIButton addBtn;
        public UIButton backBtn;
        public UIButton buyBtn;
    }

    /// <summary>
    /// 商品页：
    /// - 返回按钮：Pop（回到主页）。
    /// - 购买按钮：弹出确认窗口（ShowWin），确认后 Push 到支付成功页。
    /// </summary>
    public class GoodsPresenter : Presenter<GoodsView>
    {
        private GoodsData goodsData;
        public override void OnInit(object arg)
        {
            goodsData = new GoodsData();
            view.addBtn.SetText("+" + goodsData.count.ToString());
            
            ListenUnity(view.backBtn.onClick, () => Navigation.Pop());

            ListenUnity(view.addBtn.onClick, () =>
            {
                goodsData.count++;
                view.addBtn.SetText("+" + goodsData.count.ToString());
            });

            ListenUnity(view.buyBtn.onClick, () =>
            {
                Navigation.Push<ShoppingCartPresenter>(goodsData);
            });
        }
    }
}
