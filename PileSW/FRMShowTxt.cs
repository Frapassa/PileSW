using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PileSW
{
    public partial class FRMShowTxt : Form
    {
        string testo;
        public FRMShowTxt(string title,string Testo)
        {
            this.Text = title;
            testo = Testo;
            InitializeComponent();
            string[] linee = Testo.Split('\n');
       
            foreach (string item in linee)
            {
                string[] pezzi = item.Split('\t');

                dgw.Rows.Add();
                int idx = dgw.Rows.Count - 1;
                for (int i = 0; i < pezzi.Count(); i++)
                {
                   dgw.Rows[idx].Cells[i].Value = pezzi[i];  
                }
               
                
            }
             
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            string filename;
            SaveFileDialog sdf = new SaveFileDialog();
            sdf.Filter = "Text File|*.txt|All File|*.*";
            if (sdf.ShowDialog() != DialogResult.OK)
                return;
            filename = sdf.FileName;
            File.WriteAllText(filename, testo);
            this.Close();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FRMShowTxt_Load(object sender, EventArgs e)
        {

        }
    }
}
