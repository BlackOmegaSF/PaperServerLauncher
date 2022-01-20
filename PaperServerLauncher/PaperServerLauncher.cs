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
using System.Net;
using System.Text.RegularExpressions;

namespace PaperServerLauncher
{
    public partial class ServerLauncher : Form
    {
        public ServerLauncher()
        {
            InitializeComponent();
            cbRamUnits.SelectedIndex = Constants.UNIT_MODE_MB;

            //Load and apply settings
            string settingsFile = Path.Combine(Directory.GetCurrentDirectory(), Constants.SETTINGS_FILE_NAME);
            if (File.Exists(settingsFile))
            {
                loadSettings(settingsFile);
            }

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
            string browseDir = Directory.GetCurrentDirectory();
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
                //btnStartServer.Enabled = false;
                disableControls(this);


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
                    txtPluginStatus.AppendText("\r\nFound valid server jar");
                    serverJarPath = Path.GetFullPath(serverJarPath);
                }
                else if (Directory.Exists(serverJarPath)) //server jar is directory and exists
                {
                    txtPluginStatus.AppendText("\r\nError: Could not find server jar: Path is a directory");
                    txtServerJar.BackColor = Color.Red;
                    return;
                }
                else //server jar doesn't exist
                {
                    txtPluginStatus.AppendText("\r\nError: Could not find server jar");
                    txtServerJar.BackColor = Color.Red;
                    return;
                }

                string pluginsFolder = Path.Combine(new FileInfo(serverJarPath).Directory.FullName, "plugins");

                //Check if min RAM is met
                int minRamAdjusted;
                try
                {
                    minRamAdjusted = Formatters.getMinRam(unitMode);
                } catch (ArgumentOutOfRangeException)
                {
                    txtPluginStatus.AppendText("\r\nError: bad index selected for units");
                    return;
                }

                if (numRAMValue < minRamAdjusted)
                {
                    txtPluginStatus.AppendText("\r\nError: Minimum RAM is " + Formatters.getMinRamString(cbRamUnits.SelectedIndex));
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
                        txtPluginStatus.AppendText("\r\nPlugins directory not found, could not update plugins.");
                        return;
                    } finally
                    {
                        //Clean up created folders/etc and switch back to original working dir
                        string tempDir = Path.Combine(pluginsFolder, "BlackOmegaUpdater");
                        if (Directory.Exists(pluginsFolder) && Directory.Exists(tempDir))
                        {
                            try
                            {
                                Directory.Delete(tempDir, true);
                            } catch (Exception)
                            {
                                //Welp, I tried
                            }
                            
                        }
                        Directory.SetCurrentDirectory(new FileInfo(serverJarPath).Directory.FullName);
                    }
                }

                //Construct server start command string
                StringBuilder startCommandBuilder = new StringBuilder();
                startCommandBuilder.Append("/C java ");
                startCommandBuilder.Append(Formatters.getJVMRamString(unitMode, numRAMValue));
                if (useAikarsFlags)
                {
                    startCommandBuilder.Append(AikarFlagData.getAllFlagsString());
                }
                startCommandBuilder.Append("-jar \"");
                startCommandBuilder.Append(serverJarPath);
                startCommandBuilder.Append("\"");

