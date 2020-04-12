using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Mvvm;
using MusicTaggingLight.Logic;
using MusicTaggingLight.Models;
using MusicTaggingLight.ViewModels;

namespace MusicTaggingLight
{
    public class MainWindowViewModel : ViewModelBase
    {
        public TaggingLogic Logic { get; set; }

        #region UI Delegates
        public Func<string> SelectRootFolderFunc { get; set; }
        public Action ExitAction { get; set; }
        public Action ShowAboutWindowAction { get; set; }
        public Action ShowFNExtWindowAction { get; set; }
        public Action CloseFNExtWindowAction { get; set; }
        public Action ClearSelectionAction { get; set; }

        #endregion UI Delegates


        #region Commands
        public ICommand SelectRootFolderCommand { get; set; }
        public ICommand<MusicFileTag> SaveCommand { get; set; }
        public ICommand SearchOnlineCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand OpenAboutCommand { get; set; }
        public ICommand TagFromFileNameCommand { get; set; }
        public ICommand SaveFromFNCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand ClearSelectionCommand { get; set; }

        #endregion Commands


        #region View Properties
        private ObservableCollection<DetailViewModel> _musicFileTags;
        private ObservableCollection<DetailViewModel> _selectedItems;
        private DetailViewModel _selectedItem;
        private string _rootPath;
        private string _fileNamePattern;
        private string _resultPreview;
        private string _notificationText;
        private string _notificationColor;
        private double _DetailColumnWidth;
        private bool _detailsVisible;

