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
using System.Windows.Shapes;

namespace Raiders_2._1
{
    /// <summary>
    /// Interaction logic for LindisfarneResult.xaml
    /// </summary>
    public partial class LindisfarneResult : Window
    {

        public LindisfarneResult(string Result)
        {
            InitializeComponent();

            LabelResult.Content = Result;

            if (Result == "Defeat!")
            {
                LabelResult.Content = "Defeat!";
                Image1.Source = new BitmapImage(new Uri("Resources/HersirOverworldDefeated.png", UriKind.Relative));
                Image2.Source = new BitmapImage(new Uri("Resources/HersirOverworldDefeated.png", UriKind.Relative));
            }
        }

        private void ButtonRestartMission_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Lindisfarne = new MainWindow();
            this.Close();
            Lindisfarne.ShowDialog();
        }

        private void ButtonExitGame_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
