using System;
using System.Linq;
using DevExpress.Mvvm;
using TagLib;

namespace MusicTaggingLight.Models
{
    public class MusicFileTag : BindableBase
    {
        public bool HasChanged { get; set; }

        private string _artist;
        private string _album;
        private string _genre;
        private uint _year;
        private string _title;
        private string _comment;
        private uint _track;
        private string _filePath;
        private string _fileName;

        private byte[] _albumCover;

        /// <summary>
        /// Byte-Array, containing the AlbumCover
        /// </summary>
        public byte[] AlbumCover
        {
            get { return _albumCover; }
            set { SetProperty(ref _albumCover, value, () => AlbumCover); }
        }
        /// <summary>
        /// String, containing the track number of the music file (ID3 Tag)
        /// </summary>
        public uint Track
        {
            get { return _track; }
            set
            {
                SetProperty(ref _track, value, () => Track);
                HasChanged = true;
            }
        }

        /// <summary>
        /// String, containing the artist of the music file (ID3 Tag)
        /// </summary>
        public string Artist
        {
            get { return _artist; }
            set
            {
                SetProperty(ref _artist, value, () => Artist);
                HasChanged = true;
            }
        }

        /// <summary>
        /// String, containing the title of the music file (ID3 Tag)
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                SetProperty(ref _title, value, () => Title);
                HasChanged = true;
            }
        }

        /// <summary>
        /// String, containing the album name of the music file (ID3 Tag)
        /// </summary>
        public string Album
        {
            get { return _album; }
            set
            {
                SetProperty(ref _album, value, () => Album);
                HasChanged = true;
            }
        }

        /// <summary>
        /// String, containing the genre of the music file (ID3 Tag)
        /// </summary>
        public string Genre
        {
            get { return _genre; }
            set
            {
                SetProperty(ref _genre, value, () => Genre);
                HasChanged = true;
            }
        }

        /// <summary>
        /// String, containing the release year of the music file (ID3 Tag)
        /// </summary>
        public uint Year
        {
            get { return _year; }
            set
            {
                SetProperty(ref _year, value, () => Year);
                HasChanged = true;
            }
        }

        /// <summary>
        /// String, containing additional comments of the music file (ID3 Tag)
        /// </summary>
        public string Comment
        {
            get { return _comment; }
            set
            {
                SetProperty(ref _comment, value, () => Comment);
                HasChanged = true;
            }
        }

        /// <summary>
        /// Represents the location on the drive for the music file.
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                SetProperty(ref _filePath, value, () => FilePath);
                HasChanged = true;
            }
        }

        /// <summary>
        /// Contains the file name without path.
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set
            {
                SetProperty(ref _fileName, value, () => FileName);
                HasChanged = true;
            }
        }


        /// <summary>
        /// Checks if the current data is valid.
        /// </summary>
        /// <returns>Resturns true, if current data is valid.</returns>
        public bool IsValid()
        {
            bool isValid = true;
            if (String.IsNullOrEmpty(Artist)
                && String.IsNullOrEmpty(Album)
                && String.IsNullOrEmpty(Title)
                && String.IsNullOrEmpty(Genre)
                && String.IsNullOrEmpty(Comment))
                isValid = false;


            return isValid;
        }

        /// <summary>
        /// Convert a ID3 Tag to an object of MusicFileTag
        /// </summary>
        /// <param name="tag">The ID3 Tag, which has to be converted</param>
        /// <param name="filePath">The path to the location on the drive to the current file</param>
        /// <returns>The converted MusicFileTag object</returns>
        public static MusicFileTag ConvertToMusicFileTag(Tag tag, string filePath)
        {
            var tmp = new MusicFileTag();
            tmp.Artist = tag.FirstPerformer;
            tmp.Album = tag.Album;
            tmp.Genre = tag.FirstGenre;
            tmp.Year = tag.Year;
            tmp.Title = tag.Title;
            tmp.Comment = tag.Comment;
            if (tag.Pictures.Length >= 1)
                tmp.AlbumCover = tag.Pictures?.First().Data.Data;
            tmp.Track = tag.Track;
            tmp.FilePath = filePath;
            tmp.FileName = System.IO.Path.GetFileNameWithoutExtension(filePath);

            return tmp;
        }

        public MusicFileTag ConvertTagToMusicFileTag(Tag tag, string filePath)
        {
            Artist = tag.FirstPerformer;
            Album = tag.Album;
            Genre = tag.FirstGenre;
            Year = tag.Year;
            Title = tag.Title;
            Comment = tag.Comment;
            if (tag.Pictures.Length >= 1)
                AlbumCover = tag.Pictures?.First().Data.Data;
            Track = tag.Track;
            FilePath = filePath;
            FileName = System.IO.Path.GetFileNameWithoutExtension(filePath);

            HasChanged = false;
            return this;
        }

        public static File ConvertMusicFileTagToTag(MusicFileTag musicTag)
        {
            File tagInfo = TagLib.File.Create(musicTag.FilePath);

            tagInfo.Tag.Performers = new string[] { musicTag.Artist ?? "" };       // Sets the FirstPerformer
            tagInfo.Tag.AlbumArtists = new string[] { musicTag.Artist ?? "" };    // Sets the FirstArtist
            tagInfo.Tag.Genres = new string[] { musicTag.Genre ?? "" };           // Sets the FirstGenre
            tagInfo.Tag.Album = musicTag.Album;
            tagInfo.Tag.Title = musicTag.Title;
            tagInfo.Tag.Comment = musicTag.Comment;
            tagInfo.Tag.Year = musicTag.Year;
            tagInfo.Tag.Track = musicTag.Track;

            // Convert byte[] to IPicture[] to save AlbumCover
            // We could use ByteVector.FromPath(""); but since we need the Byte Array anyway for displaying Covers on UI,
            // we can stick to that and save ourselves the work to keep the Path to the image in store
            tagInfo.Tag.Pictures = new IPicture[] { new Picture(new ByteVector(musicTag.AlbumCover)) };
            
            return tagInfo;
        }
    }
}