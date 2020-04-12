using System;
using System.IO;
using System.Windows.Input;
using DevExpress.Mvvm;
using MusicTaggingLight.Logic;
using MusicTaggingLight.Models;


namespace MusicTaggingLight.ViewModels
{
    public class DetailViewModel : ViewModelBase
    {
        private MusicFileTag _musicFileTag;
        private bool _placeholderVisible;

        public bool PlaceholderVisible
        {
            get { return _placeholderVisible; }
            set { SetProperty(ref _placeholderVisible, value, () => PlaceholderVisible); }
        }
        public MusicFileTag MusicFileTag
        {
            get { return _musicFileTag; }
            set 
            { 
                SetProperty(ref _musicFileTag, value, () => MusicFileTag);
            }
        }
       
        #region Commands
        public ICommand SelectImageCommand { get; set; }
        #endregion

        public DetailViewModel(MusicFileTag current)
        {
            if (current != null)
            {
                MusicFileTag = current;
                PlaceholderVisible = MusicFileTag.AlbumCover == null;
            }
            else
                PlaceholderVisible = true;

            initCommands();
        }
        private void initCommands()
        {
            SelectImageCommand = new DelegateCommand<string>(this.SelectImage);
        }

        private void SelectImage(string filePath)
        {
            if (String.IsNullOrEmpty(filePath))
                return;

            byte[] selectedImage = File.ReadAllBytes(filePath);
            byte[] resizedImage = ImageResizer.ResizeImage(selectedImage);

            MusicFileTag.AlbumCover = resizedImage;
            MusicFileTag.HasChanged = true;
        }
    }
}
