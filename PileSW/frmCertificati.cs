using PileSwLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PileSwLib.UtilTest;
using PileSW;
namespace PileSW
{
    public partial class frmCertificati : Form
    {
        public frmCertificati()
        {
            InitializeComponent();
            dataGridView1.Font = new Font("Courier New", 10);
        }

        private void button1_Click(object sender, EventArgs e)
        {
          // string s= GestCertificate.PrintCertificates(textBox2.Text, System.Security.Cryptography.X509Certificates.StoreLocation.LocalMachine);
            //textBox1.Text = s;
            List<DatiCert> DatiCerts = GestCertificate.PrintCertificates(textBox2.Text, System.Security.Cryptography.X509Certificates.StoreLocation.LocalMachine);
            dataGridView1.Rows.Clear();
            foreach (var item in DatiCerts)
            {
                StringBuilder sb = new StringBuilder();
                dataGridView1.Rows.Add();
                sb.Append("       Name:"); sb.Append(item.Name); sb.AppendLine();
                sb.Append("      Issue:"); sb.Append(item.Issuer); sb.AppendLine();
                sb.Append("    Subject:"); sb.Append(item.Subject); sb.AppendLine();
                sb.Append("    Version:"); 
                sb.Append(item.Version);
                sb.Append(" Valid from "); 
                sb.Append(item.ValidFrom); 
                sb.Append(" to "); 
                sb.Append(item.ValidUntil); sb.AppendLine();
                sb.Append("Serial Num.:"); sb.Append(item.SerialNumber); sb.AppendLine();
                sb.Append(" Sign. Alg.:"); sb.Append(item.SignatureAlgorithm); sb.AppendLine();
                sb.Append(" Thumbprint:"); sb.Append(item.Thumbprint); 
 
                dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells[0].Value=sb.ToString();
              
         
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
