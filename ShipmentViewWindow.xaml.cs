using MySqlConnector;
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
    /// Interaction logic for ShipmentViewWindow.xaml
    /// </summary>
    public partial class ShipmentViewWindow : Window
    {
        public ShipmentViewWindow()
        {
            InitializeComponent();
            loadShipmentViewCompanyNameComboBox();
            loadShipmentViewProductComboBox();
            
        }

        //Otkazivanje narudzbe
        private void shipmentViewCancleButtonClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Potvrda za dodavanje narudzbe
        private void shipmentViewConfirmButtonConfirm(object sender, RoutedEventArgs e)
        {

        }

        //Ucitavanje dostavljaca za ComboBox
        private void loadShipmentViewCompanyNameComboBox()
        {
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root;";
            List<string> dostavljaci = new List<string>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT naziv FROM dobavljac";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        dostavljaci.Add(reader.GetString("naziv"));
                    }
                    shipmentViewCompanyNameComboBox.ItemsSource = dostavljaci;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating database: " + ex.Message);
            }
        }
    
        //Ucitavanje produkta za ComboBox
        private void loadShipmentViewProductComboBox()
        {
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root;";
            List<string> products = new List<string>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT produktInfo FROM pregled_produkta";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        products.Add(reader.GetString("produktInfo"));
                    }
                    shipmentViewProductComboBox.ItemsSource = products;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating database: " + ex.Message);
            }
        }
    
        
        //Ucitavanje proizvodjaca za ComboBox
        private void loadShipmentViewMakerComboBox(string produktInfo)
        {
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root;";
            List<string> maker = new List<string>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT DISTINCT proizvodjac FROM pregled_produkta WHERE produktInfo=@produktInfo";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@produktInfo", produktInfo);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        maker.Add(reader.GetString("proizvodjac"));
                    }
                    shipmentViewMakerComboBox.ItemsSource = maker;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating database: " + ex.Message);
            }
        }

        private void loadDistinctProizvodjacComboBox(object sender, SelectionChangedEventArgs e)
        {
            if(shipmentViewProductComboBox.SelectedItem != null)
            {
                string selectedItem = shipmentViewProductComboBox.SelectedItem.ToString();
                if (selectedItem != null)
                {
                    loadShipmentViewMakerComboBox(selectedItem);
                }
                else
                {
                    MessageBox.Show("Producer information is missing.");
                }
            }
        }
    }
}
