# StackUI
基于UGUI的栈式简单导航框架，适用于进行仿原生页面开发维护

## 功能

* 页面前进、后退、弹窗、回退、回退到开始、页面传参等导航功能
* 页面默认缓存，也可单独自定义某个页面或全部页面是否需要缓存
* 页面默认Resource加载，也可单独自定义某个页面或全部页面的加载方式
* 初步的MVP规范
* 弹窗，界面叠加
* 带返回键的页面模板
* 一键绑定所有物体到脚本Inspector属性面板
* 继承部分UI组件（button text image)，一般建议根据自己的业务进行扩展UGUI的组件。
* Hierarchy可根据模板创建物体，统一规范并提升开发效率


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
        //手动释放可以提高性能
        myView.loginBtn.RemoveAllListeners();

    }
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



