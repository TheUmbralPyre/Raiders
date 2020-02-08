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

    public partial class Enemy1Assault : Window
    {
        MainWindow MW = new MainWindow();

        public class Formation
        {
            public BitmapImage NotSelected { get; set; }
            public BitmapImage Selected { get; set; }
        }

        private Formation ShieldWall = new Formation() { NotSelected = new BitmapImage(new Uri("Resources/FormationShieldWall.jpg", UriKind.Relative)), Selected = new BitmapImage(new Uri("Resources/FormationShieldWallSelected.jpg", UriKind.Relative)) };
        private Formation Wedge = new Formation() { NotSelected = new BitmapImage(new Uri("Resources/FormationWedge.jpg", UriKind.Relative)), Selected = new BitmapImage(new Uri("Resources/FormationWedgeSelected.jpg", UriKind.Relative)) };
        private Formation Crescent = new Formation() { NotSelected = new BitmapImage(new Uri("Resources/FormationCrescent.jpg", UriKind.Relative)), Selected = new BitmapImage(new Uri("Resources/FormationCrescentSelected.jpg", UriKind.Relative)) };

        private double FormationBonus;
        private int EnemyCasualties;
        private int UnitCasualties;
        private int Wounded;

        public void CalculateOutcome(int SelectedFormation)
        {
            if (SelectedFormation == 0)
            {
                Formation0.Source = Wedge.Selected;
                Formation1.Source = ShieldWall.NotSelected;
                Formation2.Source = Crescent.NotSelected;

                FormationInfo.Foreground = new SolidColorBrush(Colors.Crimson);
                FormationInfo.Content = "Formation not Effective!(-%50)";
                FormationBonus = -0.5;
            }
            else if (SelectedFormation == 1)
            {
                Formation0.Source = Wedge.NotSelected;
                Formation1.Source = ShieldWall.Selected;
                Formation2.Source = Crescent.NotSelected;

                FormationInfo.Foreground = new SolidColorBrush(Colors.LightGreen);
                FormationInfo.Content = "Effective Formation!(+%50)";
                FormationBonus = 0.5;
            }
            else if (SelectedFormation == 2)
            {
                Formation0.Source = Wedge.NotSelected;
                Formation1.Source = ShieldWall.NotSelected;
                Formation2.Source = Crescent.Selected;

                FormationInfo.Foreground = new SolidColorBrush(Colors.White);
                FormationInfo.Content = "Formation has no Effect!(+%0)";
                FormationBonus = 0;
            }

            EnemyCasualties = MW.Hersir.CalculateEnemyCasualties(FormationBonus);
            UnitCasualties = (MW.Morpslaga.CalculateUnitCasualties());
            Wounded = MW.Hersir.CalculateWounded(MW.Morpslaga.CalculateUnitCasualties());

            if (EnemyCasualties < MW.Morpslaga.Number && EnemyCasualties >= 1)
            {
                KillInfo.Content = MW.Hersir.CalculateEnemyCasualties(FormationBonus) + " Of the Morpslaga Will Die!";
            }
            else if (EnemyCasualties >= MW.Morpslaga.Number)
            {
                KillInfo.Content = "All of the Morpslaga will Die!";
            }
            else
            {
                KillInfo.Content = "None of the Morpslaga will Die!";
            }

            if (UnitCasualties < MW.Hersir.Number && UnitCasualties >= 1)
            {
                DeathInfo.Content = UnitCasualties / 2 + " Hersir will be Killed!";
            }
            else if (UnitCasualties >= MW.Hersir.Number)
            {
                DeathInfo.Content = "All of the Hersir will Die!";
            }
            else
            {
                DeathInfo.Content = "No Hersir will be Killed!";
            }

            if (UnitCasualties > 1 && MW.Hersir.Wounded == 0 && UnitCasualties < MW.Hersir.Number)
            {
                WoundedInfo.Content = Wounded + " Hersir will be Wounded!";
            }
            else if (UnitCasualties <= 1 && MW.Hersir.Wounded == 0)
            {
                WoundedInfo.Content = "No Hersir will be Wounded!";
            }
            else if (UnitCasualties >= MW.Hersir.Number)
            {
                WoundedInfo.Content = "";
            }
            else
            {
                WoundedInfo.Content = "The Wounded Warriors will Perish!";
                DeathInfo.Content =(MW.Morpslaga.CalculateUnitCasualties()) + " Hersir will be Killed!";
            }

            Assault.IsEnabled = true;
        }

        public Enemy1Assault(MainWindow mainWindow)
        {
            InitializeComponent();
            MW = mainWindow;

            LabelHersirNumber.Content = "Number of Warriors:" + MW.Hersir.Number;
            LabelHersirStrength.Content = "Strength of Warriors:" + MW.Hersir.Strength;
            LabelHersirWounded.Content = "Number of Wounded:" + MW.Hersir.Wounded;

            LabelMorpslagaNumber.Content = "Number of Warriors:" + MW.Morpslaga.Number;
            LabelMorpslagaStrength.Content = "Strength of Warriors:" + MW.Morpslaga.Strength;
        }

        private void Assault_Click(object sender, RoutedEventArgs e)
        {
            MW.Hersir.Number -= UnitCasualties;
            MW.Morpslaga.Number -= EnemyCasualties;
            MW.Hersir.Wounded = Wounded;
            DialogResult = true;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            

            MorpslagaFormation.Source = Wedge.Selected;
        }

        private void Formation_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var E = e.Source as FrameworkElement;

            if (E == Formation0)
            {
                CalculateOutcome(0);
            }
            else if (E == Formation1)
            {
                CalculateOutcome(1);
            }
            else
            {
                CalculateOutcome(2);
            }
        }

    }
}

