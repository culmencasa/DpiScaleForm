# DpiScaleFormDemo
**根据DPI比例缩放窗体和控件大小的例子**



![在DPI为125%的状态下, UseDpiScale为true和false的对比](https://github.com/culmencasa/DpiScaleForm/blob/main/NoneVsDPI.png)
               >>>同一窗体，在100%下设计，在另一个DPI为125%的系统下, UseDpiScale为true和false的对比<<<


#### 2021-06-11 测试环境: Windows 10,XP, Visual Studio 2019, .Net Framework 3.5, 4.0, 4.8


#### 2021-08-11 更新

**实现思路:** 
计算当前DPI比例获得factor值, 调用Form的Scale()传入factor值. 调整控件字体.

**使用说明:**
1. 窗体继承DpiScaleForm

2. 在窗体构造的InitializeComponent()之后, 调用UseDpiScale=true或AutoDpiScale=true.

3. UseDpiScale=true 强制缩放，需要同时设置DesignFactor的值。如果当时的设计是在200%下，设置DesignFactor=2。

4. AutoDpiScale=true 使用系统的DPI缩放。不需要设置DesignFactor的值。
   仅当AutoScaleMode值为Dpi时才会启用缩放。

**备注:** 

> DpiScaleForm类只做了简单布局的缩放测试. 
  较复杂的窗体布局可能不正常. 特殊控件也未做处理.      
  显示器切换的情况未考虑.    
  代码仅作参考.

**窗体设计时的建议：**

1. 最好保证窗体设计器是在VS 100%缩放比例下运行的.或者Windows系统显示设置为100%(96 DPI).

   最好所有的容器控件使用相同的AutoScaleMode。
   
   最好所有的控件使用相同的字体。
   
   
2. 满足条件1时， 如果AutoScaleMode为None时，窗体不会随系统缩放。 如果窗体AutoScaleMode默认是Font, 系统会自动根据字体缩放. 效果因系统而异.              

   不满足条件1时，即使AutoScaleMode为None, 窗体的缩放也会因系统环境及DPI设定而异。           

3. 在窗体构造的InitializeComponent()之后, 调用UseDpiScale=true或AutoDpiScale=true.

4. 关于Win10以上系统使用manifest文件来启用DPI感知, 参见文章 

   https://docs.telerik.com/devtools/winforms/telerik-presentation-framework/dpi-support?_ga=2.20289336.1856590203.1623301720-198642324.1623301720

