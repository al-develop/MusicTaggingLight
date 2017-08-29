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
            var subfolders = this.GetSubfolders(root);

            // add tags of files from root
            var rootFiles = Directory.GetFiles(root, "*.mp3");
            foreach (var file in rootFiles)
            {
                var tagInfo = File.Create(file);
                musicFileTags.Add(MusicFileTag.ConvertTagToMusicFileTag(tagInfo.Tag, tagInfo.Name));
            }

            foreach (var folder in subfolders)
            {
                var subfolderFiles = Directory.GetFiles(folder, "*.mp3");

                // add tags of files from subfolders
                foreach (var file in subfolderFiles)
                {
                    var tagInfo = File.Create(file);
                    musicFileTags.Add(MusicFileTag.ConvertTagToMusicFileTag(tagInfo.Tag, tagInfo.Name));
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
