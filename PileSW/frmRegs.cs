using log4net;
using Microsoft.Win32;
using PileSwLib;
using PileSwLib.UtilTest;
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
    public partial class frmRegs : Form
    {
        static ILog log;
        public frmRegs(ILog _log)
        {
            int t = 0;
            InitializeComponent();
            log = _log;
            Dictionary<int, string> test = new Dictionary<int, string>();
            t++; test.Add(t, @"HKEY_LOCAL_MACHINE\SOFTWARE\wOW6432nODE\SWPosteItaliane\KIT");
            t++; test.Add(t, @"HKEY_LOCAL_MACHINE\SOFTWARE\SWPosteItaliane\KIT");
            t++; test.Add(t, @"HKEY_LOCAL_MACHINE\SOFTWARE\ComputerAssociates\UPMRollupPatch\Office");
            t++; test.Add(t, @"HKEY_LOCAL_MACHINE\SOFTWARE\ComputerAssociates\UPMRollupPatch\OS");
            t++; test.Add(t, @"HKEY_LOCAL_MACHINE\SOFTWARE\McAfee\AVEngine");
            t++; test.Add(t, @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\McAfee\AVEngine");
            t++; test.Add(t, @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\SWPOSTEITALIANE\PDL\");
            t++; test.Add(t, @"HKEY_LOCAL_MACHINE\SOFTWARE\SWPOSTEITALIANE\PDL\");
            t++; test.Add(t, @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings\ZoneMap\Domains\*.cs.poste");
            t++; test.Add(t, @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings\ZoneMap\Domains\*.rete.poste");
            t++; test.Add(t, @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings\ZoneMap\Domains");
            t++; test.Add(t, @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings\ZoneMap\Ranges");
            t++; test.Add(t, @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings\Zones\2");
            t++; test.Add(t, @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings");
            t++; test.Add(t, @"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\New Windows\Allow");
            t++; test.Add(t, @"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main");

            t++; test.Add(t, @"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\IntelliForms");
            t++; test.Add(t, @"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main");


            //Da file cfg
            foreach (var item in PileSwLib.PileSwLib.GetTestCfgRegs)
            {
                t++; test.Add(t,item.Key);
            } 
            textBox1.DataSource = new BindingSource(test, null);
            textBox1.DisplayMember = "Value";
            textBox1.ValueMember = "Key";

           
           
        }
   
        private void ReadRegistry(string chiave)
        {  RegistryKey keyb;
            string baseKey, keys, NSubVal;
   
            try
            {

                keyb = Registri.BaseKey(chiave, out baseKey, out keys, out  NSubVal);
            //
            TreeNode rootNode = new TreeNode(keyb.Name, 0, 1);
            string[] rootSubKeys = keyb.GetSubKeyNames();
            GetValuesAndData(keyb, rootNode);
            foreach (string key in rootSubKeys)
            {
                //get a list of the third level sub keys and their values
                TreeNode node = new TreeNode(key, 0, 1);
                string[] subKeys = keyb.OpenSubKey(key).GetSubKeyNames();
                foreach (string subKeysKey in subKeys)
                {
                    TreeNode subKeyNode = new TreeNode(subKeysKey, 0, 1);
                    GetValuesAndData(keyb.OpenSubKey(key).OpenSubKey(subKeysKey),
                        subKeyNode);

                    node.Nodes.Add(subKeyNode);
                }

                //get the list of values in the second level sub key and their contents
                GetValuesAndData(keyb.OpenSubKey(key), node);

                rootNode.Nodes.Add(node);
            }
            registryTreeView.Nodes.Clear();
            registryTreeView.Nodes.Add(rootNode);

            }
            catch (Exception ex)
            {
                log.Error(ex);
             
            }

;
        }
        /// <summary>
        /// Load all the values and their data into the TreeNode
        /// </summary>
        /// <param name="registryKey">The registry subkey to read from</param>
        /// <param name="node">The TreeNode to load the values in</param>
        private static void GetValuesAndData(RegistryKey registryKey, TreeNode node)
        {
            string[] values = registryKey.GetValueNames();
            foreach (string value in values)
            {
                object data = registryKey.GetValue(value);

                if (data != null)
                {
                    string stringData = data.ToString();

                    //if the data is too long, display the begining only
                    if (stringData.Length > 50)
                        stringData = stringData.Substring(0, 46) + " ...";

                    //Display the data of the value. The conditional operatore is
                    //needed because the default value has no name
                    node.Nodes.Add(value, (value == "" ? "Default" : value) +
                        ": " + stringData, 2, 2);
                }
                else
                {
                    //Display <empty> if the value is empty
                    node.Nodes.Add(value, (value == "" ? "Default" : value) +
                        ": <empty>", 2, 2);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {           
            // Get combobox selection (in handler)
            string value ;//= ((KeyValuePair<string, string>)textBox1.SelectedItem).Value;
            value = textBox1.Text;
            ReadRegistry(value);

         

        }

        private void frmRegs_Load(object sender, EventArgs e)
        {

        }
        //per alligneare la combo a dx
        private void textBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            StringAlignment Allinea = StringAlignment.Far;
            // By using Sender, one method could handle multiple ComboBoxes
            ComboBox cbx = sender as ComboBox;
            if (cbx != null)
            {
                // Always draw the background
                e.DrawBackground();

                // Drawing one of the items?
                if (e.Index >= 0)
                {
                    // Set the string alignment.  Choices are Center, Near and Far
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = Allinea;
                    sf.Alignment = Allinea;

                    // Set the Brush to ComboBox ForeColor to maintain any ComboBox color settings
                    // Assumes Brush is solid
                    Brush brush = new SolidBrush(cbx.ForeColor);

                    // If drawing highlighted selection, change brush
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                        brush = SystemBrushes.HighlightText;

                    // Draw the string
                    e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, brush, e.Bounds, sf);
                }
            }

        }

        private void registryTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}
