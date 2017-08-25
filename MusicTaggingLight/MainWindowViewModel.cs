using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Mvvm;
using File = TagLib.File;

namespace MusicTaggingLight
{
    public class MainWindowViewModel : ViewModelBase
    {
        public Func<string> SelectRootFolderFunc { get; set; }
        public Action ExitAction { get; set; }

        public ICommand SelectRootFolderCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand SearchOnlineCommand { get; set; }
        public ICommand ExitCommand { get; set; }


        private ObservableCollection<object> _musicFiles;
        private object _selectedFile;

        public object SelectedFile
        {
            get { return _selectedFile; }
            set { SetProperty(ref _selectedFile, value, () => SelectedFile); }
        }
        public ObservableCollection<object> MusicFiles
        {
            get { return _musicFiles; }
            set { SetProperty(ref _musicFiles, value, () => MusicFiles); }
        }


        public MainWindowViewModel()
        {
            MusicFiles = new ObservableCollection<object>();
            InitCommands();

        }

        private void InitCommands()
        {
            SelectRootFolderCommand = new DelegateCommand(SelectRootFolder);
            SaveCommand = new AsyncCommand(Save);
            SearchOnlineCommand = new AsyncCommand(SearchOnline);
            ExitCommand = new DelegateCommand(() => ExitAction.Invoke());

        }

        private void SelectRootFolder()
        {
            var root = SelectRootFolderFunc.Invoke();
            if (String.IsNullOrEmpty(root))
                return;

            LoadMusicFilesFromRoot(root);
        }

        private Task SearchOnline()
        {
            throw new NotImplementedException();
        }

        private Task Save()
        {
            throw new NotImplementedException();
        }

        private void LoadMusicFilesFromRoot(string root)
        {
            var subfolders = GetSubfolders(root);
            foreach (var folder in subfolders)
            {
                var files = Directory.GetFiles(folder, "*.mp3");
                foreach (var file in files)
                {
                    var tagInfo = TagLib.Mpeg.File.Create(file);
                }
            }
        }

        private IEnumerable<string> GetSubfolders(string sourcePath)
        {
            if (!Directory.Exists(sourcePath))
                return new List<string>();

            IEnumerable<string> subfolders = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories)
                                                      .Where(f => !Directory.EnumerateDirectories(f).Any());
            return subfolders;
        }
    }
}