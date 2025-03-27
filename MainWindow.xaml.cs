using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using System.Windows;
using WpfApp1.database.employee;
using System.Windows.Controls;
using WpfApp1.database.grids;
using WpfApp1.database.items;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using K4os.Compression.LZ4.Streams.Adapters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using BCrypt.Net;
using static MaterialDesignThemes.Wpf.Theme;
using System.Windows.Media;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ObservableCollection<Zaposleni> observableZaposleni;
        private ObservableCollection<string> mestoOptions;
        private ObservableCollection<PregledDostavljacaView> pregledDostavljacaViews;
        private ObservableCollection<PregledIsporukaView> observablePregledIsporuka;
        private ObservableCollection<PregledNabavkiView> observablePregledNabavki;
        private ObservableCollection<PregledIsporukaNabavke> observablePregledIsporukaNabavki;
        private ObservableCollection<PregledSkladistaView> observablePregledSkladista;
        private ObservableCollection<PregledIsporukaPoProduktu> observablePregledSpecificnihProdukta;

        private string _adminJmb;
        public MainWindow(string adminJmb)
        {
            InitializeComponent();
            observableZaposleni = new ObservableCollection<Zaposleni>();
            mestoOptions = new ObservableCollection<string>();
            pregledDostavljacaViews = new ObservableCollection<PregledDostavljacaView>();
            observablePregledIsporuka = new ObservableCollection<PregledIsporukaView>();
            observablePregledNabavki = new ObservableCollection<PregledNabavkiView>();
            observablePregledIsporukaNabavki = new ObservableCollection<PregledIsporukaNabavke>();
            observablePregledSkladista = new ObservableCollection<PregledSkladistaView>();
            observablePregledSpecificnihProdukta = new ObservableCollection<PregledIsporukaPoProduktu>();
            _adminJmb = adminJmb;
            this.DataContext = this;

        }

        //!!!!!!   Funkcije za tabele i ostalo !!!!!! TREBAJU SE RASPOREDITI PO GRIDOVAIMA POSEBNO



        //!!!!!!!!!!!!! POCETAK FUNKCIJA ZAPOSLENI GRIDA  !!!!!!!!!!!!!!!!


        //Poziva se da ocisti sve unesene inpute u svim TextBox u programu jer se koristi mainViewShow za sve
        //gridove kako se ne bi za svaki grid pisao novi kod koji radi isto
        //MOZDA BI TREBALI OVO AKO OSTANE VREMENA @!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        private void clearAllBoxData()
        {
            adminEmployeeTable.SelectedItem = null;
            adminEmployeeTable.SelectedIndex = -1;
        }

        //!!!!!!!!!!!!! KRAJ FUNKCIJA ZAPOSLENI GRIDA  !!!!!!!!!!!!!




        //!!!!!!!!!!!!!  POCETAK MENI DUGMADI  !!!!!!!!!!!!!

        private void employeeViewOpen(object sender, RoutedEventArgs e)
        {
            LoadDataTabelaZaposleni();
            adminEmployeeViewGrid.Visibility = Visibility.Visible;
        }

        private void mainViewShow(object sender, RoutedEventArgs e)
        {
            adminEmployeeViewGrid.Visibility = Visibility.Hidden;
            adminDeliveriesViewGrid.Visibility = Visibility.Hidden;
            transportersViewGrid.Visibility = Visibility.Hidden;
            storageViewGrid.Visibility = Visibility.Hidden;
            clearAllBoxData();
        }

        //Otvara prozor ShipmentView za dodavanje isporuka i narudzbi
        private void deliveriesViewOpen(object sender, RoutedEventArgs e)
        {
            loadAdminDeliveriesDataGridData();
            adminDeliveriesViewGrid.Visibility = Visibility.Visible;
        }

        //Otvara prozor sa listom dostupnih dostavljaca
        private void transportersViewOpen(object sender, RoutedEventArgs e)
        {
            LoadDataTabelaDostavljaca();
            transportersViewGrid.Visibility = Visibility.Visible;
        }

        //Otvara prozor sa listom svih dostupnih produkta koji se nalaze u prodavnici i njihovom kolicinom
        private void storageViewOpen(object sender, RoutedEventArgs e)
        {
            loadStorageView();
            storageViewGrid.Visibility = Visibility.Visible;
        }

        //!!!!!!!!!!!!!  KRAJ MENI DUGMADI  !!!!!!!!!!!!!


        //!!!!!!!!!!!!!  POCETAK DUGMADI ISPORUKE GRIDA  !!!!!!!!!!!!!



        //Otvara prozor za dodavanje NARUDZBI
        private void addDeliveryOrderButton(object sender, RoutedEventArgs e)
        {
            ShipmentViewWindow shipmentViewWindow = new ShipmentViewWindow("addNewNabavka", _adminJmb);
            shipmentViewWindow.Owner = this;
            shipmentViewWindow.ShowDialog();
        }

        //Otvara prozor za dodavanje ISPORUKA
        private void adminDeliveriesShipmentButton(object sender, RoutedEventArgs e)
        {
            if (adminDeliveriesDataGrid.SelectedItem is null)
            {
                MessageBox.Show("Odaberite nabavku za koju želite dodati isporuku!");
            }
            else
            {
                ShipmentViewWindow shipmentViewWindow = new ShipmentViewWindow(this, "addNewIsporuka");
                shipmentViewWindow.Owner = this;
                shipmentViewWindow.ShowDialog();
            }
        }


        //!!!!!!!!!!!!!  KRAJ DUGMADI ISPORUKE GRIDA  !!!!!!!!!!!!!





        //!!!!!!!!!!!!!  POCETAK FUNKCIJA DOSTAVLJACI GRIDA  !!!!!!!!!!!!!

        public void LoadDataTabelaDostavljaca()
        {
            string connectionString = "Server=localhost,3306;Database=projektni;Uid=root;Pwd=root;"; // Replace with your connection string
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    // Create the ObservableCollection that will hold the data
                    pregledDostavljacaViews.Clear();

                    // Query to retrieve data from the database
                    string query = "SELECT * FROM projektni.pregled_dostavljaca";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Add the data to the ObservableCollection
                                pregledDostavljacaViews.Add(new PregledDostavljacaView(
                                    reader.GetInt32(0),   // idDostavljaca
                                    reader.GetString(1),  // nazivDostavljaca
                                    reader.GetString(2),  // adresaDostavljaca
                                    reader.GetString(3)   // nazivMjesta
                                ));
                            }
                        }
                    }
                }



                // Bind the ObservableCollection to the DataGrid
                transporterListDataGrid.ItemsSource = null;
                transporterListDataGrid.ItemsSource = pregledDostavljacaViews;

            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }
        }


        //Prikazuje sve isporuke izabranog dostavljaca u tabeli pored
        private void transporterListDeliveriesSelect(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Clear previous data
                observablePregledIsporuka.Clear();

                // Check if any row is selected in the transporter DataGrid
                var selectedRow = transporterListDataGrid.SelectedItem; // Replace with your actual class type



                // Get the transporter name (or ID, depending on your table structure)
                var transporterName = (selectedRow as PregledDostavljacaView)?.NazivDostavljaca;  // Replace with the property that holds the name

                // Create connection
                using (var con = new MySqlConnection("Server=localhost,3306;Database=projektni;Uid=root;Pwd=root;"))
                {
                    con.Open();



                    // Query the database and filter by transporter name
                    string query = "SELECT * FROM projektni.pregled_isporuka WHERE Dostavljac = @dostavljac";
                    using (var cmd = new MySqlCommand(query, con))
                    {
                        // Pass the selected transporter name as the parameter
                        cmd.Parameters.AddWithValue("@dostavljac", transporterName);

                        // Execute the query and populate the ObservableCollection
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                observablePregledIsporuka.Add(new PregledIsporukaView(
                                    reader.GetString(0),  // NazivDostavljaca
                                    reader.GetInt32(1),   // IdPotvrde
                                    reader.GetInt32(2),   // IdProdukta
                                    reader.GetString(3),  // NazivProdukta
                                    reader.GetString(4),  // VrstaProdukta
                                    reader.GetInt32(5),   // KolicinaProdukta
                                    reader.GetString(6),  // Proizvodjac
                                    reader.GetString(7)   // Datum
                                ));
                            }
                        }
                    }
                }
                if(observablePregledIsporuka.Count == 0)
                {
                    transporterNoDataBox.Visibility = Visibility.Visible;
                }
                else
                {
                    transporterNoDataBox.Visibility= Visibility.Collapsed;
                }


                    // Bind the ObservableCollection to the DataGrid
                    transporterDeliveriesListDataGrid.ItemsSource = null;
                transporterDeliveriesListDataGrid.ItemsSource = observablePregledIsporuka;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        //!!!!!!!!!!!!!  KRAJ FUNKCIJA DOSTAVLJACI GRIDA  !!!!!!!!!!!!!




        //!!!!!!!!!!!!!  POCETAK FUNKCIJA ISPORUKA GRIDA  !!!!!!!!!!!!!

        public void loadAdminDeliveriesDataGridData()
        {
            string connectionString = "Server=localhost,3306;Database=projektni;Uid=root;Pwd=root;"; // Replace with your connection string
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    observablePregledNabavki.Clear();

                    string query = "SELECT * FROM projektni.isporuke_nabavke";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                observablePregledNabavki.Add(new PregledNabavkiView(
                                    reader.GetInt32(0),   // idNabavke
                                    reader.GetString(1),  // nazivDostavljaca
                                    reader.GetDecimal(2),  // adresaDostavljaca
                                    reader.GetString(3)  // datumDostave
                                ));
                            }
                        }
                    }
                }

                if (observablePregledNabavki == null || observablePregledNabavki.Count == 0)
                {
                    noDataMessage1.Visibility = Visibility.Visible;
                }
                else
                {
                    noDataMessage1.Visibility = Visibility.Collapsed;
                    adminDeliveriesDataGrid.ItemsSource = null;
                    adminDeliveriesDataGrid.ItemsSource = observablePregledNabavki;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }
        }

        //Prikazuje u drugu tabelu sve isporuke izabrane nabavke
        private void adminDeliveriesLoadSelectedDelivery(object sender, SelectionChangedEventArgs e)
        {
            string connectionString = "Server=localhost,3306;Database=projektni;Uid=root;Pwd=root;";
            try
            {
                var selectedRow = adminDeliveriesDataGrid.SelectedItem;
                var transporterName = (selectedRow as PregledNabavkiView)?.IdNabavke;

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    observablePregledIsporukaNabavki.Clear();

                    // Query to retrieve data from the database
                    string query = "SELECT * FROM projektni.pregled_isporuka_nabake WHERE IDNabavke = @idnabavke";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        observablePregledIsporukaNabavki.Clear();
                        cmd.Parameters.AddWithValue("@idnabavke", transporterName);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                // Add the data to the ObservableCollection
                                observablePregledIsporukaNabavki.Add(new PregledIsporukaNabavke(
                                    reader.GetInt32(0),   // idProdukta
                                    reader.GetString(1),  // nazivProdukta
                                    reader.GetString(2),  // vrstaProdukta
                                    reader.GetInt32(3),   // kolicinaProdukta
                                    reader.GetString(4),  // proizvodjac
                                    reader.GetDecimal(5), // ukupnaCenaProdukta
                                    reader.GetString(6),   // datumDostave
                                    reader.GetInt32(7), // IdNabavke
                                    reader.GetInt32(8)  // IdIsporuke
                                ));
                            }
                        }
                    }
                }

                if (observablePregledIsporukaNabavki == null || observablePregledIsporukaNabavki.Count == 0)
                {
                    noDataMessage.Visibility = Visibility.Visible;
                }
                else
                {
                    noDataMessage.Visibility = Visibility.Collapsed;
                    listDeliveriesDataGrid.ItemsSource = null;
                    // Bind the ObservableCollection to the DataGrid
                    listDeliveriesDataGrid.ItemsSource = observablePregledIsporukaNabavki;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        //Deselektuje izabranu stavku u tabeli ako se klikne bilo gde van tabele
        private void deselectTableColumn(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var point = e.GetPosition(adminEmployeeTable);
            var hitTest = adminEmployeeTable.InputHitTest(point) as UIElement;

            if (hitTest == null || hitTest is not DataGridRow)
            {
                adminEmployeeTable.SelectedItem = null;
            }

        }

        //Deselektuje izabranu stavku u tabeli ako se klikne bilo gde van tabele
        private void deselectadminDeliveriesDataGrid(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var point = e.GetPosition(adminDeliveriesDataGrid);
            var hitTest = adminDeliveriesDataGrid.InputHitTest(point) as UIElement;

            if (hitTest == null || hitTest is not DataGridRow)
            {
                adminDeliveriesDataGrid.SelectedItem = null;
                noDataMessage.Visibility = Visibility.Visible;
            }
        }

        //Deselektuje izabranu stavku u tabeli ako se klikne bilo gde van tabele
        private void deselectTransporterListDataGrid(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var point = e.GetPosition(transporterListDataGrid);
            var hitTest = transporterListDataGrid.InputHitTest(point) as UIElement;

            if (hitTest == null || hitTest is not DataGridRow)
            {
                transporterListDataGrid.SelectedItem = null;
            }
        }

        //Brisanje unosa iz "isporuka_produkta", "isporuka" tabela u MySQL
        public void deleteIsporuke(int isporukaId)
        {
            string connectionString = "Server = localhost,3306; Database = projektni; Uid = root; Pwd = root;";


            try
            {

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();


                    //brisanje iz nabavka_produkta tabele
                    try
                    {
                        string deleteIquery = "DELETE FROM nabavka_produkta WHERE isporuka_produkta_DOSTAVA_idIsporuke=@ID_isporuke";
                        using (MySqlCommand deleteFromIsporuka = new MySqlCommand(deleteIquery, connection))
                        {
                            deleteFromIsporuka.Parameters.AddWithValue("@ID_isporuke", isporukaId);
                            deleteFromIsporuka.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Greska kod birsanja u isporuka u nabavci produkta: " + ex.Message);
                    }

                    try
                    {
                        //Brisanje iz "isporuka_produkta" tabele
                        string deleteIPquery = "DELETE FROM isporuka_produkta WHERE DOSTAVA_idIsporuke=@ID_isporuke";
                        using (MySqlCommand deleteFromIsProdukta = new MySqlCommand(deleteIPquery, connection))
                        {
                            deleteFromIsProdukta.Parameters.AddWithValue("@ID_isporuke", isporukaId);
                            deleteFromIsProdukta.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Greska kod birsanja u isporuka_produkta: " + ex.Message);
                    }

                    try
                    {
                        //Brisanje iz "isporuka" tabele
                        string deleteIquery = "DELETE FROM isporuka WHERE idIsporuke=@ID_isporuke";
                        using (MySqlCommand deleteFromIsporuka = new MySqlCommand(deleteIquery, connection))
                        {
                            deleteFromIsporuka.Parameters.AddWithValue("@ID_isporuke", isporukaId);
                            deleteFromIsporuka.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Greska kod birsanja u isporuka: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to the database: " + ex.Message);
            }
        }

        //Dugme za brisanje izabrane nabavke u DataGridu u adminDeliveriesViewGrid-u
        private void deleteDeliveryOrderButton(object sender, RoutedEventArgs e)
        {
            if (adminDeliveriesDataGrid.SelectedItem is null)
            {
                MessageBox.Show("Odaberite nabavku koju želite izbrisati!");
            }
            else
            {
                ConfirmWindow confirmWindow = new ConfirmWindow(this, "deleteSelectedNabavku", _adminJmb);
                confirmWindow.ShowDialog();
            }
        }

        //Brise se izabrana nabavka iz tabele i baze
        public void deleteSelectedNabavku()
        {
            string connectionString = "Server = localhost,3306; Database = projektni; Uid = root; Pwd = root;";
            try
            {
                var selectedRow = adminDeliveriesDataGrid.SelectedItem;
                var shipmentId = (selectedRow as PregledNabavkiView)?.IdNabavke;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();


                    try
                    {
                        //Get IdIsporuke
                        string isporukaIdQeury = "SELECT ID_isporuke FROM isporuke_lista WHERE ID_nabavke=@ID_nabavke";
                        MySqlCommand getIsporukaId = new MySqlCommand(isporukaIdQeury, connection);
                        getIsporukaId.Parameters.AddWithValue("@ID_nabavke", shipmentId);

                        MySqlDataReader reader = getIsporukaId.ExecuteReader();
                        while (reader.Read())
                        {
                            var isporukaId = reader["ID_isporuke"];

                            if (isporukaId != DBNull.Value)
                            {
                                deleteIsporuke((int)isporukaId);
                            }
                        }
                        reader.Close();

                        try
                        {
                            //Brisanje iz tabele "nabavka_produkta" 
                            string nabProdQuery = "DELETE FROM nabavka_produkta WHERE NABAVKA_idPotvrde=@IdNabavke";
                            using (MySqlCommand deleteFromNabProd = new MySqlCommand(nabProdQuery, connection))
                            {
                                deleteFromNabProd.Parameters.AddWithValue("@IdNabavke", shipmentId);
                                deleteFromNabProd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex.Message);
                        }

                        try
                        {
                            //Brisanje iz tabele "nabavka"
                            string nabavkaQuery = "DELETE FROM nabavka WHERE idPotvrde=@idPotvrde";
                            using (MySqlCommand deleteNabavka = new MySqlCommand(nabavkaQuery, connection))
                            {
                                deleteNabavka.Parameters.AddWithValue("@idPotvrde", shipmentId);
                                deleteNabavka.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error" + ex.Message);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error" + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to the database: " + ex.Message);
            }

            this.loadAdminDeliveriesDataGridData();
        }

        public void addNewIsporuka(ShipmentViewWindow shipmentView)
        {
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root;";
            var selectedItem = adminDeliveriesDataGrid.SelectedItem;
            int? selectedItemId = (selectedItem as PregledNabavkiView)?.IdNabavke;

            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    try
                    {
                        //Get nabavkaID
                        var selectedRow1 = adminDeliveriesDataGrid.SelectedItem;
                        var shipmentId = (selectedRow1 as PregledNabavkiView)?.IdNabavke;

                        //Get trenutni datum
                        string datum = DateTime.Today.ToString("yyyy-MM-dd");

                        //Get menadzer jmb, treba se doraditi za razlicite menadzere
                        string menadzerJmb = _adminJmb; //Promijeniti kada napravim login!!!!!!!!!!!!


                        //Get DostavljacID
                        var selectedRow2 = adminDeliveriesDataGrid.SelectedItem;
                        var transporterName = (selectedRow2 as PregledNabavkiView)?.Naziv;

                        if (shipmentId != null && transporterName != null)
                        {
                            shipmentView.addSingleIsporuka((int)shipmentId, datum, menadzerJmb, transporterName);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Greska" + ex.Message);
                    }


                    if (selectedItemId != 0)
                    {
                        loadAdminDeliveriesDataGridData();
                        adminDeliveriesDataGrid.ItemsSource = observablePregledNabavki;
                        var tmp = observablePregledNabavki.FirstOrDefault(item => item.IdNabavke == selectedItemId);
                        if (tmp != null)
                        {
                            adminDeliveriesDataGrid.SelectedItem = tmp;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to the database: " + ex.Message);
            }
        }

        //Dugme za brisanje izabrane isporuke iz tabele
        private void adminDeleteDeliveriesShipmentButton(object sender, RoutedEventArgs e)
        {
            if (adminDeliveriesDataGrid.SelectedItem is null)
            {
                MessageBox.Show("Odaberite nabavku za koju želite izbrisati isporuku!");
            }
            else
            {
                if (listDeliveriesDataGrid.SelectedItem is null)
                {
                    MessageBox.Show("Odaberite isporuku koju želite izbrisati!");
                }
                else
                {

                    ConfirmWindow confirmWindow = new ConfirmWindow(this, "deleteSelectedIsporuku", _adminJmb);
                    confirmWindow.ShowDialog();
                }
            }
        }

        //
        public void adminDeliveriesLoadSelectedDeliveryRefresh()
        {
            string connectionString = "Server=localhost,3306;Database=projektni;Uid=root;Pwd=root;";
            try
            {
                var selectedRow = adminDeliveriesDataGrid.SelectedItem;
                var transporterName = (selectedRow as PregledNabavkiView)?.IdNabavke;

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    observablePregledIsporukaNabavki.Clear();

                    // Query to retrieve data from the database
                    string query = "SELECT * FROM projektni.pregled_isporuka_nabake WHERE IDNabavke = @idnabavke";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        observablePregledIsporukaNabavki.Clear();
                        cmd.Parameters.AddWithValue("@idnabavke", transporterName);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                observablePregledIsporukaNabavki.Add(new PregledIsporukaNabavke(
                                    reader.GetInt32(0),   // idProdukta
                                    reader.GetString(1),  // nazivProdukta
                                    reader.GetString(2),  // vrstaProdukta
                                    reader.GetInt32(3),   // kolicinaProdukta
                                    reader.GetString(4),  // proizvodjac
                                    reader.GetDecimal(5), // ukupnaCenaProdukta
                                    reader.GetString(6),   // datumDostave
                                    reader.GetInt32(7), // IdNabavke
                                    reader.GetInt32(8)  // IdIsporuke
                                ));
                            }
                        }
                    }
                }

                listDeliveriesDataGrid.ItemsSource = null;
                listDeliveriesDataGrid.ItemsSource = observablePregledIsporukaNabavki;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        //Funkcija za brisanje izabrane isporuke
        public void deleteSelectedIsporuka()
        {
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root;";
            var selectedItem = adminDeliveriesDataGrid.SelectedItem;
            int? selectedItemId = (selectedItem as PregledNabavkiView)?.IdNabavke;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    try
                    {
                        var selectedRow = listDeliveriesDataGrid.SelectedItem;
                        var isporukaId = (selectedRow as PregledIsporukaNabavke)?.Isporuka;
                        if (isporukaId != null)
                        {
                            deleteIsporuke((int)isporukaId);
                        }
                        else
                        {
                            MessageBox.Show("Nepostojeci ID isporuke prilikom brisanja isporuke!");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Greska prilikom brisanja pojedinacne isporuke " + ex.Message);
                    }


                    //Baca null kad se brise nabavka

                    if (selectedItemId != 0)
                    {
                        loadAdminDeliveriesDataGridData();
                        adminDeliveriesDataGrid.ItemsSource = observablePregledNabavki;
                        var tmp = observablePregledNabavki.FirstOrDefault(item => item.IdNabavke == selectedItemId);
                        if (tmp != null)
                        {
                            adminDeliveriesDataGrid.SelectedItem = tmp;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Not connected to a database: " + ex.Message);
            }

        }

        private void openListaMjestaClick(object sender, RoutedEventArgs e)
        {
            ListaMjestaPopUp listaMijestaWindow = new ListaMjestaPopUp();
            listaMijestaWindow.Show();
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

        //Storage View Prikazuje sve isporuke za specificni produkt koji je izabran klikom
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
                if (observablePregledSpecificnihProdukta.Count == 0)
                {
                    storageNoDataBox.Visibility = Visibility.Visible;
                }
                else
                {
                    storageNoDataBox.Visibility = Visibility.Collapsed;
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
                                    storageNoDataBox.Visibility = Visibility.Visible;
                                }
                                else
                                {
                                    storageNoDataBox.Visibility = Visibility.Collapsed;
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

        private void editTabelaZaposlenih(object sender, RoutedEventArgs e)
        {
            editTabelaZaposlenih();
        }

        private void deleteEmployee(object sender, RoutedEventArgs e)
        {
            deleteEmployee();
        }

        private void addEmployee(object sender, RoutedEventArgs e)
        {
            addNewEmployee();
        }

        //Treba poslagati ove funkicje nakon zavrsetka;
    }
}