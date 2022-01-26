using SimpleYamlDictionaryEditor.Classes;
using SimpleYamlDictionaryEditor.Controls;
using SimpleYamlDictionaryEditor.Windows;
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

namespace SimpleYamlDictionaryEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// TODO azure translate service
    /// https://akmultilanguages.azurewebsites.net/TranslateApplication
    /// https://github.com/MicrosoftTranslator/Text-Translation-API-V3-C-Sharp-Tutorial
    /// https://docs.microsoft.com/en-us/azure/cognitive-services/translator/translator-how-to-signup
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Background = MyApp.Background;

            #region Directory init
            _dirSelect = new DirectorySelection();
            _dirSelect.ReadFile();
            BrowseSelection browseSelection = new BrowseSelection(BrowseSelection.DialogType.Directory, _dirSelect.SelectedDirectory);
            browseSelection.btn.Click += delegate
            {
                _dirSelect.SelectedDirectory = browseSelection.txt.Text;
                _dirSelect.WriteFile();
            };
            grdBrowse.Children.Add(browseSelection);
            #endregion

            grdSort.Children.Add(SortButton);
            grdReplace.Children.Add(ReplaceButton);
            grdCombine.Children.Add(CombineButton);
            grdManually.Children.Add(ManuallyButton);
            //grdNew.Children.Add(NewButton);

        }

        private DirectorySelection _dirSelect;

        private CustomButton _sortButton;
        private CustomButton SortButton
        {
            get
            {
                if (_sortButton != null)
                    return _sortButton;
                _sortButton = new CustomButton("Sort file", "by keys alphabetically");
                _sortButton.btn.Click += delegate
                {
                    SortWin sortWin = new SortWin(_dirSelect.SelectedDirectory);
                    sortWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    sortWin.ShowDialog();
                };
                return _sortButton;
            }
        }

        private CustomButton _replaceButton;
        private CustomButton ReplaceButton
        {
            get
            {
                if (_replaceButton != null)
                    return _replaceButton;
                _replaceButton = new CustomButton("Replace values", "from one file to another");
                _replaceButton.btn.Click += delegate
                {
                    ReplaceWin replaceWin = new ReplaceWin(_dirSelect.SelectedDirectory);
                    replaceWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    replaceWin.ShowDialog();
                };
                return _replaceButton;
            }
        }

        private CustomButton _manuallyButton;
        private CustomButton ManuallyButton
        {
            get
            {
                if (_manuallyButton != null)
                    return _manuallyButton;
                _manuallyButton = new CustomButton("Manually", "edit 2 files");
                _manuallyButton.btn.Click += delegate
                {
                    ManuallyWin manuallyWin = new ManuallyWin(_dirSelect.SelectedDirectory);
                    manuallyWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    manuallyWin.ShowDialog();
                };
                return _manuallyButton;
            }
        }

        private CustomButton _combineButton;
        private CustomButton CombineButton
        {
            get
            {
                if (_combineButton != null)
                    return _manuallyButton;
                _combineButton = new CustomButton("Combine values", "from two files to new");
                _combineButton.btn.Click += delegate
                {
                    CombineWin combineWin = new CombineWin(_dirSelect.SelectedDirectory);
                    combineWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    combineWin.ShowDialog();
                };
                return _combineButton;
            }
        }

        //**
        private CustomButton _newButton;
        private CustomButton NewButton
        {
            get
            {
                if (_newButton != null)
                    return _manuallyButton;
                _newButton = new CustomButton("New", "new");
                _newButton.btn.Click += delegate
                {
                    ;
                };
                return _newButton;
            }
        }

    }
}
