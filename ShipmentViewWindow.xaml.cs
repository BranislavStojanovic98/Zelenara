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
using WpfApp1.database.items;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for ShipmentViewWindow.xaml
    /// </summary>
    public partial class ShipmentViewWindow : Window
    {
        private string _action;
        private MainWindow _mainWindow;

        public ShipmentViewWindow()
        {
            InitializeComponent();
            loadShipmentViewCompanyNameComboBox();
            loadShipmentViewProductComboBox();
        }
        public ShipmentViewWindow(MainWindow mainWindow, string action)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _action = action;
            loadShipmentViewCompanyNameComboBox();
            loadShipmentViewProductComboBox();
        }
        public ShipmentViewWindow(string action)
        {
            _action = action;
            _mainWindow = null;
            InitializeComponent();
            loadShipmentViewCompanyNameComboBox();
            loadShipmentViewProductComboBox();
        }

        //Otkazivanje narudzbe
        private void shipmentViewCancleButtonClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        //Kreiranje nove nabavke

        //Potvrda za dodavanje narudzbe
        private void shipmentViewConfirmButtonConfirm(object sender, RoutedEventArgs e)
        {
            ConfirmWindow confirmWindow;
            if (_action == "addNewIsporuka")
            {
                confirmWindow = new ConfirmWindow(this, "addNewIsporuka");
                confirmWindow.Owner = this;
                confirmWindow.ShowDialog();
            }
            else if (_action == "addNewNabavka")
            {
                confirmWindow = new ConfirmWindow(this, "addNewNabavka");
                confirmWindow.ShowDialog();
                if (this.Owner is MainWindow mainWindow)
                {
                    mainWindow.loadAdminDeliveriesDataGridData();
                    _mainWindow = mainWindow;
                }
            }
            else if (_action == "deleteSelectedNabavku")
            {
                confirmWindow = new ConfirmWindow(this, "deleteSelectedNabavku");
                confirmWindow.Owner = this;
                confirmWindow.ShowDialog();
                if (this.Owner is MainWindow mainWindow)
                {
                    mainWindow.loadAdminDeliveriesDataGridData();
                }
            }
        }
        public void dodajNovuNabavku()
        {
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root;";

            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    try
                    {
                        //Get nabavkaID
                        string nabavkaIdQuery = "SELECT MAX(idPotvrde)+1 FROM nabavka";
                        MySqlCommand getNabavkaId = new MySqlCommand(nabavkaIdQuery, con);
                        var nabavkaId = getNabavkaId.ExecuteScalar();

                        //Get trenutni datum
                        string datum = DateTime.Today.ToString("yyyy-MM-dd");

                        //Get menadzer jmb, treba se doraditi za razlicite menadzere
                        string menadzerJmb = "111111111111"; //Promijeniti kada napravim login!!!!!!!!!!!!


                        //Get DostavljacID
                        string dostavljacIme = shipmentViewCompanyNameComboBox.SelectedItem.ToString();
                        string dostavljacIdQuery = "SELECT idDostavljaca FROM dobavljac where Naziv=@dostavljacIme";
                        MySqlCommand getDostavljacId = new MySqlCommand(dostavljacIdQuery, con);
                        getDostavljacId.Parameters.AddWithValue("@dostavljacIme", dostavljacIme);
                        var dostavljacId = getDostavljacId.ExecuteScalar();
                        

                        //Ubacivanje u `NABAVKA` MySQL tabelu
                        string insertNabavka = "INSERT INTO nabavka (idPotvrde, Datum, MENADZER_ZAPOSLENI_JMB, DOBAVLJAC_idDostavljaca, SKLADISTE_idSkladista) " +
                        "VALUES (@idPotvrde, @Datum, @MENADZER_ZAPOSLENI_JMB, @DOBAVLJAC_idDostavljaca, @SKLADISTE_idSkladista)";

                        using (MySqlCommand cmd = new MySqlCommand(insertNabavka, con))
                        {
                            cmd.Parameters.AddWithValue("@idPotvrde", nabavkaId);
                            cmd.Parameters.AddWithValue("@Datum", datum);
                            cmd.Parameters.AddWithValue("@MENADZER_ZAPOSLENI_JMB", menadzerJmb);
                            cmd.Parameters.AddWithValue("@DOBAVLJAC_idDostavljaca", dostavljacId);
                            cmd.Parameters.AddWithValue("@SKLADISTE_idSkladista", 1);

                            // Execute the insert query
                            cmd.ExecuteNonQuery();
                        }

                        //////////////////////////////////////////////////////////////////////////////////////

                        //Get isporukaID za vise njih iz ListBoxa
                        string isporukaIdQuery = "SELECT MAX(idIsporuke) FROM isporuka";
                        MySqlCommand getIsporukaId = new MySqlCommand(isporukaIdQuery, con);
                        var isporukaId = getIsporukaId.ExecuteScalar();
                        int listBoxItemsCount = shipmentViewDeliveriesListBox.Items.Count;

                        int currentIsporukaId = (int)isporukaId;

                        //Citamo sve Produkte, kolicinu i proizvodjaca iz ListBoxa
                        //Dajemo im svima unikatne isporukaId prije no ih pohranimo u database
                        foreach (var item in shipmentViewDeliveriesListBox.Items)
                        {
                            string listBoxContent = item.ToString();
                            string[] tmp = listBoxContent.Split(new string[] { " - " }, StringSplitOptions.TrimEntries);

                            //Get ProduktID
                            string produkt = tmp[0];
                            string[] tmp2 = produkt.Split(new string[] { " " }, StringSplitOptions.None);

                            string naziv = tmp2[0];
                            string vrsta = tmp2[1];

                            string proizvodIdQuery = "SELECT idProdukta FROM produkt WHERE Naziv=@naziv AND Vrsta=@vrsta";
                            MySqlCommand getProizvodId = new MySqlCommand(proizvodIdQuery, con);
                            getProizvodId.Parameters.AddWithValue("@naziv", naziv);
                            getProizvodId.Parameters.AddWithValue("@vrsta", vrsta);

                            //Proizvod ID
                            var proizvodId = getProizvodId.ExecuteScalar();


                            //Get Kolicina produkta
                            string kolicina = tmp[1];

                            //Get proizvodjac produkta
                            string proizvodjac = tmp[2];

                            ++currentIsporukaId;

                            //Ubacivanje u `ISPORUKA` MySQL tabelu
                            string insertIsporuka = "INSERT INTO isporuka (idIsporuke, NABAVKA_idPotvrde, Datum, DOBAVLJAC_idDostavljaca) " +
                            "VALUES (@idIsporuke, @NABAVKA_idPotvrde, @Datum, @DOBAVLJAC_idDostavljaca)";

                            using (MySqlCommand cmd = new MySqlCommand(insertIsporuka, con))
                            {
                                cmd.Parameters.AddWithValue("@idIsporuke", currentIsporukaId);
                                cmd.Parameters.AddWithValue("@NABAVKA_idPotvrde", nabavkaId);
                                cmd.Parameters.AddWithValue("@Datum", datum);
                                cmd.Parameters.AddWithValue("@DOBAVLJAC_idDostavljaca", dostavljacId);

                                // Execute the insert query
                                cmd.ExecuteNonQuery();
                            }


                            //Ubacivanje u `ISPORUKA_PRODUKTA` MySQL tabelu
                            string insertIsporukaProdukta = "INSERT INTO isporuka_produkta (DOSTAVA_idIsporuke, PRODUKT_idProdukta, Kolicina) " +
                            "VALUES (@DOSTAVA_idIsporuke, @PRODUKT_idProdukta, @Kolicina)";

                            using (MySqlCommand cmd = new MySqlCommand(insertIsporukaProdukta, con))
                            {
                                cmd.Parameters.AddWithValue("@DOSTAVA_idIsporuke", currentIsporukaId);
                                cmd.Parameters.AddWithValue("@PRODUKT_idProdukta", proizvodId);
                                cmd.Parameters.AddWithValue("@Kolicina", kolicina);

                                // Execute the insert query
                                cmd.ExecuteNonQuery();
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Greska" + ex.Message);
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to the database: " + ex.Message);
            }
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
            if (shipmentViewProductComboBox.SelectedItem != null)
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

        private void addProductToTableClick(object sender, RoutedEventArgs e)
        {

            string produkt = shipmentViewProductComboBox.SelectedItem.ToString();
            string kolicina = shipmentViewQuantityTextBox.Text;
            string proizvodjac = shipmentViewMakerComboBox.SelectedItem.ToString();

            string kombinovano = $"{produkt} - {kolicina} - {proizvodjac}";

            shipmentViewDeliveriesListBox.Items.Add(kombinovano);

            shipmentViewCompanyNameComboBox.IsEnabled = shipmentViewDeliveriesListBox.Items.Count == 0;

        }

        private void removeProductFromTableClick(object sender, RoutedEventArgs e)
        {
            if (shipmentViewDeliveriesListBox.SelectedItem != null)
            {
                shipmentViewDeliveriesListBox.Items.Remove(shipmentViewDeliveriesListBox.SelectedItem);
            }
            else
            {
                MessageBox.Show("Izaberite unos koji zelite ukloniti!");
            }
            shipmentViewCompanyNameComboBox.IsEnabled = shipmentViewDeliveriesListBox.Items.Count == 0;
        }


        //Dodavanje Isporuke u selektovanu Nabavku
        public void addSingleIsporuka(int nabavkaId, string datum, string menadzerJmb, string transporterName)
        {
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root;";

            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    try
                    {
                        shipmentViewCompanyNameComboBox.SelectedItem = transporterName;
                        string dostavljacIdQuery = "SELECT idDostavljaca FROM dobavljac where Naziv=@dostavljacIme";
                        MySqlCommand getDostavljacId = new MySqlCommand(dostavljacIdQuery, con);
                        getDostavljacId.Parameters.AddWithValue("@dostavljacIme", transporterName);
                        var dostavljacId = getDostavljacId.ExecuteScalar();
                        shipmentViewCompanyNameComboBox.IsEnabled = false;
                        //Get isporukaID za vise njih iz ListBoxa
                        string isporukaIdQuery = "SELECT MAX(idIsporuke) FROM isporuka";
                        MySqlCommand getIsporukaId = new MySqlCommand(isporukaIdQuery, con);
                        var isporukaId = getIsporukaId.ExecuteScalar();
                        int listBoxItemsCount = shipmentViewDeliveriesListBox.Items.Count;

                        int currentIsporukaId = (int)isporukaId;

                        //Citamo sve Produkte, kolicinu i proizvodjaca iz ListBoxa
                        //Dajemo im svima unikatne isporukaId prije no ih pohranimo u database
                        foreach (var item in shipmentViewDeliveriesListBox.Items)
                        {
                            string listBoxContent = item.ToString();
                            string[] tmp = listBoxContent.Split(new string[] { " - " }, StringSplitOptions.TrimEntries);

                            //Get ProduktID
                            string produkt = tmp[0];
                            string[] tmp2 = produkt.Split(new string[] { " " }, StringSplitOptions.None);

                            string naziv = tmp2[0];
                            string vrsta = tmp2[1];

                            string proizvodIdQuery = "SELECT idProdukta FROM produkt WHERE Naziv=@naziv AND Vrsta=@vrsta";
                            MySqlCommand getProizvodId = new MySqlCommand(proizvodIdQuery, con);
                            getProizvodId.Parameters.AddWithValue("@naziv", naziv);
                            getProizvodId.Parameters.AddWithValue("@vrsta", vrsta);

                            //Proizvod ID
                            var proizvodId = getProizvodId.ExecuteScalar();


                            //Get Kolicina produkta
                            string kolicina = tmp[1];

                            //Get proizvodjac produkta
                            string proizvodjac = tmp[2];

                            ++currentIsporukaId;

                            //Ubacivanje u `ISPORUKA` MySQL tabelu
                            string insertIsporuka = "INSERT INTO isporuka (idIsporuke, NABAVKA_idPotvrde, Datum, DOBAVLJAC_idDostavljaca) " +
                            "VALUES (@idIsporuke, @NABAVKA_idPotvrde, @Datum, @DOBAVLJAC_idDostavljaca)";

                            using (MySqlCommand cmd = new MySqlCommand(insertIsporuka, con))
                            {
                                cmd.Parameters.AddWithValue("@idIsporuke", currentIsporukaId);
                                cmd.Parameters.AddWithValue("@NABAVKA_idPotvrde", nabavkaId);
                                cmd.Parameters.AddWithValue("@Datum", datum);
                                cmd.Parameters.AddWithValue("@DOBAVLJAC_idDostavljaca", dostavljacId);

                                // Execute the insert query
                                cmd.ExecuteNonQuery();
                            }


                            //Ubacivanje u `ISPORUKA_PRODUKTA` MySQL tabelu
                            string insertIsporukaProdukta = "INSERT INTO isporuka_produkta (DOSTAVA_idIsporuke, PRODUKT_idProdukta, Kolicina) " +
                            "VALUES (@DOSTAVA_idIsporuke, @PRODUKT_idProdukta, @Kolicina)";

                            using (MySqlCommand cmd = new MySqlCommand(insertIsporukaProdukta, con))
                            {
                                cmd.Parameters.AddWithValue("@DOSTAVA_idIsporuke", currentIsporukaId);
                                cmd.Parameters.AddWithValue("@PRODUKT_idProdukta", proizvodId);
                                cmd.Parameters.AddWithValue("@Kolicina", kolicina);

                                // Execute the insert query
                                cmd.ExecuteNonQuery();
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Greska" + ex.Message);
                    }
                }

                shipmentViewCompanyNameComboBox.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to the database: " + ex.Message);
            }
        }

    }
}
