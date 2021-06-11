using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Management;
using System.Diagnostics;

/// <summary>
/// DpiScaleForm 缩放窗体
/// 实现方式: 计算当前DPI比例获得factor值, 调用Form的Scale()传入factor值. 调整控件字体.
/// 
/// 使用说明:
/// 1. 窗体继承DpiScaleForm
/// 2. 保证窗体设计器是在VisualStudio 100%缩放比例下运行的.或者Windows显示设置为96 DPI.
/// 3. 如果窗体AutoScaleMode默认是Font, 系统会自动根据字体缩放. 效果因系统而异.
/// 4. 在窗体构造的InitializeComponent()之后, 调用UseDpiScale=true或AutoDpiScale=true.
/// 5. UseDpiScale=true 将强制按照当前DPI比例缩放.
/// 6. AutoDpiScale=true 仅当AutoScaleMode属性不等于Font时才会按照DPI比例缩放.
/// 
/// 备注: DpiScaleForm类只做了简单布局的缩放测试. 
///      较复杂的窗体布局可能不正常. 特殊控件也未做处理. 
///      显示器切换的情况未考虑.
///      仅作参考.
///      对于.NET framework 4.7以上Winform 关于DPI的处理, 参见文章 https://docs.telerik.com/devtools/winforms/telerik-presentation-framework/dpi-support?_ga=2.20289336.1856590203.1623301720-198642324.1623301720
/// </summary>
public partial class DpiScaleForm : Form
{
    #region 字段

    private bool _useDpiScale;
    private bool _autoDpiScale;
    private SizeF currentScaleFactor = new SizeF(1f, 1f);

    private Dictionary<string, Point> originalLocations = new Dictionary<string, Point>();
    private Dictionary<string, Size> originalSizes = new Dictionary<string, Size>();

    #endregion

    #region 属性

    /// <summary>
    /// 设置是否使用DPI缩放
    /// </summary>
    public bool UseDpiScale
    {
        get
        {
            return _useDpiScale;
        }
        set
        {
            bool changing = _useDpiScale != value;
            _useDpiScale = value;

            if (changing)
            {
                OnApplyUseDpiScale();
            }
        }
    }

    /// <summary>
    /// 自动缩放
    /// </summary>
    public bool AutoDpiScale
    {
        get
        {
            return _autoDpiScale;
        }
        set
        {
            bool changing = _autoDpiScale != value;
            _autoDpiScale = value;
            if (changing)
            {
                OnApplyAutoDpiScale();
            }
        }
    }

    /// <summary>
    /// 当前系统的DPI缩放因子
    /// </summary>
    public SizeF CurrentScaleFactor
    {
        get { return currentScaleFactor; }
        private set { currentScaleFactor = value; }
    }

    #endregion

    #region 构造

    public DpiScaleForm()
    {
    }


    #endregion

    #region 重写Form的成员

    protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
    {
        if (!DesignMode && UseDpiScale)
        {
            // 不处理Font模式
            if (this.AutoScaleMode == AutoScaleMode.Font)
            {
                base.ScaleControl(new SizeF(1, 1), specified);
            }
            else
            {
                base.ScaleControl(factor, specified);
            }
        }
        else
        {
            base.ScaleControl(factor, specified);
        }
    }

    #endregion

    #region 静态方法

    public static int ScaleInt(int value, SizeF scaleFactor)
    {
        return (int)Math.Round((double)((float)value * scaleFactor.Width), MidpointRounding.AwayFromZero);
    }

    #endregion

    #region 属性变更器

    protected virtual void OnApplyAutoDpiScale()
    {
        if (AutoDpiScale)
        {
            // Font模式下, 使用Window的缩放
            if (this.AutoScaleMode != AutoScaleMode.Font)
            {
                return;
            }
            else
            {
                var factor = GetCurrentScaleFactor();
                // 如果当前DPI大于100%, 则使用自定义的缩放
                if (factor > 1)
                {
                    //if (Environment.OSVersion.Version.Major >= 6)
                    //{
                    //}

                    UseDpiScale = true;
                }
            }
        }
    }

    protected virtual void OnApplyUseDpiScale()
    {
        if (UseDpiScale)
        {
            // 更改此属性会触发窗体的ScaleControl方法
            //this.AutoScaleMode = AutoScaleMode.Dpi;

            var factor = GetCurrentScaleFactor();
            CurrentScaleFactor = new SizeF(factor, factor);
            this.Scale(CurrentScaleFactor);
            this.ScaleFonts(factor);

        }
        else
        {
            this.AutoScaleMode = AutoScaleMode.None;
        }
    }

    #endregion

    #region 配合窗体的ScaleControl方法, 手动调整控件字体及特殊控件大小

    /// <summary>
    /// 
    /// </summary>
    /// <param name="scaleFactor"></param>
    public void ScaleFonts(float scaleFactor)
    {
        ScaleControlFont(this, scaleFactor);
    }

