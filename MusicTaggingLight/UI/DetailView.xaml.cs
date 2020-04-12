using System.Windows.Controls;
using System.Windows.Input;
using MusicTaggingLight.ViewModels;
using Ookii.Dialogs.Wpf;

namespace MusicTaggingLight.UI
{
    /// <summary>
    /// Code Behind for DetailView.xaml
    /// </summary>
    public partial class DetailView : UserControl
    {
        private DetailViewModel _vm;
        public DetailView()
        {
            InitializeComponent();            
        }

        private void GridAlbumCover_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var dialog = new VistaOpenFileDialog();
            dialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.gif;*.bmp) | *.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if ((bool)!dialog.ShowDialog()) // nothing selected
                return;

            _vm = this.DataContext as DetailViewModel;
            _vm.SelectImageCommand.Execute(dialog.FileName);
        }
    }
}