        public bool DetailsVisible
        {
            get { return _detailsVisible; }
            set { SetProperty(ref _detailsVisible, value, () => DetailsVisible); }
        }
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
        public string FileNamePattern
        {
            get { return _fileNamePattern; }
            set { SetProperty(ref _fileNamePattern, value, () => FileNamePattern); }
        }
        public string ResultPreview
        {
            get { return _resultPreview; }
            set { SetProperty(ref _resultPreview, value, () => ResultPreview); }
        }
        public ObservableCollection<DetailViewModel> SelectedItems
        {
            get { return _selectedItems; }
            set { SetProperty(ref _selectedItems, value, () => SelectedItems); }
        }
        public DetailViewModel SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value, () => SelectedItem); }
        }
        public double DetailColumnWidth
        {
            get { return _DetailColumnWidth; }
            set { SetProperty(ref _DetailColumnWidth, value, () => DetailColumnWidth); }
        }
        public ObservableCollection<DetailViewModel> MusicFileTags
        {
            get { return _musicFileTags; }
            set { SetProperty(ref _musicFileTags, value, () => MusicFileTags); }
        }
        #endregion View Properties

        public MainWindowViewModel()
        {
            MusicFileTags = new ObservableCollection<DetailViewModel>();
            SelectedItems = new ObservableCollection<DetailViewModel>();
            Logic = new TaggingLogic();
            DetailColumnWidth = 0;
            DetailsVisible = false;
            InitCommands();
            SetDefaultNotification();
        }
        private void InitCommands()
        {
            SelectRootFolderCommand = new AsyncCommand(SelectRootFolder);
            SaveCommand = new DelegateCommand<MusicFileTag>(Save);
            SearchOnlineCommand = new AsyncCommand(SearchOnline);
            TagFromFileNameCommand = new DelegateCommand(this.TagFromFilename);
            SaveFromFNCommand = new DelegateCommand(this.SaveFromFN);
            ExitCommand = new DelegateCommand(() => ExitAction.Invoke());
            OpenAboutCommand = new DelegateCommand(this.OpenAbout);
            ClearCommand = new DelegateCommand(this.ClearList);
            ClearSelectionCommand = new DelegateCommand(() => ClearSelectionAction.Invoke());
        }


        #region Command Implementation
        private async Task SelectRootFolder()
        {
            this.ClearList();

            RootPath = SelectRootFolderFunc.Invoke();
            if (String.IsNullOrEmpty(RootPath))
            {
                SetNotification("Root path not set", "Red");
                return;
            }

            SetDefaultNotification();
            Result<List<MusicFileTag>> loadedFiles = await Task.Run(() => Logic.LoadMusicFilesFromRoot(RootPath));
            ProcessLoadedMusicFiles(loadedFiles);
        }

        private void Save(MusicFileTag musicFileTag)
        {
            var resultList = new List<Result>();
            foreach (var tag in MusicFileTags)
            {
                //only save changed music files.
                if (tag.MusicFileTag.HasChanged)
                { 
                    resultList.Add(Logic.SaveTagToFile(tag.MusicFileTag));
                    tag.MusicFileTag.HasChanged = false;
                }                
            }

            if (resultList.Any(a => a.Status != Status.Success))
                SetNotification("Error occured!", "Red"); // TODO: Log Errors
            else
                SetNotification("Tags saved", "Green");
        }

        private void SaveFromFN()
        {
            CloseFNExtWindowAction.Invoke();

            foreach (var item in SelectedItems)
            {
                Result result = Logic.SaveTagsExtractedFromFilename(FileNamePattern, item.MusicFileTag);
                if (result.Status == Status.Success)
                    SetNotification("Tags saved", "Green");
                else
                    SetNotification(result.Message, "Red");
            }
            SelectedItems.Clear();
        }

        private void TagFromFilename()
        {
            if (SelectedItems.Count < 1)
                return;
            FileNamePattern = "%artist%-%title%";
            ResultPreview = SelectedItems.FirstOrDefault().MusicFileTag.FileName;
            ShowFNExtWindowAction.Invoke();
        }

        public void SelectionChanged(System.Collections.IList items)
        {
            SelectedItems.Clear();
            foreach (DetailViewModel item in items)
            {
                SelectedItems.Add(item);
            }
            // if select only one file we show the details
            if (SelectedItems.Count.Equals(1))
            {
                DetailColumnWidth = 400;
                DetailsVisible = true;
            }
            else
            {
                DetailColumnWidth = 1;
                DetailsVisible = false;
                SelectedItem = null;
            }
        }

        private Task SearchOnline()
        {
            // not implemented yet
            return null;
        }

        private void ClearList()
        {
            this.MusicFileTags.Clear();
            this.RootPath = string.Empty;
            SetDefaultNotification();
        }

        private void OpenAbout()
        {
            ShowAboutWindowAction.Invoke();
        }
        #endregion Command Implementation


        #region Notification Methods
        private void SetDefaultNotification()
        {
            this.SetNotification("Ready", "Green");
        }

        public void SetNotification(string text, string color)
        {
            this.NotificationText = text;
            this.NotificationColor = color;
        }

        #endregion Notification Methods


        #region DragDrop Handler
        internal void DragDropFiles(List<string> musicFiles)
        {
            Result<List<MusicFileTag>> loadedFiles = Logic.LoadMusicFilesFromList(musicFiles);
            ProcessLoadedMusicFiles(loadedFiles);
            SetDefaultNotification();
        }

        internal async Task DragDropDirectory(List<string> directories)
        {
            Result<List<MusicFileTag>> loadedFiles = new Result<List<MusicFileTag>>(new List<MusicFileTag>());
            foreach (var dir in directories)
            {
                Result<List<MusicFileTag>> filesFromDirectory = await Task.Run(() => Logic.LoadMusicFilesFromRoot(dir));
                loadedFiles.Data.AddRange(filesFromDirectory.Data);
            }
            ProcessLoadedMusicFiles(loadedFiles);
            SetDefaultNotification();
        }
        #endregion DragDrop Handler

        private void ProcessLoadedMusicFiles(Result<List<MusicFileTag>> loadedFiles)
        {
            if (loadedFiles.Status != Status.Success)
            {
                SetNotification($"Error {loadedFiles.Exception.Message} in {loadedFiles.Message}", "Red");
                return;
            }

            foreach (var tag in loadedFiles.Data)
            {
                if (tag.IsValid())
                {
                    MusicFileTags.Add(new DetailViewModel(tag));
                }
            }
        }
    }
}