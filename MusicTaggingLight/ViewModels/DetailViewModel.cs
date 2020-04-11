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
		private MusicFileTag musicFileTag;

		public MusicFileTag MusicfileTag
		{
			get { return musicFileTag; }
			set 
			{
				SetProperty(ref musicFileTag, value, () => MusicfileTag); 
			}
		}


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
			MusicfileTag = fileTag;

		}
	
		public void Save()
		{
			mwvm.SaveCommand.Execute(MusicfileTag);
		}
	}
}
