﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DpiScaleFormDemo
{
    public partial class Form1 : DpiScaleForm
    {
        public Form1()
        {
            InitializeComponent();


            // 窗体是在Winows 96DPI （100%）显示设置下设计的。
            DesignFactor = 1f;
            UseDpiScale = true;
        }
    }
}