    /// <summary>
    /// 递归设置控件字体
    /// </summary>
    /// <param name="control"></param>
    /// <param name="factor"></param>
    protected virtual void ScaleControlFont(Control control, float factor)
    {
        // 方法1: 直接设置容器控件的字体后, 其子控件字体也会跟着变化.
        // 方法2: 不设置容器控件的字体, 递归设置其子控件的字体.
        if (control is Form || control is GroupBox || control is ContainerControl || control is ScrollableControl)
        {
            foreach (Control child in control.Controls)
            {
                ScaleControlFont(child, factor);
            }
        }
        else
        {
            control.Font = new Font(control.Font.FontFamily,
                   control.Font.Size * factor,
                   control.Font.Style, control.Font.Unit);
        }

    }


    protected virtual void ScaleSpecialControl(Control control, float factor)
    {
        // 暂无实现
    }

    #endregion

    #region 获取当前DPI缩放


    [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
    public static extern int GetDeviceCaps(IntPtr hDC, int nIndex);


    /// <summary>
    /// 见http://pinvoke.net/default.aspx/gdi32/GetDeviceCaps.html
    /// </summary>
    public enum DeviceCap
    {
        /// <summary>
        /// Logical pixels inch in X
        /// </summary>
        LOGPIXELSX = 88,
        /// <summary>
        /// Logical pixels inch in Y
        /// </summary>
        LOGPIXELSY = 90,
        VERTRES = 10,
        DESKTOPVERTRES = 117,

    }


    /// <summary>
    /// 获取缩放值, 使用DeviceCap
    /// </summary>
    /// <returns></returns>
    private float GetScalingFactor()
    {
        using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
        {
            IntPtr desktop = g.GetHdc();
            int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
            int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);


            float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;


            return ScreenScalingFactor;
        }
    }

    /// <summary>
    /// 获取缩放值, 使用ManagementClass
    /// </summary>
    /// <returns></returns>
    private float GetScalingFactorUseMC()
    {
        int PixelsPerXLogicalInch = 0;
        int PixelsPerYLogicalInch = 0;
        using (ManagementClass mc = new ManagementClass("Win32_DesktopMonitor"))
        {
            using (ManagementObjectCollection moc = mc.GetInstances())
            {
                foreach (ManagementObject item in moc)
                {
                    PixelsPerXLogicalInch = int.Parse((item.Properties["PixelsPerXLogicalInch"].Value.ToString()));
                    PixelsPerYLogicalInch = int.Parse((item.Properties["PixelsPerYLogicalInch"].Value.ToString()));
                    break;
                }
            }
        }

        if (PixelsPerXLogicalInch <= 0)
        {
            return 1;
        }

        float factor = PixelsPerXLogicalInch * 1f / 96;

        return factor;
    }

    /// <summary>
    /// 获取缩放值, 使用Graphics.DpiX属性
    /// </summary>
    /// <returns></returns>
    private float GetScaleFactorUseGDI()
    {
        float factor = 1;
        if (this.IsHandleCreated)
        {
            using (Graphics currentGraphics = Graphics.FromHwnd(this.Handle))
            {
                float dpixRatio = currentGraphics.DpiX / 96;
                float dpiyRatio = currentGraphics.DpiY / 96;
                if (dpixRatio > 1)
                {
                    factor = dpixRatio;
                }
            }
        }
        else
        {
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                float dpiX = graphics.DpiX;
                float dpiY = graphics.DpiY;
            }
        }

        return factor;
    }

    /// <summary>
    /// 获取当前的缩放值(SizeF单位)
    /// 备注: 由于几个方法都不准, 多次检查
    /// </summary>
    /// <returns></returns>
    public float GetCurrentScaleFactor()
    {
        float factor = GetScalingFactor();
        // 再次检查
        if (factor == 1)
        {
            factor = GetScalingFactorUseMC();
        }
        // 再次检查
        if (factor == 1)
        {
            factor = GetScaleFactorUseGDI();
        }

        return factor;
    }

    #endregion

    #region 保存控件在设计器中的大小和位置, 根据DPI值进行缩放(弃用)

    protected void SaveControls(Control parent)
    {
        if (parent is Form)
        {
            originalSizes.Add(parent.Name, parent.Size);
        }

        foreach (Control con in parent.Controls)
        {
            if (con.Dock == DockStyle.None)
            {
                originalLocations.Add(con.Name, con.Location);
            }

            originalSizes.Add(con.Name, con.Size);

            if (con.Controls.Count > 0)
            {
                SaveControls(con);
            }
        }
    }

    protected void SetControls(Control parent)
    {
        if (parent is Form)
        {
            var size = originalSizes[parent.Name];

            parent.Width = ScaleInt(size.Width, this.CurrentScaleFactor);
            parent.Height = ScaleInt(size.Height, this.CurrentScaleFactor);
        }
        foreach (Control con in parent.Controls)
        {
            con.Left = ScaleInt(con.Left, this.CurrentScaleFactor);
            con.Top = ScaleInt(con.Top, this.CurrentScaleFactor);

            var size = originalSizes[con.Name];
            con.Width = ScaleInt(size.Width, this.CurrentScaleFactor);
            con.Height = ScaleInt(size.Height, this.CurrentScaleFactor);

            if (con.Controls.Count > 0)
            {
                SetControls(con);
            }
        }
    }

    #endregion

}

