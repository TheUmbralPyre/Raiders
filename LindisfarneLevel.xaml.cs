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

namespace Raiders_2._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Image[,] Field = new Image[6,8];
        private BitmapImage MarchImage = new BitmapImage(new Uri("Resources/Move.png", UriKind.Relative));
        private BitmapImage MissionMarker = new BitmapImage(new Uri("Resources/MissionMarker.png", UriKind.Relative));
        private int TurnNumber = 1;

        public abstract class Unit
        {
            public int X { get; set; }
            public int Y { get; set; }
            public BitmapImage Overworld { get; set; }
            public BitmapImage OverworldSelected { get; set; }
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

            public void ActionCheck(Image[,] Field, BitmapImage MarchImage, BitmapImage Thegn, BitmapImage Morpslaga, BitmapImage ThegnAssault, BitmapImage MorpslagaAssault)
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
                            if (Field[Y + y, X + x].Source == Thegn)
                            {
                                Field[Y + y, X + x].Source = ThegnAssault;
                            }
                            if (Field[Y + y, X + x].Source == Morpslaga)
                            {
                                Field[Y + y, X + x].Source = MorpslagaAssault;
                            }
                        }
                        else if (CheckY)
                        {
                            if (Field[Y + y, X].Source == null)
                            {
                                Field[Y + y, X].Source = MarchImage;
                            }
                            if (Field[Y + y, X].Source == Thegn)
                            {
                                Field[Y + y, X].Source = ThegnAssault;
                            }
                            if (Field[Y + y, X].Source == Morpslaga)
                            {
                                Field[Y + y, X].Source = MorpslagaAssault;
                            }
                        }
                        else if (CheckX)
                        {
                            if (Field[Y, X + x].Source == null)
                            {
                                Field[Y, X + x].Source = MarchImage;
                            }
                            if (Field[Y, X + x].Source == Thegn)
                            {
                                Field[Y, X + x].Source = ThegnAssault;
                            }
                            if (Field[Y, X + x].Source == Morpslaga)
                            {
                                Field[Y, X + x].Source = MorpslagaAssault;
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
            public void Select(Image SelectedPicture, Label SelectedName, Label SelectedNumber, Label SelectedStrength, Label SelectedSpecial, RichTextBox SelectedText,Image[,] Field)
            {
                SelectedPicture.Source = Picture;
                SelectedName.Content = Name;
                SelectedName.Foreground = new SolidColorBrush(Colors.LightSkyBlue);
                SelectedNumber.Content = "Warriors: " + Number;
                SelectedStrength.Content = "Strength: " + Strength;
                SelectedSpecial.Content = "Wounded: " + Wounded;
                SelectedText.Selection.Text = Description;
                SelectedText.IsEnabled = true;
                Field[Y, X].Source = OverworldSelected;
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
                int EnemyCasualties = (int)((Number * Strength) + (Wounded * (Strength / 2)));
                EnemyCasualties += (int)(EnemyCasualties * FormationBonus); 
                return EnemyCasualties; 
            }
            public int CalculateWounded(int HersirCasualties)
            {
                int wounded = Wounded;

                if (Wounded == 0)
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
                }
                else
                {
                    Actions = 2;
                }
                LabelActions.Content = "Actions Left : " + Actions;
            }           
        }
        public class Enemy0 : Unit
        {
            public BitmapImage OverworldAssault { get; set; }
            
            public void Select(Image SelectedPicture, Label SelectedName, Label SelectedNumber, Label SelectedStrength, Label SelectedSpecial, RichTextBox SelectedText, Image[,] Field, int TurnNumber)
            {
                SelectedPicture.Source = Picture;
                SelectedName.Content = Name;
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
                Field[Y, X].Source = OverworldSelected;
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
            public void Reinforce(Image[,] Field, BitmapImage Morpslaga, BitmapImage MorpslagaAssault,ref int MorpslagaNumber, int TurnNumber)
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
                                if (Field[Y + y, X + x].Source == Morpslaga)
                                {
                                    MorpslagaNumber++;
                                    MessageBox.Show("The Morpslaga have been reinforced by 1!", "SLOW baby SLOW!");
                                }
                                if (Field[Y + y, X + x].Source == MorpslagaAssault)
                                {
                                    MorpslagaNumber++;
                                    MessageBox.Show("The Morpslaga have been reinforced by 1!", "SLOW baby SLOW!");
                                }
                            }
                            else if (CheckY)
                            {
                                if (Field[Y + y, X].Source == Morpslaga)
                                {
                                    MorpslagaNumber++;
                                    MessageBox.Show("The Morpslaga have been reinforced by 1!", "SLOW baby SLOW!");
                                }
                                if (Field[Y + y, X].Source == MorpslagaAssault)
                                {
                                    MorpslagaNumber++;
                                    MessageBox.Show("The Morpslaga have been reinforced by 1!", "SLOW baby SLOW!");
                                }
                            }
                            else if (CheckX)
                            {
                                if (Field[Y, X + x].Source == Morpslaga)
                                {
                                    MorpslagaNumber++;
                                    MessageBox.Show("The Morpslaga have been reinforced by 1!", "SLOW baby SLOW!");
                                }
                                if (Field[Y, X + x].Source == MorpslagaAssault)
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
            public BitmapImage OverworldAssault { get; set; }
            
            public void Select(Image SelectedPicture, Label SelectedName, Label SelectedNumber, Label SelectedStrength, Label SelectedSpecial, RichTextBox SelectedText, Image[,] Field)
            {
                SelectedPicture.Source = Picture;
                SelectedName.Content = Name;
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
                Field[Y, X].Source = OverworldSelected;
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
                            Field[Y - 1, X].Source = Overworld;
                            Y -= 1;

                            if (Y != Field.GetLowerBound(0))
                            {
                                if (Field[Y - 1, X].Source == null)
                                {
                                    Field[Y, X].Source = null;
                                    Field[Y - 1, X].Source = Overworld;
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
                            Field[Y + 2, X].Source = Overworld;
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
                            Field[Y + 1, X].Source = Overworld;
                            Y += 1;

                            if (Y != Field.GetUpperBound(0))
                            {
                                if (Field[Y + 1, X].Source == null)
                                {
                                    Field[Y, X].Source = null;
                                    Field[Y + 1, X].Source = Overworld;
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
                            Field[Y - 2, X].Source = Overworld;
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
                            Field[Y, X - 1].Source = Overworld;
                            X -= 1;

                            if (X != Field.GetLowerBound(1))
                            {
                                if (Field[Y, X - 1].Source == null)
                                {
                                    Field[Y, X].Source = null;
                                    Field[Y, X - 1].Source = Overworld;
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
                            Field[Y, X + 2].Source = Overworld;
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
                            Field[Y, X + 1].Source = Overworld;
                            X += 1;

                            if (X != Field.GetUpperBound(1))
                            {
                                if (Field[Y, X + 1].Source == null)
                                {
                                    Field[Y, X].Source = null;
                                    Field[Y, X + 1].Source = Overworld;
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
                            Field[Y, X - 2].Source = Overworld;
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
                        Field[Y + 1, X].Source = Overworld;
                        Y += 1;
                    }
                }
                else if (Y == HersirY + 2)
                {
                    if (Field[Y - 1, X].Source == null)
                    {
                        Field[Y, X].Source = null;
                        Field[Y - 1, X].Source = Overworld;
                        Y -= 1;
                    }
                }

                if (X == HersirX - 2)
                {
                    if (Field[Y, X + 1].Source == null)
                    {
                        Field[Y, X].Source = null;
                        Field[Y, X + 1].Source = Overworld;
                        X += 1;
                    }
                }
                else if (X == HersirX + 2)
                {
                    if (Field[Y, X - 1].Source == null)
                    {
                        Field[Y, X].Source = null;
                        Field[Y, X - 1].Source = Overworld;
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
                    else if (Field[y, x].Source == Thegn.OverworldAssault)
                    {
                        Field[y, x].Source = Thegn.Overworld;
                    }
                    else if (Field[y, x].Source == Morpslaga.OverworldAssault)
                    {
                        Field[y, x].Source = Morpslaga.Overworld;
                    }

                }
            }
        }

        public Player Hersir = new Player() { Y = 2, X = 2, Number = 20, Strength = 0.4, Wounded = 0, Actions = 20, Description = Properties.Resources.HersirDescription, Name = "Hersir", Overworld = new BitmapImage(new Uri("Resources/HersirOverworld.png", UriKind.Relative)), Picture = new BitmapImage(new Uri("Resources/HersirPicture.jpg", UriKind.Relative)), OverworldSelected = new BitmapImage(new Uri("Resources/HersirOverworldSelected.png", UriKind.Relative)) };
        public Enemy0 Thegn = new Enemy0() { Y = 3, X = 2, Number = 20, Strength = 0.25, Description = Properties.Resources.ThegnDescription, Name = "Thegn", Overworld = new BitmapImage(new Uri("Resources/ThegnOverworld.png", UriKind.Relative)), Picture = new BitmapImage(new Uri("Resources/ThegnPicture.jpg", UriKind.Relative)), OverworldSelected = new BitmapImage(new Uri("Resources/ThegnOverworldSelected.png", UriKind.Relative)), OverworldAssault = new BitmapImage(new Uri("Resources/ThegnOverworldAssault.png", UriKind.Relative))};
        public Enemy1 Morpslaga = new Enemy1() { Y = 3, X = 1, Number = 15, Strength = 0.5, Description = Properties.Resources.MorpslagaDescription, Name = "Morpslaga", Overworld = new BitmapImage(new Uri("Resources/MorpslagaOverworld.png", UriKind.Relative)), Picture = new BitmapImage(new Uri("Resources/MorpslagaPicture.jpg", UriKind.Relative)), OverworldSelected = new BitmapImage(new Uri("Resources/MorpslagaOverworldSelected.png", UriKind.Relative)), OverworldAssault = new BitmapImage(new Uri("Resources/MorpslagaOverworldAssault.png", UriKind.Relative))};

        readonly Mission RaidLindisfarne = new Mission() {Title = "The Raid of Lindisfarne", Objective1 = "Reach the Abby", Objective1Completed = false, Objective2 = "Escape the Island", Objective2Completed = false};
        readonly Mission FireBaptism = new Mission() { Title = "Batism by Fire", Objective1 = "Defeat the Morpslaga", Objective1Completed = false, Objective2 = "Defeat the Thegn", Objective2Completed = false };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Lindisfarne_Initialized(object sender, EventArgs e)
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

            Field[Hersir.Y, Hersir.X].Source = Hersir.Overworld;
            Field[Thegn.Y, Thegn.X].Source = Thegn.Overworld;
            Field[Morpslaga.Y, Morpslaga.X].Source = Morpslaga.Overworld;

            TextBlockMissionTitle.Text = RaidLindisfarne.Title;
            TextBlockMissionObjective1.Text = RaidLindisfarne.Objective1;
            TextBlockMissionObjective2.Text = RaidLindisfarne.Objective2;
            TextBlockMissionObjective2.Foreground = new SolidColorBrush(Colors.Gray);
            LabelActions.Content = "Actions Left : 2";

            /*
            LindisfarneInfo1 Info1 = new LindisfarneInfo1();
            Info1.ShowDialog();
            */

        }

        private void FieldBox_MouseEnter(object sender, MouseEventArgs e)
        {
            Image image = sender as Image;

            if (image.Source.ToString() == Hersir.Overworld.ToString())
            {
                TextRightClick.Text = "";
                TextLeftClick.Text = "Select the Hersir";
            }
            else if (image.Source.ToString() == Hersir.OverworldSelected.ToString())
            {
                TextRightClick.Text = "Action Check";
                TextLeftClick.Text = "Cancel Actions";
            }           
            
            else if (image.Source.ToString() == Thegn.Overworld.ToString())
            {
                TextLeftClick.Text = "Select the Thegn";
                TextRightClick.Text = "";
            }
            else if (image.Source.ToString() == Thegn.OverworldAssault.ToString())
            {
                TextLeftClick.Text = "Attack the Thegn";
                TextRightClick.Text = "";
            }
            
            else if (image.Source.ToString() == Morpslaga.Overworld.ToString())
            {
                TextLeftClick.Text = "Select the Morpslaga";
                TextRightClick.Text = "";
            }
            else if (image.Source.ToString() == Morpslaga.OverworldAssault.ToString())
            {
                TextLeftClick.Text = "Attack the Morpslaga";
                TextRightClick.Text = "";
            }
            
            else if (image.Source.ToString() == Morpslaga.OverworldSelected.ToString() || image.Source.ToString() == Thegn.OverworldSelected.ToString())
            {
                TextLeftClick.Text = "";
                TextRightClick.Text = "Check Outcome of Attack";
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

                TextLeftClick.Text = "March To Location " + Location;
                TextRightClick.Text = "";
            }
        }
        private void FieldBox_MouseLeave(object sender, MouseEventArgs e)
        {
            TextLeftClick.Text = "";
            TextRightClick.Text = "";
        }

        private void ButtonEndTurn_Click(object sender, RoutedEventArgs e)
        {
            MapClean();
            Hersir.UpdateActions(LabelActions, true);
            Hersir.RecoverWounded();
            Hersir.Select(SelectedPicture, SelectedName, SelectedNumber, SelectedStrength, SelectedSpecial, SelectedText, Field);

            if (Thegn.Number > 0)
            {
                if (Thegn.CheckForEnemy(Morpslaga.Y, Morpslaga.X))
                {
                    int MorpNumber = Morpslaga.Number;
                    Thegn.Reinforce(Field, Morpslaga.Overworld, Morpslaga.OverworldAssault, ref MorpNumber, TurnNumber);
                    Morpslaga.Number = MorpNumber;
                }
                if (Thegn.CheckForEnemy(Hersir.Y, Hersir.X) && Thegn.Number > 10)
                {
                    MessageBox.Show("The Thegn have sallied you!");

                    int UnitCasualties = (int)(Thegn.Number * Thegn.Strength);
                    int EnemyCasualties = (int)((Hersir.Number * Hersir.Strength) + (Hersir.Wounded * (Hersir.Strength / 2)));

                    Hersir.AssaultLog(Hersir.Number, Hersir.Strength, Thegn.Number, Thegn.Strength, Hersir.Wounded);
                    Hersir.AssaultConclude(UnitCasualties);
                    Thegn.AssaultConclude(Field, EnemyCasualties);
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
                    MessageBox.Show("The Morpslaga have Ambushed you!");

                    int UnitCasualties = (int)(Morpslaga.Number * Morpslaga.Strength);
                    int EnemyCasualties = (int)((Hersir.Number * Hersir.Strength) + (Hersir.Wounded * (Hersir.Strength / 2)));

                    Hersir.AssaultLog(Hersir.Number, Hersir.Strength, Morpslaga.Number, Morpslaga.Strength, Hersir.Wounded);
                    Hersir.AssaultConclude(UnitCasualties);
                    Morpslaga.AssaultConclude(Field, EnemyCasualties);
                }
            }
            
            TurnNumber++;
            
            Hersir.Select(SelectedPicture, SelectedName, SelectedNumber, SelectedStrength, SelectedSpecial, SelectedText, Field);
            if (Thegn.Number > 0)
            {
                Field[Thegn.Y, Thegn.X].Source = Thegn.Overworld;
            }
            if (Morpslaga.Number > 0)
            {
                Field[Morpslaga.Y, Morpslaga.X].Source = Morpslaga.Overworld;
            }
        }

        private void Field_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image image = sender as Image;

            if (e.ChangedButton == MouseButton.Right)
            {
                if (image.Source == Hersir.OverworldSelected)
                {
                    Hersir.ActionCheck(Field, MarchImage,Thegn.Overworld,Morpslaga.Overworld,Thegn.OverworldAssault,Morpslaga.OverworldAssault);
                }
                else if (image.Source == Thegn.OverworldSelected)
                {
                    Hersir.AssaultLog(Hersir.Number, Hersir.Strength, Thegn.Number, Thegn.Strength * 2, Hersir.Wounded);
                }
                else if (image.Source == Morpslaga.OverworldSelected)
                {
                    Hersir.AssaultLog(Hersir.Number, Hersir.Strength, Morpslaga.Number, Morpslaga.Strength, Hersir.Wounded);
                }
            }
            if (e.ChangedButton == MouseButton.Left)
            {
                if (image.Source.ToString() == Hersir.Overworld.ToString())
                {
                    Hersir.Select(SelectedPicture,SelectedName,SelectedNumber,SelectedStrength,SelectedSpecial,SelectedText, Field);
                    FieldBox_MouseEnter(sender, e);
                    if (Thegn.Number > 0)
                    {
                        Field[Thegn.Y, Thegn.X].Source = Thegn.Overworld;
                    }
                    if (Morpslaga.Number > 0)
                    {
                        Field[Morpslaga.Y, Morpslaga.X].Source = Morpslaga.Overworld;
                    }
                }
                else if (image.Source.ToString() == Hersir.OverworldSelected.ToString())
                {
                    MapClean();
                }

                else if (image.Source.ToString() == Thegn.Overworld.ToString())
                {
                    Thegn.Select(SelectedPicture, SelectedName, SelectedNumber, SelectedStrength, SelectedSpecial, SelectedText, Field, TurnNumber);
                    FieldBox_MouseEnter(sender, e);
                    if (Morpslaga.Number > 0 && Field[Morpslaga.Y, Morpslaga.X].Source.ToString() != Morpslaga.OverworldAssault.ToString())
                    {
                        Field[Morpslaga.Y, Morpslaga.X].Source = Morpslaga.Overworld;
                    }
                    Field[Hersir.Y, Hersir.X].Source = Hersir.Overworld;
                }
                else if (image.Source.ToString() == Thegn.OverworldAssault.ToString() && Hersir.Actions > 0)
                {
                    Enemy0Assault ThegnAssault = new Enemy0Assault(this);
                    ThegnAssault.ShowDialog();
                    Hersir.Select(SelectedPicture, SelectedName, SelectedNumber, SelectedStrength, SelectedSpecial, SelectedText, Field);
                    Hersir.UpdateActions(LabelActions, false);
                    MapClean();
                    
                    if (Hersir.Number <= 0)
                    {
                        Hersir.MissionResult(false, this);
                    }
                    if (Thegn.Number <= 0)
                    {
                        Field[Thegn.Y, Thegn.X].Source = null;
                        Thegn.X = 0;
                        Thegn.Y = 0;
                    }
                }

                else if (image.Source.ToString() == Morpslaga.Overworld.ToString())
                {
                    Morpslaga.Select(SelectedPicture, SelectedName, SelectedNumber, SelectedStrength, SelectedSpecial, SelectedText, Field);
                    FieldBox_MouseEnter(sender, e);
                    if (Thegn.Number > 0 && Field[Thegn.Y, Thegn.X].Source.ToString() != Thegn.OverworldAssault.ToString())
                    {
                        Field[Thegn.Y, Thegn.X].Source = Thegn.Overworld;
                    }
                    Field[Hersir.Y, Hersir.X].Source = Hersir.Overworld;
                }
                else if (image.Source.ToString() == Morpslaga.OverworldAssault.ToString() && Hersir.Actions > 0)
                {
                    Enemy1Assault MorpslagaAssault = new Enemy1Assault(this);
                    MorpslagaAssault.ShowDialog();
                    MapClean();
                    Hersir.Select(SelectedPicture, SelectedName, SelectedNumber, SelectedStrength, SelectedSpecial, SelectedText, Field);
                    Hersir.UpdateActions(LabelActions, false);

                    if (Hersir.Number <= 0)
                    {
                        Hersir.MissionResult(false, this);
                    }
                    if (Morpslaga.Number <= 0)
                    {
                        Field[Morpslaga.Y, Morpslaga.X].Source = null;
                        Morpslaga.Y = 0;
                        Morpslaga.X = 0;
                    }
                }

                else if (image.Source.ToString() == MarchImage.ToString() && Hersir.Actions > 0)
                {
                    Hersir.MarchConclude(Field, image);
                    FieldBox_MouseEnter(sender, e);
                    MapClean();
                    Hersir.UpdateActions(LabelActions, false);          
                    
                    if (Hersir.X == 7 && Hersir.Y == 5 && !RaidLindisfarne.Objective1Completed)
                    {
                        RaidLindisfarne.Objective1Completed = true;
                        TextBlockMissionObjective1.Foreground = new SolidColorBrush(Colors.Gold);
                        TextBlockMissionObjective2.Foreground = new SolidColorBrush(Colors.White);
                    }
                    if (Hersir.X == 0 && Hersir.Y == 0 && RaidLindisfarne.Objective1Completed)
                    {
                        Hersir.MissionResult(true, this);
                    }
                }
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

        private void TextBlockMissionObjective1_MouseEnter(object sender, MouseEventArgs e)
        {
            if (TextBlockMissionObjective1.Text.ToString() == RaidLindisfarne.Objective1 && !RaidLindisfarne.Objective1Completed)
            {
                MapClean();
                Field[5, 7].Source = MissionMarker;
            }
            else if (TextBlockMissionObjective1.Text.ToString() == FireBaptism.Objective1 && !FireBaptism.Objective1Completed)
            {
                MapClean();
                Morpslaga.Select(SelectedPicture, SelectedName, SelectedNumber, SelectedStrength, SelectedSpecial, SelectedText, Field);
                Field[Hersir.Y, Hersir.X].Source = Hersir.Overworld;
                if (Thegn.Number > 0)
                {
                    Field[Thegn.Y, Thegn.X].Source = Thegn.Overworld;
                }
            }
        }
        private void TextBlockMissionObjective1_MouseLeave(object sender, MouseEventArgs e)
        {
            if (TextBlockMissionObjective1.Text.ToString() == RaidLindisfarne.Objective1 && !RaidLindisfarne.Objective1Completed)
            {
                if (Morpslaga.Y == 5 && Morpslaga.X == 7)
                {
                    Field[5, 7].Source = Morpslaga.Overworld;
                }
                else
                {
                    Field[5, 7].Source = null;
                }
            }
        }

        private void TextBlockMissionObjective2_MouseEnter(object sender, MouseEventArgs e)
        {
            if (TextBlockMissionObjective2.Text.ToString() == RaidLindisfarne.Objective2 && RaidLindisfarne.Objective1Completed)
            {
                MapClean();
                Field[0, 0].Source = MissionMarker;
            }
            else if (TextBlockMissionObjective2.Text.ToString() == FireBaptism.Objective2)
            {
                MapClean();
                Thegn.Select(SelectedPicture, SelectedName, SelectedNumber, SelectedStrength, SelectedSpecial, SelectedText, Field, TurnNumber);
                Field[Hersir.Y, Hersir.X].Source = Hersir.Overworld;
                if (Morpslaga.Number > 0)
                {
                    Field[Morpslaga.Y, Morpslaga.X].Source = Morpslaga.Overworld;
                }
            }
        }
        private void TextBlockMissionObjective2_MouseLeave(object sender, MouseEventArgs e)
        {
            if (TextBlockMissionObjective2.Text.ToString() == RaidLindisfarne.Objective2 && RaidLindisfarne.Objective1Completed)
            {
                if (Morpslaga.Y == 0 && Morpslaga.X == 0)
                {
                    Field[0, 0].Source = Morpslaga.Overworld;
                }
                else
                {
                    Field[0, 0].Source = null;
                }
            }
        }
    }
}