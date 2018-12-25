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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for DirectionsResults.xaml
    /// </summary>
    public partial class DirectionsResults : Window
    {
        public DirectionsResults()
        {
            InitializeComponent();
        }

       

        private void btnHome_Click(object sender, RoutedEventArgs e) //event that happens when button is clicked
        {
            MainWindow Home = new MainWindow(); //create new instance of MainWindow 
            Home.Show(); //show MainWindow
            this.Close(); //close current window
        }

        private void txtBoxDisplay_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}
