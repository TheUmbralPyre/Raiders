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
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Lindisfarne = new MainWindow();
            this.Close();
            Lindisfarne.ShowDialog();          
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
