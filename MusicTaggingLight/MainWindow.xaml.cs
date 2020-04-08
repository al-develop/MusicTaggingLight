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
        private readonly MainWindowViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = this.DataContext as MainWindowViewModel;
            vm.SelectRootFolderFunc = new Func<string>(SelectRootFolderDialog);
            vm.ExitAction = new Action(() => Application.Current.Shutdown(0));
            vm.ShowAboutWindowAction = new Action(this.ShowAboutWindow);
            vm.ShowFNExtWindowAction = new Action(this.ShowFNExtrWindow);
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
        private void ShowFNExtrWindow()
        {
            var dialog = new FromFNWindow(this.vm);
            this.Opacity = 0.7;
            bool? dialogActive = dialog.ShowDialog();
            if (dialogActive == false)
                this.Opacity = 1.0;
        }

        private void dgrFileTags_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString().ToLower() == "albumcover")
                e.Cancel = true;
        }

        private void dgrFileTags_Drop(object sender, DragEventArgs e)
        {
            vm.ClearCommand.Execute(null);

            string[] data = (string[])e.Data.GetData(nameof(DataFormats.FileDrop));

            if (!CheckForMp3(data))
                return;

            var directories = new List<string>();
            var files = new List<string>();

            // Validate, if dropped data are files or directories.
            // Both have different approaches how to handle the input.
            foreach (var d in data)
            {
                if (IsDirectory(d))
                    directories.Add(d);
                else
                    files.Add(d);
            }

            if (directories.Count > 0)
                vm.DragDropDirectory(directories);

            if (files.Count > 0)
                vm.DragDropFiles(files);
        }

        /// <summary>
        /// Check if a given path is a directory or a file
        /// </summary>
        /// <param name="path">string, which has to be validated</param>
        /// <returns>true if the param is a directory, false if it's a file</returns>
        private bool IsDirectory(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            return (attr & FileAttributes.Directory) == FileAttributes.Directory;
        }

        private bool CheckForMp3(string[] data)
        {
            foreach (var d in data)
            {
                var info = new FileInfo(d);
                if (info.Extension == ".mp3")
                {
                    return true;
                }
            }

            vm.SetNotification("Only *.mp3 files supported!", "Orange");
            return false;
        }
    }
}