                //Save settings
                Settings settingsToSave = new Settings(serverJarPath, numRAMValue, unitMode, useAikarsFlags, updatePluginsChecked);
                string settingsJson = JsonConvert.SerializeObject(settingsToSave);
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), Constants.SETTINGS_FILE_NAME), settingsJson);

                //Launch server
                System.Diagnostics.Process.Start("CMD.exe", startCommandBuilder.ToString());
                this.Close();
                
            }
            finally
            {
                //btnStartServer.Enabled = true;
                enableControls(this);
            }



        }

        //Update plugins
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
                string[] existingFiles = Directory.GetFiles(tempDir);
                if (existingFiles.Length > 0)
                {
                    foreach(string file in existingFiles)
                    {
                        try
                        {
                            File.Delete(file);
                        } catch
                        {
                            //Do nothing, I don't feel like chasing this issue
                        }
                    }
                }
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
                                        txtPluginStatus.AppendText("\r\nExtracted " + pluginFileName);
                                    }
                                }
                            }
                        }
                    } catch (IOException)
                    {
                        txtPluginStatus.AppendText("\r\nError: Could not update plugin " + pluginFileName + "\r\n - IOException");
                        continue;
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

                    //Get latest version tag from Github
                    try
                    {
                        RepoInfo info = NetworkUtils.GetLatestRelease(item.owner, item.repo);
                        //Compare versions
                        Version existingVersion = new Version(Regex.Replace(item.version, "[^0-9.]", ""));
                        Version latestVersion = new Version(info.releaseTag);
                        if (existingVersion.CompareTo(latestVersion) <= 0) //Current version is older and needs to be updated
                        {
                            txtPluginStatus.AppendText("\r\nPlugin " + item.id + " is outdated, updating...");

                            //Download updated plugin file
                            string downloadPath = Path.Combine(tempDir, Path.GetFileNameWithoutExtension(updateInfoFile) + ".jar");
                            WebClient webClient = new WebClient();
                            webClient.DownloadFile(info.downloadUrl, downloadPath);
                            //If it downloaded, move the downloaded file to plugins and overwrite
                            if (File.Exists(downloadPath))
                            {
                                try
                                {
                                    string pluginPath = Path.Combine(pluginsFolder, Path.GetFileNameWithoutExtension(updateInfoFile) + ".jar");
                                    if (File.Exists(pluginPath))
                                    {
                                        File.Delete(pluginPath);
                                    }
                                    File.Move(downloadPath, pluginPath);
                                } catch (Exception)
                                {
                                    txtPluginStatus.AppendText("\r\nError writing downloaded plugin " + item.id);
                                }
                            }
                            else
                            {
                                txtPluginStatus.AppendText("\r\nError downloading update for plugin " + item.id);
                                continue;
                            }

                        }
                        else //Plugin is up to date
                        {
                            txtPluginStatus.AppendText("\r\nPlugin " + item.id + " is up to date");
                        }

                    } catch (HttpListenerException e)
                    {
                        txtPluginStatus.AppendText("\r\nHttpError: Could not update plugin " + item.id);
                        Console.WriteLine("Error " + e.ErrorCode.ToString() + ": " + e.Message);
                        continue;
                    } catch (WebException e)
                    {
                        txtPluginStatus.AppendText("\r\nHttpError: Could not update plugin " + item.id);
                        Console.WriteLine(e.StackTrace);
                    }


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

        private void loadSettings(string settingsFile)
        {
            using (StreamReader r = new StreamReader(settingsFile))
            {
                string json = r.ReadToEnd();
                Utils.Settings loadedSettings = JsonConvert.DeserializeObject<Utils.Settings>(json);
                txtServerJar.Text = loadedSettings.serverJarPath;
                numRAM.Value = loadedSettings.ramValue;
                cbRamUnits.SelectedIndex = loadedSettings.unitMode;
                cbxAikarsFlags.Checked = loadedSettings.useAikarFlags;
                cbxUpdatePlugins.Checked = loadedSettings.updatePlugins;
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select Settings File";
            ofd.InitialDirectory = Directory.GetCurrentDirectory();
            ofd.Filter = "JSON Files (*.json)|*.json";
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(ofd.FileName))
                {
                    try
                    {
                        loadSettings(ofd.FileName);
                    } catch (Exception)
                    {
                        txtPluginStatus.AppendText("\r\nError loading settings");
                    }

                } else
                {
                    txtPluginStatus.AppendText("\r\nError: You chose a file that doesn't exist...");
                }
                
            }
        }

        private void disableControls(Control con)
        {
            btnStartServer.Enabled = false;
            menuStrip1.Enabled = false;
            fileToolStripMenuItem.Enabled = false;
            loadToolStripMenuItem.Enabled = false;
            resetToolStripMenuItem.Enabled = false;
            helpToolStripMenuItem.Enabled = false;
            aboutToolStripMenuItem.Enabled = false;
            txtServerJar.Enabled = false;
            btnBrowseJar.Enabled = false;
            grpJavaFlags.Enabled = false;
            numRAM.Enabled = false;
            cbRamUnits.Enabled = false;
            cbxAikarsFlags.Enabled = false;
            grpPlugins.Enabled = false;
            cbxUpdatePlugins.Enabled = false;
        }

        private void enableControls(Control con)
        {
            btnStartServer.Enabled = true;
            menuStrip1.Enabled = true;
            fileToolStripMenuItem.Enabled = true;
            loadToolStripMenuItem.Enabled = true;
            resetToolStripMenuItem.Enabled = true;
            helpToolStripMenuItem.Enabled = true;
            aboutToolStripMenuItem.Enabled = true;
            txtServerJar.Enabled = true;
            btnBrowseJar.Enabled = true;
            grpJavaFlags.Enabled = true;
            numRAM.Enabled = true;
            cbRamUnits.Enabled = true;
            cbxAikarsFlags.Enabled = true;
            grpPlugins.Enabled = true;
            cbxUpdatePlugins.Enabled = true;
            
        }
            
    }
}
