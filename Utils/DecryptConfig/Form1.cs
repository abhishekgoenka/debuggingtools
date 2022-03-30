using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DecryptConfig
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnDecrypt_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
                {
                    InitialDirectory = "c:\\",
                    Filter = @"Config files (*.exe)|*.exe|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = true
                };
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    try
                    {
                        // Open the configuration file and retrieve the connectionStrings section.
                        Configuration configuration = ConfigurationManager.OpenExeConfiguration(fileDialog.FileName);
                        ConfigurationSection configSection =
                        configuration.GetSection(CboSections.SelectedItem.ToString()) as ConnectionStringsSection ??
                        (ConfigurationSection)(configuration.GetSection(CboSections.SelectedItem.ToString()) as AppSettingsSection);

                        //AppSettingsSection configSection = configuration.GetSection("appSettings") as AppSettingsSection;

                        if ((!(configSection.ElementInformation.IsLocked)) &&
                            (!(configSection.SectionInformation.IsLocked)))
                        {
                            if (!configSection.SectionInformation.IsProtected)
                            {
                                //this line will encrypt the file
                                configSection.SectionInformation.ProtectSection
                                    ("DataProtectionConfigurationProvider");
                            }

                            if (configSection.SectionInformation.IsProtected)//encrypt is true so encrypt
                            {
                                //this line will decrypt the file. 
                                configSection.SectionInformation.UnprotectSection();
                            }
                            //re-save the configuration file section
                            configSection.SectionInformation.ForceSave = true;
                            // Save the current configuration

                            configuration.Save();
                            Process.Start("notepad.exe", configuration.FilePath);
                            //configFile.FilePath 
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }  
                }
                catch (Exception ex)
                {
                    MessageBox.Show(@"Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

                    
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CboSections.Items.Add("appSettings");

            CboSections.SelectedIndex = 0;
        }
    }
}
