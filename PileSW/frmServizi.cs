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
    public partial class frmServizi : Form
    {
        List<PileSwLib.UtilTest.Servizi.servizio> servizi;
        public frmServizi(List<PileSwLib.UtilTest.Servizi.servizio> servizi)
        {
            int r = 0;
            InitializeComponent();
            this.servizi = servizi;
            foreach (var item in servizi)
            {
                dataGridView1.Rows.Add(); 
                dataGridView1.Rows[r].Cells[0].Value = item.DisplayName;
                dataGridView1.Rows[r].Cells[1].Value = item.ServiceName;
                dataGridView1.Rows[r].Cells[2].Value = item.Status; r++;
            }
        }
    }
}
