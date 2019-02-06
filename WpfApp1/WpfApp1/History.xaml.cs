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
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for History.xaml
    /// </summary>
    public partial class History : Window
    {
        public History()
        {
            InitializeComponent();
        }

        //Make sure to check the connection string
        //private const string connectionString = "Data Source=DIT-NB1828823\\SQLEXPRESS; database=APPDCADB; integrated security = true;";
        private const string connectionString = "Data Source=DIT-NB1829233\\SQLEXPRESS; database=APPDCADB; integrated security = true;";
        private DataTable PastQueries = new DataTable();

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Display = new MainWindow();
            Display.Show();
            this.Hide();
        }
    }
}
