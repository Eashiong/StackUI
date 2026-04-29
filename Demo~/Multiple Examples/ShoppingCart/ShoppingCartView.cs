using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace StackUI.Demo.MultipleExamples
{
    public class ShoppingCartView : PageView
    {
        public UIButton buyBtn;
        public Text text;
    }

    public class ShoppingCartPresenter : Presenter<ShoppingCartView>
    {
        public override void OnInit(object arg)
        {
            GoodsData goodsData = arg as GoodsData;

            view.text.text = "购买" + goodsData.count + "个商品，总价" + goodsData.priceUnit * goodsData.count + "元";
            
            view.backAction = () => Navigation.Pop();
            ListenUnity(view.buyBtn.onClick, () =>
            {
                Navigation.ShowWin<ConfirmDialogPresenter>(new ConfirmDialogArg
                {
                    message = "确认购买该商品？",
                    onConfirm = () => Navigation.Push<PayPresenter>()
                });
            });
           
        }
    }

}
