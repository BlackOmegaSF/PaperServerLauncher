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
            cbRamUnits.SelectedIndex = 0;
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
            //TODO do math based on loaded settings
            //TODO Set label accordingly
        }

        //Hide text caret on plugin output text box
        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            HideCaret(textBox1.Handle);
        }

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
