using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DpiScaleFormDemo
{
    class DemoContxt : ApplicationContext
    {
        public DemoContxt()
        {
            new Form1().Show();

            new Form2().Show();
        }
    }
}
