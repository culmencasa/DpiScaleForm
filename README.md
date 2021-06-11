# DpiScaleFormDemo
**根据DPI比例缩放窗体和控件大小的例子**



![在DPI为125%的状态下, UseDpiScale为true和false的对比](https://github.com/culmencasa/DpiScaleForm/blob/main/NoneVsDPI.png)
               (在DPI为125%的状态下, UseDpiScale为true和false的对比)


2021-06-11 测试环境: Windows 10,XP, Visual Studio 2019, .Net Framework 3.5

实现方式: 计算当前DPI比例获得factor值, 调用Form的Scale()传入factor值. 调整控件字体.

使用说明:
1. 窗体继承DpiScaleForm
2. 保证窗体设计器是在VisualStudio 100%缩放比例下运行的.或者Windows显示设置为96 DPI.
3. 如果窗体AutoScaleMode默认是Font, 系统会自动根据字体缩放. 效果因系统而异.
4. 在窗体构造的InitializeComponent()之后, 调用UseDpiScale=true或AutoDpiScale=true.
5. UseDpiScale=true 将强制按照当前DPI比例缩放.
6. AutoDpiScale=true 仅当AutoScaleMode属性不等于Font时才会按照DPI比例缩放.

备注: DpiScaleForm类只做了简单布局的缩放测试. 
     较复杂的窗体布局可能不正常. 特殊控件也未做处理. 
     显示器切换的情况未考虑.
     仅作参考.
     
对于.NET framework 4.7以上Winform 关于DPI的处理, 参见文章 https://docs.telerik.com/devtools/winforms/telerik-presentation-framework/dpi-support?_ga=2.20289336.1856590203.1623301720-198642324.1623301720


