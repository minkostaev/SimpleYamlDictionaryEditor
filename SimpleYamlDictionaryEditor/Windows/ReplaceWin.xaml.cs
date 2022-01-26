using SimpleYamlDictionaryEditor.Classes;
using SimpleYamlDictionaryEditor.Controls;
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
using System.Windows.Shapes;

namespace SimpleYamlDictionaryEditor.Windows
{
    /// <summary>
    /// Interaction logic for ReplaceWin.xaml
    /// </summary>
    public partial class ReplaceWin : Window
    {
        public ReplaceWin(string dirPath)
        {
            InitializeComponent();
            this.Background = MyApp.Background;

            grdReplace.Children.Add(ReplaceButton);

            var files = YamlDictionary.DictionaryWithFiles(dirPath);
            if (files.Count == 0)
            {
                lblInfo.Content = "No files found.\nGo back and select folder with yaml file(s).";
                lblInfo.Foreground = Brushes.Red;
            }
            else
            {
                lblInfo.Content = "'File A' will replace values from 'File B'\nif it finds equal keys.\n'File B' will be changed, but 'File A' not!";
            }

            cbReplaceA.SelectedValuePath = "Key";
            cbReplaceA.DisplayMemberPath = "Value";
            cbReplaceA.ItemsSource = files;

            cbReplaceB.SelectedValuePath = "Key";
            cbReplaceB.DisplayMemberPath = "Value";
            cbReplaceB.ItemsSource = files;

        }

        private CustomButton _replaceButton;
        private CustomButton ReplaceButton
        {
            get
            {
                if (_replaceButton != null)
                    return _replaceButton;
                _replaceButton = new CustomButton("Replace", "and save");
                _replaceButton.btn.Click += ReplaceButton_Click;
                return _replaceButton;
            }
        }

        private void ReplaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (cbReplaceA.SelectedItem is KeyValuePair<string, string> && cbReplaceB.SelectedItem is KeyValuePair<string, string>)
            {
                var selectedFileA = ((KeyValuePair<string, string>)cbReplaceA.SelectedItem).Key;
                var selectedFileB = ((KeyValuePair<string, string>)cbReplaceB.SelectedItem).Key;

                var ymlA = YamlDictionary.FileToDictionary(selectedFileA);
                var ymlB = YamlDictionary.FileToDictionary(selectedFileB);
                int missingKeys = YamlDictionary.ReplaceDictionary(ymlA, ymlB);
                string msg = string.Empty;
                if (missingKeys > 0)
                {
                    msg = "\nMissing keys not applied: " + missingKeys.ToString();
                }
                bool success = YamlDictionary.DictionaryToFile(ymlB, selectedFileB);
                if (success)
                {
                    MessageBox.Show("'File B' updated with 'File A' values and saved!" + msg, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Error!", "Fail", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Select file(s) from the combo!", "No file(s) selected", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    
    }
}
