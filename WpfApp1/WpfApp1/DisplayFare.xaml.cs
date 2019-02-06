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
    /// Interaction logic for DisplayFare.xaml
    /// </summary>
    public partial class DisplayFare : Window
    {
        public DisplayFare()
        {
            InitializeComponent();
            dtpFDateFilter.SelectedDate = DateTime.Now;
            dgFareHistory.ItemsSource = DBGuide.RetrieveFareRecordFromDatabase().DefaultView; //How to fill datagrid using datatable: https://stackoverflow.com/questions/20770438/how-to-bind-datatable-to-datagrid
            tbxTotalFare.Text = string.Format("S{0:C}",DBGuide.FareHistoryTotalCollected());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime dateFilter = dtpFDateFilter.SelectedDate.Value;
                dgFareHistory.ItemsSource = null; //How to clear datagrid https://stackoverflow.com/questions/14472894/clear-datagrid-values-in-wpf
                dgFareHistory.Items.Refresh();
                dgFareHistory.ItemsSource = DBGuide.RetrieveFareRecordFromDatabase(dateFilter).DefaultView;
                tbxTotalFare.Text = string.Format("S{0:C}", DBGuide.FareHistoryTotalCollected(dateFilter));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please select a date");
            }
           
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Home = new MainWindow();
            Home.Show();
            this.Close();
        }
    }
}
