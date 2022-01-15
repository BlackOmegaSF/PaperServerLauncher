using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Runtime.InteropServices;
using System.IO;

namespace PaperServerLauncher
{
    public partial class ServerLauncher : Form
    {
        public ServerLauncher()
        {
            InitializeComponent();
            cbRamUnits.SelectedIndex = 1;

            //TODO Load and apply settings

            //Get and update RAM
            updateInstalledRamLabel(cbRamUnits.SelectedIndex);
        }

        private void numRAM_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void updateInstalledRamLabel(int unitMode)
        {
            ManagementObjectSearcher ramSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");
            UInt64 ramBytes = 0;
            foreach(ManagementObject ramObject in ramSearcher.Get())
            {
                ramBytes += (UInt64)ramObject["Capacity"];
            }
            //DEBUG
            ramBytes = (UInt64)(16 * Math.Pow(2, 30));
            //Doo math, since it's the same no matter the units
            UInt64 recRam;
            //If you have less than 4GB, recommend none
            if (ramBytes < (4 * Math.Pow(2, 30)))
            {
                recRam = 0;
            }
            else if (ramBytes < (20 * Math.Pow(2, 30)))
            {
                recRam = (UInt64)(ramBytes - (2 * Math.Pow(2, 30)));
            }
            else
            {
                recRam = (UInt64)(16 * Math.Pow(2, 30));
            }

            //TODO format based on loaded settings
            //Special case for potatoes
            if (recRam == 0)
            {
                lblCurrentRam.Text = "Running the server is not recommended, not enough RAM!";
                numRAM.Value = 0;
                return;
            }
            UInt64 displayRAM = 0;
            UInt64 displayRecRam = 0;
            string ramUnits = "";
            if (unitMode == Utils.Constants.UNIT_MODE_MB)
            {
                ramUnits = "MB";
                displayRAM = ramBytes / (UInt64)Math.Pow(2, 20);
                displayRecRam = recRam / (UInt64)Math.Pow(2, 20);
            } 
            else if (unitMode == Utils.Constants.UNIT_MODE_GB)
            {
                ramUnits = "GB";
                displayRAM = ramBytes / (UInt64)Math.Pow(2, 30);
                displayRecRam = recRam / (UInt64)Math.Pow(2, 30);
            }
            //Set label accordingly
            lblCurrentRam.Text = "Recommend " + displayRecRam.ToString() + ramUnits + " of " + displayRAM.ToString() + ramUnits + " installed";
            numRAM.Value = displayRecRam;
        }


        //Hide text caret on plugin output text box
        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            HideCaret(textBox1.Handle);
        }

        //Hide text caret on plugin output text box
        private void textBox1_GotFocus(object sender, EventArgs e)
        {
            HideCaret(textBox1.Handle);
        }


        //Handle browsing for server jar
        private void btnBrowseJar_Click(object sender, EventArgs e)
        {
            //Browse to dir based on what's in the box, if it's valid
            string browseDir = AppDomain.CurrentDomain.BaseDirectory;
            string extgText = txtServerJar.Text;
            if (File.Exists(extgText) || Directory.Exists(extgText))
            {
                browseDir = extgText;
            }
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select Server Jar";
            ofd.InitialDirectory = browseDir;
            ofd.Filter = "Jar Files (*.jar)|*.jar";
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtServerJar.Text = ofd.FileName;
            }
        }
    }
}
