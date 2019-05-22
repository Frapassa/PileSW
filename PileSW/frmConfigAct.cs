using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PileSW
{
    public partial class frmConfigAct : Form
    {
        private string p;

        public frmConfigAct()
        {
            InitializeComponent();
        }

        public frmConfigAct(string p)
        {
            // TODO: Complete member initialization
            this.p = p;
            InitializeComponent();
        }

        private void frmConfigAct_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = p;
        }

 
    }
}
