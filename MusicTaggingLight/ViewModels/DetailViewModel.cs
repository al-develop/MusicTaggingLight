using DevExpress.Mvvm;
using MusicTaggingLight.Logic;
using MusicTaggingLight.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicTaggingLight.ViewModels
{
    public class DetailViewModel : ViewModelBase
    {
		private MainWindowViewModel mwvm;
		private MusicFileTag mfTag;

		#region View Properties

		private byte[] _albumCover;
		private uint _track;
		private uint _year;
		private string _album;
		private string _genre;
		private string _title;
		private string _artist;
		private string _fileName;
		private string _comment;


		public byte[] AlbumCover
		{
			get { return _albumCover; }
			set { SetProperty(ref _albumCover, value, () => AlbumCover); }
		}
        public uint Track
		{
			get { return _track; }
			set { SetProperty(ref _track, value, () => Track); }
		}
		public uint Year
		{
			get { return _year; }
			set { SetProperty(ref _year, value, () => Year); }
		}
		public string Album
		{
			get { return _album; }
			set { SetProperty(ref _album, value, () => Album); }
		}
		public string Genre
		{
			get { return _genre; }
			set { SetProperty(ref _genre, value, () => Genre); }
		}
		public string Title
		{
			get { return _title; }
			set { SetProperty(ref _title, value, () => Title); }
		}
		public string Artist
		{
			get { return _artist; }
			set { SetProperty(ref _artist, value, () => Artist); }
		}
		public string FileName
		{
			get { return _fileName; }
			set { SetProperty(ref _fileName, value, () => FileName); }
		}
		public string Comment
		{
			get { return _comment; }
			set { SetProperty(ref _comment, value, () => Comment); }
		}
        #endregion

        #region Commands

		public ICommand SaveTagsCommand { get; set; }

        #endregion

		private void initCommands()
		{
			SaveTagsCommand = new DelegateCommand(this.Save);
		}


        public DetailViewModel(MainWindowViewModel parent)
		{
			this.mwvm = parent;
			initCommands();
		}

		public void SetItemToShow(MusicFileTag fileTag)
		{
			mfTag = fileTag;

			Title = mfTag.Title;
			Artist = mfTag.Artist;
			Album = mfTag.Album;
			Track = mfTag.Track;
			AlbumCover = mfTag.AlbumCover;
			Year = mfTag.Year;
			Genre = mfTag.Genre;
			FileName = mfTag.FileName;
			Comment = mfTag.Comment;
		}
	
		public void Save()
		{
			

			mfTag.Title = Title;
			mfTag.Artist = Artist;
			mfTag.Album = Album;
			mfTag.Track = Track;
			mfTag.AlbumCover = AlbumCover;
			mfTag.Year = Year;
			mfTag.Genre = Genre;
			mfTag.FileName = FileName;
			mfTag.Comment = Comment;

			mwvm.SaveCommand.Execute(mfTag);
		}
	}
}
