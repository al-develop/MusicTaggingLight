using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using TagLib;

namespace MusicTaggingLight.Models
{
    public class MusicFileTag : BindableBase
    {
        private string _artist;
        private string _album;
        private string _genre;
        private uint _year;
        private string _title;
        private string _comment;
        private uint _track;
        private string _file;

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
            set { SetProperty(ref _track, value, () => Track); }
        }

        /// <summary>
        /// String, containing the artist of the music file (ID3 Tag)
        /// </summary>
        public string Artist
        {
            get { return _artist; }
            set { SetProperty(ref _artist, value, () => Artist); }
        }

        /// <summary>
        /// String, containing the title of the music file (ID3 Tag)
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value, () => Title); }
        }

        /// <summary>
        /// String, containing the album name of the music file (ID3 Tag)
        /// </summary>
        public string Album
        {
            get { return _album; }
            set { SetProperty(ref _album, value, () => Album); }
        }

        /// <summary>
        /// String, containing the genre of the music file (ID3 Tag)
        /// </summary>
        public string Genre
        {
            get { return _genre; }
            set { SetProperty(ref _genre, value, () => Genre); }
        }

        /// <summary>
        /// String, containing the release year of the music file (ID3 Tag)
        /// </summary>
        public uint Year
        {
            get { return _year; }
            set { SetProperty(ref _year, value, () => Year); }
        }

        /// <summary>
        /// String, containing additional comments of the music file (ID3 Tag)
        /// </summary>
        public string Comment
        {
            get { return _comment; }
            set { SetProperty(ref _comment, value, () => Comment); }
        }

        /// <summary>
        /// Represents the location on the drive for the music file.
        /// </summary>
        public string File
        {
            get { return _file; }
            set { SetProperty(ref _file, value, () => File); }
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
        public static MusicFileTag ConvertTagToMusicFileTag(Tag tag, string filePath)
        {
            var tmp = new MusicFileTag();
            tmp.Artist = tag.FirstPerformer;
            tmp.Album = tag.Album;
            tmp.Genre = tag.FirstGenre;
            tmp.Year = tag.Year;
            tmp.Title = tag.Title;
            tmp.Comment = tag.Comment;
            tmp.Track = tag.Track;
            tmp.File = filePath;
            return tmp;
        }

        public static File ConvertMusicFileTagToTag(MusicFileTag musicTag)
        {
            File tagInfo = TagLib.File.Create(musicTag.File);

            //tagInfo.Tag.Clear();
            tagInfo.Tag.Performers = new string[] { musicTag.Artist ?? ""};       // Sets the FirstPerformer
            tagInfo.Tag.AlbumArtists = new string[] { musicTag.Artist ?? "" };    // Sets the FirstArtist
            tagInfo.Tag.Genres = new string[] { musicTag.Genre ?? "" };           // Sets the FirstGenre
            tagInfo.Tag.Album = musicTag.Album;
            tagInfo.Tag.Title = musicTag.Title;
            tagInfo.Tag.Comment = musicTag.Comment;

            tagInfo.Tag.Year = musicTag.Year;
            tagInfo.Tag.Track = musicTag.Track;

            return tagInfo;
        }
    }
}