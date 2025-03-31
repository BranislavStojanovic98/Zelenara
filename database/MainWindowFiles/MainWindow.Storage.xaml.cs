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
    //Stavke za Skladiste Grid

    public partial class MainWindow
    {
        //Otvara prozor sa listom svih dostupnih produkta koji se nalaze u prodavnici i njihovom kolicinom
        private void storageViewOpen(object sender, RoutedEventArgs e)
        {
            loadStorageView();
            storageViewGrid.Visibility = Visibility.Visible;
            adminEmployeeViewGrid.Visibility = Visibility.Hidden;
            adminDeliveriesViewGrid.Visibility = Visibility.Hidden;
            transportersViewGrid.Visibility = Visibility.Hidden;
            clearAllBoxData();
        }

        //Ucitava sve produkte u skladistu
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
                                        reader.GetInt32(0),         //ID Nabavke
                                        reader.GetInt32(1),         //ID Isporuke
                                        reader.GetInt32(2),         //Kolicina Produkta
                                        reader.GetString(3),        //Proizvodjac
                                        reader.GetDecimal(4),       //Cena
                                        reader.GetString(5),        //Dostavljac
                                        reader.GetString(6),        //Datum
                                        reader.GetInt32(7)          //ID Produkta
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

        //Ucitavanje specificnog produkta preko pretrage sa Produkt ID u Skladiste gridu
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
                                        reader.GetInt32(0),         //ID Nabavke
                                        reader.GetInt32(1),         //ID Isporuke
                                        reader.GetInt32(2),         //Kolicina
                                        reader.GetString(3),        //Proizvodjac
                                        reader.GetDecimal(4),       //Cena
                                        reader.GetString(5),        //Dostavljac
                                        reader.GetString(6),        //Datum
                                        reader.GetInt32(7)          //ID Produkta
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

        //Ucitavanje specificnog produkta preko pretrage sa Produkt ID u Skladiste gridu
        private void loadStorageSpecificProductDataGridSearch(string prodcer)
        {
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    observablePregledSpecificnihProdukta.Clear();


                    try
                    {
                        using (MySqlCommand cmd = new MySqlCommand("filter_by_producers", connection))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@name", prodcer);
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    observablePregledSpecificnihProdukta.Add(new PregledIsporukaPoProduktu(
                                        reader.GetInt32(0),         //ID Nabavke
                                        reader.GetInt32(1),         //ID Isporuke
                                        reader.GetInt32(2),         //Kolicina
                                        reader.GetString(3),        //Proizvodjac
                                        reader.GetDecimal(4),       //Cena
                                        reader.GetString(5),        //Dostavljac
                                        reader.GetString(6),        //Datum
                                        reader.GetInt32(7)          //ID Produkta
                                        ));
                                }
                                if (observablePregledSpecificnihProdukta.Count == 0)
                                {
                                    MessageBox.Show("Nepostojeci Dostavljač");
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

        //Dugme kada pritisnemo pretrazi u storage View, za trazenje produkta po produkt ID-ju
        private void storageViewSearchByProductId(object sender, RoutedEventArgs e)
        {
            string idStr = storageViewSearchBox.Text;
            if (idStr != "")
            {
                if(int.TryParse(idStr, out int id))
                {
                    loadStorageSpecificProductDataGridSearch(id);
                }
                else
                {
                    loadStorageSpecificProductDataGridSearch(idStr);
                }
            }
            else
            {
                MessageBox.Show("Unesite postojeći Produkt ID ili Naziv Proizvođača!");
            }
        }
    }
}
