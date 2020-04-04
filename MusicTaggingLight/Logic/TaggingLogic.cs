using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TagLib;
using File = TagLib.File;

namespace MusicTaggingLight.Logic
{
    public class TaggingLogic
    {
        internal List<MusicFileTag> LoadMusicFilesFromRoot(string root)
        {
            var musicFileTags = new List<MusicFileTag>();
            var foldersList = new List<string>();
            foldersList.Add(root);
            // add subfolders
            foldersList.AddRange(this.GetSubfolders(root));

            foreach (var folder in foldersList)
            {
                var folderContent = Directory.GetFiles(folder, "*.mp3");
                foreach (var file in folderContent)
                {
                    try
                    {
                        var tagInfo = File.Create(file);
                        musicFileTags.Add(MusicFileTag.ConvertTagToMusicFileTag(tagInfo.Tag, tagInfo.Name));
                    } catch (CorruptFileException e)
                    {
                        Console.WriteLine("error {0} in {1}", e.Message, file);
                    }
                }

            }
            
            return musicFileTags;
        }

        private IEnumerable<string> GetSubfolders(string sourcePath)
        {
            if (!Directory.Exists(sourcePath))
                return new List<string>();

            IEnumerable<string> subfolders = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories)
                                                      .Where(f => !Directory.EnumerateDirectories(f).Any());
            return subfolders;
        }

        public void SaveTagToFile(MusicFileTag tag)
        {
            if (String.IsNullOrEmpty(tag.File))
                return;

            File tagInfo = MusicFileTag.ConvertMusicFileTagToTag(tag);
            tagInfo.Save();

        }
    }
}
