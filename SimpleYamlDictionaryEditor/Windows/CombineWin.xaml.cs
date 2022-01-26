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
    /// Interaction logic for CombineWin.xaml
    /// </summary>
    public partial class CombineWin : Window
    {
        public CombineWin(string dirPath)
        {
            InitializeComponent();
            this.Background = MyApp.Background;

            grdReplace.Children.Add(CombineButton);

            var files = YamlDictionary.DictionaryWithFiles(dirPath);
            if (files.Count == 0)
            {
                lblInfo.Content = "No files found.\nGo back and select folder with yaml file(s).";
                lblInfo.Foreground = Brushes.Red;
            }
            else
            {
                lblInfo.Content = "'File A' will replace values from 'File B'\nif it finds equal keys. NO change to files!\nThe result will be stored in NEW file.";
            }

            cbReplaceA.SelectedValuePath = "Key";
            cbReplaceA.DisplayMemberPath = "Value";
            cbReplaceA.ItemsSource = files;

            cbReplaceB.SelectedValuePath = "Key";
            cbReplaceB.DisplayMemberPath = "Value";
            cbReplaceB.ItemsSource = files;

        }


        private CustomButton _combineButton;
        private CustomButton CombineButton
        {
            get
            {
                if (_combineButton != null)
                    return _combineButton;
                _combineButton = new CustomButton("Combine", "and save");
                _combineButton.btn.Click += CombineButton_Click;
                return _combineButton;
            }
        }

        private void CombineButton_Click(object sender, RoutedEventArgs e)
        {
            if (cbReplaceA.SelectedItem is KeyValuePair<string, string> && cbReplaceB.SelectedItem is KeyValuePair<string, string>)
            {
                var selectedFileA = ((KeyValuePair<string, string>)cbReplaceA.SelectedItem).Key;
                var selectedFileB = ((KeyValuePair<string, string>)cbReplaceB.SelectedItem).Key;

                var ymlA = YamlDictionary.FileToDictionary(selectedFileA);
                var ymlB = YamlDictionary.FileToDictionary(selectedFileB);
                var ymlCombined = YamlDictionary.CombineDictionary(ymlA, ymlB);

                string newFile = selectedFileA + "_combined.yml";
                bool success = YamlDictionary.DictionaryToFile(ymlCombined, newFile);
                if (success)
                {
                    MessageBox.Show("'File B' combined with 'File A' into file: \n" + newFile, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
