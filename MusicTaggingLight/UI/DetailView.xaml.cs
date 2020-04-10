using MusicTaggingLight.Models;
using MusicTaggingLight.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicTaggingLight.UI
{
    /// <summary>
    /// Lógica de interacción para DetailView.xaml
    /// </summary>
    public partial class DetailView : UserControl
    {

        private DetailViewModel dvm;

        public DetailView(DetailViewModel vm)
        {
            InitializeComponent();
            this.DataContext = this.dvm = vm;
        }
    }
}
