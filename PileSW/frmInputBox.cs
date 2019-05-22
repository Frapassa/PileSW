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
    public partial class frmInputBox : Form
    {
        public string txtInput;
        public frmInputBox(string messaggio,string title)
        {
            InitializeComponent();
            this.Text = title;
            this.label1.Text = messaggio;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            txtInput = textBox1.Text;
        }
    }
}
