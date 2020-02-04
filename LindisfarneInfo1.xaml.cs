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
    /// Interaction logic for LindisfarneInfo1.xaml
    /// </summary>
    public partial class LindisfarneInfo1 : Window
    {
        public LindisfarneInfo1()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            TextBox.Selection.Text = Properties.Resources.LindisfarneInfo1;
        }

        private void Mouse_Click(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
        }
    }
}
