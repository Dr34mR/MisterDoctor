using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using EngineDoctor.Helpers;
using MisterDoctor.Helpers;
using MisterDoctor.Plugins;
using MisterDoctor.Plugins.Classes;
using MisterDoctor.Properties;

// ReSharper disable LocalizableElement
// ReSharper disable PossibleNullReferenceException

namespace MisterDoctor.Forms
{
    public partial class FormSettings : Form
    {
        private readonly Dictionary<string, Form> _formCache = new();

        private const string AboutPage = "~ About";

        public FormSettings()
        {
            InitializeComponent();
        }

        private Plugin Plugin { get; set; }

        public void Setup(Plugin plugin)
        {
            Icon = Resources.favicon;
            Text = $"{plugin.Name} {plugin.Version} by {plugin.Author}";

            StartPosition = FormStartPosition.CenterParent;

            MaximumSize = Size;

            MinimizeBox = false;
            MaximizeBox = false;

            Plugin = plugin;

            FormBorderStyle = FormBorderStyle.FixedDialog;

            var nameColumn = new DataGridViewTextBoxColumn
            {
                Name = nameof(Setting.Name),
                HeaderText = nameof(Setting.Name)
            };

            gridSetting.BorderStyle = BorderStyle.FixedSingle;

            gridSetting.Columns.Add(nameColumn);

            gridSetting.Columns[nameof(Setting.Name)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            gridSetting.ReadOnly = true;

            gridSetting.RowHeadersVisible = false;
            gridSetting.ColumnHeadersVisible = false;

            gridSetting.AllowUserToAddRows = false;
            gridSetting.AllowUserToDeleteRows = false;
            gridSetting.AllowUserToOrderColumns = false;
            gridSetting.AllowUserToResizeColumns = false;
            gridSetting.AllowUserToResizeRows = false;

            gridSetting.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            gridSetting.CellBorderStyle = DataGridViewCellBorderStyle.None;

            gridSetting.SelectionChanged += DataGrid_SelectionChanged;
            gridSetting.BackgroundColor = Color.White;
            gridSetting.BorderStyle = BorderStyle.Fixed3D;

            // Insert 'About' Page

            var aboutRow = new DataGridViewRow();
            aboutRow.Cells.Add(new DataGridViewTextBoxCell
            {
                Value = AboutPage
            });
            gridSetting.Rows.Add(aboutRow);

            // Now the settings

            foreach (var setting in plugin.Settings)
            {
                var newRow = new DataGridViewRow();

                newRow.Cells.Add(new DataGridViewTextBoxCell
                {
                    Value = setting.Name
                });

                gridSetting.Rows.Add(newRow);
            }

            FormClosing += Form_FormClosing;

            CancelButton = btnClose;

            btnClose.Click += btnClose_Click;
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnloadForms();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DataGrid_SelectionChanged(object sender, EventArgs e)
        {
            var currentRow = gridSetting.CurrentRow;
            var settingName = (string) currentRow.Cells[nameof(Setting.Name)].Value;

            var isAboutPage = settingName == AboutPage;
            var setting = Plugin.Settings.FirstOrDefault(i => i.Name == settingName);

            pnlFormContainer.Controls.Clear();

            Form formToLoad;
            var needShow = false;

            var keyName = isAboutPage ? AboutPage : setting.Name;

            if (_formCache.ContainsKey(keyName))
            {
                formToLoad = _formCache[keyName];
            }
            else
            {
                if (isAboutPage)
                {
                    var aboutPage = new FormSettingAbout(Plugin);
                    aboutPage.ExportSettings += Plugin_ExportSettings;
                    aboutPage.ImportSettings += Plugin_ImportSettigns;
                    formToLoad = aboutPage;
                }
                else
                {
                    formToLoad = SettingFormFactory.CreateForm(setting);
                }

                formToLoad.TopLevel = false;
                formToLoad.AutoScroll = true;
                formToLoad.FormBorderStyle = FormBorderStyle.None;
                formToLoad.Dock = DockStyle.Fill;
                _formCache.Add(keyName, formToLoad);
                needShow = true;
            }
            
            pnlFormContainer.Controls.Add(formToLoad);
            if (needShow) formToLoad.Show();
        }

        private void UnloadForms()
        {
            var allPairs = _formCache.Where(i => i.Key != AboutPage).ToList();

            foreach (var form in allPairs)
            {
                _formCache.Remove(form.Key);
            }

            var allForms = allPairs.Select(i => i.Value).ToList();
            
            foreach (var form in allForms)
            {
                switch (form)
                {
                    case FormSettingString stringForm:
                    {
                        stringForm.Setting.ValueString = stringForm.ReturnValue;
                        break;
                    }
                    case FormSettingStringList listForm:
                    {
                        listForm.Setting.ValueStringList = listForm.ReturnValue;
                        break;
                    }                    
                    case FormSettingKeyValue keyForm:
                    {
                        keyForm.Setting.ValueKeyValues = keyForm.ReturnValue;
                        break;
                    }
                    case FormSettingInt intForm:
                    {
                        intForm.Setting.ValueInt = intForm.ReturnValue;
                        break;
                    }
                }
                form.Dispose();
            }
        }

        private void Plugin_ExportSettings(object sender, EventArgs e)
        {
            using var saveFileDialog = new SaveFileDialog
            {
                AddExtension = true,
                DefaultExt = "settings",
                Filter = "Settings File (*.settings)|*.settings",
                OverwritePrompt = true,
                FileName = Plugin.Name,
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                CreatePrompt = true
            };

            var result = saveFileDialog.ShowDialog(this);

            if (result != DialogResult.OK) return;

            var fileName = saveFileDialog.FileName;
            if (string.IsNullOrEmpty(fileName)) return;

            UnloadForms();

            var outSettings = Plugin.CleanSettings();

            var jsonText = JsonSerializer.Serialize(outSettings, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                File.WriteAllText(fileName, jsonText);

                MessageBox.Show(this, "Save Successful", Plugin.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show(this, "Error Saving", Plugin.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Plugin_ImportSettigns(object sender, EventArgs e)
        {
            using var openFileDialog = new OpenFileDialog
            {
                AddExtension = true,
                DefaultExt = "settings",
                Filter = "Settings File (*.settings)|*.settings",
                FileName = Plugin.Name,
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                CheckFileExists = true,
                Multiselect = false,
                ValidateNames = true
            };

            var result = openFileDialog.ShowDialog(this);

            if (result != DialogResult.OK) return;

            var fileName = openFileDialog.FileName;
            if (string.IsNullOrEmpty(fileName)) return;
            if (!File.Exists(fileName)) return;

            UnloadForms();

            try
            {
                var fileText = File.ReadAllText(fileName);
                var settingsObj = JsonSerializer.Deserialize<Settings>(fileText);

                settingsObj = Plugin.CleanSettings(settingsObj);

                Plugin.LoadSettings(settingsObj);

                MessageBox.Show(this, "Load Successful", Plugin.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show(this, "Error Loading", Plugin.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
