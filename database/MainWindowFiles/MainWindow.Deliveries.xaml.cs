using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.database.grids;

namespace WpfApp1 
{ 
    //Stavke za rad sa Nabavkama i Isporukama

    public partial class MainWindow
    {
        //Otvara prozor za dodavanje NARUDZBI
        private void addDeliveryOrderButton(object sender, RoutedEventArgs e)
        {
            ShipmentViewWindow shipmentViewWindow = new ShipmentViewWindow("addNewNabavka", _adminJmb, _theme, _language);
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
                ShipmentViewWindow shipmentViewWindow = new ShipmentViewWindow(this, "addNewIsporuka", _theme, _language);
                shipmentViewWindow.Owner = this;
                shipmentViewWindow.ShowDialog();
            }
        }

        //Ucitava informacije o Nabavkama u tabelu
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

                if (observablePregledIsporukaNabavki == null || observablePregledIsporukaNabavki.Count == 0)
                {
                    noDataMessage.Visibility = Visibility.Visible;
                }
                else
                {
                    noDataMessage.Visibility = Visibility.Collapsed;
                    listDeliveriesDataGrid.ItemsSource = null;
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
                ConfirmWindow confirmWindow = new ConfirmWindow(this, "deleteSelectedNabavku", _adminJmb, _language);
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

        //Dodavanje nove Isporuke za postojecu izabranu Nabavku
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
                        string menadzerJmb = _adminJmb;


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

                    ConfirmWindow confirmWindow = new ConfirmWindow(this, "deleteSelectedIsporuku", _adminJmb, _language);
                    confirmWindow.ShowDialog();
                }
            }
        }

        //Ponovno ucitavanje Isporuka za izabranu Nabavku nakon promijene u bazi(Dodavanje, brisanje)
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


        //Otvara prozor ShipmentView za dodavanje isporuka i narudzbi
        private void deliveriesViewOpen(object sender, RoutedEventArgs e)
        {
            loadAdminDeliveriesDataGridData();
            adminDeliveriesViewGrid.Visibility = Visibility.Visible;
            adminEmployeeViewGrid.Visibility = Visibility.Hidden;
            transportersViewGrid.Visibility = Visibility.Hidden;
            storageViewGrid.Visibility = Visibility.Hidden;
            clearAllBoxData();
        }
    }
}
