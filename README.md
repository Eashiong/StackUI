# StackUI
基于UGUI的栈式UI导航框架，

## 功能列表

* 页面前进、后退、弹窗、回退、回退到开始、页面传参等导航功能
* 页面默认缓存，也可单独自定义某个页面或全部页面是否需要缓存
* 页面默认Resource加载，也可单独自定义某个页面或全部页面的加载方式
* 初步的MVP规范
* 支持弹窗，界面叠加
* UI生命周期
* 带返回键的页面模板
* 一键绑定所有物体到脚本Inspector属性面板
* 继承部分UI组件（button text image)，一般建议根据自己的业务进行扩展UGUI的组件。
* Hierarchy可根据模板创建物体，统一规范并提升开发效率

## 页面打开与关闭

| 函数 | 功能| 备注 |
| ------- | ------- |------- |
|Navigation.AddTable<Type>("ID","assetName");    |    注册页面     |  ID自己指定的唯一ID   assetName UI资源，详解看下文  |
|Current    |    获取当前页     |       |
|Push    |    打开一个页面     |    当前页面关闭，新页面覆盖    |
|PopAndPush     |    与当前页面置换     |  当前页面关闭，且不保留历史，也就是说新页面后退不会回到这个旧页面上，而是会回到这个旧页面的上一页
|PushAndRemoveAll    |    清除历史记录并打开一个新页面   | 不再保留所有的页面打开历史，也就是说如果新页面也点击后退是没有任何效果的    |
|PushAndRemoveUntil    |    清除历史记录并打开一个新页面   | 删除页面历史直到满足你设定的条件时停止删除    |
|Pop    |    退出当前页   | 关闭当前页面，将会显示上一页    |
|PopUntil    |    持续退出页面,直到符合条件为止   | 可以实现一路退出到你想要的那个页面为止   |
|ShowWin    |    叠加显示一个窗口     |       |
|HideWin    |    隐藏窗口     |       |
|ExistWin    |    当前某窗口是否打开     |       |
|GetWin    |    获取某窗口   |     |
|Clear    |    删除所有页面和窗口   |     |


## 页面生命周期

  ```
        //资源加载后调用
        public override void OnAssetLoaded()
        {
            base.OnAssetLoaded();
        }
        //被打开的时候调用（重复打开不会调用它）
        //arg 页面传参
        public override void OnInit(object arg)
        {
            base.OnInit(arg);
        }
        //已经是打开状态下 重复打开时调用
        //arg 页面传参
        public override void OnReInit(object arg)
        {
            base.OnReInit(arg);
        }
        //关闭时候调用
        public override void OnClose()
        {
            base.OnClose();
        }
        //释放资源前调用(如果Navigation.SetDontDestroyAsset，或注册的时候,标识了隐藏的时候销毁资源)
        //它在OnClose后调用
        public override void OnDispose()
        {
            base.OnDispose();

        }
  ```
## 资源加载方式
你应该有一个全局的脚本，游戏一开始时候注册所有页面
```
void Awake()
{
    //LoadingPresenter为逻辑代码，“LoadingPresenter”页面ID，"LoadingWin"资源路径
    Navigation.AddTable<LoadingPresenter>("LoadingPresenter","LoadingWin");
    Navigation.AddTable<SimplePresenter>("SimplePresenter","SimpleView");
    .......

}
```

默认页面关闭不会删除gameobject和资源，只是隐藏，如果希望页面关闭的时候也删除资源 请使用：
```Navigation.AddTable<LoadingPresenter>("LoadingPresenter","LoadingWin",false);```
这样页面关闭的时候才会触发生命周期函数OnDispose





资源加载默认使用Reources.Load,你的预制体应该放在Resources目录

你也可以指定加载方式，如果想用Assetbundle，自己实现CustomLoader即可

```
Navigation.AddTable<LoadingPresenter>("LoadingPresenter","LoadingWin", false,CustomLoader);
private GameObject CustomLoader(string sourceName)
{
    var prefab = Resources.Load<GameObject>(sourceName);
    if (prefab == null)
    {
        throw new System.Exception("找不到资源，请检查拼写:" + sourceName);
    }
    var go = GameObject.Instantiate(prefab);
    go.name = sourceName;
    return go;
}
    
```

同一套UI代码，你还可以动态的在任意位置使用不同的UI，前提是他们的逻辑应该是相同的，只是皮肤不同而已

