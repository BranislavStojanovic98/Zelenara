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
    /// Interaction logic for ConfirmWindow.xaml
    /// </summary>
    public partial class ConfirmWindow : Window
    {

        private MainWindow _mainWindow;
        private ShipmentViewWindow _shipmentWindow;
        private EmployeeConfigWindow _employeeConfigWindow;
        private string _action;
        private string _jmb;

        public ConfirmWindow()
        {
            InitializeComponent();
        }

        public ConfirmWindow(MainWindow mainWindow, string action, string jmb)
        {
            _mainWindow = mainWindow;
            _action = action;
            _jmb = jmb;
            InitializeComponent();
        }

        public ConfirmWindow(ShipmentViewWindow shipmentViewWindow, string action)
        {
            _shipmentWindow = shipmentViewWindow;
            _action = action;
            InitializeComponent();
        }

        public ConfirmWindow(EmployeeConfigWindow employeeConfigWindow , string action, string jmb)
        {
            _employeeConfigWindow = employeeConfigWindow;
            _action = action;
            _jmb = jmb;
            InitializeComponent();
        }

        private void employeeConfirmationButtonNoClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void employeeConfirmationButtonYesClick(object sender, RoutedEventArgs e)
        {
            if (_action == "add")
            {
                // Call the method to add a new employee
                _employeeConfigWindow.DodajNovogZaposlenog(); // Assuming DodajNovogZaposlenog is a method in MainWindow
            }
            else if (_action == "edit")
            {
                // Call the method to edit the selected employee
                _employeeConfigWindow.IzmjeniZaposlenog(); // Assuming EditZaposleni is a method in MainWindow
            }
            else if (_action == "delete")
            {
                _employeeConfigWindow.IzbrisiZaposlenog();
            }
            //TODO Napravi da se DataGrid refreshuje kad se doda nova nabavka!!!
            else if (_action == "addNewNabavka")
            {
                _shipmentWindow.dodajNovuNabavku(_shipmentWindow.Jmb);
                _shipmentWindow.Close();
            }
            else if (_action == "deleteSelectedNabavku")
            {
                _mainWindow.deleteSelectedNabavku();
            }
            else if (_action == "addNewIsporuka")
            {
                if (this.Owner is ShipmentViewWindow shipmentViewWindow && shipmentViewWindow.Owner is MainWindow mainWindow)
                {
                    mainWindow.addNewIsporuka(shipmentViewWindow);
                    _mainWindow = mainWindow;
                }
            }
            else if (_action == "deleteSelectedIsporuku")
            {
                _mainWindow.deleteSelectedIsporuka();
            }
            else if(_action == "addEmployeeAccount")
            {
                _mainWindow.addEmployeeAccount();
            }
            else if (_action == "changeEmployeeAccount")
            {
                _mainWindow.changeEmployeeAccountCall();
            }

            if (this.Owner is ShipmentViewWindow shipmentViewWindow1)
            {
                shipmentViewWindow1.Close();
            }

            this.Close();


            if (_mainWindow != null)
            {
                _mainWindow.Activate();
                _mainWindow.Focus();
            }
        }
    }
}
