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
        private MediaPlayer ResultMusic = new MediaPlayer();
        private void MusicLoop(object sender, EventArgs e)
        {
            ResultMusic.Position = TimeSpan.Zero;
            ResultMusic.Play();
        }

        public LindisfarneResult(string Result)
        {
            InitializeComponent();

            LabelResult.Content = Result;
            ResultMusic.MediaEnded += new EventHandler(MusicLoop);
            ResultMusic.Open(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "GameResources/ThemeVictory.wav"));

            if (Result == "Defeat!")
            {
                LabelResult.Content = "Defeat!";
                Image1.Source = new BitmapImage(new Uri("Resources/HersirOverworldDefeated.png", UriKind.Relative));
                Image2.Source = new BitmapImage(new Uri("Resources/HersirOverworldDefeated.png", UriKind.Relative));
                ResultMusic.Open(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "GameResources/ThemeDefeat.wav"));
            }
            ResultMusic.Play();
        }

        private void ButtonRestartMission_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Lindisfarne = new MainWindow();
            ResultMusic.Stop();
            this.Close();
            Lindisfarne.ShowDialog();
        }
    }
}
