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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Media;

namespace Raiders_2._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Class Map
        private Image[,] Field = new Image[6,8];
        public BitmapImage MarchImage = new BitmapImage(new Uri("Resources/Move.png", UriKind.Relative));
        private BitmapImage MissionImage = new BitmapImage(new Uri("Resources/Mission.png", UriKind.Relative));
        private BitmapImage MissionReceive = new BitmapImage(new Uri("Resources/MissionReceive.png", UriKind.Relative));
        //

        //Class Music
        private MediaPlayer LindisfarneMusic = new MediaPlayer();
        public SoundPlayer MarchSF = new SoundPlayer(System.AppDomain.CurrentDomain.BaseDirectory + "GameResources/ThemeMarch.wav");
        private SoundPlayer AssaultSF = new SoundPlayer(System.AppDomain.CurrentDomain.BaseDirectory + "GameResources/ThemeAssaultPlan.wav");
        private bool CurrentMusicIsPeace = false;
        //
        private int TurnNumber = 1;

        public abstract class Unit
        {
            public int X { get; set; }
            public int Y { get; set; }
            public OverworldImage Overworld { get; set; }
            public BitmapImage Picture { get; set; }
            public int Number { get; set; }
            public double Strength { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
        public class Player : Unit
        {          
            public int Wounded { get; set; }
            public int Actions { get; set; }
            public int Rank { get; set; }

            public void ActionCheck(Image[,] Field, BitmapImage MarchImage, OverworldImage Thegn, OverworldImage Morpslaga, BitmapImage Mission, BitmapImage MissionReceive)
            {
                for (int y = -1; y < 2; y++)
                {
                    for (int x = -1; x < 2; x++)
                    {
                        bool CheckY;
                        bool CheckX;

                        if (y == 1)
                        {
                            CheckY = Y != Field.GetUpperBound(0);
                        }
                        else
                        {
                            CheckY = Y != 0;
                        }

                        if (x == 1)
                        {
                            CheckX = X != Field.GetUpperBound(1); ;
                        }
                        else
                        {
                            CheckX = X != 0;
                        }

                        if (CheckY && CheckX)
                        {
                            if (Field[Y + y, X + x].Source == null)
                            {
                                Field[Y + y, X + x].Source = MarchImage;
                            }
                            if (Field[Y + y, X + x].Source == Thegn.Alpha)
                            {
                                Field[Y + y, X + x].Source = Thegn.Assault;
                            }
                            if (Field[Y + y, X + x].Source == Morpslaga.Alpha)
                            {
                                Field[Y + y, X + x].Source = Morpslaga.Assault;
                            }
                            if (Field[Y + y, X + x].Source == Mission)
                            {
                                Field[Y + y, X + x].Source = MissionReceive;
                            }
                        }
                        else if (CheckY)
                        {
                            if (Field[Y + y, X].Source == null)
                            {
                                Field[Y + y, X].Source = MarchImage;
                            }
                            if (Field[Y + y, X].Source == Thegn.Alpha)
                            {
                                Field[Y + y, X].Source = Thegn.Assault;
                            }
                            if (Field[Y + y, X].Source == Morpslaga.Alpha)
                            {
                                Field[Y + y, X].Source = Morpslaga.Assault;
                            }
                            if (Field[Y + y, X].Source == Mission)
                            {
                                Field[Y + y, X].Source = MissionReceive;
                            }
                        }
                        else if (CheckX)
                        {
                            if (Field[Y, X + x].Source == null)
                            {
                                Field[Y, X + x].Source = MarchImage;
                            }
                            if (Field[Y, X + x].Source == Thegn.Alpha)
                            {
                                Field[Y, X + x].Source = Thegn.Assault;
                            }
                            if (Field[Y, X + x].Source == Morpslaga.Alpha)
                            {
                                Field[Y, X + x].Source = Morpslaga.Assault;
                            }
                            if (Field[Y, X + x].Source == Mission)
                            {
                                Field[Y + y, X + x].Source = MissionReceive;
                            }
                        }
                    }
                }
            }
            public void MarchConclude(Image[,] Field, Image image)
            {
                image.Source = Field[Y, X].Source;
                Field[Y, X].Source = null;
                for (int y = 0; y < Field.GetLength(0); y++)
                {
                    for (int x = 0; x < Field.GetLength(1); x++)
                    {
                        if (Field[y, x].Source == image.Source)
                        {
                            Y = y;
                            X = x;
                        }
                    }
                }
            }
            public void Select(Image SelectedPicture, TextBlock SelectedName, Label SelectedNumber, Label SelectedStrength, Label SelectedSpecial, RichTextBox SelectedText,Image[,] Field)
            {
                SelectedPicture.Source = Picture;
                SelectedName.Text = Name;
                SelectedName.Foreground = new SolidColorBrush(Colors.LightSkyBlue);
                SelectedNumber.Content = "Warriors: " + Number;
                SelectedStrength.Content = "Strength: " + Strength;
                SelectedSpecial.Content = "Wounded: " + Wounded;
                SelectedText.Selection.Text = Description;
                SelectedText.IsEnabled = true;
                Field[Y, X].Source = Overworld.Selected;
            }
            
            public void AssaultLog(int UnitNumber, double UnitStrenght, int EnemyNumber, double EnemyStrenght, int Wounded)
            {
                string LogOne = null;
                string LogTwo = null;

                int UnitCasualties = (int)(EnemyNumber * EnemyStrenght);
                int EnemyCasualties = (int)((UnitNumber * UnitStrenght) + (Wounded * (UnitStrenght / 2)));

                if (EnemyCasualties > EnemyNumber)
                {
                    LogOne = "The enemy warriors have all been slaughtered!";
                }
                else if (EnemyCasualties < 1)
                {
                    LogOne = "Your enemies haven't suffered casualties!";
                }
                else
                {
                    LogOne = EnemyCasualties + " of the enemies died during the Assault!";
                }

                if (UnitCasualties > UnitNumber)
                {
                    MessageBox.Show("All of your Warriors have been sent to Vallhala!", "Assault Results");
                    //MissionResult(Defeat, "Tonight You shall feast in the great Hall!");
                }
                else if (UnitCasualties <= 0)
                {
                    LogTwo = "\nYour warriors haven't suffered casualties!";
                    MessageBox.Show((LogOne + LogTwo), "Assault Log");
                }
                else
                {
                    if (Wounded == 0)
                    {
                        LogTwo = "\n" + UnitCasualties / 2 + " of your warriors will feast in Vallhala!";
                        MessageBox.Show((LogOne + LogTwo), "Assault Log");
                        if ((UnitCasualties % 2) != 0 && UnitCasualties > 1)
                        {
                            Wounded = (UnitCasualties / 2) + 1;
                            if (Wounded == 1)
                            {
                                MessageBox.Show(Wounded + " of your warriors has been Wounded!", "『Living Proof』");
                            }
                            else
                            {
                                MessageBox.Show(Wounded + " of your warriors have been Wounded!", "『Living Proof』");
                            }
                        }
                        else if (UnitCasualties > 1)
                        {
                            Wounded = UnitCasualties / 2;
                            if (Wounded == 1)
                            {
                                MessageBox.Show(Wounded + " of your warriors has been Wounded!", "『Living Proof』");
                            }
                            else
                            {
                                MessageBox.Show(Wounded + " of your warriors have been Wounded!", "『Living Proof』");
                            }
                        }
                    }
                    else
                    {
                        LogTwo = "\n" + UnitCasualties + " of your warriors will feast in Vallhala!";
                        MessageBox.Show((LogOne + LogTwo), "Assault Log");
                        Wounded = 0;
                        MessageBox.Show("Your wounded warriors have died!", "『Living Proof』");
                    }
                }

                EnemyNumber -= EnemyCasualties;
                UnitNumber -= UnitCasualties;
            }
            public void AssaultConclude (int UnitCasualties)
            {
                Number -= UnitCasualties;

                if (Wounded == 0)
                {
                    if ((UnitCasualties % 2) != 0 && UnitCasualties > 1)
                    {
                        Wounded = (UnitCasualties / 2) + 1;
                    }
                    else if (UnitCasualties > 1)
                    {
                        Wounded = UnitCasualties / 2;
                    }
                }
                else
                {
                    Wounded = 0;
                }
            }
            
            public int CalculateEnemyCasualties(double FormationBonus)
            {
                int EnemyCasualties;
                if (Rank < 2)
                {
                  EnemyCasualties = (int)((Number * Strength) + (Wounded * (Strength / 2)));
                }
                else
                {
                    EnemyCasualties = (int)((Number * Strength) + (Wounded * Strength));
                }
                EnemyCasualties += (int)(EnemyCasualties * FormationBonus); 
                return EnemyCasualties; 
            }
            public int CalculateWounded(int HersirCasualties)
            {
                int wounded = Wounded;

                if (Wounded == 0)
                {
                    if (Rank < 1)
                    {
                        if ((HersirCasualties % 2) != 0 && HersirCasualties > 1)
                        {
                            wounded = (HersirCasualties / 2) + 1;
                        }
                        else if (HersirCasualties > 1)
                        {
                            wounded = HersirCasualties / 2;
                        }
                    }
                    else
                    {
                        wounded = HersirCasualties;
                    }
                }
                else
                {
                    wounded = 0;
                }
                return wounded;
            }
            public void RecoverWounded()
            {
                if (Wounded > 0)
                {
                    Number += Wounded;
                    Wounded = 0;
                    MessageBox.Show("The Wounded warriors have recovered!", "Living proof that the worst can be beat!");
                }
            }

            public void MissionResult(bool Victory,MainWindow MW)
            {
                if (Victory)
                {
                    LindisfarneResult Result = new LindisfarneResult("Victory!");
                    MW.Close();
                    Result.ShowDialog();

                }

                if (!Victory)
                {
                    LindisfarneResult Result = new LindisfarneResult("Defeat!");
                    MW.Close();
                    Result.ShowDialog();
                }
            }

            public void UpdateActions(Label LabelActions, bool EndTurn)
            {
                if (!EndTurn)
                {
                    Actions--;
                    switch (Actions)
                    {
                        case 1:
                            LabelActions.Foreground = new SolidColorBrush(Colors.Yellow);
                            break;
                        case 0:
                            LabelActions.Foreground = new SolidColorBrush(Colors.Red);
                            break;
                    }
                }
                else
                {
                    Actions = 2;
                    LabelActions.Foreground = new SolidColorBrush(Colors.LightGreen);
                }
                LabelActions.Content = "Actions Left : " + Actions;
            }
            public void RankUp()
            {
                Rank++;
                RankUp rankUp = new RankUp(Rank);

                switch (Rank)
                {
                    case 1:
                        Description = Properties.Resources.HersirDescription1;
                        rankUp.ShowDialog();
                        Overworld.Alpha = new BitmapImage(new Uri("Resources/HersirOverworld1.png",UriKind.Relative));
                        Overworld.Selected = new BitmapImage(new Uri("Resources/HersirOverworldSelected1.png", UriKind.Relative));
                        break;
                    case 2:
                        Description = Properties.Resources.HersirDescription2;
                        rankUp.ShowDialog();
                        Overworld.Alpha = new BitmapImage(new Uri("Resources/HersirOverworld2.png", UriKind.Relative));
                        Overworld.Selected = new BitmapImage(new Uri("Resources/HersirOverworldSelected2.png", UriKind.Relative));
                        break;
                    case 3:
                        Description = Properties.Resources.HersirDescription3;
                        rankUp.ShowDialog();
                        Overworld.Alpha = new BitmapImage(new Uri("Resources/HersirOverworld3.png", UriKind.Relative));
                        Overworld.Selected = new BitmapImage(new Uri("Resources/HersirOverworldSelected3.png", UriKind.Relative));
                        break;
                }
            }
        }
        public class Enemy0 : Unit
        {            
            public void Select(Image SelectedPicture, TextBlock SelectedName, Label SelectedNumber, Label SelectedStrength, Label SelectedSpecial, RichTextBox SelectedText, Image[,] Field, int TurnNumber)
            {
                SelectedPicture.Source = Picture;
                SelectedName.Text = Name;
                SelectedName.Foreground = new SolidColorBrush(Colors.Red);
                SelectedNumber.Content = "Warriors: " + Number;
                SelectedStrength.Content = "Strength: " + Strength;
                
                if (TurnNumber % 2 == 0)
                {
                    SelectedSpecial.Content = "Will Reinforce!";
                }
                else
                {
                    SelectedSpecial.Content = "Won't Reinforce!";
                }

                SelectedText.Selection.Text = Description;
                SelectedText.IsEnabled = true;
                Field[Y, X].Source = Overworld.Selected;
            }
            public void AssaultConclude(Image[,] Field, int EnemyCasualties)
            {
                Number -= EnemyCasualties;
                if (Number <= 0)
                {
                    Field[Y, X].Source = null;
                }
            }

            public int CalculateUnitCasualties()
            {
                return (int)(Number * (2 * Strength));
            }

            public bool CheckForEnemy(int HersirY, int HersirX)
            {
                if ((X == HersirX - 1 || X == HersirX + 1) && (Y == HersirY - 1 || Y == HersirY + 1))
                {
                    return true;                    
                }
                else if ((Y == HersirY - 1 || Y == HersirY + 1) && X == HersirX)
                {
                    return true;
                }
                else if ((X == HersirX - 1 || X == HersirX + 1) && Y == HersirY)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            public void Reinforce(Image[,] Field, OverworldImage Morpslaga,ref int MorpslagaNumber, int TurnNumber)
            {
                if (TurnNumber % 2 == 0 && MorpslagaNumber < 15)
                {
                    if (Number < 20)
                    {
                        Number++;
                        MessageBox.Show("The Thegn have Reinforced themselves by 1!", "Slow, slow baby that's what I like!");
                    }
                    for (int y = -1; y < 2; y++)
                    {
                        for (int x = -1; x < 2; x++)
                        {
                            bool CheckY;
                            bool CheckX;

                            if (y == 1)
                            {
                                CheckY = Y != Field.GetUpperBound(0);
                            }
                            else
                            {
                                CheckY = Y != 0;
                            }

                            if (x == 1)
                            {
                                CheckX = X != Field.GetUpperBound(1); ;
                            }
                            else
                            {
                                CheckX = X != 0;
                            }

                            if (CheckY && CheckX)
                            {
                                if (Field[Y + y, X + x].Source == Morpslaga.Alpha)
                                {
                                    MorpslagaNumber++;
                                    MessageBox.Show("The Morpslaga have been reinforced by 1!", "SLOW baby SLOW!");
                                }
                                if (Field[Y + y, X + x].Source == Morpslaga.Assault)
                                {
                                    MorpslagaNumber++;
                                    MessageBox.Show("The Morpslaga have been reinforced by 1!", "SLOW baby SLOW!");
                                }
                            }
                            else if (CheckY)
                            {
                                if (Field[Y + y, X].Source == Morpslaga.Alpha)
                                {
                                    MorpslagaNumber++;
                                    MessageBox.Show("The Morpslaga have been reinforced by 1!", "SLOW baby SLOW!");
                                }
                                if (Field[Y + y, X].Source == Morpslaga.Assault)
                                {
                                    MorpslagaNumber++;
                                    MessageBox.Show("The Morpslaga have been reinforced by 1!", "SLOW baby SLOW!");
                                }
                            }
                            else if (CheckX)
                            {
                                if (Field[Y, X + x].Source == Morpslaga.Alpha)
                                {
                                    MorpslagaNumber++;
                                    MessageBox.Show("The Morpslaga have been reinforced by 1!", "SLOW baby SLOW!");
                                }
                                if (Field[Y, X + x].Source == Morpslaga.Assault)
                                {
                                    MorpslagaNumber++;
                                    MessageBox.Show("The Morpslaga have been reinforced by 1!", "SLOW baby SLOW!");
                                }
                            }
                        }
                    }
                }
            }
        }
        public class Enemy1 : Unit
        {            
            public void Select(Image SelectedPicture, TextBlock SelectedName, Label SelectedNumber, Label SelectedStrength, Label SelectedSpecial, RichTextBox SelectedText, Image[,] Field)
            {
                SelectedPicture.Source = Picture;
                SelectedName.Text = Name;
                SelectedName.Foreground = new SolidColorBrush(Colors.Red);
                SelectedNumber.Content = "Warriors: " + Number;
                SelectedStrength.Content = "Strength: " + Strength;
                if (Number <= 10)
                {
                    SelectedSpecial.Content = "Escaping";
                }
                if (Number > 10)
                {
                    SelectedSpecial.Content = "Scouting";
                }
                SelectedText.Selection.Text = Description;
                SelectedText.IsEnabled = true;
                Field[Y, X].Source = Overworld.Selected;
            }
            public void AssaultConclude(Image[,] Field, int EnemyCasualties)
            {
                Number -= EnemyCasualties;
                if (Number <= 0)
                {
                    Field[Y, X].Source = null;
                }
            }

            public int CalculateUnitCasualties()
            {
                return (int)(Number * Strength);
            }

            public void Escape(Image[,] Field, int HersirY, int HersirX)
            {
                if (Y == HersirY - 1)
                {
                    if (Y != Field.GetLowerBound(0))
                    {
                        if (Field[Y - 1, X].Source == null)
                        {
                            Field[Y, X].Source = null;
                            Field[Y - 1, X].Source = Overworld.Alpha;
                            Y -= 1;

                            if (Y != Field.GetLowerBound(0))
                            {
                                if (Field[Y - 1, X].Source == null)
                                {
                                    Field[Y, X].Source = null;
                                    Field[Y - 1, X].Source = Overworld.Alpha;
                                    Y -= 1;
                                }
                            }
                        }
                    }
                    else if (Y == Field.GetLowerBound(0))
                    {
                        if (Field[Y + 2, X].Source == null)
                        {
                            Field[Y, X].Source = null;
                            Field[Y + 2, X].Source = Overworld.Alpha;
                            Y += 2;
                        }
                    }
                }
                else if (Y == HersirY + 1)
                {
                    if (Y != Field.GetUpperBound(0))
                    {
                        if (Field[Y + 1, X].Source == null)
                        {
                            Field[Y, X].Source = null;
                            Field[Y + 1, X].Source = Overworld.Alpha;
                            Y += 1;

                            if (Y != Field.GetUpperBound(0))
                            {
                                if (Field[Y + 1, X].Source == null)
                                {
                                    Field[Y, X].Source = null;
                                    Field[Y + 1, X].Source = Overworld.Alpha;
                                    Y += 1;
                                }
                            }
                        }
                    }
                    else if (Y == Field.GetUpperBound(0))
                    {
                        if (Field[Y - 2, X].Source == null)
                        {
                            Field[Y, X].Source = null;
                            Field[Y - 2, X].Source = Overworld.Alpha;
                            Y -= 2;
                        }
                    }
                }

                if (X == HersirX - 1)
                {
                    if (X != Field.GetLowerBound(1))
                    {
                        if (Field[Y, X - 1].Source == null)
                        {
                            Field[Y, X].Source = null;
                            Field[Y, X - 1].Source = Overworld.Alpha;
                            X -= 1;

                            if (X != Field.GetLowerBound(1))
                            {
                                if (Field[Y, X - 1].Source == null)
                                {
                                    Field[Y, X].Source = null;
                                    Field[Y, X - 1].Source = Overworld.Alpha;
                                    X -= 1;
                                }
                            }
                        }
                    }
                    else if (X == Field.GetLowerBound(1))
                    {
                        if (Field[Y, X + 2].Source == null)
                        {
                            Field[Y, X].Source = null;
                            Field[Y, X + 2].Source = Overworld.Alpha;
                            X += 2;
                        }
                    }
                }
                else if (X == HersirX + 1)
                {
                    if (X != Field.GetUpperBound(1))
                    {
                        if (Field[Y, X + 1].Source == null)
                        {
                            Field[Y, X].Source = null;
                            Field[Y, X + 1].Source = Overworld.Alpha;
                            X += 1;

                            if (X != Field.GetUpperBound(1))
                            {
                                if (Field[Y, X + 1].Source == null)
                                {
                                    Field[Y, X].Source = null;
                                    Field[Y, X + 1].Source = Overworld.Alpha;
                                    X += 1;
                                }
                            }
                        }
                    }
                    else if (X == Field.GetUpperBound(1))
                    {
                        if (Field[Y, X - 2].Source == null)
                        {
                            Field[Y, X].Source = null;
                            Field[Y, X - 2].Source = Overworld.Alpha;
                            X -= 2;
                        }
                    }
                }
            }
            public void Scout(Image[,] Field, int HersirY, int HersirX)
            {
                if (Y == HersirY - 2)
                {
                    if (Field[Y + 1, X].Source == null)
                    {
                        Field[Y, X].Source = null;
                        Field[Y + 1, X].Source = Overworld.Alpha;
                        Y += 1;
                    }
                }
                else if (Y == HersirY + 2)
                {
                    if (Field[Y - 1, X].Source == null)
                    {
                        Field[Y, X].Source = null;
                        Field[Y - 1, X].Source = Overworld.Alpha;
                        Y -= 1;
                    }
                }

                if (X == HersirX - 2)
                {
                    if (Field[Y, X + 1].Source == null)
                    {
                        Field[Y, X].Source = null;
                        Field[Y, X + 1].Source = Overworld.Alpha;
                        X += 1;
                    }
                }
                else if (X == HersirX + 2)
                {
                    if (Field[Y, X - 1].Source == null)
                    {
                        Field[Y, X].Source = null;
                        Field[Y, X - 1].Source = Overworld.Alpha;
                        X -= 1;
                    }
                }
            }

            public bool CheckForEnemy(int HersirY, int HersirX)
            {
                if ((X == HersirX - 1 || X == HersirX + 1) && (Y == HersirY - 1 || Y == HersirY + 1))
                {
                    return true;
                }
                else if ((Y == HersirY - 1 || Y == HersirY + 1) && X == HersirX)
                {
                    return true;
                }
                else if ((X == HersirX - 1 || X == HersirX + 1) && Y == HersirY)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private class Mission
        {
            public string Title { get; set; }
            public string Objective1 { get; set; }
            public bool Objective1Completed { get; set; }
            public string Objective2 { get; set; }
            public bool Objective2Completed { get; set; }
        }
        public class OverworldImage
        {
            public BitmapImage Alpha { get; set; }
            public BitmapImage Selected { get; set; }
            public BitmapImage Assault { get; set; }
        }

        private void MapClean()
        {
            for (int y = 0; y < Field.GetLength(0); y++)
            {
                for (int x = 0; x < Field.GetLength(1); x++)
                {
                    if (Field[y,x].Source == MarchImage)
                    {
                        Field[y, x].Source = null;
                    }
                    else if (Field[y, x].Source == Thegn.Overworld.Assault)
                    {
                        Field[y, x].Source = Thegn.Overworld.Alpha;
                    }
                    else if (Field[y, x].Source == Morpslaga.Overworld.Assault)
                    {
                        Field[y, x].Source = Morpslaga.Overworld.Alpha;
                    }
                    else if (Field[y, x].Source == MissionReceive)
                    {
                        Field[y, x].Source = MissionImage;
                    }
                }
            }
        }

        private void MusicBattle()
        {
            LindisfarneMusic.Open(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "GameResources/ThemeBattle.wav"));
        }
        private void MusicPeace()
        {
            LindisfarneMusic.Open(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "GameResources/ThemePeace.wav"));
        }
        private void MusicLoop(object sender, EventArgs e)
        {
            LindisfarneMusic.Position = TimeSpan.Zero;
            LindisfarneMusic.Play();
        }

        private void ChangeClickImage(string Left, string Right)
        {
            BitmapImage ClickLeft = new BitmapImage(new Uri("Resources/ClickLeft.png", UriKind.Relative));
            BitmapImage ClickRight = new BitmapImage(new Uri("Resources/ClickRight.png", UriKind.Relative));
            BitmapImage ClickNeutral = new BitmapImage(new Uri("Resources/ClickNeutral.png", UriKind.Relative));

            TextLeftClick.Text = Left;
            TextRightClick.Text = Right;

            if (Left == "")
            {
                ImageLeftClick.Source = ClickNeutral;
            }
            else
            {
                ImageLeftClick.Source = ClickLeft;
            }

            if (Right == "")
            {
                ImageRightClick.Source = ClickNeutral;
            }
            else
            {
                ImageRightClick.Source = ClickRight;
            }
        }

        public Player Hersir = new Player() { Y = 0, X = 0, Number = 20, Strength = 0.4, Wounded = 0, Actions = 2, Description = Properties.Resources.HersirDescription, Name = "Hersir", Rank = 0, Picture = new BitmapImage(new Uri("Resources/HersirPicture.png", UriKind.Relative)), Overworld = new OverworldImage { Alpha = new BitmapImage(new Uri("Resources/HersirOverworld.png", UriKind.Relative)), Selected = new BitmapImage(new Uri("Resources/HersirOverworldSelected.png", UriKind.Relative)) } };
        public Enemy0 Thegn = new Enemy0() { Y = 3, X = 3, Number = 20, Strength = 0.25, Description = Properties.Resources.ThegnDescription, Name = "Thegn", Picture = new BitmapImage(new Uri("Resources/ThegnPicture.png", UriKind.Relative)), Overworld = new OverworldImage { Alpha = new BitmapImage(new Uri("Resources/ThegnOverworld.png", UriKind.Relative)), Selected = new BitmapImage(new Uri("Resources/ThegnOverworldSelected.png", UriKind.Relative)), Assault = new BitmapImage(new Uri("Resources/ThegnOverworldAssault.png", UriKind.Relative)) } };
        public Enemy1 Morpslaga = new Enemy1() { Y = 2, X = 5, Number = 15, Strength = 0.5, Description = Properties.Resources.MorpslagaDescription, Name = "Morpslaga", Picture = new BitmapImage(new Uri("Resources/MorpslagaPicture.png",UriKind.Relative)), Overworld = new OverworldImage { Alpha = new BitmapImage(new Uri("Resources/MorpslagaOverworld.png", UriKind.Relative)), Selected = new BitmapImage(new Uri("Resources/MorpslagaOverworldSelected.png", UriKind.Relative)), Assault = new BitmapImage(new Uri("Resources/MorpslagaOverworldAssault.png", UriKind.Relative)) } };

        readonly Mission RaidLindisfarne = new Mission() {Title = "The Raid of Lindisfarne", Objective1 = "Reach the Abby[5, 7]", Objective1Completed = false, Objective2 = "Escape the Island[0, 0]", Objective2Completed = false};
        readonly Mission FireBaptism = new Mission() { Title = "Batism by Fire", Objective1 = "Defeat the Morpslaga", Objective1Completed = false, Objective2 = "Defeat the Thegn", Objective2Completed = false };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Lindisfarne_Loaded(object sender, EventArgs e)
        {
            int NumberOfFieldBox = 0;
            for (int y = 0; y < Field.GetLength(0); y++)
            {
                for (int x = 0; x < Field.GetLength(1); x++)
                {
                    Field[y, x] = (Image)this.FindName(("FieldBox" + NumberOfFieldBox));//Searches for the FieldBox
                    NumberOfFieldBox++;
                    Field[y, x].MouseEnter += FieldBox_MouseEnter;
                    Field[y, x].MouseLeave += FieldBox_MouseLeave;

                    if (x == Field.GetLength(1))
                    {
                        NumberOfFieldBox++;
                    }
                }
            }

            Field[Hersir.Y, Hersir.X].Source = Hersir.Overworld.Alpha;
            Field[Thegn.Y, Thegn.X].Source = Thegn.Overworld.Alpha;
            Field[Morpslaga.Y, Morpslaga.X].Source = Morpslaga.Overworld.Alpha;
            Field[1, 1].Source = MissionImage;

            TextBlockMissionTitle.Text = RaidLindisfarne.Title;
            TextBlockMissionObjective1.Text = RaidLindisfarne.Objective1;
            TextBlockMissionObjective2.Text = RaidLindisfarne.Objective2;
            TextBlockMissionObjective2.Foreground = new SolidColorBrush(Colors.Gray);
            LabelActions.Content = "Actions Left : 2";

            /*            
            LindisfarneInfo1 Info1 = new LindisfarneInfo1();
            Info1.ShowDialog();
            */

            LindisfarneMusic.MediaEnded += new EventHandler(MusicLoop);
            MusicPeace();
            LindisfarneMusic.Play();
        }

        private void FieldBox_MouseEnter(object sender, MouseEventArgs e)
        {
            Image image = sender as Image;
            BitmapImage ClickLeft = new BitmapImage(new Uri("Resources/ClickLeft.png", UriKind.Relative));
            BitmapImage ClickRight = new BitmapImage(new Uri("Resources/ClickRight.png", UriKind.Relative));
            BitmapImage ClickNeutral = new BitmapImage(new Uri("Resources/ClickNeutral.png", UriKind.Relative));

            try
            {
                if (image.Source.ToString() == Hersir.Overworld.Alpha.ToString())
                {
                    ChangeClickImage("Select the Hersir", "");
                }
                else if (image.Source.ToString() == Hersir.Overworld.Selected.ToString())
                {
                    ChangeClickImage("Action Check", "Cancel Actions");
                }

                else if (image.Source.ToString() == Thegn.Overworld.Alpha.ToString())
                {
                    ChangeClickImage("Select the Thegn", "");
                }
                else if (image.Source.ToString() == Thegn.Overworld.Assault.ToString())
                {
                    ChangeClickImage("Attack the Thegn", "");
                }

                else if (image.Source.ToString() == Morpslaga.Overworld.Alpha.ToString())
                {
                    ChangeClickImage("Select the Morpslaga", "");
                }
                else if (image.Source.ToString() == Morpslaga.Overworld.Assault.ToString())
                {
                    ChangeClickImage("Attack the Morpslaga", "");
                }

                else if (image.Source.ToString() == Morpslaga.Overworld.Selected.ToString() || image.Source.ToString() == Thegn.Overworld.Selected.ToString())
                {
                    ChangeClickImage("", "Check Outcome of Attack");
                }

                else if (image.Source.ToString() == MarchImage.ToString())
                {
                    string Location = null;
                    for (int y = 0; y < Field.GetLength(0); y++)
                    {
                        for (int x = 0; x < Field.GetLength(1); x++)
                        {
                            if (Field[y, x] == image)
                            {
                                Location = "Field[" + y + "," + x + "].";
                            }

                        }
                    }

                    ChangeClickImage("March to Location " + Location, "");
                }
            }
            catch
            {

            }
        }
        
        private void FieldBox_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeClickImage("", "");
        }

        private void ButtonEndTurn_Click(object sender, RoutedEventArgs e)
        {
            MapClean();
            Hersir.UpdateActions(LabelActions, true);
            Hersir.RecoverWounded();
            Hersir.Select(SelectedPicture, SelectedNameText, SelectedNumber, SelectedStrength, SelectedSpecial, SelectedText, Field);

            if (Thegn.Number > 0)
            {
                if (Thegn.CheckForEnemy(Morpslaga.Y, Morpslaga.X))
                {
                    int MorpNumber = Morpslaga.Number;
                    Thegn.Reinforce(Field, Morpslaga.Overworld, ref MorpNumber, TurnNumber);
                    Morpslaga.Number = MorpNumber;
                }
                if (Thegn.CheckForEnemy(Hersir.Y, Hersir.X) && Thegn.Number > 10)
                {
                    ThegnSally TS = new ThegnSally(this);
                    TS.ShowDialog();
                }
            }
            if (Morpslaga.Number > 0)
            {
                if (!Morpslaga.CheckForEnemy(Hersir.Y, Hersir.X) && Morpslaga.Number > 10)
                {
                    Morpslaga.Scout(Field, Hersir.Y, Hersir.X);
                    if ((Thegn.CheckForEnemy(Hersir.Y, Hersir.X) || Morpslaga.CheckForEnemy(Hersir.Y, Hersir.X)) && CurrentMusicIsPeace == false)
                    {
                        MusicBattle();
                        LindisfarneMusic.Play();
                        CurrentMusicIsPeace = true;
                    }

                }
                else if (Morpslaga.CheckForEnemy(Hersir.Y, Hersir.X) && Morpslaga.Number <= 10)
                {
                    Morpslaga.Escape(Field, Hersir.Y, Hersir.X);
                    if ((!Thegn.CheckForEnemy(Hersir.Y, Hersir.X) && !Morpslaga.CheckForEnemy(Hersir.Y, Hersir.X)) && CurrentMusicIsPeace == true)
                    {
                        MusicPeace();
                        LindisfarneMusic.Play();
                        CurrentMusicIsPeace = false;
                    }
                }
                else if (Morpslaga.CheckForEnemy(Hersir.Y, Hersir.X) && Morpslaga.Number > 10)
                {
                    MorpslagaAmbush MP = new MorpslagaAmbush(this);
                    MP.ShowDialog();
                }
            }
            
            TurnNumber++;
            
            Hersir.Select(SelectedPicture, SelectedNameText, SelectedNumber, SelectedStrength, SelectedSpecial, SelectedText, Field);
            
            if (Hersir.Number <= 0 && Hersir.Wounded <= 0)
            {
                LindisfarneMusic.Stop();
                Hersir.MissionResult(false, this);
            }
            if (Thegn.Number > 0)
            {
                Field[Thegn.Y, Thegn.X].Source = Thegn.Overworld.Alpha;
            }
            if (Morpslaga.Number > 0)
            {
                Field[Morpslaga.Y, Morpslaga.X].Source = Morpslaga.Overworld.Alpha;
            }
        }

        private void Field_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image image = sender as Image;

            switch (e.ChangedButton)
            {
                case MouseButton.Right:

                    if (image.Source == Hersir.Overworld.Selected)
                    {
                        Hersir.ActionCheck(Field, MarchImage, Thegn.Overworld, Morpslaga.Overworld, MissionImage, MissionReceive);
                    }
                    else if (image.Source == Thegn.Overworld.Selected)
                    {
                        Hersir.AssaultLog(Hersir.Number, Hersir.Strength, Thegn.Number, Thegn.Strength * 2, Hersir.Wounded);
                    }
                    else if (image.Source == Morpslaga.Overworld.Selected)
                    {
                        Hersir.AssaultLog(Hersir.Number, Hersir.Strength, Morpslaga.Number, Morpslaga.Strength, Hersir.Wounded);
                    }
                    break;
                
                case MouseButton.Left:

                    if (image.Source == Hersir.Overworld.Alpha)
                    {
                        Hersir.Select(SelectedPicture, SelectedNameText, SelectedNumber, SelectedStrength, SelectedSpecial, SelectedText, Field);
                        FieldBox_MouseEnter(sender, e);
                        if (Thegn.Number > 0)
                        {
                            Field[Thegn.Y, Thegn.X].Source = Thegn.Overworld.Alpha;
                        }
                        if (Morpslaga.Number > 0)
                        {
                            Field[Morpslaga.Y, Morpslaga.X].Source = Morpslaga.Overworld.Alpha;
                        }
                    }
                    else if (image.Source == Hersir.Overworld.Selected)
                    {
                        MapClean();
                    }

                    else if (image.Source == Thegn.Overworld.Alpha)
                    {
                        Thegn.Select(SelectedPicture, SelectedNameText, SelectedNumber, SelectedStrength, SelectedSpecial, SelectedText, Field, TurnNumber);
                        FieldBox_MouseEnter(sender, e);
                        if (Morpslaga.Number > 0 && Field[Morpslaga.Y, Morpslaga.X].Source.ToString() != Morpslaga.Overworld.Assault.ToString())
                        {
                            Field[Morpslaga.Y, Morpslaga.X].Source = Morpslaga.Overworld.Alpha;
                        }
                        Field[Hersir.Y, Hersir.X].Source = Hersir.Overworld.Alpha;
                    }
                    else if (image.Source == Thegn.Overworld.Assault && Hersir.Actions > 0)
                    {
                        AssaultSF.Play();
                        EnemyAssault ThegnAssault = new EnemyAssault(this,Thegn.Name);
                        ThegnAssault.ShowDialog();

                        if (ThegnAssault.DialogResult == true)
                        {
                            if (Hersir.Number <= 0 && Hersir.Wounded <= 0)
                            {
                                LindisfarneMusic.Stop();
                                Hersir.MissionResult(false, this);
                            }
                            if (Thegn.Number <= 0)
                            {
                                Hersir.RankUp();
                                Field[Thegn.Y, Thegn.X].Source = null;
                                FireBaptism.Objective2Completed = true;

                                if (FireBaptism.Objective1Completed)
                                {
                                    MessageBox.Show("Baptism by Fire has been Completed!", "Mission Finished");
                                    Hersir.RankUp();
                                }
                            }

                            Hersir.Select(SelectedPicture, SelectedNameText, SelectedNumber, SelectedStrength, SelectedSpecial, SelectedText, Field);
                            Hersir.UpdateActions(LabelActions, false);
                            MapClean();
                        }
                    }

                    else if (image.Source == Morpslaga.Overworld.Alpha)
                    {
                        Morpslaga.Select(SelectedPicture, SelectedNameText, SelectedNumber, SelectedStrength, SelectedSpecial, SelectedText, Field);
                        FieldBox_MouseEnter(sender, e);
                        if (Thegn.Number > 0 && Field[Thegn.Y, Thegn.X].Source.ToString() != Thegn.Overworld.Assault.ToString())
                        {
                            Field[Thegn.Y, Thegn.X].Source = Thegn.Overworld.Alpha;
                        }
                        Field[Hersir.Y, Hersir.X].Source = Hersir.Overworld.Alpha;
                    }
                    else if (image.Source == Morpslaga.Overworld.Assault && Hersir.Actions > 0)
                    {
                        AssaultSF.Play();
                        EnemyAssault MorpslagaAssault = new EnemyAssault(this, Morpslaga.Name);
                        MorpslagaAssault.ShowDialog();

                        if (MorpslagaAssault.DialogResult == true)
                        {
                            if (Hersir.Number <= 0 && Hersir.Wounded <= 0)
                            {
                                LindisfarneMusic.Stop();
                                Hersir.MissionResult(false, this);
                            }
                            if (Morpslaga.Number <= 0)
                            {
                                Hersir.RankUp();
                                Field[Morpslaga.Y, Morpslaga.X].Source = null;
                                FireBaptism.Objective1Completed = true;
                                if (FireBaptism.Objective2Completed)
                                {
                                    MessageBox.Show("Baptism by Fire has been Completed!", "Mission Finished");
                                    Hersir.RankUp();
                                }
                            }

                            MapClean();
                            Hersir.Select(SelectedPicture, SelectedNameText, SelectedNumber, SelectedStrength, SelectedSpecial, SelectedText, Field);
                            Hersir.UpdateActions(LabelActions, false);
                        }
                    }

                    else if (image.Source == MarchImage && Hersir.Actions > 0 || image.Source == MissionReceive)
                    {
                        if (image.Source.ToString() == MissionReceive.ToString())
                        {
                            MessageBox.Show("An old man tells the Hersir to kill The Anglo-Saxon warriors... \n New Mission recived:Baptisim by Fire \n Click on the mission title to change missions", "Baptisim by Fire");
                            TextBlockMissionTitle.MouseLeftButtonUp += TextBlockMissionTitle_MouseLeftButtonUp;
                            Hersir.Select(SelectedPicture, SelectedNameText, SelectedNumber, SelectedStrength, SelectedSpecial, SelectedText, Field);
                        }
                        MarchSF.Play();
                        Hersir.MarchConclude(Field, image);
                        FieldBox_MouseEnter(sender, e);
                        MapClean();
                        Hersir.UpdateActions(LabelActions, false);

                        if (Thegn.CheckForEnemy(Hersir.Y, Hersir.X) && Thegn.Number > 0 && CurrentMusicIsPeace == false)
                        {
                            MusicBattle();
                            LindisfarneMusic.Play();
                            CurrentMusicIsPeace = true;
                        }
                        else if (Morpslaga.CheckForEnemy(Hersir.Y, Hersir.X) && Morpslaga.Number > 0 && CurrentMusicIsPeace == false)
                        {
                            MusicBattle();
                            LindisfarneMusic.Play();
                            CurrentMusicIsPeace = true;
                        }
                        else if ((!Thegn.CheckForEnemy(Hersir.Y, Hersir.X) && !Morpslaga.CheckForEnemy(Hersir.Y, Hersir.X)) && CurrentMusicIsPeace == true)
                        {
                            MusicPeace();
                            LindisfarneMusic.Play();
                            CurrentMusicIsPeace = false;
                        }

                        if (Hersir.X == 7 && Hersir.Y == 5 && !RaidLindisfarne.Objective1Completed)
                        {
                            RaidLindisfarne.Objective1Completed = true;
                            TextBlockMissionObjective1.Foreground = new SolidColorBrush(Colors.Gold);
                            TextBlockMissionObjective2.Foreground = new SolidColorBrush(Colors.White);
                        }
                        if (Hersir.X == 0 && Hersir.Y == 0 && RaidLindisfarne.Objective1Completed)
                        {
                            LindisfarneMusic.Stop();
                            Hersir.MissionResult(true, this);
                        }
                    }
                    break;
            }
        }

        private void TextBlockMissionTitle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (TextBlockMissionTitle.Text.ToString() == RaidLindisfarne.Title.ToString())
            {
                TextBlockMissionTitle.Text = FireBaptism.Title;
                TextBlockMissionObjective1.Text = FireBaptism.Objective1;
                TextBlockMissionObjective2.Text = FireBaptism.Objective2;
                if (FireBaptism.Objective1Completed)
                {
                    TextBlockMissionObjective1.Foreground = new SolidColorBrush(Colors.Gold);
                }
                else
                {
                    TextBlockMissionObjective1.Foreground = new SolidColorBrush(Colors.White);
                }
                if (FireBaptism.Objective2Completed)
                {
                    TextBlockMissionObjective2.Foreground = new SolidColorBrush(Colors.Gold);
                }
                else
                {
                    TextBlockMissionObjective2.Foreground = new SolidColorBrush(Colors.White);
                }
            }
            else
            {
                TextBlockMissionTitle.Text = RaidLindisfarne.Title;
                TextBlockMissionObjective1.Text = RaidLindisfarne.Objective1;
                TextBlockMissionObjective2.Text = RaidLindisfarne.Objective2;
                if (RaidLindisfarne.Objective1Completed)
                {
                    TextBlockMissionObjective1.Foreground = new SolidColorBrush(Colors.Gold);
                    TextBlockMissionObjective2.Foreground = new SolidColorBrush(Colors.White);
                }
                else
                {
                    TextBlockMissionObjective1.Foreground = new SolidColorBrush(Colors.White);
                    TextBlockMissionObjective2.Foreground = new SolidColorBrush(Colors.Gray);
                }
            }
        }
    }
}