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
    /// Interaction logic for Enemy0Assault.xaml
    /// </summary>
    public partial class EnemyAssault : Window
    {
        MainWindow MW = new MainWindow();
        public class Formation
        {
            public BitmapImage NotSelected { get; set; }
            public BitmapImage Selected { get; set; }
        }
        private SoundPlayer AssaultEndSF = new SoundPlayer("C:/Users/User/Desktop/Projects/C#/Raiders2/ThemeAssaultEnd.wav");
        enum Enemy
        {
            Thegn,
            Morpslaga
        }
        private Enemy EnemyType;

        private Formation ShieldWall = new Formation() { NotSelected = new BitmapImage(new Uri("Resources/FormationShieldWall.jpg", UriKind.Relative)), Selected = new BitmapImage(new Uri("Resources/FormationShieldWallSelected.jpg", UriKind.Relative)) };
        private Formation Wedge = new Formation() { NotSelected = new BitmapImage(new Uri("Resources/FormationWedge.jpg", UriKind.Relative)), Selected = new BitmapImage(new Uri("Resources/FormationWedgeSelected.jpg", UriKind.Relative)) };
        private Formation Crescent = new Formation() { NotSelected = new BitmapImage(new Uri("Resources/FormationCrescent.jpg", UriKind.Relative)), Selected = new BitmapImage(new Uri("Resources/FormationCrescentSelected.jpg", UriKind.Relative)) };
        
        private double FormationBonus;
        private int EnemyCasualties;
        private int UnitCasualties;
        private int Wounded;

        public void CalculateOutcomeThegn(int SelectedFormation)
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

            EnemyCasualties = MW.Hersir.CalculateEnemyCasualties(FormationBonus);
            UnitCasualties = MW.Thegn.CalculateUnitCasualties();
            Wounded = MW.Hersir.CalculateWounded(MW.Thegn.CalculateUnitCasualties());

            if (EnemyCasualties < MW.Thegn.Number && EnemyCasualties >= 1)
            {
                KillInfo.Content = MW.Hersir.CalculateEnemyCasualties(FormationBonus) + " Of the Thegn Will Die!";
            }
            else if (EnemyCasualties >= MW.Thegn.Number)
            {
                KillInfo.Content = "All of the Thegn will Die!";
            }
            else
            {
                KillInfo.Content = "None of the Thegn will Die!";
            }

            if (UnitCasualties <= 1)
            {
                DeathInfo.Content = UnitCasualties + " of the Hersir will be Killed!";
            }
            else if (UnitCasualties < MW.Hersir.Number)
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
                DeathInfo.Content = (MW.Thegn.CalculateUnitCasualties()) + " Hersir will be Killed!";
            }

            Assault.IsEnabled = true;
        }
        public void CalculateOutcomeMorpslaga(int SelectedFormation)
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

            if (UnitCasualties <= 1)
            {
                DeathInfo.Content = UnitCasualties  + " of the Hersir will be Killed!";
            }
            else if (UnitCasualties < MW.Hersir.Number)
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
                DeathInfo.Content = (MW.Morpslaga.CalculateUnitCasualties()) + " Hersir will be Killed!";
            }

            Assault.IsEnabled = true;
        }

        public EnemyAssault(MainWindow mainWindow, string EnemyName)
        {
            InitializeComponent();
            MW = mainWindow;

            if (EnemyName == "Morpslaga")
            {
                EnemyType = Enemy.Morpslaga;
            }
            else
            {
                EnemyType = Enemy.Thegn;
            }

            LabelHersirNumber.Content = "Number of Warriors:" + MW.Hersir.Number;
            LabelHersirStrength.Content = "Strength of Warriors:" + MW.Hersir.Strength;
            LabelHersirWounded.Content = "Number of Wounded:" + MW.Hersir.Wounded;


            switch(EnemyType)
            {
                case Enemy.Morpslaga:
                    LabelEnemyNumber.Content = "Number of Warriors:" + MW.Morpslaga.Number;
                    LabelEnemyStrength.Content = "Strength of Warriors:" + MW.Morpslaga.Strength;
                    break;
                case Enemy.Thegn:
                    LabelEnemyNumber.Content = "Number of Warriors:" + MW.Thegn.Number;
                    LabelEnemyStrength.Content = "Strength of Warriors:" + MW.Thegn.Strength;
                    break;
            }
        }

        private void Assault_Click(object sender, RoutedEventArgs e)
        {
            AssaultEndSF.PlaySync();
            MW.Hersir.Number -= UnitCasualties;

            switch (EnemyType)
            {
                case Enemy.Morpslaga:
                    MW.Thegn.Number -= EnemyCasualties;
                    break;
                case Enemy.Thegn:
                    MW.Thegn.Number -= EnemyCasualties;
                    break;
            }

            MW.Hersir.Wounded = Wounded;
            DialogResult = true;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            EnemyFormation.Source = ShieldWall.Selected;
            
            switch (EnemyType)
            {
                case Enemy.Morpslaga:
                    EnemyFormation.Source = Crescent.Selected;
                    ImageEnemy.Source = MW.Morpslaga.Overworld;
                    break;
                case Enemy.Thegn:
                    EnemyFormation.Source = ShieldWall.Selected;
                    ImageEnemy.Source = MW.Thegn.Overworld;
                    break;
            }
        }

        private void Formation_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var E = e.Source as FrameworkElement;

            switch (EnemyType)
            {
                case Enemy.Morpslaga:
                    if (E == Formation0)
                    {
                        CalculateOutcomeMorpslaga(0);
                    }
                    else if (E == Formation1)
                    {
                        CalculateOutcomeMorpslaga(1);
                    }
                    else
                    {
                        CalculateOutcomeMorpslaga(2);
                    }
                    break;
                case Enemy.Thegn:
                    if (E == Formation0)
                    {
                        CalculateOutcomeThegn(0);
                    }
                    else if (E == Formation1)
                    {
                        CalculateOutcomeThegn(1);
                    }
                    else
                    {
                        CalculateOutcomeThegn(2);
                    }
                    break;
            }
        }

    }
}
