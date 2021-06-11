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
    public partial class Form1 : DpiScaleForm
    {
        public Form1()
        {
            InitializeComponent();

            this.UseDpiScale = true;
        }
    }
}
