using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StackUI.Demo
{
    public class SkinViewRegister : MonoBehaviour
    {
        public const string white = "StackUIDemo/DynamicSkin/SkinView-White";
        public const string red = "StackUIDemo/DynamicSkin/SkinView-Red";

        IEnumerator Start()
        {
            Navigation.AddTable<SkinPresenter>(white);

            Navigation.Push<SkinPresenter>();

            yield return new WaitForSeconds(3);



            Navigation.SetAssetName<SkinPresenter>(red);
            //如果当前已经在这个页面了 动态置换
            //如果不在 业务下次某个时候切换到这个页面的时候 会自动使用新皮肤
            if(Navigation.IsCurrent<SkinPresenter>())
            {
                Navigation.PopAndPush<SkinPresenter>();
            }


        }


    }
}