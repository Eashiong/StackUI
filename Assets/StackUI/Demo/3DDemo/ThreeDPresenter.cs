using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StackUI;
namespace Demo
{
    public class ThreeDPresenter : Presenter
    {
        private ThreeDView myView;
        private ThreeDData data;
        public override void OnInit(object arg)
        {
            base.OnInit(arg);
            myView = view as ThreeDView;
            data = new ThreeDData(); 
            data.info = arg as ContentData.ContentInfo;

            
            myView.backAction = () => Navigation.Pop(this);

           
            myView.dynamicCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            myView.dynamicCube.transform.SetParent(myView.transform);
            myView.dynamicCube.transform.localPosition = new Vector3(0,0,6);
            myView.dynamicCube.GetComponent<MeshRenderer>().material.color = data.info.color;
            myView.speed = data.info.speed;
            myView.cubeName.text = data.info.name;


        }
        public override void OnClose()
        {
            base.OnClose();
            //谁创建 谁负责删除
            //也可以不删除 隐藏起来 但这就要求下次打开(OnInit)的时候 去服务器拉数据，然后更新这个模型，而不是再次创建他
            Object.Destroy(myView.dynamicCube);


        }
    }
}
