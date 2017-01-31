using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoundariesTileEditor
{
    public partial class Form2 : Form
    {
        public Form1 form1;
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            form1.newIndex = int.Parse(textBox1.Text);
            
            this.Close();
        }
    }
}
