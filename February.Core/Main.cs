using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ionic.Zip;
using Microsoft.Win32;

namespace February.Core
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private readonly string gameDir = @"C:\Program Files (x86)\Steam\steamapps\common\Prey"; // Devs: Put your game's directory here

        private void Main_Load(object sender, EventArgs e)
        {
            if (File.Exists(@"./February.Dev.dll"))
            {
                DevOptions_Danielle.Enabled = true;
                DevOptions_Whiplash.Enabled = true;
            }

            if (gameDir != "")
            {
                Environment.CurrentDirectory = gameDir;
            }

            ModList_Danielle.Items.Clear();
            ModList_Whiplash.Items.Clear();
            

            if (!File.Exists("./Binaries/Danielle/x64/Release/PreyDll.dll"))
            {
                MessageBox.Show("Put this file in your Prey install folder, then reopen", "\"We're locked out.\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            if (!File.Exists("./Whiplash/Binaries/Danielle/x64/Release/PreyDll.dll"))
            {
                ModList_Whiplash.Items.Add("Not installed");
            }
            else
            {
                foreach (var variable in Directory.GetFiles("./Whiplash/GameSDK/Precache"))
                {
                    ModList_Whiplash.Items.Add(Path.GetFileName(variable));
                    
                }
                ModList_Whiplash.Enabled = true;
                PlayGame_Whiplash.Enabled = true;
                InstallMod_Whiplash.Enabled = true;
            }

            foreach (var variable in Directory.GetFiles("./GameSDK/Precache"))
            {
                ModList_Danielle.Items.Add(Path.GetFileName(variable));
            }
            ModList_Danielle.Enabled = true;
            PlayGame_Danielle.Enabled = true;
            InstallMod_Danielle.Enabled = true;
        }

        private void ModList_Danielle_SelectedIndexChanged(object sender, EventArgs e)
        {
            FileName_Danielle.Text = ModList_Danielle.Text;
            DeleteFile_Danielle.Enabled = true;
            OpenFile_Danielle.Enabled = true;
            if (ModList_Danielle.Text != "")
            {
                try
                {
                    if (ZipFile.CheckZip(@$"./GameSDK/Precache/{ModList_Danielle.Text}"))
                    {
                        TextBox_Danielle.Text = "Not a packaged file (zip-like), contents can be read.";
                    }
                }
                catch (ZipException)
                {
                    TextBox_Danielle.Text = "Packaged file, contents cannot be read.";
                }
            }
            else
            {
                FileName_Danielle.Text = "Select a file";
            }
        }

        private void ModList_Whiplash_SelectedIndexChanged(object sender, EventArgs e)
        {
            FileName_Whiplash.Text = ModList_Whiplash.Text;
            DeleteFile_Whiplash.Enabled = true;
            OpenFile_Whiplash.Enabled = true;
            try
            {
                if (ZipFile.CheckZip(@$"./Whiplash/GameSDK/Precache/{ModList_Whiplash.Text}"))
                {
                    TextBox_Whiplash.Text = "Not a packaged file (zip-like), contents can be read.";
                }
            }
            catch (ZipException)
            {
                TextBox_Whiplash.Text = "Packaged file, contents cannot be read.";
            }
        }

        private void TabPage_Danielle_Click(object sender, EventArgs e)
        {

        }

        private void DevOptions_Danielle_Click(object sender, EventArgs e)
        {
            Dev.Dev dev = new();
            dev.ShowDialog();
        }

        private void OpenFile_Danielle_Click(object sender, EventArgs e)
        {
            ExploreFile($"./GameSDK/Precache/{ModList_Danielle.Text}");
        }

        private void OpenFile_Whiplash_Click(object sender, EventArgs e)
        {
            ExploreFile($"./Whiplash/GameSDK/Precache/{ModList_Whiplash.Text}");
        }

        public static void ExploreFile(string filePath)
        {
            filePath = Path.GetFullPath(filePath);
            Process.Start("explorer.exe", $"/select,\"{filePath}\"");
        }

        private void DeleteFile_Danielle_Click(object sender, EventArgs e)
        {
            if (ModList_Danielle.Text == "patch.pak")
            {
                if (MessageBox.Show($"Are you sure you want to delete {ModList_Danielle.Text}?\nNote: This file is neccesary for the game to run.", "February", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    File.Delete($"./GameSDK/Precache/{ModList_Danielle.Text}");
                    ModList_Danielle.Items.Remove(ModList_Danielle.Text);
                }
            }
            else
            {
                if (MessageBox.Show($"Are you sure you want to delete {ModList_Danielle.Text}?", "February", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    File.Delete($"./GameSDK/Precache/{ModList_Danielle.Text}");
                    ModList_Danielle.Items.Remove(ModList_Danielle.Text);
                }
            }
        }

        private void DeleteFile_Whiplash_Click(object sender, EventArgs e)
        {
            if (ModList_Whiplash.Text is 
                "patch.pak" or 
                "Campaign.pak" or 
                "Campaign_textures-part0.pak" or 
                "Campaign_textures-part1.pak" or 
                "Campaign_textures-part2.pak" or 
                "Campaign_textures-part3.pak" or 
                "Campaign_textures-part4.pak" or 
                "System.pak" or 
                "System_textures.pak")
            {
                if (MessageBox.Show($"Are you sure you want to delete {ModList_Whiplash.Text}?\nNote: This file is neccesary for the game to run.", "February", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    File.Delete($"./Whiplash/GameSDK/Precache/{ModList_Whiplash.Text}");
                    ModList_Whiplash.Items.Remove(ModList_Whiplash.Text);
                    FileName_Whiplash.Text = "Select a file";
                }
            }
            else
            {
                if (MessageBox.Show($"Are you sure you want to delete {ModList_Whiplash.Text}?", "February", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    File.Delete($"./Whiplash/GameSDK/Precache/{ModList_Whiplash.Text}");
                    ModList_Whiplash.Items.Remove(ModList_Whiplash.Text);
                    FileName_Whiplash.Text = "Select a file";
                }
            }
        }

        private void PlayGame_Danielle_Click(object sender, EventArgs e)
        {
            if (File.Exists(@".\Binaries\Danielle\x64\Release\steam_api64.dll")) // Steam
            {
                RegistryKey? key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam");
                string steamLocation = key.GetValue("DisplayIcon").ToString().Replace(@"\uninstall.exe", "");
                Process.Start($@"{steamLocation}\steam.exe", "steam://rungameid/480490//");
            }
            else // GOG or Epic Games
            {
                Process.Start(@".\Binaries\Danielle\x64\Release\Prey.exe");
            }
            
        }

        private void PlayGame_Whiplash_Click(object sender, EventArgs e)
        {
            if (File.Exists(@".\Binaries\Danielle\x64\Release\steam_api64.dll")) // Steam
            {
                RegistryKey? key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam");
                string steamLocation = key.GetValue("DisplayIcon").ToString().Replace(@"\uninstall.exe", "");
                Process.Start($@"{steamLocation}\steam.exe", "steam://rungameid/480490//-loadFrom=Whiplash");
            }
            else // GOG or Epic Games
            {
                Process.Start(@".\Binaries\Danielle\x64\Release\Prey.exe", "-loadFrom=Whiplash");
            }
        }
    }
}
