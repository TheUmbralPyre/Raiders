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
    /// Interaction logic for RankUp.xaml
    /// </summary>
    public partial class RankUp : Window
    {
        public RankUp(int Rank)
        {
            InitializeComponent();

            switch (Rank)
            {
                case 2:
                    LabelRankMessage1.Content = "New Augmentation Received: Undefeated";
                    LabelRankMessage2.Content = "All the strength that I have, all the life that's left in me!";
                    ImageRank.Source = new BitmapImage(new Uri("Resources/HersirOverworldSelected2.png",UriKind.Relative));
                    break;
                case 3:
                    LabelRankMessage1.Content = "New Augmentation Received: Back From The Dead";
                    LabelRankMessage2.Content = "Full of love, full of light, full of fight!";
                    ImageRank.Source = new BitmapImage(new Uri("Resources/HersirOverworldSelected3.png", UriKind.Relative));
                    break;

            }
        }

        private void ButtonRank_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
