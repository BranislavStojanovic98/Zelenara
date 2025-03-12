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
    /// Interaction logic for EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {

        private string _employeeJmb;
        public EmployeeWindow(string employeeJmb)
        {
            InitializeComponent();
            _employeeJmb = employeeJmb;
        }

        private void openCashierWindowClick(object sender, RoutedEventArgs e)
        {
            CashierWindow cashierWindow = new CashierWindow(_employeeJmb);
            cashierWindow.Show();
        }

        private void logoutActionClick(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            loginWindow.Focus();
            this.Close();
            MessageBox.Show("Uspjesno ste se izlogovali");
        }
    }
}
