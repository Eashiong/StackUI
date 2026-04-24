using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StackUI;
namespace StackUI.Demo
{
    
    public class SkinView : View
    {
        public Button red;
        public Button white;
    }

    public class SkinPresenter:Presenter<SkinView>
    {

        public override void OnInit(object arg)
        {
            base.OnInit(arg);

            ListenUnity(view.red.onClick,()=>
            {
                if(Navigation.GetAssetName(this.id) == SkinViewRegister.white)
                {
                    
                    Navigation.SetAssetName(this.id,SkinViewRegister.red);
                    Navigation.PopAndPush(this.id);
                }

            });

            ListenUnity(view.white.onClick,()=>
            {
                if(Navigation.GetAssetName(this.id) == SkinViewRegister.red)
                {
                    Navigation.SetAssetName(this.id,SkinViewRegister.white);
                    Navigation.PopAndPush(this.id);
                }
            });
        }
    }
}