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
using static PaperServerLauncher.Utils;
using System.IO.Compression;
using Newtonsoft.Json;

namespace PaperServerLauncher
{
    public partial class ServerLauncher : Form
    {
        public ServerLauncher()
        {
            InitializeComponent();
            cbRamUnits.SelectedIndex = Constants.UNIT_MODE_MB;

            //TODO Load and apply settings

            //Get and update RAM
            updateRamUnits(cbRamUnits.SelectedIndex, true);
        }

        private void numRAM_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            //Clear min ram label when value changes
            lblMinRam.Visible = false;
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
            string displayRAM = "?";
            UInt64 displayRecRam = 0;
            string ramUnits = "";
            if (unitMode == Constants.UNIT_MODE_MB)
            {
                ramUnits = "MB";
                displayRAM = (ramBytes / (UInt64)Math.Pow(2, 20)).ToString();
                displayRecRam = recRam / (UInt64)Math.Pow(2, 20);
            } 
            else if (unitMode == Constants.UNIT_MODE_GB)
            {
                ramUnits = "GB";
                displayRAM = (ramBytes / (UInt64)Math.Pow(2, 30)).ToString();
                displayRecRam = recRam / (UInt64)Math.Pow(2, 30);
            }
            else
            {
                Console.WriteLine("Error: invalid unit mode");
            }
            string displayMinRAM = "?";
            try
            {
                displayMinRAM = Formatters.getMinRamString(unitMode);
            } catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Error: invalid unit mode");
            }
            //Set label accordingly
            lblCurrentRam.Text = "Recommend " + displayRecRam.ToString() + ramUnits + " of " + displayRAM.ToString() + ramUnits + " installed (minimum " + displayMinRAM + ")";
            if (updateNumUpDown)
            {
                numRAM.Value = displayRecRam;
            }
            //Change minimum RAM label units
            StringBuilder minRAMText = new StringBuilder("Minimum RAM is ");
            try
            {
                minRAMText.Append(Formatters.getMinRamString(unitMode));
            } catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Error: invalid unit mode");
                minRAMText.Append("?");
            }
            lblMinRam.Text = minRAMText.ToString();
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
                case Constants.UNIT_MODE_MB:
                    numRAM.Value *= 1024;
                    break;

                case Constants.UNIT_MODE_GB:
                    numRAM.Value = (numRAM.Value - (numRAM.Value % 1024)) / 1024;
                    break;
                default:
                    Console.WriteLine("Error: bad index selected for units");
                    break;
            }
        }

        //Start Server button clicked
        private void btnStartServer_Click(object sender, EventArgs e)
        {
            try
            {
                //Disable start server button
                btnStartServer.Enabled = false;

                //Clear previous output
                txtPluginStatus.Text = "";
                lblMinRam.Visible = false;

                //Grab states of boxes just in case
                string serverJarPath = txtServerJar.Text;
                int unitMode = cbRamUnits.SelectedIndex;
                int numRAMValue = (int)numRAM.Value;
                bool useAikarsFlags = cbxAikarsFlags.Checked;
                bool updatePluginsChecked = cbxUpdatePlugins.Checked;

                //Grab original working dir to restore to when needed
                string originalWorkingDir = Directory.GetCurrentDirectory();

                //Check if server jar file exists
                txtPluginStatus.AppendText("Checking server jar file...");
                if (File.Exists(serverJarPath)) //server jar is file and exists
                {
                    //Convert path to full, non-relative path and continue with server starting
                    txtPluginStatus.AppendText("\nFound valid server jar");
                    serverJarPath = Path.GetFullPath(serverJarPath);
                }
                else if (Directory.Exists(serverJarPath)) //server jar is directory and exists
                {
                    txtPluginStatus.AppendText("\nError: Could not find server jar: Path is a directory");
                    txtServerJar.BackColor = Color.Red;
                    return;
                }
                else //server jar doesn't exist
                {
                    txtPluginStatus.AppendText("\nError: Could not find server jar");
                    txtServerJar.BackColor = Color.Red;
                    return;
                }

                //Check if min RAM is met
                int minRamAdjusted;
                try
                {
                    minRamAdjusted = Formatters.getMinRam(unitMode);
                } catch (ArgumentOutOfRangeException)
                {
                    txtPluginStatus.AppendText("\nError: bad index selected for units");
                    return;
                }

                if (numRAMValue < minRamAdjusted)
                {
                    txtPluginStatus.AppendText("\nError: Minimum RAM is " + Formatters.getMinRamString(cbRamUnits.SelectedIndex));
                    lblMinRam.Visible = true;
                    return;
                }

                if (updatePluginsChecked) //Plugins should be checked and updated
                {
                    try
                    {
                        updatePlugins(new FileInfo(serverJarPath).Directory.FullName);
                    } catch (DirectoryNotFoundException)
                    {
                        txtPluginStatus.AppendText("\nPlugins directory not found, could not update plugins.");
                        return;
                    } finally
                    {
                        //TODO Clean up created folders/etc and switch back to original working dir
                    }
                }

                //TODO Continue starting process

                
            }
            finally
            {
                btnStartServer.Enabled = true;
            }



        }

        private void updatePlugins(string serverDir)
        {
            string pluginsFolder = Path.Combine(serverDir, "plugins");

            //If plugins folder doesn't exist, throw exception
            if (!Directory.Exists(pluginsFolder))
            {
                throw new DirectoryNotFoundException();
            }

            //Set working directory
            Directory.SetCurrentDirectory(pluginsFolder);

            string[] plugins = Directory.GetFiles(pluginsFolder, "*.jar");
            string tempDir = "";
            if(plugins.Length > 0)
            {
                //Create directory to extract into for evaluation
                tempDir = Directory.CreateDirectory("BlackOmegaUpdater").FullName;
            }
            foreach (string plugin in plugins) //Traverse the found jar files and extract info file if present
            {
                if (File.Exists(plugin))
                {
                    string pluginFileName = Path.GetFileNameWithoutExtension(plugin);
                    try
                    {
                        using (ZipArchive pluginArchive = ZipFile.OpenRead(plugin)) //Open the plugin as zip file
                        {
                            foreach (ZipArchiveEntry entry in pluginArchive.Entries) //Traverse each file in the plugin
                            {
                                if (entry.Name.Equals(Constants.UPDATER_INFO_FILE_NAME, StringComparison.OrdinalIgnoreCase)) //Match and ignore case
                                {
                                    string destinationPath = Path.GetFullPath(Path.Combine(tempDir, pluginFileName + ".json"));
                                    if (destinationPath.StartsWith(tempDir, StringComparison.Ordinal))
                                    {
                                        entry.ExtractToFile(destinationPath);
                                        txtPluginStatus.AppendText("\nExtracted " + pluginFileName);
                                    }
                                }
                            }
                        }
                    } catch (IOException)
                    {
                        txtPluginStatus.AppendText("\nError: Could not update plugin " + pluginFileName + "\n - IOException");
                    }
                }
            }
            //Process extracted info files
            string[] updateInfoFiles = Directory.GetFiles(tempDir, "*.json");
            foreach (string updateInfoFile in updateInfoFiles)
            {
                using (StreamReader r = new StreamReader(updateInfoFile))
                {
                    string json = r.ReadToEnd();
                    Utils.UpdateInfoItem item = JsonConvert.DeserializeObject<Utils.UpdateInfoItem>(json);

                    //TODO Check if update is needed
                }
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

        //Clear min ram label when value changes
        private void numRAM_ValueChanged(object sender, EventArgs e)
        {
            lblMinRam.Visible = false;
        }
    }
}
