﻿using System;
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
    /// Interaction logic for ThegnSally.xaml
    /// </summary>
    public partial class ThegnSally : Window
    {
        public class Formation
        {
            public BitmapImage Alpha { get; set; }
            public BitmapImage Selected { get; set; }
        }
        Random Rand = new Random();

        MainWindow MW = new MainWindow();
        private Formation ShieldWall = new Formation() { Alpha = new BitmapImage(new Uri("Resources/FormationShieldWall.jpg", UriKind.Relative)), Selected = new BitmapImage(new Uri("Resources/FormationShieldWallSelected.jpg", UriKind.Relative)) };
        private Formation Wedge = new Formation() { Alpha = new BitmapImage(new Uri("Resources/FormationWedge.jpg", UriKind.Relative)), Selected = new BitmapImage(new Uri("Resources/FormationWedgeSelected.jpg", UriKind.Relative)) };
        private Formation Crescent = new Formation() { Alpha = new BitmapImage(new Uri("Resources/FormationCrescent.jpg", UriKind.Relative)), Selected = new BitmapImage(new Uri("Resources/FormationCrescentSelected.jpg", UriKind.Relative)) };
        private SoundPlayer AssaultEndSF = new SoundPlayer(System.AppDomain.CurrentDomain.BaseDirectory + "GameResources/ThemeAssaultEnd.wav");
        private double FormationBonus;

        public ThegnSally(MainWindow mainWindow)
        {
            InitializeComponent();
            MW = mainWindow;

            int FormationNumber = Rand.Next(1, 4);

            switch (FormationNumber)
            {
                case 1:
                    ImageThegnFormation.Source = Wedge.Selected;
                    break;
                case 2:
                    ImageThegnFormation.Source = ShieldWall.Selected;
                    break;
                case 3:
                    ImageThegnFormation.Source = Crescent.Selected;
                    break;
            }

            ImageHersirFormation1.Source = Wedge.Alpha;
            ImageHersirFormation2.Source = ShieldWall.Alpha;
            ImageHersirFormation3.Source = Crescent.Alpha;
        }

        private void ImageHersirFormation_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((sender as Image).Source == Wedge.Alpha)
            {
                ImageHersirFormation1.Source = Wedge.Selected;
                ImageHersirFormation2.Source = ShieldWall.Alpha;
                ImageHersirFormation3.Source = Crescent.Alpha;

            }
            else if ((sender as Image).Source == ShieldWall.Alpha)
            {
                ImageHersirFormation1.Source = Wedge.Alpha;
                ImageHersirFormation2.Source = ShieldWall.Selected;
                ImageHersirFormation3.Source = Crescent.Alpha;
            }
            else if ((sender as Image).Source == Crescent.Alpha)
            {
                ImageHersirFormation1.Source = Wedge.Alpha;
                ImageHersirFormation2.Source = ShieldWall.Alpha;
                ImageHersirFormation3.Source = Crescent.Selected;
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (ImageThegnFormation.Source == Wedge.Selected && ImageHersirFormation3.Source == Crescent.Selected)
            {
                FormationBonus = 0.25;
            }
            else if (ImageThegnFormation.Source == ShieldWall.Selected && ImageHersirFormation1.Source == Wedge.Selected)
            {
                FormationBonus = 0.25;
            }
            else if (ImageThegnFormation.Source == Crescent.Selected && ImageHersirFormation2.Source == ShieldWall.Selected)
            {
                FormationBonus = 0.25;
            }
            else
            {
                FormationBonus = -0.50;
            }

            int EnemyCasualties = MW.Hersir.CalculateEnemyCasualties(FormationBonus);
            int UnitCasualties = MW.Thegn.CalculateUnitCasualties();
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
            MW.Thegn.Number -= EnemyCasualties;
            MW.Hersir.Wounded = Wounded;
            DialogResult = true;
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
