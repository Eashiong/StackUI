using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StackUI;
namespace StackUI.Demo
{
    public class ThreeDPresenter : Presenter<ThreeDView>
    {
        private ThreeDData data;
        public override void OnInit(object arg)
        {
            data = new ThreeDData(); 
            data.info = arg as ContentData.ContentInfo;

            
            view.backAction = () => Navigation.Pop(this);

           
            view.dynamicCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            view.dynamicCube.transform.SetParent(view.transform);
            view.dynamicCube.transform.localPosition = new Vector3(0,0,6);
            view.dynamicCube.GetComponent<MeshRenderer>().material.color = data.info.color;
            view.speed = data.info.speed;
            view.cubeName.text = data.info.name;


        }
        public override void OnClose()
        {
            base.OnClose();
            //谁创建 谁负责删除
            //也可以不删除 隐藏起来 但这就要求下次打开(OnInit)的时候 去服务器拉数据，然后更新这个模型，而不是再次创建他
            Object.Destroy(view.dynamicCube);


        }
    }
}
