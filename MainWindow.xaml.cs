﻿using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using System.Windows;
using WpfApp1.database.employee;
using System.Windows.Controls;
using WpfApp1.database.grids;
using WpfApp1.database.items;
using System.Data;

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


        public MainWindow()
        {
            InitializeComponent();
            observableZaposleni = new ObservableCollection<Zaposleni>();
            mestoOptions = new ObservableCollection<string>();
            pregledDostavljacaViews = new ObservableCollection<PregledDostavljacaView>();
            observablePregledIsporuka = new ObservableCollection<PregledIsporukaView>();
            observablePregledNabavki = new ObservableCollection<PregledNabavkiView>();
            observablePregledIsporukaNabavki = new ObservableCollection<PregledIsporukaNabavke>();
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



        //Zaposleni DataGrid tj Tabela
        private void employeeTableInfoUpdate(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedItem = adminEmployeeTable.SelectedItem as Zaposleni;

            // Toggle the button visibility based on the selection
            if (selectedItem != null)
            {
                employeeInfoBoxUpdateButton.Visibility = Visibility.Visible; // Show the button when an item is selected

                employeeInfoBoxName.Text = selectedItem.Ime;
                employeeInfoBoxLastname.Text = selectedItem.Prezime;
                employeeInfoBoxJMB.Text = selectedItem.JMB;
                employeeInfoBoxCity.Text = selectedItem.Mjesto;
            }
            else
            {
                employeeInfoBoxUpdateButton.Visibility = Visibility.Collapsed; // Hide the button when no item is selected

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
            ShipmentViewWindow shipmentViewWindow = new ShipmentViewWindow();
            shipmentViewWindow.ShowDialog();
        }

        //Otvara prozor za dodavanje ISPORUKA
        private void adminDeliveriesShipmentButton(object sender, RoutedEventArgs e)
        {
            ShipmentViewWindow shipmentViewWindow = new ShipmentViewWindow();
            shipmentViewWindow.ShowDialog();
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

                    // Create the ObservableCollection that will hold the data
                    observablePregledNabavki.Clear();


                    // Query to retrieve data from the database
                    string query = "SELECT * FROM projektni.isporuke_nabavke";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                DateOnly? datumDostave = reader.IsDBNull(3) ? (DateOnly?)null : DateOnly.FromDateTime(reader.GetDateTime(3));


                                // Add the data to the ObservableCollection
                                observablePregledNabavki.Add(new PregledNabavkiView(
                                    reader.GetInt32(0),   // idNabavke
                                    reader.GetString(1),  // nazivDostavljaca
                                    reader.GetDecimal(2),  // adresaDostavljaca
                                    datumDostave   // datumDostave
                                ));
                            }
                        }
                    }
                }

                // Bind the ObservableCollection to the DataGrid
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
            string connectionString = "Server=localhost,3306;Database=projektni;Uid=root;Pwd=root;"; // Replace with your connection string
            try
            {
                var selectedRow = adminDeliveriesDataGrid.SelectedItem; // Replace with your actual class type

                // Get the transporter name (or ID, depending on your table structure)
                var transporterName = (selectedRow as PregledNabavkiView)?.IdNabavke;
                

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    
                    // Create the ObservableCollection that will hold the data
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

                                DateOnly? datumDostave = reader.IsDBNull(6) ? (DateOnly?)null : DateOnly.FromDateTime(reader.GetDateTime(6));


                                // Add the data to the ObservableCollection
                                observablePregledIsporukaNabavki.Add(new PregledIsporukaNabavke(
                                    reader.GetInt32(0),   // idNabavke
                                    reader.GetString(1),  // nazivProdukta
                                    reader.GetString(2),  // vrstaProdukta
                                    reader.GetInt32(3),   // kolicinaProdukta
                                    reader.GetString(4),  // proizvodjac
                                    reader.GetDecimal(5), // ukupnaCenaProdukta
                                    datumDostave,   // datumDostave
                                    reader.GetInt32(7) 
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

            if(hitTest == null || hitTest is not DataGridRow)
            {
                transporterListDataGrid.SelectedItem = null;
            }
        }


        //!!!!!!!!!!!!!  KRAJ FUNKCIJA ISPORUKA GRIDA  !!!!!!!!!!!!!
    }
}