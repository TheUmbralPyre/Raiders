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
        private Image[,] Field = new Image[6, 8];
        public BitmapImage MarchImage = new BitmapImage(new Uri("Resources/Move.png", UriKind.Relative));
        
        private BitmapImage MissionImage = new BitmapImage(new Uri("Resources/Mission.png", UriKind.Relative));
        private BitmapImage MissionReceive = new BitmapImage(new Uri("Resources/MissionReceive.png", UriKind.Relative));

        public SoundPlayer MarchSF = new SoundPlayer(System.AppDomain.CurrentDomain.BaseDirectory + "GameResources/ThemeMarch.wav");
        private SoundPlayer AssaultSF = new SoundPlayer(System.AppDomain.CurrentDomain.BaseDirectory + "GameResources/ThemeAssaultPlan.wav");
        
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

            public void ActionCheck(Image[,] Field, BitmapImage MarchImage, OverworldImage Thegn, OverworldImage Morpslaga, OverworldImage Unknown , BitmapImage Mission, BitmapImage MissionReceive)
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
                            else if (Field[Y + y, X + x].Source == Thegn.Alpha)
                            {
                                Field[Y + y, X + x].Source = Thegn.Assault;
                            }
                            else if (Field[Y + y, X + x].Source == Morpslaga.Alpha)
                            {
                                Field[Y + y, X + x].Source = Morpslaga.Assault;
                            }
                            else if (Field[Y + y, X + x].Source == Mission)
                            {
                                Field[Y + y, X + x].Source = MissionReceive;
                            }
                            else if (Field[Y + y, X + x].Source == Unknown.Alpha)
                            {
                                Field[Y + y, X + x].Source = Unknown.Assault;
                            }
                        }
                        else if (CheckY)
                        {
                            if (Field[Y + y, X].Source == null)
                            {
                                Field[Y + y, X].Source = MarchImage;
                            }
                            else if (Field[Y + y, X].Source == Thegn.Alpha)
                            {
                                Field[Y + y, X].Source = Thegn.Assault;
                            }
                            else if (Field[Y + y, X].Source == Morpslaga.Alpha)
                            {
                                Field[Y + y, X].Source = Morpslaga.Assault;
                            }
                            else if (Field[Y + y, X].Source == Mission)
                            {
                                Field[Y + y, X].Source = MissionReceive;
                            }
                            else if (Field[Y + y, X].Source == Unknown.Alpha)
                            {
                                Field[Y + y, X].Source = Unknown.Assault;
                            }
                        }
                        else if (CheckX)
                        {
                            if (Field[Y, X + x].Source == null)
                            {
                                Field[Y, X + x].Source = MarchImage;
                            }
                            else if (Field[Y, X + x].Source == Thegn.Alpha)
                            {
                                Field[Y, X + x].Source = Thegn.Assault;
                            }
                            else if (Field[Y, X + x].Source == Morpslaga.Alpha)
                            {
                                Field[Y, X + x].Source = Morpslaga.Assault;
                            }
                            else if (Field[Y, X + x].Source == Mission)
                            {
                                Field[Y, X + x].Source = MissionReceive;
                            }
                            else if (Field[Y, X + x].Source == Unknown.Alpha)
                            {
                                Field[Y, X + x].Source = Unknown.Assault;
                            }
                        }
                    }
                }
            } //Checks what the Hersir can do
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
            } //Changes the Hersir's location to the clicked FieldBox
            public void Select(Image SelectedPicture, TextBlock SelectedName, Label SelectedNumber, Label SelectedStrength, Label SelectedSpecial, RichTextBox SelectedText, Image[,] Field)
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
            } //Shows Warrior info

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

            public void MissionResult(bool Victory, MainWindow MW)
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
            } //Gives either Victory or Defeat

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
            } //Reduce the actions left or increase them depending on wheter it is the end of the turn
            public void RankUp()
            {
                Rank++;
                RankUp rankUp = new RankUp(Rank);

                switch (Rank)
                {
                    case 1:
                        Description = Properties.Resources.HersirDescription1;
                        rankUp.ShowDialog();
                        Overworld.Alpha = new BitmapImage(new Uri("Resources/HersirOverworld1.png", UriKind.Relative));
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
            } //Gives a message based on the newly received rank
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
            } //Show Warrior Info
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
            public void Reinforce(Image[,] Field, OverworldImage Morpslaga, ref int MorpslagaNumber, int TurnNumber)
            {
                if (Number < 20 && TurnNumber % 2 == 0)
                {
                    Number++;
                    if (Number < 20)
                    {
                        Number++;
                        MessageBox.Show("The Thegn have Reinforced themselves by 2!", "Slow, slow baby that's what I like!");
                    }
                    else
                    {
                        MessageBox.Show("The Thegn have Reinforced themselves by 1!", "Slow, slow baby that's what I like!");
                    }
                }
                if (MorpslagaNumber < 18)
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
            } //If turn number is an even number the thegn reinforce themselves and the morpslaga(if they are near the thegn)
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
            } //show Warrior info
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
            } //Run away from the Hersir
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
            } //Chase the Hersir 

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
        public class Enemy2 : Unit
        {
            public void Select(Image SelectedPicture, TextBlock SelectedName, Label SelectedNumber, Label SelectedStrength, Label SelectedSpecial, RichTextBox SelectedText, Image[,] Field)
            {
                SelectedPicture.Source = Picture;
                SelectedName.Text = Name;
                SelectedName.Foreground = new SolidColorBrush(Colors.LightSkyBlue);
                SelectedNumber.Content = "Warriors: " + Number;
                SelectedStrength.Content = "Strength: " + Strength;
                SelectedSpecial.Content = "?";
                SelectedText.Selection.Text = Description;
                SelectedText.IsEnabled = true;
                Field[Y, X].Source = Overworld.Selected;
            }
            public int CalculateUnitCasualties()
            {
                return (int)(Number * Strength);
            }

            public void Scout(Image[,] Field, int HersirY, int HersirX)
            {
                if (HersirY != Field.GetUpperBound(0))
                {
                    Field[Y, X].Source = null;
                    Field[HersirY + 1, HersirX].Source = Overworld.Alpha;
                    Y = HersirY + 1;
                    X = HersirX;
                }
                else
                {
                    Field[Y, X].Source = null;
                    Field[HersirY - 1, HersirX].Source = Overworld.Alpha;
                    Y = HersirY - 1;
                    X = HersirX;
                }
            } //Teleport to Hersir
        }

        private class Mission
        {
            public string Title { get; set; }
            public string Objective1 { get; set; }
            public bool Objective1Completed { get; set; }
            public string Objective2 { get; set; }
            public bool Objective2Completed { get; set; }
            public bool MissionCompleted { get; set; }

            public void Update(TextBlock MissionTitle, TextBlock MissionObjective1, TextBlock MissionObjective2)
            {
                MissionTitle.Text = Title;

                if (Objective1Completed && Objective2Completed)
                {
                    MissionCompleted = true;
                }

                if (Objective1Completed)
                {
                    MissionObjective1.Text = Objective1;
                    MissionObjective1.Foreground = new SolidColorBrush(Colors.Gold);
                }
                else
                {
                    MissionObjective1.Text = Objective1;
                    MissionObjective1.Foreground = new SolidColorBrush(Colors.White);
                }

                if (Objective2Completed)
                {
                    MissionObjective2.Text = Objective2;
                    MissionObjective2.Foreground = new SolidColorBrush(Colors.Gold);
                }
                else
                {
                    MissionObjective2.Text = Objective2;
                    MissionObjective2.Foreground = new SolidColorBrush(Colors.White);
                }
            } //If the objective is completed it has a golden foreground
            public void ShowAdvanceWindow(string Title, string Content)
            {
                MissionAdvance MA = new MissionAdvance(Title, Content);
                MA.ShowDialog();
            } //Opens a new window with text
        }
        public class OverworldImage
        {
            public BitmapImage Alpha { get; set; }
            public BitmapImage Selected { get; set; }
            public BitmapImage Assault { get; set; }
        } //Made to simplify the 3 kinds of images that a warrior has

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
                    else if (Field[y, x].Source == Thegn.Overworld.Assault || Field[y, x].Source == Thegn.Overworld.Selected)
                    {
                        Field[y, x].Source = Thegn.Overworld.Alpha;
                    }
                    else if (Field[y, x].Source == Morpslaga.Overworld.Assault || Field[y, x].Source == Morpslaga.Overworld.Selected)
                    {
                        Field[y, x].Source = Morpslaga.Overworld.Alpha;
                    }
                    else if (Field[y, x].Source == Unknown.Overworld.Assault || Field[y, x].Source == Unknown.Overworld.Selected)
                    {
                        Field[y, x].Source = Unknown.Overworld.Alpha;
                    }
                    else if (Field[y, x].Source == MissionReceive)
                    {
                        Field[y, x].Source = MissionImage;
                    }
                }
            }
        }   //Turns Morpslaga and Thegn Assault and Selected images into their Alpha version, Mission Receive into MissionImage and cleans MarchImage
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
        } //Simplify the MouseEnter_FieldBoxCode

        public Player Hersir = new Player() { Y = 0, X = 0, Number = 28, Strength = 0.30, Wounded = 0, Actions = 2, Description = Properties.Resources.HersirDescription, Name = "Hersir", Rank = 0, Picture = new BitmapImage(new Uri("Resources/HersirPicture.png", UriKind.Relative)), Overworld = new OverworldImage { Alpha = new BitmapImage(new Uri("Resources/HersirOverworld.png", UriKind.Relative)), Selected = new BitmapImage(new Uri("Resources/HersirOverworldSelected.png", UriKind.Relative)) } };
        public Enemy0 Thegn = new Enemy0() { Y = 4, X = 4, Number = 25, Strength = 0.25, Description = Properties.Resources.ThegnDescription, Name = "Thegn", Picture = new BitmapImage(new Uri("Resources/ThegnPicture.png", UriKind.Relative)), Overworld = new OverworldImage { Alpha = new BitmapImage(new Uri("Resources/ThegnOverworld.png", UriKind.Relative)), Selected = new BitmapImage(new Uri("Resources/ThegnOverworldSelected.png", UriKind.Relative)), Assault = new BitmapImage(new Uri("Resources/ThegnOverworldAssault.png", UriKind.Relative)) } };
        public Enemy1 Morpslaga = new Enemy1() { Y = 2, X = 4, Number = 25, Strength = 0.45, Description = Properties.Resources.MorpslagaDescription, Name = "Morpslaga", Picture = new BitmapImage(new Uri("Resources/MorpslagaPicture.png",UriKind.Relative)), Overworld = new OverworldImage { Alpha = new BitmapImage(new Uri("Resources/MorpslagaOverworld.png", UriKind.Relative)), Selected = new BitmapImage(new Uri("Resources/MorpslagaOverworldSelected.png", UriKind.Relative)), Assault = new BitmapImage(new Uri("Resources/MorpslagaOverworldAssault.png", UriKind.Relative)) } };
        public Enemy2 Unknown = new Enemy2() { X = 5, Y = 4, Number = 0, Strength = 0.30, Description = Properties.Resources.UnknownDescription, Name = "?", Picture = new BitmapImage(new Uri("Resources/UnknownPicture.png", UriKind.Relative)), Overworld = new OverworldImage() { Alpha = new BitmapImage(new Uri("Resources/UnknownOverworld.png", UriKind.Relative)), Assault = new BitmapImage(new Uri("Resources/UnknownOverworldAssault.png", UriKind.Relative)), Selected = new BitmapImage(new Uri("Resources/UnknownOverworldSelected.png", UriKind.Relative)) } };
        
        readonly Mission RaidLindisfarne = new Mission() {Title = "The Raid of Lindisfarne", Objective1 = "Reach the Abby[5, 7]", Objective1Completed = false, Objective2 = "Escape the Island[0, 0]", Objective2Completed = false, MissionCompleted = false};
        readonly Mission FireBaptism = new Mission() { Title = "Batism by Fire", Objective1 = "Defeat the Morpslaga", Objective1Completed = false, Objective2 = "Defeat the Thegn", Objective2Completed = false, MissionCompleted = false };
        readonly Mission Truth = new Mission() { Title = "The Truth", Objective1 = "???", Objective1Completed = false, Objective2 = "???", Objective2Completed = false, MissionCompleted = false};

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
                    Field[y, x] = (Image)this.FindName(("FieldBox" + NumberOfFieldBox));                  //Searches for the FieldBox
                    NumberOfFieldBox++;
                    Field[y, x].MouseEnter += FieldBox_MouseEnter;
                    Field[y, x].MouseLeave += FieldBox_MouseLeave;

                    if (x == Field.GetLength(1))
                    {
                        NumberOfFieldBox++;
                    }
                }
            } //Assigns FieldBox elements to Field array so it can be easily manipulated

            Field[Hersir.Y, Hersir.X].Source = Hersir.Overworld.Alpha;
            Field[Thegn.Y, Thegn.X].Source = Thegn.Overworld.Alpha;
            Field[Morpslaga.Y, Morpslaga.X].Source = Morpslaga.Overworld.Alpha;
            Field[1, 1].Source = MissionImage;
            // Shows the warriors on the map and the mission

            RaidLindisfarne.Update(TextBlockMissionTitle, TextBlockMissionObjective1, TextBlockMissionObjective2);
            //Shows Main Mission
        }

        private void FieldBox_MouseEnter(object sender, MouseEventArgs e)
        {
            Image image = sender as Image;
            BitmapImage ClickLeft = new BitmapImage(new Uri("Resources/ClickLeft.png", UriKind.Relative));
            BitmapImage ClickRight = new BitmapImage(new Uri("Resources/ClickRight.png", UriKind.Relative));
            BitmapImage ClickNeutral = new BitmapImage(new Uri("Resources/ClickNeutral.png", UriKind.Relative));

            try
            {
                if (image.Source == Hersir.Overworld.Alpha)
                {
                    ChangeClickImage("Select the Hersir", "");
                }
                else if (image.Source == Hersir.Overworld.Selected)
                {
                    ChangeClickImage("Cancel Actions", "Action Check");
                }

                else if (image.Source == Thegn.Overworld.Alpha)
                {
                    ChangeClickImage("Select the Thegn", "");
                }
                else if (image.Source == Thegn.Overworld.Assault)
                {
                    ChangeClickImage("Attack the Thegn", "");
                }

                else if (image.Source == Morpslaga.Overworld.Alpha)
                {
                    ChangeClickImage("Select the Morpslaga", "");
                }
                else if (image.Source == Morpslaga.Overworld.Assault)
                {
                    ChangeClickImage("Attack the Morpslaga", "");
                }

                else if (image.Source == MarchImage)
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
                    //Finds the Y and X of the targeted MarchImage

                    ChangeClickImage("March to Location " + Location, "");
                }
                else if (image.Source == MissionReceive)
                {
                    ChangeClickImage("Start \"Baptism by Fire\"", "");
                }
            }
            catch
            {
                // FieldBox_MouseEnter can sometimes cause a null exception and crash the game and that is why this try catch is here
            }
        }
        
        private void FieldBox_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeClickImage("", "");
        }

        private void ButtonEndTurn_Click(object sender, RoutedEventArgs e)
        {
            Hersir.UpdateActions(LabelActions, true); //Restores actions to 2
            Hersir.RecoverWounded();
            Hersir.Select(SelectedPicture, SelectedNameText, SelectedNumber, SelectedStrength, SelectedSpecial, SelectedText, Field);

            if (Thegn.Number > 0)
            {
                int MorpNumber = Morpslaga.Number; //Cannot give Morpslaga.Number as a ref
                Thegn.Reinforce(Field, Morpslaga.Overworld, ref MorpNumber, TurnNumber);
                Morpslaga.Number = MorpNumber;

                if (Thegn.CheckForEnemy(Hersir.Y, Hersir.X) && Thegn.Number > 10)
                {
                    AssaultSF.PlaySync();
                    ThegnSally TS = new ThegnSally(this);
                    TS.ShowDialog();
                    TS.Close();
                }
            } 
            if (Morpslaga.Number > 0)
            {
                if (!Morpslaga.CheckForEnemy(Hersir.Y, Hersir.X) && Morpslaga.Number > 10)
                {
                    Morpslaga.Scout(Field, Hersir.Y, Hersir.X);
                }
                else if (Morpslaga.CheckForEnemy(Hersir.Y, Hersir.X) && Morpslaga.Number <= 10)
                {
                    Morpslaga.Escape(Field, Hersir.Y, Hersir.X);
                }
                else if (Morpslaga.CheckForEnemy(Hersir.Y, Hersir.X) && Morpslaga.Number > 10)
                {
                    AssaultSF.PlaySync();
                    MorpslagaAmbush MP = new MorpslagaAmbush(this);
                    MP.ShowDialog();
                    MP.Close();
                }
            }
            if (Unknown.Number > 0)
            {
                Unknown.Scout(Field, Hersir.Y, Hersir.X);
                UnknownSally US = new UnknownSally(this);
                US.ShowDialog();
                US.Close();
                if (Unknown.Number <= 0)
                {
                    Truth.ShowAdvanceWindow("Defeat the Monsters:Completed", "As the monsters fall, The Warriors realize that they have killed their worst enemy-Themselves.");
                    Hersir.RankUp();
                    Truth.ShowAdvanceWindow("The Truth:Completed", "The world returns to normal. The Hersir look at the corpses of the monsters as they slowly turn to ash and disappear. The doubt and sadness of the Hersir slowly begin to disappear. They open their eyes to a beautiful world, one which they hadn't seen in such a long time.The Hersir finally discover their purpose and know that there is only one thing left to do.");
                    Field[Unknown.Y, Unknown.X].Source = null;
                    Truth.Objective2Completed = true;
                    Truth.MissionCompleted = true;
                    RaidLindisfarne.Update(TextBlockMissionTitle, TextBlockMissionObjective1, TextBlockMissionObjective2);
                    TextBlockMissionTitle.MouseLeftButtonUp -= TextBlockMissionTitle_MouseLeftButtonUp;

                    ImageMap.Source = new BitmapImage(new Uri("Resources/MapLindisfarne.jpg", UriKind.Relative));
                } //The other warriors won't attack you when they don't have the number but the Unknown always attack you
            }

            TurnNumber++;
            
            Hersir.Select(SelectedPicture, SelectedNameText, SelectedNumber, SelectedStrength, SelectedSpecial, SelectedText, Field);
            
            if (Hersir.Number <= 0 && Hersir.Wounded <= 0)
            {
                Hersir.MissionResult(false, this);
            }
            MapClean();
        }

        private void FieldBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image image = sender as Image;

            switch (e.ChangedButton)
            {
                case MouseButton.Right:

                    if (image.Source == Hersir.Overworld.Selected)
                    {
                        Hersir.ActionCheck(Field, MarchImage, Thegn.Overworld, Morpslaga.Overworld, Unknown.Overworld, MissionImage, MissionReceive);
                    }
                    break;
                
                case MouseButton.Left:

                    if (image.Source == Hersir.Overworld.Alpha)
                    {
                        Hersir.Select(SelectedPicture, SelectedNameText, SelectedNumber, SelectedStrength, SelectedSpecial, SelectedText, Field);
                        FieldBox_MouseEnter(sender, e);
                        if (Thegn.Number > 0 && Field[Thegn.Y, Thegn.X].Source != Thegn.Overworld.Assault)
                        {
                            Field[Thegn.Y, Thegn.X].Source = Thegn.Overworld.Alpha;
                        }
                        if (Morpslaga.Number > 0 && Field[Morpslaga.Y, Morpslaga.X].Source != Morpslaga.Overworld.Assault)
                        {
                            Field[Morpslaga.Y, Morpslaga.X].Source = Morpslaga.Overworld.Alpha;
                        }
                        if (Unknown.Number > 0 && Field[Unknown.Y, Unknown.X].Source != Unknown.Overworld.Assault)
                        {
                            Field[Unknown.Y, Unknown.X].Source = Unknown.Overworld.Alpha;
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
                        if (Morpslaga.Number > 0 && Field[Morpslaga.Y, Morpslaga.X].Source != Morpslaga.Overworld.Assault)
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
                                Hersir.MissionResult(false, this);
                            }
                            if (Thegn.Number <= 0)
                            {
                                FireBaptism.ShowAdvanceWindow("Defeat the Thegn:Completed", "The Thegn have fallen. Unlike the Morpslaga, their duty was to protect the innocent. However, this doesn't mean that their actions had less meaning. They fought without fear and showed the most honor.When the Hersir looked at the Thegn, they saw something that they lacked.It was something they had been searching for all this time... ");
                                Hersir.RankUp();
                                Field[Thegn.Y, Thegn.X].Source = null;
                                FireBaptism.Objective2Completed = true;
                                FireBaptism.Update(TextBlockMissionTitle, TextBlockMissionObjective1, TextBlockMissionObjective2);

                                if (FireBaptism.Objective1Completed)
                                {
                                    FireBaptism.ShowAdvanceWindow("Baptism by Fire:Finished", "The Hersir have experienced so much in such a short time.They fought against mighty warriors and learned from them.They must rest and contemplate what they have learned.");
                                    FireBaptism.MissionCompleted = true;
                                    Truth.Update(TextBlockMissionTitle, TextBlockMissionObjective1, TextBlockMissionObjective2);
                                    TextBlockMissionObjective1.Text = "Rest at Field[" + Thegn.Y + ", " + Thegn.X + "]";
                                    Truth.ShowAdvanceWindow("The Truth:Started", "The Hersir have decided to rest before advancing any further.");
                                }
                            }

                            Hersir.Select(SelectedPicture, SelectedNameText, SelectedNumber, SelectedStrength, SelectedSpecial, SelectedText, Field);
                            Hersir.UpdateActions(LabelActions, false);
                            MapClean();
                        }
                        ThegnAssault.Close();
                    }

                    else if (image.Source == Morpslaga.Overworld.Alpha)
                    {
                        Morpslaga.Select(SelectedPicture, SelectedNameText, SelectedNumber, SelectedStrength, SelectedSpecial, SelectedText, Field);
                        FieldBox_MouseEnter(sender, e);
                        if (Thegn.Number > 0 && Field[Thegn.Y, Thegn.X].Source != Thegn.Overworld.Assault)
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
                                Hersir.MissionResult(false, this);
                            }
                            if (Morpslaga.Number <= 0)
                            {
                                FireBaptism.ShowAdvanceWindow("Defeat The Morpslaga:Finished", "The Morpslaga have fallen. They died thinking that they didn't regain honor, but they fought the Hersir long enough for the civilians of Lindisfarne to escape. They could have escaped Lindisfarne, but they choose to die a hero's death. The all-father will welcome them in his hall. But the Morpslaga weren't the only ones who gained something...");
                                Hersir.RankUp();
                                Field[Morpslaga.Y, Morpslaga.X].Source = null;
                                FireBaptism.Objective1Completed = true;
                                FireBaptism.Update(TextBlockMissionTitle, TextBlockMissionObjective1, TextBlockMissionObjective2);

                                if (FireBaptism.Objective2Completed)
                                {
                                    FireBaptism.ShowAdvanceWindow("Baptism by Fire:Finished", "The Hersir have experienced so much in such a short time.They fought against mighty warriors and learned from them.");
                                    FireBaptism.MissionCompleted = true;
                                    Truth.Update(TextBlockMissionTitle, TextBlockMissionObjective1, TextBlockMissionObjective2);
                                    TextBlockMissionObjective1.Text = "Rest at Field[" + Thegn.Y + ", " + Thegn.X + "]";
                                    Truth.ShowAdvanceWindow("The Truth:Started", "The Honor, Duty, and Sacrifice displayed by their enemies baffled the Hersir. They decided to rest and contemplate on the happenings of this strange day.");
                                }
                            }

                            MapClean();
                            Hersir.Select(SelectedPicture, SelectedNameText, SelectedNumber, SelectedStrength, SelectedSpecial, SelectedText, Field);
                            Hersir.UpdateActions(LabelActions, false);
                        }
                        MorpslagaAssault.Close();
                    }

                    else if (image.Source == Unknown.Overworld.Alpha)
                    {
                        Unknown.Select(SelectedPicture, SelectedNameText, SelectedNumber, SelectedStrength, SelectedSpecial, SelectedText, Field);
                        FieldBox_MouseEnter(sender, e);
                        Field[Hersir.Y, Hersir.X].Source = Hersir.Overworld.Alpha;
                    }
                    else if (image.Source == Unknown.Overworld.Assault && Hersir.Actions > 0)
                    {
                        AssaultSF.Play();
                        EnemyAssault UnknownAssault = new EnemyAssault(this, Unknown.Name);
                        UnknownAssault.ShowDialog();

                        if (UnknownAssault.DialogResult == true)
                        {
                            if (Hersir.Number <= 0 && Hersir.Wounded <= 0)
                            {
                                Hersir.MissionResult(false, this);
                            }
                            if (Unknown.Number <= 0)
                            {
                                Truth.ShowAdvanceWindow("Defeat the Monsters:Completed", "As the monsters fall, The Warriors realize that they have killed their worst enemy-Themselves.");
                                Hersir.RankUp();
                                Truth.ShowAdvanceWindow("The Truth:Completed", "The world returns to normal. The Hersir look at the corpses of the monsters as they slowly turn to ash and disappear. The doubt and sadness of the Hersir slowly begin to disappear. They open their eyes to a beautiful world, one which they hadn't seen in such a long time.The Hersir finally discover their purpose and know that there is only one thing left to do.");
                                Field[Unknown.Y, Unknown.X].Source = null;
                                Truth.Objective2Completed = true;
                                Truth.MissionCompleted = true;
                                RaidLindisfarne.Update(TextBlockMissionTitle, TextBlockMissionObjective1, TextBlockMissionObjective2);
                                TextBlockMissionTitle.MouseLeftButtonUp -= TextBlockMissionTitle_MouseLeftButtonUp;

                                ImageMap.Source = new BitmapImage(new Uri("Resources/MapLindisfarne.jpg", UriKind.Relative));
                            }

                            Hersir.Select(SelectedPicture, SelectedNameText, SelectedNumber, SelectedStrength, SelectedSpecial, SelectedText, Field);
                            Hersir.UpdateActions(LabelActions, false);
                            MapClean();
                        }
                        UnknownAssault.Close();
                    }

                    else if (image.Source == MarchImage && Hersir.Actions > 0 || image.Source == MissionReceive)
                    {
                        if (image.Source.ToString() == MissionReceive.ToString())
                        {
                            FireBaptism.ShowAdvanceWindow("Baptism by Fire:Started", "As the Hersir slowly familiarize themselves with the newly-found world, they feel something. The feeling is a nostalgic one, one that they haven't tasted for a long time. Their blood starts to boil as they realize what the mysterious feeling is...Determination!");
                            TextBlockMissionTitle.MouseLeftButtonUp += TextBlockMissionTitle_MouseLeftButtonUp;
                            Hersir.Select(SelectedPicture, SelectedNameText, SelectedNumber, SelectedStrength, SelectedSpecial, SelectedText, Field);
                        }
                        MarchSF.Play();
                        Hersir.MarchConclude(Field, image);
                        FieldBox_MouseEnter(sender, e);
                        MapClean();
                        Hersir.UpdateActions(LabelActions, false);

                        if (Hersir.Y == 5 && Hersir.X == 7 && !RaidLindisfarne.Objective1Completed && Truth.MissionCompleted)
                        {
                            RaidLindisfarne.Objective1Completed = true;
                            RaidLindisfarne.ShowAdvanceWindow("Reach the Abby:Completed", "As our Warriors breach the gates and enter the abbey, they discover that everyone has already left. They awe at the beauty of the architecture and gasp at the complexity. The Hersir enter the main hall only to find unimaginable amounts of treasure.");

                            RaidLindisfarne.Update(TextBlockMissionTitle, TextBlockMissionObjective1, TextBlockMissionObjective2);
                        }
                        else if (Hersir.Y == 0 && Hersir.X == 0 && RaidLindisfarne.Objective1Completed)
                        {
                            RaidLindisfarne.ShowAdvanceWindow("The Raid of Lindisfarne:Finished ", "Before leaving, the remaining Hersir locate and bury the bodies of their fallen brethren,the Morpslaga and the Thegn. Spiritually empowered and materialistically rich, the Heroes depart from Lindisfarne, and our story ends...");
                            Hersir.MissionResult(true, this);
                        }
                        else if (Hersir.Y == Thegn.Y && Hersir.X == Thegn.X && FireBaptism.MissionCompleted && !Truth.Objective1Completed)
                        {
                            ImageMap.Source = new BitmapImage(new Uri("Resources/MapLindisfarneNight.jpg", UriKind.Relative));

                            Truth.ShowAdvanceWindow("Rest:Finished", "As the Warriors begin to rest and regain their energy, the world around them appears to change. They see others near their location. After investigating the appearance of the Others, they realize that the Others look exactly like them.");
                            Truth.Objective1Completed = true;
                            Truth.Update(TextBlockMissionTitle, TextBlockMissionObjective1, TextBlockMissionObjective2);
                            TextBlockMissionObjective2.Text = "Defeat the Monsters";

                            Field[Unknown.Y, Unknown.X].Source = Unknown.Overworld.Alpha;
                            Unknown.Number = Hersir.Number * 3;
                        }
                    }
                    break;
            }
        }

        private void TextBlockMissionTitle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (TextBlockMissionTitle.Text.ToString() == RaidLindisfarne.Title.ToString())
            {
                if (!FireBaptism.MissionCompleted)
                {
                    FireBaptism.Update(TextBlockMissionTitle, TextBlockMissionObjective1, TextBlockMissionObjective2);
                }
                else
                {
                    Truth.Update(TextBlockMissionTitle, TextBlockMissionObjective1, TextBlockMissionObjective2);
                    if (Truth.Objective1Completed)
                    {
                        TextBlockMissionObjective2.Text = "Defeat the Monsters";
                    }
                    else
                    {
                        TextBlockMissionObjective1.Text = "Rest at Field[" + Thegn.Y + ", " + Thegn.X + "]";
                    }
                }
            }
            else if (!FireBaptism.MissionCompleted)
            {
                RaidLindisfarne.Update(TextBlockMissionTitle, TextBlockMissionObjective1, TextBlockMissionObjective2);
            }
            else
            {
                RaidLindisfarne.Update(TextBlockMissionTitle, TextBlockMissionObjective1, TextBlockMissionObjective2);
            }
        }
    }
}