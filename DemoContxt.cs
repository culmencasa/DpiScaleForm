using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DpiScaleFormDemo
{
    class DemoContxt : ApplicationContext
    {
        Queue<int> forms = new Queue<int>();

        public DemoContxt()
        {
            var form1 = new Form1();
            var form2 = new Form2();
            var form3 = new Form3();
            forms.Enqueue(1);
            forms.Enqueue(2);
            forms.Enqueue(3);

            form1.FormClosed += Form_FormClosed;
            form2.FormClosed += Form_FormClosed;
            form3.FormClosed += Form_FormClosed;

            form1.Show();
            form2.Show();
            form3.Show();

        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            forms.Dequeue();
            if (forms.Count == 0)
            {
                ExitThread();
            }
        }
    }
}
