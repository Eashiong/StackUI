# UI MVP

将业务拆分为三个部分，即Presenter，Model，View。这并不是一个完全标准的mvp，只是适合我们自己项目的mvp

所有的示例在MVP/Demo，打开MVP/Scenes 运行查看

## MVP概念

#### View

视图，负责获取用户的输入，但并不处理用户的输入，负责定义自己动画的，但自己不调用自己去播放动画。由于unity prefab完成了view的大部分代码，所以这里view需要把prefab中的gameobject对象定义到代码中即可

view的代码应该是非常的短，因为它几乎只包含组件属性的引用

#### Presenter

直接翻译是主持人的意思，用来处理用户的输入；从服务器、数据库等地获取数据model，根据model更新view；根据输入修改model;
presenter的代码量应该是最多的

#### model
数据层，负责数据定义和数据的 读取接口

Presenter关联引用view和model，view和model无法互相调用
Presenter可以决定使用什么view，并不会影响功能逻辑

## 页面导航
应在APP启动页使用Navigation.AddTable定义好所有的路由
Navigation.Push 打开页面  Navigation.Pop页面后退
Push和Pop都可以传入参数给下一个页面 

## 工具
Hierarchy面板，MVP/Simpel UI选项卡创建一个简单的UI，它会帮你创建好相机，画布，并初始化一些他们的属性
Hierarchy面板, MVP/Return Ui，将创建一个可以返回的UI,并初始化相机、画布

框架修改了创建Image Text Button等一些基础组件的创建方式，自动会关闭一些属性来带来性能提升，比如关闭富文本，射线事件检测等，因为只有关键的一些UI才需要事件检测

用这些工具创建的UI，并绑定好view或其派生下来的组件，单机绑定按钮可以方便把物体绑定到组件上，请避免使用Find的方式，它效率比较低

符合下面的规范，点击按钮可以自动绑定好所有物体
* 物体名和变量名相同
* prefab上不会存在多个和需要绑定的物体同名的物体

## UI
Image  Text Button封装为UIImage  UIText  UIButton。用来方便后面的扩展。UIButon区分开了点击区域和图标显示区域，这样我们就能方便制作触控区域更大的按钮，它还定义了按钮频率，防止暴力点击带来的BUG



