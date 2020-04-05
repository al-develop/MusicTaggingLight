using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Mvvm;
using MusicTaggingLight.Logic;
using TagLib;
using File = TagLib.File;

namespace MusicTaggingLight
{
    public class MainWindowViewModel : ViewModelBase
    {
        public TaggingLogic Logic { get; set; }
        public Func<string> SelectRootFolderFunc { get; set; }
        public Action ExitAction { get; set; }

        public ICommand SelectRootFolderCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand SearchOnlineCommand { get; set; }
        public ICommand ExitCommand { get; set; }


        private ObservableCollection<MusicFileTag> _musicFileTags;
        private MusicFileTag _selectedFile;
        private string _rootPath;

        public string RootPath
        {
            get { return _rootPath; }
            set { SetProperty(ref _rootPath, value, () => RootPath); }
        }
        public MusicFileTag SelectedFile
        {
            get { return _selectedFile; }
            set { SetProperty(ref _selectedFile, value, () => SelectedFile); }
        }
        public ObservableCollection<MusicFileTag> MusicFileTags
        {
            get { return _musicFileTags; }
            set { SetProperty(ref _musicFileTags, value, () => MusicFileTags); }
        }

        public MainWindowViewModel()
        {
            MusicFileTags = new ObservableCollection<MusicFileTag>();
            Logic = new TaggingLogic();
            InitCommands();
        }

        private void InitCommands()
        {
            SelectRootFolderCommand = new DelegateCommand(SelectRootFolder);
            SaveCommand = new DelegateCommand(Save);
            SearchOnlineCommand = new AsyncCommand(SearchOnline);
            ExitCommand = new DelegateCommand(() => ExitAction.Invoke());

        }

        private void SelectRootFolder()
        {
            RootPath = SelectRootFolderFunc.Invoke();
            if (String.IsNullOrEmpty(RootPath))
                return;
            
            List<MusicFileTag> loaded = Logic.LoadMusicFilesFromRoot(RootPath);
            MusicFileTags = new ObservableCollection<MusicFileTag>(loaded.Where(w => w.IsValid()));
        }

        private Task SearchOnline()
        {
            // not implemented yet
            return null;
        }

        private void Save()
        {
            foreach (var tag in MusicFileTags)
            {
                Logic.SaveTagToFile(tag);
            }
        }
    }
}