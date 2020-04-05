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
                    } catch (CorruptFileException e)
                    {
                        Console.WriteLine("error {0} in {1}", e.Message, file);
                        return new Result<List<MusicFileTag>>(file, Status.Error, e);
                    }
                }

            }

            return new Result<List<MusicFileTag>>(musicFileTags);
        }

        private IEnumerable<string> GetSubfolders(string sourcePath)
        {
            if (!Directory.Exists(sourcePath))
                return new List<string>();

            IEnumerable<string> subfolders = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories)
                                                      .Where(f => !Directory.EnumerateDirectories(f).Any());
            return subfolders;
        }

        public Result SaveTagToFile(MusicFileTag tag)
        {
            if (String.IsNullOrEmpty(tag.File))
                return new Result("No File specified", Status.Error);

            File tagInfo = MusicFileTag.ConvertMusicFileTagToTag(tag);
            tagInfo.Save();

            return new Result("", Status.Success);
        }
    }
}
