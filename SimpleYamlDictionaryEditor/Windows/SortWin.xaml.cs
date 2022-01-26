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
    /// Interaction logic for SortWin.xaml
    /// </summary>
    public partial class SortWin : Window
    {
        public SortWin(string dirPath)
        {
            InitializeComponent();
            this.Background = MyApp.Background;

            grdSort.Children.Add(SortButton);

            var files = YamlDictionary.DictionaryWithFiles(dirPath);
            if (files.Count == 0)
            {
                lblInfo.Content = "No files found.\nGo back and select folder with yaml file(s).";
                lblInfo.Foreground = Brushes.Red;
            }
            else
            {
                lblInfo.Content = "It'll alphabetically sort and\nsave/replace the selected file.";
            }

            cbYamSort.SelectedValuePath = "Key";
            cbYamSort.DisplayMemberPath = "Value";
            cbYamSort.ItemsSource = files;

        }

        private CustomButton _sortButton;
        private CustomButton SortButton
        {
            get
            {
                if (_sortButton != null)
                    return _sortButton;
                _sortButton = new CustomButton("Sort", "and save");
                _sortButton.btn.Click += SortButton_Click;
                return _sortButton;
            }
        }

        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            if (cbYamSort.SelectedItem is KeyValuePair<string, string>)
            {
                var selectedFile = ((KeyValuePair<string, string>)cbYamSort.SelectedItem).Key;

                var ymlDictionary = YamlDictionary.FileToDictionary(selectedFile);
                if (ymlDictionary.Count == 1)
                {
                    MessageBox.Show("It might have duplicate keys.\n" + "Look for key: " +
                        ymlDictionary.FirstOrDefault().Key, "Invalid file", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var sortedDictionary = new SortedDictionary<string, string>(ymlDictionary);
                var dictionary = new Dictionary<string, string>(sortedDictionary);

                bool success = YamlDictionary.DictionaryToFile(dictionary, selectedFile);
                if (success)
                {
                    MessageBox.Show("File sorted and saved!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Error!", "Fail", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Select file from the combo!", "No file selected", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        
    }
}
