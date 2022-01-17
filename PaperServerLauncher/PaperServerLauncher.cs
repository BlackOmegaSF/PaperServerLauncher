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
            updateRamUnits(cbRamUnits.SelectedIndex, true);
        }

        private void numRAM_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void updateRamUnits(int unitMode, bool updateNumUpDown)
        {
            ManagementObjectSearcher ramSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");
            UInt64 ramBytes = 0;
            foreach(ManagementObject ramObject in ramSearcher.Get())
            {
                ramBytes += (UInt64)ramObject["Capacity"];
            }
            //Do math, since it's the same no matter the units
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

            //Format based on loaded settings
            //Special case for potatoes
            if (recRam == 0)
            {
                lblCurrentRam.Text = "Running the server is not recommended, not enough RAM!";
                if (updateNumUpDown)
                {
                    numRAM.Value = 0;
                }
                return;
            }
            UInt64 displayRAM = 0;
            UInt64 displayRecRam = 0;
            string ramUnits = "";
            if (unitMode == (int)Utils.Constants.UNIT_MODE_MB)
            {
                ramUnits = "MB";
                displayRAM = ramBytes / (UInt64)Math.Pow(2, 20);
                displayRecRam = recRam / (UInt64)Math.Pow(2, 20);
            } 
            else if (unitMode == (int)Utils.Constants.UNIT_MODE_GB)
            {
                ramUnits = "GB";
                displayRAM = ramBytes / (UInt64)Math.Pow(2, 30);
                displayRecRam = recRam / (UInt64)Math.Pow(2, 30);
            }
            //Set label accordingly
            lblCurrentRam.Text = "Recommend " + displayRecRam.ToString() + ramUnits + " of " + displayRAM.ToString() + ramUnits + " installed";
            if (updateNumUpDown)
            {
                numRAM.Value = displayRecRam;
            }
        }


        //Hide text caret on plugin output text box
        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);
        private void txtPluginStatus_TextChanged(object sender, EventArgs e)
        {
            HideCaret(txtPluginStatus.Handle);
        }

        //Hide text caret on plugin output text box
        private void txtPluginStatus_GotFocus(object sender, EventArgs e)
        {
            HideCaret(txtPluginStatus.Handle);
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

        //Update when changing units
        private void cbRamUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateRamUnits(cbRamUnits.SelectedIndex, false);
            //Calculate and update numRam
            switch (cbRamUnits.SelectedIndex)
            {
                case (int)Utils.Constants.UNIT_MODE_MB:
                    numRAM.Value *= 1024;
                    break;

                case (int)Utils.Constants.UNIT_MODE_GB:
                    numRAM.Value = (numRAM.Value - (numRAM.Value % 1024)) / 1024;
                    break;
            }
        }

        //Start Server button clicked
        private void btnStartServer_Click(object sender, EventArgs e)
        {
            //Check if server jar file exists
            if (File.Exists(txtServerJar.Text))
            {
                //TODO Continue with server starting
            }
            else if (Directory.Exists(txtServerJar.Text))
            {
                if (txtPluginStatus.Text != "")
                {
                    txtPluginStatus.AppendText(Environment.NewLine);
                }
                txtPluginStatus.AppendText("Error: Could not find server jar: Path is a directory");
                txtServerJar.BackColor = Color.Red;
                return;
            }
            else
            {
                if (txtPluginStatus.Text != "")
                {
                    txtPluginStatus.AppendText(Environment.NewLine);
                }
                txtPluginStatus.AppendText("Error: Could not find server jar");
                txtServerJar.BackColor = Color.Red;
                return;
            }

            if (cbxUpdatePlugins.Checked) //Plugins should be checked and updated
            {

            }

        }

        //Clear red color when text changed
        private void txtServerJar_TextChanged(object sender, EventArgs e)
        {
            if (txtServerJar.BackColor == Color.Red)
            {
                txtServerJar.BackColor = Color.White;
            }
        }
    }
}
