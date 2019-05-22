using PTLib.CFG;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PileSW
{
    public partial class frmCheckName : Form
    {
        private string input;
        private string regex;
        private ListNameRegEx NomiRegEx;
        public frmCheckName()
        {
            InitializeComponent();
        }

        public frmCheckName(ListNameRegEx NomiRegEx)
        {
            InitializeComponent();
            this.NomiRegEx = NomiRegEx;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            input = textBox1.Text;
            regex = (string)textBox2.SelectedValue;
            // @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
            if (IsValid(input, regex))
                textBox3.Text = "Ok";
            else
                textBox3.Text = "No";
        }
        bool IsValid(string strIn,string Reg)
        {
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn,Reg);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
  Dictionary<string, string> dic = new Dictionary<string, string>();
        private void frmCheckName_Load(object sender, EventArgs e)
        {
            dic = NomiRegEx.GetRegExs();
            textBox2.DataSource = new BindingSource(dic, null);
            textBox2.DisplayMember = "Key";
            textBox2.ValueMember = "Value";

        }

        private void textBox2_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_ValueMemberChanged(object sender, EventArgs e)
        {
            textBox4.Text =(string)textBox2.SelectedValue;
            /*
                        
            if (k is  string)

            else
           textBox3.Text = (( KeyValuePair<String,String>)k).Value;*/
        }
    }
}
