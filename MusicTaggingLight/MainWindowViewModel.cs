using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Mvvm;
using MusicTaggingLight.Models;
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
        internal Action ShowAboutWindowAction { get; set; }


        public ICommand SelectRootFolderCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand SearchOnlineCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand OpenAboutCommand { get; set; }


        private ObservableCollection<MusicFileTag> _musicFileTags;
        private MusicFileTag _selectedFile;
        private string _rootPath;
        private string _notificationText;
        private string _notificationColor;

        public string NotificationColor
        {
            get { return _notificationColor; }
            set { SetProperty(ref _notificationColor, value, () => NotificationColor); }
        }
        public string NotificationText
        {
            get { return _notificationText; }
            set { SetProperty(ref _notificationText, value, () => NotificationText); }
        }
        public string RootPath
        {
            get { return _rootPath; }
            set { SetProperty(ref _rootPath, value, () => RootPath); }
        }
        public MusicFileTag SelectedFile
        {
            get { return _selectedFile; }
            set 
            { 
                SetProperty(ref _selectedFile, value, () => SelectedFile);                
            }
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
            SetDefaultNotification();
        }

        private void InitCommands()
        {
            SelectRootFolderCommand = new DelegateCommand(SelectRootFolder);
            SaveCommand = new DelegateCommand(Save);
            SearchOnlineCommand = new AsyncCommand(SearchOnline);
            ExitCommand = new DelegateCommand(() => ExitAction.Invoke());
            OpenAboutCommand = new DelegateCommand(this.OpenAbout);
        }

        private void OpenAbout()
        {
            ShowAboutWindowAction.Invoke();
        }

        private void SelectRootFolder()
        {
            RootPath = SelectRootFolderFunc.Invoke();
            if (String.IsNullOrEmpty(RootPath))
            {
                SetNotification("Root path not set", "Red");
                return;
            }

            SetDefaultNotification();

            Result<List<MusicFileTag>> loadedFiles = Logic.LoadMusicFilesFromRoot(RootPath);
            if(loadedFiles.Status != Status.Success)
            {
                SetNotification($"Error {loadedFiles.Exception.Message} in {loadedFiles.Message}", "Red");
                return;
            }

            SetDefaultNotification();
            MusicFileTags = new ObservableCollection<MusicFileTag>(loadedFiles.Data.Where(w => w.IsValid()));
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
                Result result = Logic.SaveTagToFile(tag);
                if(result.Status == Status.Success)
                    SetNotification("Tags saved", "Green");
                else
                    SetNotification(result.Message, "Red");
            }
        }

        private void SetDefaultNotification()
        {
            this.SetNotification("Ready", "Green");
        }

        private void SetNotification(string text, string color)
        {
            this.NotificationText = text;
            this.NotificationColor = color;
        }
    }
}