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
using MusicTaggingLight.UI;
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
            vm.ExitAction = new Action(() => Application.Current.Shutdown(0));
            vm.ShowAboutWindowAction = new Action(this.ShowAboutWindow);
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
        private void ShowAboutWindow()
        {
            var about = new AboutWindow();
            this.Opacity = 0.7;
            bool? dialogActive = about.ShowDialog();
            if (dialogActive == false)
                this.Opacity = 1.0;
        }

        private void dgrFileTags_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString().ToLower() == "albumcover")
                e.Cancel = true;
        }


        private void dgrFileTags_DragEnter(object sender, DragEventArgs e)
        {
            // TODO: implement DragDrop
        }

        private void dgrFileTags_Drop(object sender, DragEventArgs e)
        {
            // TODO: implement DragDrop
        }

        private void TxtNotification_ToolTipClosing(object sender, ToolTipEventArgs e)
        {

        }
    }
}