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
    /// Interaction logic for Enemy0Assault.xaml
    /// </summary>
    public partial class Enemy0Assault : Window
    {
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

                FormationInfo.Foreground = new SolidColorBrush(Colors.LightGreen);
                FormationInfo.Content = "Effective Formation!(+%50)";
                FormationBonus = 0.5;
            }
            else if (SelectedFormation == 1)
            {
                Formation0.Source = Wedge.NotSelected;
                Formation1.Source = ShieldWall.Selected;
                Formation2.Source = Crescent.NotSelected;

                FormationInfo.Foreground = new SolidColorBrush(Colors.White);
                FormationInfo.Content = "Formation has no Effect!(+%0)";
                FormationBonus = 0;
            }
            else if (SelectedFormation == 2)
            {
                Formation0.Source = Wedge.NotSelected;
                Formation1.Source = ShieldWall.NotSelected;
                Formation2.Source = Crescent.Selected;

                FormationInfo.Foreground = new SolidColorBrush(Colors.Crimson);
                FormationInfo.Content = "Formation not Effective!(-%50)";
                FormationBonus = -0.5;
            }

            EnemyCasualties = ((MainWindow)Application.Current.MainWindow).Hersir.CalculateEnemyCasualties(FormationBonus);
            UnitCasualties = (((MainWindow)Application.Current.MainWindow).Thegn.CalculateUnitCasualties());
            Wounded = ((MainWindow)Application.Current.MainWindow).Hersir.CalculateWounded(((MainWindow)Application.Current.MainWindow).Thegn.CalculateUnitCasualties());


            if (EnemyCasualties < ((MainWindow)Application.Current.MainWindow).Thegn.Number)
            {
                KillInfo.Content = "Number of Enemey Casualties:" + ((MainWindow)Application.Current.MainWindow).Hersir.CalculateEnemyCasualties(FormationBonus);
            }
            else
            {
                KillInfo.Content = "All of the enemies will Die!";
            }
            
            if (UnitCasualties < ((MainWindow)Application.Current.MainWindow).Hersir.Number && (((MainWindow)Application.Current.MainWindow).Thegn.CalculateUnitCasualties() / 2) > 0)
            {
                DeathInfo.Content = "Number of Hersir Casualties:" + (((MainWindow)Application.Current.MainWindow).Thegn.CalculateUnitCasualties() / 2);
            }
            else if (UnitCasualties < 1)
            {
                DeathInfo.Content = "No Warriors will Die!";
            }
            else
            {
                DeathInfo.Content = "All of the Hersir will Die!";
            }
            
            if (UnitCasualties > 1 && ((MainWindow)Application.Current.MainWindow).Hersir.Wounded == 0)
            {
                WoundedInfo.Content = "Number of Hersir Wounded:" + Wounded;
            }
            else if (UnitCasualties  <= 1)
            {
                WoundedInfo.Content = "No Warriors will be Wounded!";
            }
            else
            {
                WoundedInfo.Content = "The Wounded Warriors will Perish!";
                DeathInfo.Content = "Number of Hersir Casualties:" + (((MainWindow)Application.Current.MainWindow).Thegn.CalculateUnitCasualties());
            }

            Assault.IsEnabled = true;
        }

        public Enemy0Assault()
        {
            InitializeComponent();
        }

        private void Assault_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Hersir.Number -= UnitCasualties;
            ((MainWindow)Application.Current.MainWindow).Thegn.Number -= EnemyCasualties;
            ((MainWindow)Application.Current.MainWindow).Hersir.Wounded = Wounded;
            DialogResult = true;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            LabelHersirNumber.Content = "Number of Warriors:" + ((MainWindow)Application.Current.MainWindow).Hersir.Number;
            LabelHersirStrength.Content = "Strength of Warriors:" + ((MainWindow)Application.Current.MainWindow).Hersir.Strength;
            LabelHersirWounded.Content = "Number of Wounded:" + ((MainWindow)Application.Current.MainWindow).Hersir.Wounded;

            LabelThegnNumber.Content = "Number of Warriors:" + ((MainWindow)Application.Current.MainWindow).Thegn.Number;
            LabelThegnStrength.Content = "Strength of Warriors:" + ((MainWindow)Application.Current.MainWindow).Thegn.Strength;

            ThegnFormation.Source = ShieldWall.Selected;
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