```
Navigation.SetAssetName("LoadingPresenter","NewLoadingWin");
Navigation.ShowWin("LoadingPresenter");
```

## 开始

制作一个简单的登录页和主页，演示他们之间的跳转逻辑

1，导入代码到你的项目，并打开SpawnViewEditor.cs 修改函数SpawnCanvas. 尤其是修改如下两行代码
```
  static Transform SpawnCanvas(Camera cam)
  {
      .......
      //修改成自己需要的分辨率和适配方式
      scaler.referenceResolution = new Vector2(750,1624);
      scaler.matchWidthOrHeight = 1;
      ......
  }
  ```
  2，新建场景，并在场景中创建一个EventSystem
  
  3，在场景中单击Hierarchy MVP/Simple UI,并更名为LoginView,在画布下新建一个按钮并更名为LoginBtn，完成后生成prefab并放到Resources下
  
  4，新建视图类脚本LoginView.cs
  ```
  using StackUI;
  public class LoginView : View
  {
      public UIButton loginBtn;
  }
  ```
  脚本挂载到prefab LoginView上，并在Inspector上点击绑定引用
  
  5，新建脚本LoginPresenter.cs
  ```
    using StackUI;
    public class LoginPresenter:Presenter
    {
        private LoginView myView;
        public override void OnInit(object arg)
        {
            base.OnInit(arg);
            myView =  view as LoginView;
            myView.loginBtn.AddListener(()=> Navigation.Push("MainPresenter","Hello"));
        }
        public override void OnClose()
        {
            base.OnClose();
            myView.loginBtn.RemoveAllListeners();

        }
    }
  ```
这里是UI全部的生命周期函数

  ```
        //资源加载后调用
        public override void OnAssetLoaded()
        {
            base.OnAssetLoaded();
        }
        //被打开的时候调用（重复打开不会调用它）
        public override void OnInit(object arg)
        {
            base.OnInit(arg);
        }
        //已经是打开状态下 重复打开时调用
        public override void OnReInit(object arg)
        {
            base.OnReInit(arg);
        }
        //关闭时候调用
        public override void OnClose()
        {
            base.OnClose();
        }
        //释放资源前调用(如果Navigation.SetDontDestroyAsset，或注册的时候,标识了隐藏的时候销毁资源)
        //它在OnClose后调用
        public override void OnDispose()
        {
            base.OnDispose();

        }
  ```
  我们已经完成了登录界面 接下来制作一个主页
  
  6，在新场景中单击Hierarchy MVP/Return UI,并更名为MainView,完成后生成prefab并放到Resources下
  
  7，新建视图类脚本MainView.cs，并继承PageView,除此之外我们不加任何代码
  ```
using StackUI;
public class MainView : PageView
{
    
}
  ```
  绑定在prefab上，并在Inspector上点击绑定引用
  
  8，编写MainPresenter.cs
  ```
  using StackUI;
  public class MainPresenter:Presenter
{
    MainView myView;
    public override void OnInit(object arg)
    {
        base.OnInit(arg);
        Debug.Log(arg as string);//输出 Hello
        myView = view as MainView;
        myView.backAction = ()=> Navigation.Pop(this);//页面后退

    }
    public override void OnClose()
    {
        base.OnClose();
        myView.backAction = null;
    }
}
  ```
  
  2个页面到此制作完成
  
 9，在场景中新建一个物体并挂载脚本Main.cs，用于注册我们制作好的两个页面
 ```
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StackUI;
public class Test: MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //注册类、ID、和资源名
        //建议ID和类名一致
        Navigation.AddTable<LoginPresenter>("LoginPresenter","LoginView");
        Navigation.AddTable<MainPresenter>("MainPresenter","MainView");

        //传入ID  打开登录页
        Navigation.Push("LoginPresenter");
        
    }

}

 ```
 
 
 运行！点击登录按钮会跳转到mainView，在mainView点后退图标会回到登录
 
 查看demo了解更多使用方式
  
  
  
## Demo
运行场景Demo/Scenes/SampleScene.unity,游戏分辨率尽量设置成竖屏，因为demo中的界面是这么适配的

demo涵盖如下使用场景
* 无UI完全3D
* 闪屏画面
* 菜单UI，并带返回键
* 展示3D场景和UI混合
* loading窗口叠加在其他UI界面上
* 页面之间传参



