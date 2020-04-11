# MusicTaggingLight
This is a small tool, which helps to tag mp3 Files. 
You select a root Folder and all \*.mp3 song files are loaded in the UI.
There you can edit the ID3 Tags through the DataGrid or the PropertyGrid.
For saving the edited data, just click the button or use the Ctrl+S shortcut.

![preview](https://github.com/jvegaf/MusicTaggingLight/blob/master/musictagginglight.png)
![preview](https://github.com/jvegaf/MusicTaggingLight/blob/master/musictagginglight-edit.png)

# For Developers - what to contribute?
This project is far from being finished. I just implemented the basic stuff for tagging \*.mp3 files.
Some ideas to make this tool a bit better are:
- implementing a "auto-tagging through online search" functionality. For this, I want to implement a search through "freedb" and maybe other APIs/WebServices.
- reading tags from CD Text
- removing the "save" button and save the tags while editing. Like- you edit a tag and when you go over to the enxt one, the just edited tag is already saved.
But it would definitely need the functionality to rollback everything, is something goes wrong.
- support for more file types. Currently, only \*.mp3 files are supported. It would be a more useful tool, if there would be the possibility to edit other files as well.

# Used Libraries
- DevExpress.Mvvm (NuGet)
- Ookii.Dialogs (NuGet)
- taglib-sharp (NuGet)
- Xceed.WPFToolkit (NuGet)
- Adonis UI Toolkit (NuGet)
- icons8 for Icons (https://icons8.com/)
