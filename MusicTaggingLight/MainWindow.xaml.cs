using System;
using System.Collections.Generic;
using System.IO;
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
using Ookii.Dialogs.Wpf;

namespace MusicTaggingLight
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = vm = new MainWindowViewModel();
            vm.SelectRootFolderFunc = new Func<string>(SelectRootFolderDialog);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns the root directory of the selected folder</returns>
        private string SelectRootFolderDialog()
        {
            var dialog = new VistaFolderBrowserDialog();
            if (dialog.ShowDialog() == true)
                return dialog.SelectedPath;
            return "";
        }
    }
}