using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using System.Windows;
using WpfApp1.database.employee;
using System.Windows.Controls;
using WpfApp1.database.grids;
using WpfApp1.database.items;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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




        //Ucitavanje informacija tabele zaposlenih
        private void LoadDataTabelaZaposleni()
        {
            string connectionString = "Server=localhost,3306;Database=projektni;Uid=root;Pwd=root;"; // Replace with your connection string

            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    observableZaposleni.Clear();

                    // Load Mesto Options
                    MySqlCommand cmd1 = new MySqlCommand("SELECT DISTINCT infoMjesta FROM projektni.mjesto_sa_postanskimbr", con);
                    MySqlDataReader reader1 = cmd1.ExecuteReader();
                    while (reader1.Read())
                    {
                        mestoOptions.Add(reader1["infoMjesta"].ToString());
                    }
                    reader1.Close();

                    // Load Zaposleni Data
                    MySqlCommand cmd2 = new MySqlCommand("SELECT * FROM projektni.pregled_zaposlenih", con);
                    MySqlDataReader reader2 = cmd2.ExecuteReader();
                    while (reader2.Read())
                    {
                        observableZaposleni.Add(new Zaposleni(
                            reader2.GetString(0), // JMB
                            reader2.GetString(1), // Ime
                            reader2.GetString(2), // Prezime
                            reader2.GetString(3)  // Mjesto
                        ));
                    }
                    reader2.Close();
                }

                adminEmployeeTable.ItemsSource = null;
                adminEmployeeTable.ItemsSource = observableZaposleni;

            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }
        }

        //Promijena postojecih informacija u tabeli
        public void editTabelaZaposlenih()
        {
            try
            {
                // Assuming adminEmployeeTable contains the selected Zaposleni item
                var selectedZaposleni = adminEmployeeTable.SelectedItem as Zaposleni;

                // Ensure there is a selected employee to edit
                if (selectedZaposleni != null)
                {
                    // Get the new values from the TextBoxes
                    string newIme = employeeInfoBoxName.Text; // TextBox for Ime
                    string newPrezime = employeeInfoBoxLastname.Text; // TextBox for Prezime
                    string newMjesto = employeeInfoBoxCity.Text; // TextBox for Mjesto

                    // Check if the values are different before updating
                    if (!string.IsNullOrEmpty(newIme) && newIme != selectedZaposleni.Ime)
                    {
                        selectedZaposleni.Ime = newIme; // Update the property
                        UpdateDatabaseField("Ime", newIme, selectedZaposleni.JMB); // Update the database
                    }

                    if (!string.IsNullOrEmpty(newPrezime) && newPrezime != selectedZaposleni.Prezime)
                    {
                        selectedZaposleni.Prezime = newPrezime; // Update the property
                        UpdateDatabaseField("Prezime", newPrezime, selectedZaposleni.JMB); // Update the database
                    }

                    if (!string.IsNullOrEmpty(newMjesto) && newMjesto != selectedZaposleni.Mjesto)
                    {
                        selectedZaposleni.Mjesto = newMjesto; // Update the property
                        UpdateDatabaseField("MJESTO_PostanskiBroj", newMjesto, selectedZaposleni.JMB); // Update the database
                    }


                    // Optionally, deselect the row
                    adminEmployeeTable.SelectedItem = null;
                    adminEmployeeTable.SelectedIndex = -1;

                }
                else
                {
                    MessageBox.Show("No item selected.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during editing: " + ex.Message);
            }
            LoadDataTabelaZaposleni();
        }

        //Dodavanje novog zaposlenog u tabelu
        public void DodajNovogZaposlenog()
        {
            // Get values from TextBoxes
            string ime = employeeInfoBoxName.Text;
            string prezime = employeeInfoBoxLastname.Text;
            string jmb = employeeInfoBoxJMB.Text;
            string postBr = employeeInfoBoxCity.Text;

            // Check if any field is empty
            if (string.IsNullOrWhiteSpace(ime) || string.IsNullOrWhiteSpace(prezime) || string.IsNullOrWhiteSpace(jmb) || string.IsNullOrWhiteSpace(postBr))
            {
                MessageBox.Show("Sva polja moraju biti popunjena!");
                return;
            }

            // Check if JMB is exactly 12 characters
            if (jmb.Length != 12)
            {
                MessageBox.Show("JMB mora biti tačno 12 brojeva");
                return;
            }

            

            // Create a connection to the MySQL database
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root;"; // Adjust as needed

            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    // SQL query to insert a new employee
                    string sql = "INSERT INTO Zaposleni (JMB, Ime, Prezime, MJESTO_PostanskiBroj) VALUES (@jmb, @ime, @prezime, @postBr)";

                    // Create a command and set its parameters
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@jmb", jmb);
                        cmd.Parameters.AddWithValue("@ime", ime);
                        cmd.Parameters.AddWithValue("@prezime", prezime);
                        cmd.Parameters.AddWithValue("@postBr", postBr);

                        // Execute the query and check the result
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Novi zaposleni uspješno dodat");
                            observableZaposleni.Add(new Zaposleni(jmb, ime, prezime, postBr)); // Add to the ObservableCollection
                            LoadDataTabelaZaposleni();
                            // Optionally, clear the TextBoxes after adding the employee
                            employeeInfoBoxName.Clear();
                            employeeInfoBoxLastname.Clear();
                            employeeInfoBoxJMB.Clear();
                            employeeInfoBoxCity.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Greška prilikom dodavanja zaposlenog");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occurred during the database operation
                MessageBox.Show("Greška: " + ex.Message);
            }
        }

        //Brisanje postojeceg zaposlenog
        public void IzbrisiZaposlenog()
        {
            // Check if the JMB input is valid (must be 12 characters long)
            if (employeeInfoBoxJMB.Text.Length != 12)
            {
                MessageBox.Show("JMB mora biti tačno 12 brojeva");
                return;
            }

            string jmb = employeeInfoBoxJMB.Text;
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root;"; // Your connection string

            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    // Delete from 'menadzer' table where ZAPOSLENI_JMB matches
                    try
                    {
                        string deleteMenadzerSql = "DELETE FROM menadzer WHERE ZAPOSLENI_JMB = @JMB";
                        using (MySqlCommand cmd1 = new MySqlCommand(deleteMenadzerSql, con))
                        {
                            cmd1.Parameters.AddWithValue("@JMB", jmb);
                            cmd1.ExecuteNonQuery();
                        }
                    }
                    catch (Exception e)
                    {
                        // Handle specific error for deleting from menadzer table
                        MessageBox.Show("Error while deleting from menadzer: " + e.Message);
                    }

                    // Delete from 'Zaposleni' table where JMB matches
                    try
                    {
                        string deleteZaposleniSql = "DELETE FROM Zaposleni WHERE JMB = @JMB";
                        using (MySqlCommand cmd2 = new MySqlCommand(deleteZaposleniSql, con))
                        {
                            cmd2.Parameters.AddWithValue("@JMB", jmb);
                            int rowsAffected = cmd2.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Brisanje uspješno izvršeno");
                                LoadDataTabelaZaposleni();
                            }
                            else
                            {
                                MessageBox.Show("Nepostojeci JMB");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        // Handle error for deleting from Zaposleni table
                        MessageBox.Show("Error while deleting from Zaposleni: " + e.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle connection errors or any other unexpected errors
                MessageBox.Show("Error connecting to the database: " + ex.Message);
            }
        }

        // Method to update the database
        private void UpdateDatabaseField(string columnName, string newValue, string jmb)
        {
            string connectionString = "Server=localhost,3306;Database=projektni;Uid=root;Pwd=root;";
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    string query = $"UPDATE projektni.zaposleni SET {columnName} = @newValue WHERE JMB = @jmb";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@newValue", newValue);
                        cmd.Parameters.AddWithValue("@jmb", jmb);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error updating database: " + e.Message);
            }
        }

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





        //!!!!!!!!!!!!!  POCETAK DUGMADI ZAPOSLENI GRIDA  !!!!!!!!!!!!!

        //Admin aplikacija, Izmjena inforamcija zaposlenih
        private void adminEmployeeInfoUpdate(object sender, RoutedEventArgs e)
        {
            ConfirmWindow confirmWindow = new ConfirmWindow(this, "edit");
            confirmWindow.ShowDialog();
        }

        //Admin aplikacija, Brisanje iz tabele zaposlenih
        private void adminEmployeeInfoDelete(object sender, RoutedEventArgs e)
        {
            ConfirmWindow confirmWindow = new ConfirmWindow(this, "delete");
            confirmWindow.ShowDialog();
        }

        //Admin aplikacija, Dodavanje novog zaposlenog
        private void adminEmployeeInfoAdd(object sender, RoutedEventArgs e)
        {

            ConfirmWindow confirmWindow = new ConfirmWindow(this, "add");
            confirmWindow.ShowDialog();
        }

        //Funkcija za trazenje Postanskog broja na osnovu imena grada
        private string getPostanskiBrojFromImeMjesta()
        {
            string connectionString = "Server=localhost,3306;Database=projektni;Uid=root;Pwd=root;";
            try
            {
                var selectedEmployee = adminEmployeeTable.SelectedItem as Zaposleni;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    MySqlCommand cmd1 = new MySqlCommand("SELECT infoMjesta FROM projektni.mjesto_sa_postanskimbr", connection);
                    MySqlDataReader reader1 = cmd1.ExecuteReader();
                    mestoOptions.Clear();
                    while (reader1.Read())
                    {
                        mestoOptions.Add(reader1["infoMjesta"].ToString());
                    }
                   

                    foreach(var item in mestoOptions)
                    {
                        string listMjesta = item.ToString();
                        string[] tmp = listMjesta.Split(new string[] { "-" }, StringSplitOptions.None);
                        if (tmp[1] == selectedEmployee.Mjesto)
                        {
                            return tmp[0];
                        }
                    }
                    reader1.Close();
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error connectiong to database: " + ex.Message);
            }
            return null;
        }


        //Zaposleni DataGrid tj Tabela
        private void employeeTableInfoUpdate(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedItem = adminEmployeeTable.SelectedItem as Zaposleni;

            
            if (selectedItem != null)
            {
                employeeInfoBoxUpdateButton.Visibility = Visibility.Visible;

                employeeInfoBoxName.Text = selectedItem.Ime;
                employeeInfoBoxLastname.Text = selectedItem.Prezime;
                employeeInfoBoxJMB.Text = selectedItem.JMB;
                employeeInfoBoxCity.Text = getPostanskiBrojFromImeMjesta();
            }
            else
            {
                employeeInfoBoxUpdateButton.Visibility = Visibility.Collapsed;

                employeeInfoBoxName.Clear();
                employeeInfoBoxLastname.Clear();
                employeeInfoBoxJMB.Clear();
                employeeInfoBoxCity.Clear();
            }
        }




        //!!!!!!!!!!!!!  KRAJ DUGMADI ZAPOSLENI GRIDA  !!!!!!!!!!!!!





        //!!!!!!!!!!!!!  POCETAK DUGMADI ISPORUKE GRIDA  !!!!!!!!!!!!!



        //Otvara prozor za dodavanje NARUDZBI
        private void addDeliveryOrderButton(object sender, RoutedEventArgs e)
        {
            ShipmentViewWindow shipmentViewWindow = new ShipmentViewWindow("addNewNabavka");
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

                adminDeliveriesDataGrid.ItemsSource = null;
                adminDeliveriesDataGrid.ItemsSource = observablePregledNabavki;

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

                listDeliveriesDataGrid.ItemsSource = null;
                // Bind the ObservableCollection to the DataGrid
                listDeliveriesDataGrid.ItemsSource = observablePregledIsporukaNabavki;

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
                ConfirmWindow confirmWindow = new ConfirmWindow(this, "deleteSelectedNabavku");
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

                        shipmentView.addSingleIsporuka((int)shipmentId, datum, menadzerJmb, transporterName);

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
                else {

                    ConfirmWindow confirmWindow = new ConfirmWindow(this, "deleteSelectedIsporuku");
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
                using(MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    try
                    {
                        var selectedRow = listDeliveriesDataGrid.SelectedItem;
                        var isporukaId =(selectedRow as PregledIsporukaNabavke)?.Isporuka;
                        deleteIsporuke((int)isporukaId);
                    }
                    catch(Exception ex)
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
                using(MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM projektni.sadrzaj_skladista";
                    using(MySqlCommand cmd = new MySqlCommand(query,connection))
                    {
                        observablePregledSkladista.Clear();
                        using(MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while(reader.Read())
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
            catch(Exception ex)
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
                using(MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    observablePregledSpecificnihProdukta.Clear();


                    string query = "SELECT * from projektni.pregled_isporuka_pojedinacnog_produkta WHERE ID_Produkta=@ID_Produkta";
                    try
                    {
                        using (MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@ID_Produkta", selectedItemId);
                            using(MySqlDataReader reader = cmd.ExecuteReader())
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
                    catch(Exception ex)
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
                                if(observablePregledSpecificnihProdukta.Count == 0)
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

        //Treba poslagati ove funkicje nakon zavrsetka;
    }
}