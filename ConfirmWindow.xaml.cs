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
        private string _action;

        public ConfirmWindow()
        {
            InitializeComponent();
        }

        public ConfirmWindow(MainWindow mainWindow, string action)
        {
            _mainWindow = mainWindow;
            _action = action;
            InitializeComponent();
        }

        public ConfirmWindow(ShipmentViewWindow shipmentViewWindow, string action)
        {
            _shipmentWindow = shipmentViewWindow;
            _action = action;
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
                _mainWindow.DodajNovogZaposlenog(); // Assuming DodajNovogZaposlenog is a method in MainWindow
            }
            else if (_action == "edit")
            {
                // Call the method to edit the selected employee
                _mainWindow.editTabelaZaposlenih(); // Assuming EditZaposleni is a method in MainWindow
            }
            else if (_action == "delete")
            {
                _mainWindow.IzbrisiZaposlenog();
            }
            //TODO Napravi da se DataGrid refreshuje kad se doda nova nabavka!!!
            else if (_action == "addNewNabavka")
            {
                _shipmentWindow.dodajNovuNabavku();
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
