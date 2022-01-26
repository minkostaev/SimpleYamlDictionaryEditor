using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleYamlDictionaryEditor.Controls
{
    /// <summary>
    /// Interaction logic for BrowseSelection.xaml
    /// </summary>
    public partial class BrowseSelection : UserControl
    {
        public enum DialogType { Directory, File }
        
        public BrowseSelection(DialogType dialogType, string defaultPath = "")
        {
            InitializeComponent();

            if (dialogType == DialogType.Directory)
            {
                txt.IsReadOnly = true;
                txt.Text = defaultPath == null ? "" : defaultPath;
                btn.Click += delegate { BrowseDirectory(); };
            }
        }

        private void BrowseDirectory()
        {
            // Create a "Save As" dialog for selecting a directory (HACK)
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.InitialDirectory = txt.Text; // Use current value for initial dir
            dialog.Title = "Select a Directory"; // instead of default "Save As"
            dialog.Filter = "Directory|*.this.directory"; // Prevents displaying files
            dialog.FileName = "select"; // Filename will then be "select.this.directory"
            if (dialog.ShowDialog() == true)
            {
                string path = dialog.FileName;
                // Remove fake filename from resulting path
                path = path.Replace("\\select.this.directory", "");
                path = path.Replace(".this.directory", "");
                // If user has changed the filename, create the new directory
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                // Our final value is in path
                txt.Text = path;
            }
        }

        //if (this.Dialog is System.Windows.Forms.FolderBrowserDialog)
        //{
        //    System.Windows.Forms.FolderBrowserDialog dirDlg = (System.Windows.Forms.FolderBrowserDialog)this.Dialog;
        //    if (dirDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //    {
        //        txt.Text = dirDlg.SelectedPath;
        //    }
        //}
        //else if (this.Dialog is Microsoft.Win32.OpenFileDialog)
        //{
        //    Microsoft.Win32.OpenFileDialog opnDlg = (Microsoft.Win32.OpenFileDialog)this.Dialog;
        //    opnDlg.Filter = this.FileExtensions;
        //    if (opnDlg.ShowDialog() == true)
        //    {
        //        txt.Text = opnDlg.FileName;
        //    }
        //}
        //else if (this.Dialog is System.Windows.Forms.OpenFileDialog)
        //{
        //    System.Windows.Forms.OpenFileDialog opnDlg = (System.Windows.Forms.OpenFileDialog)this.Dialog;
        //    opnDlg.Filter = this.FileExtensions;

        //    System.Windows.Forms.DialogResult result = opnDlg.ShowDialog();
        //    if (result == System.Windows.Forms.DialogResult.OK)
        //    {
        //        txt.Text = opnDlg.FileName;
        //    }
        //}

    }
}
