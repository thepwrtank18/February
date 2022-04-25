using System.Diagnostics;
using Ionic.Zip;
using Microsoft.Win32;
// ReSharper disable StringLiteralTypo
// ReSharper disable CommentTypo
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable InconsistentNaming
#pragma warning disable CS8602

namespace February.Core
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private string? gameDir;

        private void Main_Load(object sender, EventArgs e)
        {
            // basic checks
            switch (Environment.Is64BitOperatingSystem)
            {
                case false:
                    MessageBox.Show("Invalid architecture", "\"We're locked out.\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                    break;
            }

            if (File.Exists(@"./February.Dev.dll"))
            {
                DevOptions_Danielle.Enabled = true;
                DevOptions_Whiplash.Enabled = true;
            }

            try
            {
                gameDir = File.ReadAllText("./dir.txt");

                if (!string.IsNullOrWhiteSpace(gameDir))
                {
                    if (Directory.Exists(gameDir))
                    {
                        Environment.CurrentDirectory = gameDir;
                    }
                }
            }
            catch (FileNotFoundException)
            {

            }

            if (!File.Exists("./Binaries/Danielle/x64/Release/PreyDll.dll"))
            {
                MessageBox.Show("Put files in your Prey install folder, then reopen", "\"We're locked out.\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            ModList_Danielle.Items.Clear();
            ModList_Whiplash.Items.Clear();

            // real checks
            if (!File.Exists("./Whiplash/Binaries/Danielle/x64/Release/PreyDll.dll"))
            {
                ModList_Whiplash.Items.Add("Not installed");
            }
            else
            {
                foreach (string file in Directory.GetFiles("./Whiplash/GameSDK/Precache"))
                {
                    if (Path.GetFileName(file) is not ("patch.pak" or
                        "Campaign.pak" or
                        "Campaign_textures-part0.pak" or
                        "Campaign_textures-part1.pak" or
                        "Campaign_textures-part2.pak" or
                        "Campaign_textures-part3.pak" or
                        "Campaign_textures-part4.pak" or
                        "System.pak" or
                        "System_textures.pak"))
                    {
                        ModList_Whiplash.Items.Add(Path.GetFileName(file).Replace(".pak", "").Replace("ZZZZZ_ModLoad_", ""));
                    }
                }

                switch (ModList_Whiplash.Items.Count)
                {
                    case 0:
                        ModList_Whiplash.Enabled = false;
                        ModList_Whiplash.Items.Add("No mods installed");
                        break;
                    default:
                        ModList_Whiplash.Enabled = true;
                        break;
                }

                PlayGame_Whiplash.Enabled = true;
                InstallMod_Whiplash.Enabled = true;
            }

            foreach (string file in Directory.GetFiles("./GameSDK/Precache"))
            {
                if (Path.GetFileName(file) is not "patch.pak")
                {
                    ModList_Danielle.Items.Add(Path.GetFileName(file).Replace(".pak", "").Replace("ZZZZZ_ModLoad_", ""));
                }
            }

            switch (ModList_Danielle.Items.Count)
            {
                case 0:
                    ModList_Danielle.Enabled = false;
                    ModList_Danielle.Items.Add("No mods installed");
                    break;
                default:
                    ModList_Danielle.Enabled = true;
                    break;
            }

            PlayGame_Danielle.Enabled = true;
            InstallMod_Danielle.Enabled = true;
        }

        private void ModList_Danielle_SelectedIndexChanged(object sender, EventArgs e)
        {
            FileName_Danielle.Text = ModList_Danielle.Text;
            DeleteFile_Danielle.Enabled = true;
            OpenFile_Danielle.Enabled = true;
            RenameFile_Danielle.Enabled = true;
            if (ModList_Danielle.Text != "")
            {
                try
                {
                    if (ZipFile.CheckZip(@$"./GameSDK/Precache/ZZZZZ_ModLoad_{ModList_Danielle.Text}.pak"))
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
                DeleteFile_Danielle.Enabled = false;
                OpenFile_Danielle.Enabled = false;
                RenameFile_Danielle.Enabled = false;
            }
        }

        private void ModList_Whiplash_SelectedIndexChanged(object sender, EventArgs e)
        {
            FileName_Whiplash.Text = ModList_Whiplash.Text;
            DeleteFile_Whiplash.Enabled = true;
            OpenFile_Whiplash.Enabled = true;
            RenameFile_Whiplash.Enabled = true;
            if (ModList_Whiplash.Text != "")
            {
                try
                {
                    if (ZipFile.CheckZip(@$"./Whiplash/GameSDK/Precache/ZZZZZ_ModLoad_{ModList_Whiplash.Text}.pak"))
                    {
                        TextBox_Whiplash.Text = "Not a packaged file (zip-like), contents can be read.";
                    }
                }
                catch (ZipException)
                {
                    TextBox_Whiplash.Text = "Packaged file, contents cannot be read.";
                }
                catch (FileNotFoundException)
                {
                    TextBox_Whiplash.Text = "Unmanaged file. Can cause problems with load order.";
                }
            }
            else
            {
                FileName_Whiplash.Text = "Select a file";
                DeleteFile_Whiplash.Enabled = false;
                OpenFile_Whiplash.Enabled = false;
                RenameFile_Whiplash.Enabled = false;
            }
        }

        private void TabPage_Danielle_Click(object sender, EventArgs e)
        {

        }

        private void DevOptions_Danielle_Click(object sender, EventArgs e)
        {
            
        }

        private void OpenFile_Danielle_Click(object sender, EventArgs e)
        {
            if (!File.Exists($"./GameSDK/Precache/ZZZZZ_ModLoad_{ModList_Danielle.Text}.pak"))
            {
                Process.Start("explorer", Path.GetFullPath("./GameSDK/Precache"));
            }
            else
            {
                ExploreFile($"./GameSDK/Precache/ZZZZZ_ModLoad_{ModList_Danielle.Text}.pak");
            }
        }

        private void OpenFile_Whiplash_Click(object sender, EventArgs e)
        {
            if (!File.Exists($"./Whiplash/GameSDK/Precache/ZZZZZ_ModLoad_{ModList_Whiplash.Text}.pak"))
            {
                Process.Start("explorer", Path.GetFullPath("./Whiplash/GameSDK/Precache"));
            }
            else
            {
                ExploreFile($"./Whiplash/GameSDK/Precache/ZZZZZ_ModLoad_{ModList_Whiplash.Text}.pak");
            }
        }

        public static void ExploreFile(string filePath)
        {
            filePath = Path.GetFullPath(filePath);
            Process.Start("explorer.exe", $"/select,\"{filePath}\"");
        }

        private void DeleteFile_Danielle_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to delete {ModList_Danielle.Text}?", "February", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                File.Delete($"./GameSDK/Precache/ZZZZZ_ModLoad_{ModList_Danielle.Text}.pak");
                ModList_Danielle.Items.Remove(ModList_Danielle.Text);
                FileName_Danielle.Text = "Select a file";
                TextBox_Danielle.Text = "";
                if (ModList_Danielle.Items.Count == 0)
                {
                    ModList_Danielle.Enabled = false;
                    ModList_Danielle.Items.Add("No mods installed");
                }
            }
        }

        private void DeleteFile_Whiplash_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to delete {ModList_Whiplash.Text}?", "February", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                File.Delete($"./Whiplash/GameSDK/Precache/ZZZZZ_ModLoad_{ModList_Whiplash.Text}.pak");
                ModList_Whiplash.Items.Remove(ModList_Whiplash.Text);
                FileName_Whiplash.Text = "Select a file";
                TextBox_Whiplash.Text = "";
                if (ModList_Whiplash.Items.Count == 0)
                {
                    ModList_Whiplash.Enabled = false;
                    ModList_Whiplash.Items.Add("No mods installed");
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

        private void InstallMod_Danielle_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Package file|*.pak";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                InstallMod installMod = new();
                installMod.Mod = openFileDialog.FileName;
                installMod.Type = 0;
                if (installMod.ShowDialog() == DialogResult.OK)
                {
                    ModList_Danielle.Items.Add(installMod.NewName);
                    ModList_Danielle.Enabled = true;
                    ModList_Danielle.Items.Remove("No mods installed");
                }
            }
        }

        private void InstallMod_Whiplash_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Package file|*.pak";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                InstallMod installMod = new();
                installMod.Mod = openFileDialog.FileName;
                installMod.Type = 1;
                if (installMod.ShowDialog() == DialogResult.OK)
                {
                    ModList_Whiplash.Items.Add(installMod.NewName);
                    ModList_Whiplash.Enabled = true;
                    ModList_Whiplash.Items.Remove("No mods installed");
                }
            }
        }

        private void RenameFile_Danielle_Click(object sender, EventArgs e)
        {
            RenameMod renameMod = new();
            renameMod.Mod = ModList_Danielle.Text;
            renameMod.Type = 0;
            if (renameMod.ShowDialog() == DialogResult.OK)
            {
                ModList_Danielle.Items.Remove(ModList_Danielle.Text);
                ModList_Danielle.Items.Add(renameMod.NewName);
            }
        }

        private void RenameFile_Whiplash_Click(object sender, EventArgs e)
        {
            RenameMod renameMod = new();
            renameMod.Mod = ModList_Whiplash.Text;
            renameMod.Type = 1;
            if (renameMod.ShowDialog() == DialogResult.OK)
            {
                ModList_Whiplash.Items.Remove(ModList_Whiplash.Text);
                ModList_Whiplash.Items.Add(renameMod.NewName);
            }
        }
    }
}
