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
using System.Media;

namespace Raiders_2._1
{
    /// <summary>
    /// Interaction logic for MorpslagaAmbush.xaml
    /// </summary>
    public partial class MorpslagaAmbush : Window
    {
        MainWindow MW = new MainWindow();
        MainWindow.OverworldImage[] MorpslagaOverworld = new MainWindow.OverworldImage[3];
        Random Rand = new Random();
        int FormationOrder;
        private double FormationBonus;
        private SoundPlayer AssaultEndSF = new SoundPlayer(System.AppDomain.CurrentDomain.BaseDirectory + "GameResources/ThemeAssaultEnd.wav");

        public MorpslagaAmbush(MainWindow mainWindow)
        {
            InitializeComponent();
            MW = mainWindow;
            ImageHersir2.Source = MW.Hersir.Overworld.Alpha;

            MorpslagaOverworld[0] = new MainWindow.OverworldImage() {Alpha = new BitmapImage(new Uri("Resources/MorpslagaOverworld.png", UriKind.Relative)), Selected = new BitmapImage(new Uri("Resources/MorpslagaOverworldSelected.png", UriKind.Relative)) };
            MorpslagaOverworld[1] = new MainWindow.OverworldImage() { Alpha = new BitmapImage(new Uri("Resources/FalseMorpslaga1.png", UriKind.Relative)), Selected = new BitmapImage(new Uri("Resources/FalseMorpslagadSelected1.png", UriKind.Relative)) };
            MorpslagaOverworld[2] = new MainWindow.OverworldImage() { Alpha = new BitmapImage(new Uri("Resources/FalseMorpslaga2.png", UriKind.Relative)), Selected = new BitmapImage(new Uri("Resources/FalseMorpslagaSelected2.png", UriKind.Relative)) };

            FormationOrder = Rand.Next(1, 4);
            
            switch (FormationOrder)
            {
                case 1:
                    ImageMorpslaga1.Source = MorpslagaOverworld[0].Alpha;
                    ImageMorpslaga2.Source = MorpslagaOverworld[1].Alpha;
                    ImageMorpslaga3.Source = MorpslagaOverworld[2].Alpha;
                    break;
                case 2:
                    ImageMorpslaga1.Source = MorpslagaOverworld[2].Alpha;
                    ImageMorpslaga2.Source = MorpslagaOverworld[0].Alpha;
                    ImageMorpslaga3.Source = MorpslagaOverworld[1].Alpha;
                    break;
                case 3:
                    ImageMorpslaga1.Source = MorpslagaOverworld[2].Alpha;
                    ImageMorpslaga2.Source = MorpslagaOverworld[1].Alpha;
                    ImageMorpslaga3.Source = MorpslagaOverworld[0].Alpha;
                    break;
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (FormationOrder == 1 && ImageHersir1.Source.ToString() == MW.Hersir.Overworld.Alpha.ToString())
            {
                FormationBonus = 0.25;
            }
            else if (FormationOrder == 2 && ImageHersir2.Source.ToString() == MW.Hersir.Overworld.Alpha.ToString())
            {
                FormationBonus = 0.25;
            }
            else if (FormationOrder == 3 && ImageHersir3.Source.ToString() == MW.Hersir.Overworld.Alpha.ToString())
            {
                FormationBonus = 0.25;
            }
            else
            {
                FormationBonus = -0.50;
            }

            int EnemyCasualties = MW.Hersir.CalculateEnemyCasualties(FormationBonus);
            int UnitCasualties = MW.Morpslaga.CalculateUnitCasualties();
            if (UnitCasualties > MW.Hersir.Number) UnitCasualties = MW.Hersir.Number;
            int Wounded = MW.Hersir.CalculateWounded(UnitCasualties);

            if (MW.Hersir.Rank == 3 && MW.Hersir.Wounded != 0)
            {
                if (MW.Hersir.Number != 0)
                {
                    MW.Hersir.Wounded += MW.Hersir.Number;
                }
                else 
                {
                    UnitCasualties = 0;
                }
                
                Wounded = MW.Hersir.Wounded;
            } 

            AssaultEndSF.PlaySync();
            MW.Hersir.Number -= UnitCasualties;
            MW.Morpslaga.Number -= EnemyCasualties;
            MW.Hersir.Wounded = Wounded;
            DialogResult = true;
        }

        private void ImageHersir_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MW.MarchSF.Play();
            if ((sender as Image) == ImageHersir1)
            {
                ImageHersir1.Source = MW.Hersir.Overworld.Alpha;
                ImageHersir2.Source = MW.MarchImage;
                ImageHersir3.Source = MW.MarchImage;
            }
            else if ((sender as Image) == ImageHersir2)
            {
                ImageHersir1.Source = MW.MarchImage;
                ImageHersir2.Source = MW.Hersir.Overworld.Alpha;
                ImageHersir3.Source = MW.MarchImage;
            }
            else if ((sender as Image) == ImageHersir3)
            {
                ImageHersir1.Source = MW.MarchImage;
                ImageHersir2.Source = MW.MarchImage;
                ImageHersir3.Source = MW.Hersir.Overworld.Alpha;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DialogResult != true)
            {
                e.Cancel = true;
            }
        }
    }
}
