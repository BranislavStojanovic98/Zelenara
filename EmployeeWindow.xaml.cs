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
        private string _theme;
        private string _language;

        private ObservableCollection<PregledSkladistaView> observablePregledSkladista;
        private ObservableCollection<PregledIsporukaPoProduktu> observablePregledSpecificnihProdukta;
        public EmployeeWindow(string employeeJmb)
        {
            _employeeJmb = employeeJmb;
            getTheme();
            InitializeComponent();

            if (_theme == "Theme1")
            {
                changeTheme1(null, null);
            }
            else if (_theme == "Theme2")
            {
                changeTheme2(null, null);
            }

            observablePregledSkladista = new ObservableCollection<PregledSkladistaView>();
            observablePregledSpecificnihProdukta = new ObservableCollection<PregledIsporukaPoProduktu>();
        }

        private void openCashierWindowClick(object sender, RoutedEventArgs e)
        {
            CashierWindow cashierWindow = new CashierWindow(_employeeJmb, _theme, _language);
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

        private void changeThemeDefault(object sender, RoutedEventArgs e)
        {
            _theme = "Default";

            upperGrid.Background = SystemColors.ControlLightBrush;
            menuGrid.Background = SystemColors.ControlLightBrush;
            employeeMainGrid.Background = SystemColors.ControlLightLightBrush;
            bottomGrid.Background = SystemColors.ControlLightBrush;

            storageViewGrid.Background = new SolidColorBrush(Colors.Beige);
            searchGrid.Background = new SolidColorBrush(Colors.Bisque);
            storageAvailableProductsDataGrid.Background = new SolidColorBrush(Colors.Bisque);
            storageSpecificProductDataGrid.Background = new SolidColorBrush(Colors.Bisque);
            storageViewSearchButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
        }

        private void changeTheme1(object sender, RoutedEventArgs e)
        {
            _theme = "Theme1";

            upperGrid.Background = new SolidColorBrush(Colors.Bisque);
            menuGrid.Background = new SolidColorBrush(Colors.Bisque);
            employeeMainGrid.Background = new SolidColorBrush(Colors.Beige);
            bottomGrid.Background = new SolidColorBrush(Colors.Bisque);

            storageViewGrid.Background = new SolidColorBrush(Colors.Beige);
            searchGrid.Background = new SolidColorBrush(Colors.Bisque);
            storageAvailableProductsDataGrid.Background = new SolidColorBrush(Colors.Bisque);
            storageSpecificProductDataGrid.Background = new SolidColorBrush(Colors.Bisque);
            storageViewSearchButton.Background = SystemColors.ControlLightLightBrush;
        }

        private void changeTheme2(object sender, RoutedEventArgs e)
        {
            _theme = "Theme2";

            upperGrid.Background = new SolidColorBrush(Colors.BurlyWood);
            menuGrid.Background = new SolidColorBrush(Colors.BurlyWood);
            employeeMainGrid.Background = new SolidColorBrush(Colors.Brown);
            bottomGrid.Background = new SolidColorBrush(Colors.BurlyWood);

            storageViewGrid.Background = new SolidColorBrush(Colors.Brown);
            searchGrid.Background = new SolidColorBrush(Colors.BurlyWood);
            storageAvailableProductsDataGrid.Background = new SolidColorBrush(Colors.BurlyWood);
            storageSpecificProductDataGrid.Background = new SolidColorBrush(Colors.BurlyWood);
            storageViewSearchButton.Background = SystemColors.ControlLightLightBrush;
        }

        private void changeLanguageSerbian(object sender, RoutedEventArgs e)
        {
            _language = "Serbian";

            welcomeLabel.Content = "Dobrodošli";
            menuLabel.Content = "Meni";
            cashierViewOpenButton.Content = "Kasa";
            employeeStorageShowButton.Content = "Magacin";
            optionsMenu.Padding = new Thickness(35, 0, 6, 0);
            optionsMenu.Header = "Opcije";

            themeMenuItem.Header = "Teme";
            themeMenuItem2.Header = "Tema 1";
            themeMenuItem3.Header = "Tema 2";

            languageMenu.Header = "Jezik";
            languageMenuItem1.Header = "Srpski";
            languageMenuItem2.Header = "Engleski";

            logoutButton.Content = "Odjavi se";

            storageLabel1.Content = "Dostupni produkti";
            storageLabel2.Content = "Lista isporuka selektovanog produkta";
            searchLabel.Content = "Pretraga";
            storageViewSearchButton.Content = "Pretraži";
            storageDataGrid1ProductId.Header = "ID Produkta";
            storageDataGrid1Name.Header = "Naziv";
            storageDataGrid1Type.Header = "Vrsta";
            storageDataGrid1Amount.Header = "Količina";
            storageDataGrid1Price.Header = "Cijena";

            storageDataGrid2DeliveryId.Header = "ID Isporuke";
            storageDataGrid2Amount.Header = "količina";
            storageDataGrid2Price.Header = "Cijena";
            storageDataGrid2Transporter.Header = "Dostavljač";
            storageDataGrid2Date.Header = "Datum Isporuke";

            employeeStorageBackButton.Content = "Nazad";
        }
        private void changeLanguageEnglish(object sender, RoutedEventArgs e)
        {
            _language = "English";

            welcomeLabel.Content = "Welcome";
            menuLabel.Content = "Menu";
            cashierViewOpenButton.Content = "Cashbox";
            employeeStorageShowButton.Content = "Storage";
            optionsMenu.Padding = new Thickness(27, 0, 6, 0);
            optionsMenu.Header = "Options";

            themeMenuItem.Header = "Theme";
            themeMenuItem2.Header = "Theme 1";
            themeMenuItem3.Header = "Theme 2";

            languageMenu.Header = "Language";
            languageMenuItem1.Header = "Serbian";
            languageMenuItem2.Header = "English";

            logoutButton.Content = "Log out";

            storageLabel1.Content = "Available Products";
            storageLabel2.Content = "List of Deliveries of Selected Product";
            searchLabel.Content = "Search";
            storageViewSearchButton.Content = "Search";
            storageDataGrid1ProductId.Header = "Product ID";
            storageDataGrid1Name.Header = "Name";
            storageDataGrid1Type.Header = "Type";
            storageDataGrid1Amount.Header = "Amount";
            storageDataGrid1Price.Header = "Price";

            storageDataGrid2DeliveryId.Header = "Delivery ID";
            storageDataGrid2Amount.Header = "Amount";
            storageDataGrid2Price.Header = "Price";
            storageDataGrid2Transporter.Header = "Transporter";
            storageDataGrid2Date.Header = "Date";

            employeeStorageBackButton.Content = "Back";
        }

        //Sacuva trenutnu temu kao preferiranu u zaposleni_nalog tabelu database-a
        private void saveTheme(object sender, EventArgs e)
        {
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE zaposleni_nalog SET theme=@theme WHERE zaposleni_JMB = @zaposleni_JMB";

                    try
                    {
                        using (MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@theme", _theme);
                            cmd.Parameters.AddWithValue("@zaposleni_JMB", _employeeJmb);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        //Provjeri koju temu je korisnik sacuvao prilikom prosle sesije
        private void getTheme()
        {
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT theme FROM  zaposleni_nalog WHERE zaposleni_JMB = @zaposleni_JMB";

                    try
                    {
                        using (MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@zaposleni_JMB", _employeeJmb);
                            object result = cmd.ExecuteScalar();

                            if (result != DBNull.Value && result != null)
                            {
                                _theme = result.ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
