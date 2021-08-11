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
    public partial class Form3 : DpiScaleForm
    {
        public Form3()
        {
            InitializeComponent();

            // 使用AutoDpiScale需要在设计器中设置AutoScaleMode为AutoScaleMode.Dpi
            // 优先使用系统增强的DPI设置
            this.AutoDpiScale = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("OK");
        }
    }
}
