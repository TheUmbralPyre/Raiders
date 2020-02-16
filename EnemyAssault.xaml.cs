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
            public BitmapImage Alpha { get; set; }
            public BitmapImage Selected { get; set; }
        }
        public enum Enemy
        {
            Thegn,
            Morpslaga
        }

        private SoundPlayer AssaultEndSF = new SoundPlayer("C:/Users/User/Desktop/Projects/C#/Raiders2/ThemeAssaultEnd.wav");
        private Random Rand = new Random();
        private int EnemyFormationNumber;
        private Enemy EnemyType;

        private Formation Wedge = new Formation() { Alpha = new BitmapImage(new Uri("Resources/FormationWedge.jpg", UriKind.Relative)), Selected = new BitmapImage(new Uri("Resources/FormationWedgeSelected.jpg", UriKind.Relative)) };
        private Formation ShieldWall = new Formation() { Alpha = new BitmapImage(new Uri("Resources/FormationShieldWall.jpg", UriKind.Relative)), Selected = new BitmapImage(new Uri("Resources/FormationShieldWallSelected.jpg", UriKind.Relative)) };
        private Formation Crescent = new Formation() { Alpha = new BitmapImage(new Uri("Resources/FormationCrescent.jpg", UriKind.Relative)), Selected = new BitmapImage(new Uri("Resources/FormationCrescentSelected.jpg", UriKind.Relative)) };
        
        private double FormationBonus;
        private int EnemyCasualties;
        private int UnitCasualties;
        private int Wounded;

        public void CalculateOutcome(int SelectedFormation, int UnitCasualtiesCalculation, Enemy EnemyType)
        {
            if (EnemyFormationNumber == 1)
            {
                if (SelectedFormation == 1)
                {
                    Formation0.Source = Wedge.Selected;
                    Formation1.Source = ShieldWall.Alpha;
                    Formation2.Source = Crescent.Alpha;

                    FormationInfo.Foreground = new SolidColorBrush(Colors.White);
                    FormationInfo.Content = "Formation has no Effect!(+%0)";
                    FormationBonus = 0;
                }
                else if (SelectedFormation == 2)
                {
                    Formation0.Source = Wedge.Alpha;
                    Formation1.Source = ShieldWall.Selected;
                    Formation2.Source = Crescent.Alpha;

                    FormationInfo.Foreground = new SolidColorBrush(Colors.Crimson);
                    FormationInfo.Content = "Formation not Effective!(-%50)";
                    FormationBonus = -0.5;
                }
                else if (SelectedFormation == 3)
                {
                    Formation0.Source = Wedge.Alpha;
                    Formation1.Source = ShieldWall.Alpha;
                    Formation2.Source = Crescent.Selected;

                    FormationInfo.Foreground = new SolidColorBrush(Colors.LightGreen);
                    FormationInfo.Content = "Effective Formation!(+%50)";
                    FormationBonus = 0.5;
                }
            }
            else if (EnemyFormationNumber == 2)
            {
                if (SelectedFormation == 1)
                {
                    Formation0.Source = Wedge.Selected;
                    Formation1.Source = ShieldWall.Alpha;
                    Formation2.Source = Crescent.Alpha;

                    FormationInfo.Foreground = new SolidColorBrush(Colors.LightGreen);
                    FormationInfo.Content = "Effective Formation!(+%50)";
                    FormationBonus = 0.5;
                }
                else if (SelectedFormation == 2)
                {
                    Formation0.Source = Wedge.Alpha;
                    Formation1.Source = ShieldWall.Selected;
                    Formation2.Source = Crescent.Alpha;

                    FormationInfo.Foreground = new SolidColorBrush(Colors.White);
                    FormationInfo.Content = "Formation has no Effect!(+%0)";
                    FormationBonus = 0;
                }
                else if (SelectedFormation == 3)
                {
                    Formation0.Source = Wedge.Alpha;
                    Formation1.Source = ShieldWall.Alpha;
                    Formation2.Source = Crescent.Selected;

                    FormationInfo.Foreground = new SolidColorBrush(Colors.Crimson);
                    FormationInfo.Content = "Formation not Effective!(-%50)";
                    FormationBonus = -0.5;
                }
            }
            else if (EnemyFormationNumber == 3)
            {
                if (SelectedFormation == 1)
                {
                    Formation0.Source = Wedge.Selected;
                    Formation1.Source = ShieldWall.Alpha;
                    Formation2.Source = Crescent.Alpha;

                    FormationInfo.Foreground = new SolidColorBrush(Colors.Crimson);
                    FormationInfo.Content = "Formation not Effective!(-%50)";
                    FormationBonus = -0.5;
                }
                else if (SelectedFormation == 2)
                {
                    Formation0.Source = Wedge.Alpha;
                    Formation1.Source = ShieldWall.Selected;
                    Formation2.Source = Crescent.Alpha;

                    FormationInfo.Foreground = new SolidColorBrush(Colors.LightGreen);
                    FormationInfo.Content = "Effective Formation!(+%50)";
                    FormationBonus = 0.5;
                }
                else if (SelectedFormation == 3)
                {
                    Formation0.Source = Wedge.Alpha;
                    Formation1.Source = ShieldWall.Alpha;
                    Formation2.Source = Crescent.Selected;

                    FormationInfo.Foreground = new SolidColorBrush(Colors.White);
                    FormationInfo.Content = "Formation has no Effect!(+%0)";
                    FormationBonus = 0;
                }
            }

            EnemyCasualties = MW.Hersir.CalculateEnemyCasualties(FormationBonus);
            UnitCasualties = UnitCasualtiesCalculation;
            if (UnitCasualties > MW.Hersir.Number) UnitCasualties = MW.Hersir.Number;
            Wounded = MW.Hersir.CalculateWounded(UnitCasualties);

            if (EnemyType == Enemy.Thegn)
            {
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
            }
            else
            {
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
            }

            if (UnitCasualties == 1 && MW.Hersir.Rank < 1 && MW.Hersir.Rank < 1)
            {
                DeathInfo.Content = UnitCasualties + " of the Hersir will be Killed!";
            }
            else if (UnitCasualties < MW.Hersir.Number && UnitCasualties > 1 && MW.Hersir.Rank < 1)
            {
                DeathInfo.Content = UnitCasualties / 2 + " Hersir will be Killed!";
            }
            else if (UnitCasualties >= MW.Hersir.Number && MW.Hersir.Rank < 1)
            {
                DeathInfo.Content = "All of the Hersir will Die!";
            }
            else
            {
                DeathInfo.Content = "No Hersir will be Killed!";
            }

            if (UnitCasualties > 1 && MW.Hersir.Wounded == 0)
            {
                WoundedInfo.Content = Wounded + " Hersir will be Wounded!";
            }
            else if (UnitCasualties <= 1 && MW.Hersir.Wounded == 0)
            {
                WoundedInfo.Content = "No Hersir will be Wounded!";
            }
            else if (UnitCasualties >= MW.Hersir.Number && MW.Hersir.Rank != 3)
            {
                WoundedInfo.Content = "";
            }
            else
            {
                if (MW.Hersir.Rank != 3)
                {
                    WoundedInfo.Content = "The Wounded Warriors will Perish!";

                    if (EnemyType == Enemy.Thegn)
                    {
                        DeathInfo.Content = (MW.Thegn.CalculateUnitCasualties()) + " Hersir will be Killed!";
                    }
                    else
                    {
                        DeathInfo.Content = (MW.Morpslaga.CalculateUnitCasualties()) + " Hersir will be Killed!";
                    }
                }
                else
                {
                    WoundedInfo.Content = "The Wounded Warriors won't fall!";
                    UnitCasualties = 0;
                    Wounded = MW.Hersir.Wounded;
                }
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

            ImageHersir.Source = MW.Hersir.Overworld.Alpha;

            switch (EnemyType)
            {
                case Enemy.Morpslaga:
                    LabelEnemyNumber.Content = "Number of Warriors:" + MW.Morpslaga.Number;
                    LabelEnemyStrength.Content = "Strength of Warriors:" + MW.Morpslaga.Strength;
                    ImageEnemy.Source = MW.Morpslaga.Overworld.Alpha;
                    break;
                case Enemy.Thegn:
                    LabelEnemyNumber.Content = "Number of Warriors:" + MW.Thegn.Number;
                    LabelEnemyStrength.Content = "Strength of Warriors:" + MW.Thegn.Strength;
                    ImageEnemy.Source = MW.Thegn.Overworld.Alpha;
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
                    MW.Morpslaga.Number -= EnemyCasualties;
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
            EnemyFormationNumber = Rand.Next(1, 4);
            switch (EnemyFormationNumber)
            {
                case 1:
                    EnemyFormation.Source = Wedge.Selected;
                    break;
                case 2:
                    EnemyFormation.Source = ShieldWall.Selected;
                    break;
                case 3:
                    EnemyFormation.Source = Crescent.Selected;
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
                        CalculateOutcome(1, MW.Morpslaga.CalculateUnitCasualties(), EnemyType);
                    }
                    else if (E == Formation1)
                    {
                        CalculateOutcome(2, MW.Morpslaga.CalculateUnitCasualties(), EnemyType);
                    }
                    else
                    {
                        CalculateOutcome(3, MW.Morpslaga.CalculateUnitCasualties(), EnemyType);
                    }
                    break;
                case Enemy.Thegn:
                    if (E == Formation0)
                    {
                        CalculateOutcome(1, MW.Thegn.CalculateUnitCasualties(), EnemyType);
                    }
                    else if (E == Formation1)
                    {
                        CalculateOutcome(2, MW.Thegn.CalculateUnitCasualties(), EnemyType);
                    }
                    else
                    {
                        CalculateOutcome(3, MW.Thegn.CalculateUnitCasualties(), EnemyType);
                    }
                    break;
            }
        }

    }
}
