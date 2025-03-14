using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WpfApp1.database.grids;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {

        private string _employeeJmb;
        private ObservableCollection<PregledSkladistaView> observablePregledSkladista;
        private ObservableCollection<PregledIsporukaPoProduktu> observablePregledSpecificnihProdukta;
        public EmployeeWindow(string employeeJmb)
        {
            InitializeComponent();
            _employeeJmb = employeeJmb;
            observablePregledSkladista = new ObservableCollection<PregledSkladistaView>();
            observablePregledSpecificnihProdukta = new ObservableCollection<PregledIsporukaPoProduktu>();
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

        private void loadStorageView()
        {
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM projektni.sadrzaj_skladista";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        observablePregledSkladista.Clear();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                observablePregledSkladista.Add(new PregledSkladistaView(
                                    reader.GetInt32(0),
                                    reader.GetInt32(1),         //ID Produkta
                                    reader.GetString(2),        //Naziv produkta
                                    reader.GetString(3),        //Vrsta produkta
                                    reader.GetInt32(4),         //Ukupna kolicina produkta
                                    reader.GetDecimal(5)));     //Ukupna cijena produkta
                            }
                        }
                    }
                }

                storageAvailableProductsDataGrid.ItemsSource = null;
                storageAvailableProductsDataGrid.ItemsSource = observablePregledSkladista;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to connect do database: " + ex.Message);
            }
        }

        private void loadstorageSpecificProductDataGrid(object sender, SelectionChangedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root";
            var selectedItem = storageAvailableProductsDataGrid.SelectedItem;
            int? selectedItemId = (selectedItem as PregledSkladistaView)?.ProduktId;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    observablePregledSpecificnihProdukta.Clear();


                    string query = "SELECT * from projektni.pregled_isporuka_pojedinacnog_produkta WHERE ID_Produkta=@ID_Produkta";
                    try
                    {
                        using (MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@ID_Produkta", selectedItemId);
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    observablePregledSpecificnihProdukta.Add(new PregledIsporukaPoProduktu(
                                        reader.GetInt32(0),
                                        reader.GetInt32(1),
                                        reader.GetInt32(2),
                                        reader.GetDecimal(3),
                                        reader.GetString(4),
                                        reader.GetString(5),
                                        reader.GetInt32(6)
                                        ));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Greska " + ex.Message);
                    }
                }

                storageSpecificProductDataGrid.ItemsSource = null;
                storageSpecificProductDataGrid.ItemsSource = observablePregledSpecificnihProdukta;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in connectiong to database " + ex.Message);
            }
        }

        private void loadStorageSpecificProductDataGridSearch(int produktId)
        {
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    observablePregledSpecificnihProdukta.Clear();


                    string query = "SELECT * from projektni.pregled_isporuka_pojedinacnog_produkta WHERE ID_Produkta=@ID_Produkta";
                    try
                    {
                        using (MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@ID_Produkta", produktId);
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    observablePregledSpecificnihProdukta.Add(new PregledIsporukaPoProduktu(
                                        reader.GetInt32(0),
                                        reader.GetInt32(1),
                                        reader.GetInt32(2),
                                        reader.GetDecimal(3),
                                        reader.GetString(4),
                                        reader.GetString(5),
                                        reader.GetInt32(6)
                                        ));
                                }
                                if (observablePregledSpecificnihProdukta.Count == 0)
                                {
                                    MessageBox.Show("Nepostojeci Produkt ID");
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Greska " + ex.Message);
                    }
                }

                storageSpecificProductDataGrid.ItemsSource = null;
                storageSpecificProductDataGrid.ItemsSource = observablePregledSpecificnihProdukta;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in connectiong to database " + ex.Message);
            }
        }

        //Dugme kada pritisnemo prerazi u storage View, za trazenje produkta po produkt ID-ju
        private void storageViewSearchByProductId(object sender, RoutedEventArgs e)
        {
            string idStr = storageViewSearchBox.Text;
            if (idStr != "")
            {
                int id = int.Parse(idStr);
                loadStorageSpecificProductDataGridSearch(id);
            }
            else
            {
                MessageBox.Show("Unesite postojeći Produkt ID!");
            }
        }

        private void employeeViewShow(object sender, RoutedEventArgs e)
        {
            storageViewGrid.Visibility = Visibility.Collapsed;
            storageViewSearchBox.Clear();
            storageAvailableProductsDataGrid.SelectedItem = null;
        }

        private void employeeOpenStorageClick(object sender, RoutedEventArgs e)
        {
            storageViewGrid.Visibility=Visibility.Visible;
            loadStorageView();
        }
    }
}
