using SimpleYamlDictionaryEditor.Classes;
using SimpleYamlDictionaryEditor.Controls;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for ManuallyWin.xaml
    /// </summary>
    public partial class ManuallyWin : Window
    {
        public ManuallyWin(string dirPath)
        {
            InitializeComponent();
            this.Background = MyApp.Background;
            this.Title = "Manually edit yaml(s) - two files can be worked on at the same time - side by side";

            grdSaveA.Children.Add(SaveAButton);
            grdSaveB.Children.Add(SaveBButton);

            dtTable = new DataTable();
            dtTable.Columns.Add("Keys");
            dtTable.Columns.Add("File A");
            dtTable.Columns.Add("File B");
            
            dtGrd.ItemsSource = dtTable.DefaultView;
            dtGrd.CanUserAddRows = false;
            dtGrd.Loaded += DtGrd_Loaded;
            dtGrd.Background = Brushes.SkyBlue;

            var files = YamlDictionary.DictionaryWithFiles(dirPath);
            if (files.Count == 0)
            {
                dtTable.Rows.Add("No files found.", "Go back and select folder with yaml file(s).", "");
            }

            cbFileA.SelectedValuePath = "Key";
            cbFileA.DisplayMemberPath = "Value";
            cbFileA.ItemsSource = files;
            cbFileA.SelectionChanged += cbFile_SelectionChanged;

            cbFileB.SelectedValuePath = "Key";
            cbFileB.DisplayMemberPath = "Value";
            cbFileB.ItemsSource = files;
            cbFileB.SelectionChanged += cbFile_SelectionChanged;
        }

        private DataTable dtTable;

        private void cbFile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = (ComboBox)sender;
            if (cb == null)
                return;
            if (e.AddedItems.Count != 1)
                return;

            cb.IsEnabled = false;
            var ymlFile = (KeyValuePair<string, string>)e.AddedItems[0];
            var name = cb.Name;
            if (name == "cbFileA")
            {
                var ymlA = YamlDictionary.FileToDictionary(ymlFile.Key);
                DictionaryToDataTable(ymlA, 1);
            }
            else if (name == "cbFileB")
            {
                var ymlB = YamlDictionary.FileToDictionary(ymlFile.Key);
                DictionaryToDataTable(ymlB, 2);
            }
        }
        private void DictionaryToDataTable(Dictionary<string, string> dictionary, int colIndex)
        {
            foreach (var kv in dictionary)
            {
                var rows = dtTable.AsEnumerable().Where(r => r.Field<string>("Keys") == kv.Key).ToList();
                if (rows.Count() == 0)
                {
                    if (colIndex == 1)
                    {
                        dtTable.Rows.Add(kv.Key, kv.Value, "");
                    }
                    else if (colIndex == 2)
                    {
                        dtTable.Rows.Add(kv.Key, "", kv.Value);
                    }
                }
                else if (rows.Count() == 1)
                {
                    rows[0][colIndex] = kv.Value;
                }
            }
        }

        private void DtGrd_Loaded(object sender, RoutedEventArgs e)
        {
            int i = 0;
            foreach (var c in dtGrd.Columns)
            {
                if (i == 0)
                {
                    c.Width = new DataGridLength(30, DataGridLengthUnitType.Star);
                    c.IsReadOnly = true;
                    c.CanUserSort = true;
                    //c.CanUserResize = true;
                }
                else
                {
                    c.Width = new DataGridLength(35, DataGridLengthUnitType.Star);
                    c.CanUserSort = false;
                    //c.CanUserResize = false;
                }
                c.CanUserReorder = false;
                i++;
            }
        }


        private CustomButton _saveAButton;
        private CustomButton SaveAButton
        {
            get
            {
                if (_saveAButton != null)
                    return _saveAButton;
                _saveAButton = new CustomButton("Save", "File A");
                _saveAButton.btn.Click += SaveAButton_Click;
                return _saveAButton;
            }
        }
        private void SaveAButton_Click(object sender, RoutedEventArgs e)
        {
            if (cbFileA.SelectedItem is KeyValuePair<string, string>)
            {
                GridToFile(cbFileA);
            }
            else
            {
                MessageBox.Show("Select file from the combo!", "No file selected", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private CustomButton _saveBButton;
        private CustomButton SaveBButton
        {
            get
            {
                if (_saveBButton != null)
                    return _saveBButton;
                _saveBButton = new CustomButton("Save", "File B");
                _saveBButton.btn.Click += SaveBButton_Click;
                return _saveBButton;
            }
        }
        private void SaveBButton_Click(object sender, RoutedEventArgs e)
        {
            if (cbFileA.SelectedItem is KeyValuePair<string, string>)
            {
                GridToFile(cbFileB);
            }
            else
            {
                MessageBox.Show("Select file from the combo!", "No file selected", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void GridToFile(ComboBox cb)
        {
            var selectedFile = ((KeyValuePair<string, string>)cb.SelectedItem).Key;
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (DataRow row in dtTable.Rows)
            {
                var vl = cb.Name;
                if (vl == "cbFileA")
                    vl = row.ItemArray[1].ToString();
                else if (vl == "cbFileB")
                    vl = row.ItemArray[2].ToString();
                else
                    vl = "";
                dictionary.Add(row.ItemArray[0].ToString(), vl);
            }
            bool success = YamlDictionary.DictionaryToFile(dictionary, selectedFile);
            if (success)
            {
                MessageBox.Show("File updated!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Error!", "Fail", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
