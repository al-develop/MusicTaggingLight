using MusicTaggingLight.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TagLib;
using File = TagLib.File;

namespace MusicTaggingLight.Logic
{
    public class TaggingLogic
    {

        private string artist = "%artist%";
        private string album = "%album%";
        private string track = "%track%";
        private string year = "%year%";
        private string genre = "%genre%";
        private string title = "%title%";

        internal Result<List<MusicFileTag>> LoadMusicFilesFromRoot(string root)
        {
            var musicFileTags = new List<MusicFileTag>();
            var foldersList = new List<string>();
            foldersList.Add(root);
            // add subfolders
            foldersList.AddRange(this.GetSubfolders(root));

            Regex reg = new Regex(@"^((?!\._).)*$");

            foreach (var folder in foldersList)
            {
                var folderContent = Directory.GetFiles(folder, "*.mp3")
                    .Where(path => reg.IsMatch(path))
                    .ToList();

                foreach (var file in folderContent)
                {
                    try
                    {
                        var tagInfo = File.Create(file);
                        musicFileTags.Add(MusicFileTag.ConvertTagToMusicFileTag(tagInfo.Tag, tagInfo.Name));
                    } 
                    catch (CorruptFileException e)
                    {
                        return HandleError(file, e);
                    }
                }

            }

            return new Result<List<MusicFileTag>>(musicFileTags, Status.Success);
        }

        internal Result<List<MusicFileTag>> LoadMusicFilesFromList(List<string> folderContent)
        {
            var musicFileTags = new List<MusicFileTag>();
            foreach (var file in folderContent)
            {
                try
                {
                    // make sure only *.mp3 files are processed
                    FileInfo info = new FileInfo(file);
                    if (info.Extension != ".mp3")
                        continue;

                    var tagInfo = File.Create(file);
                    musicFileTags.Add(MusicFileTag.ConvertTagToMusicFileTag(tagInfo.Tag, tagInfo.Name));
                }
                catch (CorruptFileException e)
                {
                    return HandleError(file, e);
                }
            }

            return new Result<List<MusicFileTag>>(musicFileTags, Status.Success);
        }

        private static Result<List<MusicFileTag>> HandleError(string file, CorruptFileException e)
        {
            Console.WriteLine("error {0} in {1}", e.Message, file);
            return new Result<List<MusicFileTag>>(file, Status.Error, e);
        }

        private IEnumerable<string> GetSubfolders(string sourcePath)
        {
            if (!Directory.Exists(sourcePath))
                return new List<string>();

            IEnumerable<string> subfolders = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories);
                                                      //.Where(f => !Directory.EnumerateDirectories(f).Any());
            return subfolders;
        }

        public Result SaveTagToFile(MusicFileTag tag)
        {
            if (String.IsNullOrEmpty(tag.FilePath))
                return new Result("No FilePath specified", Status.Error);

            File tagInfo = MusicFileTag.ConvertMusicFileTagToTag(tag);
            tagInfo.Save();
            RenameFile(tag);
            return new Result("", Status.Success);
        }

        
        private void RenameFile(MusicFileTag tag)
        {
            string CurrentFName = System.IO.Path.GetFileNameWithoutExtension(tag.FilePath);
            if (CurrentFName == tag.FileName) return;

            FileInfo currentFile = new FileInfo(tag.FilePath);
            currentFile.MoveTo(currentFile.Directory.FullName + "\\" + tag.FileName + currentFile.Extension);
        }

        public Result SaveTagsExtractedFromFilename(string pattern, MusicFileTag tag)
        {
            List<string> filenameComponents = tag.FileName.Split('-').Select(x => { return x.Trim(); }).ToList();
            var patternComponents = pattern.Split('-').Select(x => { return x.Trim(); }).ToList();

            if (pattern.Contains(artist))
            {
                var index = patternComponents.IndexOf(artist);
                tag.Artist = filenameComponents[index];
            }
            if (pattern.Contains(title))
            {
                var index = patternComponents.IndexOf(title);
                tag.Title = filenameComponents[index];
            }
            if (pattern.Contains(year))
            {
                var index = patternComponents.IndexOf(year);
                tag.Year = Convert.ToUInt32(filenameComponents[index]);
            }
            if (pattern.Contains(album))
            {
                var index = patternComponents.IndexOf(album);
                tag.Album = filenameComponents[index];
            }
            if (pattern.Contains(track))
            {
                var index = patternComponents.IndexOf(track);
                tag.Track = Convert.ToUInt32(filenameComponents[index]);
            }
            if (pattern.Contains(genre))
            {
                var index = patternComponents.IndexOf(genre);
                tag.Genre = filenameComponents[index];
            }

            return SaveTagToFile(tag);
        }
    }
}
