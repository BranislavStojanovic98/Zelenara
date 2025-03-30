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
using static System.Runtime.InteropServices.JavaScript.JSType;
using WpfApp1.database.items;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for CashierWindow.xaml
    /// </summary>
    public partial class CashierWindow : Window
    {

        private string _employeeJmb;
        decimal ukupnaCena = (long)0.0;
        public CashierWindow(string employeeJmb, string theme, string language)
        {

            InitializeComponent();
            if(theme == "Theme1")
            {
                cashierViewGrid.Background = new SolidColorBrush(Colors.Beige);
                bottomGrid.Background = new SolidColorBrush(Colors.Bisque);
                addItemGrid.Background = new SolidColorBrush(Colors.Bisque);
                receiptListBox.Background = new SolidColorBrush(Colors.Bisque);
                addItemToListButton.Background = SystemColors.ControlLightLightBrush;
                cashierRecieptRemoveItemButton.Background = SystemColors.ControlLightLightBrush;
                cashierRecieptPrintButton.Background = SystemColors.ControlLightLightBrush;
            }
            else if(theme == "Theme2")
            {
                cashierViewGrid.Background = new SolidColorBrush(Colors.Brown);
                bottomGrid.Background = new SolidColorBrush(Colors.BurlyWood);
                addItemGrid.Background = new SolidColorBrush(Colors.BurlyWood);
                receiptListBox.Background = new SolidColorBrush(Colors.BurlyWood);
                addItemToListButton.Background = SystemColors.ControlLightLightBrush;
                cashierRecieptRemoveItemButton.Background = SystemColors.ControlLightLightBrush;
                cashierRecieptPrintButton.Background = SystemColors.ControlLightLightBrush;
            }

            if(language == "English")
            {
                label1.Content = "Bill";
                label2.Content = "Add Product";
                label3.Content = "Product:";
                label4.Content = "Amount";
                textBlock.Text = "Total:";

                addItemToListButton.Content = "Add";
                cashierRemoveItem.Content = "Remove";
                cashierRecieptPrintButton.Content = "Print";
                cashierRecieptRemoveItemButton.Content = "Clear";
                closeButton.Content = "Back";
            }

                loadProductListComboBoxComboBox();
            _employeeJmb = employeeJmb;
        }

        //Ucitava sve moguce produkte
        private void loadProductListComboBoxComboBox()
        {
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root;";
            List<string> maker = new List<string>();

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
                        maker.Add(reader.GetString("produktInfo"));
                    }
                    productListComboBox.ItemsSource = maker;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating database: " + ex.Message);
            }
        }

        //Dodaje stavku na racun
        private void addItemToListClick(object sender, RoutedEventArgs e)
        {
            string produkt = productListComboBox.SelectedItem.ToString();
            string kolicina = amountItemBox.Text;
            int rez;
            
            if (!(int.TryParse(kolicina, out rez)))
            {
                MessageBox.Show("Unesite cijelobrojnu vrijednost za količinu");
                return;
            }

            //Trazimo cijenu produkta

            string[] tmp = produkt.Split(new string[] { " " }, StringSplitOptions.TrimEntries);

            string naziv = tmp[0];
            string vrsta = tmp[1];

            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root;";

            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    string proizvodIdQuery = "SELECT Cena FROM produkt WHERE Naziv=@naziv AND Vrsta=@vrsta";

                    MySqlCommand getCena = new MySqlCommand(proizvodIdQuery, con);
                    getCena.Parameters.AddWithValue("@naziv", naziv);
                    getCena.Parameters.AddWithValue("@vrsta", vrsta);

                    //Proizvod ID
                    var tmpCena = getCena.ExecuteScalar();

                    decimal cena = (decimal)tmpCena * (int.Parse(kolicina));

                    string kombinovano = $"{produkt} - {kolicina} - {cena}";
                    ukupnaCena += cena;
                    receiptListBox.Items.Add(kombinovano);
                    
                }
                receiptTotalCostBox.Text = ukupnaCena.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connectiong to database: " + ex.Message);
            }
        }

        //Dodaj racun u database
        private void confirmReceiptAdditionClick(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root;";

            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    string idRacunaQuery = "SELECT MAX(idRacuna)+1 FROM račun";
                    MySqlCommand getidRacuna = new MySqlCommand(idRacunaQuery, con);
                    var idRacuna = getidRacuna.ExecuteScalar();
                    if (idRacuna is DBNull)
                    {
                        idRacuna = 1;
                    }

                    string prodanaStavkaQuery = "SELECT MAX(prodanaStavka) FROM kupovina_produkta";
                    MySqlCommand getprodanaStavka = new MySqlCommand(prodanaStavkaQuery, con);
                    var prodanaStavka = getprodanaStavka.ExecuteScalar();
                    if (prodanaStavka is DBNull)
                    {
                        prodanaStavka = 0;
                    }
                    int currentProdanaStavka = (int)prodanaStavka;

                    //Get trenutni datum
                    string datum = DateTime.Today.ToString("yyyy-MM-dd");

                    string insertRacun = "INSERT INTO račun (idRacuna, VremeIzdavanja, RADNIK_ZAPOSLENI_JMB) " +
                        "VALUES (@idRacuna, @VremeIzdavanja, @RADNIK_ZAPOSLENI_JMB)";

                    using (MySqlCommand cmd = new MySqlCommand(insertRacun, con))
                    {
                        cmd.Parameters.AddWithValue("@idRacuna", idRacuna);
                        cmd.Parameters.AddWithValue("@VremeIzdavanja", datum);
                        cmd.Parameters.AddWithValue("@RADNIK_ZAPOSLENI_JMB", _employeeJmb);
                        // Execute the insert query
                        cmd.ExecuteNonQuery();
                    }

                    string insertKupovinaProdukta = "INSERT INTO kupovina_produkta (RAČUN_idRacuna, Kolicina, Cena, sadrzaj_skladista_idProdukta, prodanaStavka) " +
                            "VALUES (@RAČUN_idRacuna, @Kolicina, @Cena,  @sadrzaj_skladista_idProdukta, @prodanaStavka)";

                    foreach (var item in receiptListBox.Items)
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

                        decimal cena = decimal.Parse(tmp[2]);

                        ++currentProdanaStavka;

                        using (MySqlCommand cmd = new MySqlCommand(insertKupovinaProdukta, con))
                        {
                            cmd.Parameters.AddWithValue("@RAČUN_idRacuna", idRacuna);
                            cmd.Parameters.AddWithValue("@Kolicina", tmp[1]);
                            cmd.Parameters.AddWithValue("@Cena", cena);
                            cmd.Parameters.AddWithValue("@sadrzaj_skladista_idProdukta", proizvodId);
                            cmd.Parameters.AddWithValue("@prodanaStavka", currentProdanaStavka);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                receiptListBox.Items.Clear();
                ukupnaCena = (long)0.0;
                receiptTotalCostBox.Text = ukupnaCena.ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error connecting to database: " + ex.Message);
            }
        }

        //Ukloni izabranu stavku iz racuna
        private void removeItemFromListBoxClick(object sender, RoutedEventArgs e)
        {
            if (receiptListBox.SelectedItem != null)
            {
                string item = receiptListBox.SelectedItem.ToString();
                string[] tmp = item.Split(new string[] { " - " }, StringSplitOptions.None);

                string cena = tmp[2];

                decimal tmpCena = decimal.Parse(tmp[2]);
                ukupnaCena -= tmpCena;

                receiptListBox.Items.Remove(receiptListBox.SelectedItem);
                receiptTotalCostBox.Text = ukupnaCena.ToString();
            }
            else
            {
                MessageBox.Show("Izaberite stavku koju zelite ukloniti!");
            }
        }

        //Uklanja sve produkte koji su na racunu
        private void clearReceiptListClick(object sender, RoutedEventArgs e)
        {
            receiptListBox.Items.Clear();
            ukupnaCena = (long)0.0;
            receiptTotalCostBox.Text = ukupnaCena.ToString();
        }

        private void closeCashierWindowClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }


}