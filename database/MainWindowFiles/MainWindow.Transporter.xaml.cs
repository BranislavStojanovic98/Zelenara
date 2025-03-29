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

    //Stavlke za Dostavljaci grid
    public partial class MainWindow
    {

        //Otvara Dostavljaci grid
        private void transportersViewOpen(object sender, RoutedEventArgs e)
        {
            LoadDataTabelaDostavljaca();
            transportersViewGrid.Visibility = Visibility.Visible;
            adminEmployeeViewGrid.Visibility = Visibility.Hidden;
            adminDeliveriesViewGrid.Visibility = Visibility.Hidden;
            storageViewGrid.Visibility = Visibility.Hidden;
            clearAllBoxData();
        }

        //Ucitava listu dostupnih dostavljaca
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
                observablePregledIsporuka.Clear();

                var selectedRow = transporterListDataGrid.SelectedItem;

                var transporterName = (selectedRow as PregledDostavljacaView)?.NazivDostavljaca;

                using (var con = new MySqlConnection("Server=localhost,3306;Database=projektni;Uid=root;Pwd=root;"))
                {
                    con.Open();

                    string query = "SELECT * FROM projektni.pregled_isporuka WHERE Dostavljac = @dostavljac";
                    using (var cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@dostavljac", transporterName);

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
                if (observablePregledIsporuka.Count == 0)
                {
                    transporterNoDataBox.Visibility = Visibility.Visible;
                }
                else
                {
                    transporterNoDataBox.Visibility = Visibility.Collapsed;
                }

                transporterDeliveriesListDataGrid.ItemsSource = null;
                transporterDeliveriesListDataGrid.ItemsSource = observablePregledIsporuka;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
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
    }
}
