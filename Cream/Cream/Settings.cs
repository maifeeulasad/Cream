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

namespace Cream
{
    public partial class Settings : Form
    {

        DefaultSetting settings;

        public Settings()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            settings = DefaultSetting.GetDefaultSetting();
            textBoxLocation.Text = settings.Location;
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            Process.Start(settings.Location);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            settings.Save();
            this.Close();
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderPicker = new FolderBrowserDialog();
            if (DialogResult.OK == folderPicker.ShowDialog())
            {
                settings.Location = folderPicker.SelectedPath;
                textBoxLocation.Text = settings.Location;
            }
        }

    }

    public class DefaultSetting : ApplicationSettingsBase
    {
        private static DefaultSetting setting = null;
        public static DefaultSetting GetDefaultSetting()
        {
            if (setting == null)
                setting = new DefaultSetting();
            return setting;
        }

        private DefaultSetting()
        {

        }

        [UserScopedSetting()]
        [DefaultSettingValue("C://")]
        public string Location
        {
            get
            {
                return (string)this["Location"];
            }
            set
            {
                this["Location"] = (string) value;
            }
        }
    }

}
