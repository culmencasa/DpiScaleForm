using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DpiScaleFormDemo
{
    public partial class Form2 : DpiScaleForm
    {
        public Form2()
        {
            InitializeComponent();


            // 窗体是在Winows 192DPI （200%）显示设置下设计的。
            DesignFactor = 2f;
            UseDpiScale = true;
        }
    }
}
