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
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        private MediaPlayer MainMenuMusic = new MediaPlayer();
        private void MusicLoop(object sender, EventArgs e)
        {
            MainMenuMusic.Position = TimeSpan.Zero;
            MainMenuMusic.Play();
        }

        public MainMenu()
        {
            InitializeComponent();

            MainMenuMusic.MediaEnded += new EventHandler(MusicLoop);
            MainMenuMusic.Open(new Uri("C:/Users/User/Desktop/Projects/C#/Raiders2/ThemeMainMenu.wav"));
            MainMenuMusic.Play();
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            MainMenuMusic.Stop();
            MainWindow Lindisfarne = new MainWindow();
            this.Close();
            Lindisfarne.ShowDialog();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
