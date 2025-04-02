using Microsoft.IdentityModel.Tokens;
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
using WpfApp1.database.employee;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for EmployeeAccountWindow.xaml
    /// </summary>
    public partial class EmployeeAccountWindow : Window
    {

        private string _adminJmb;
        private string _language;
        private Zaposleni _zaposleni;
        private bool _canClose = true;

        public EmployeeAccountWindow(Zaposleni selectedItem, string jmb, string language, string theme)
        {
            _adminJmb = jmb;
            _language = language;
            _zaposleni = selectedItem;
            InitializeComponent();

            if(theme == "Default")
            {
                adminEmployeeLoginAccountGrid.Background = SystemColors.ControlLightBrush;
            }
            else if(theme == "Theme1")
            {
                adminEmployeeLoginAccountGrid.Background = new SolidColorBrush(Colors.Bisque);
            }
            else if(theme == "Theme2")
            {
                adminEmployeeLoginAccountGrid.Background = new SolidColorBrush(Colors.BurlyWood);
            }

            if(language == "Serbian")
            {
                employeeLabel1.Content = "Korisnički nalog";
                employeeLabel2.Content = "zaposlenog";
                employeeLabel3.Content = "Korisničko ime:";
                employeeLabel4.Content = "Lozinka:";
                employeeLabel5.Content = "Potvrda lozinke:";

                employeeAccountConfirmButton.Content = "Potvrdi";
                employeeAccountConfirmChangesButton.Content = "Izmijeni";
            }
            else if(language == "English")
            {
                employeeLabel1.Content = "User employee";
                employeeLabel2.Content = "account";
                employeeLabel3.Content = "Username:";
                employeeLabel4.Content = "Password:";
                employeeLabel5.Content = "Confirm Password:";

                employeeAccountConfirmButton.Content = "Confirm";
                employeeAccountConfirmChangesButton.Content = "Change";
            }

                loadExistingEmployeeAccountInfo(selectedItem);

            if(employeeAccountNameBox.Text == "")
            {
                employeeAccountConfirmButton.Visibility = Visibility.Visible;
                employeeAccountConfirmChangesButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                employeeAccountConfirmButton.Visibility = Visibility.Collapsed;
                employeeAccountConfirmChangesButton.Visibility = Visibility.Visible;
            }

        }

        //Funkcija za dodavanje naloga zaposlenog
        public void addEmployeeAccount()
        {
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root";

            string username = employeeAccountNameBox.Text;
            string password = "";
            bool isAdmin = isAdminCheckBox.IsChecked == true;
            bool isAdminTrue = false;

            if (employeeAccountPasswordBox.Password == employeeAccountPasswordConfirmBox.Password)
            {
                _canClose = false;
                password = employeeAccountPasswordBox.Password;
            }
            else
            {
                _canClose = false;
                return;
            }

            if (password == "")
            {
                _canClose = false;
                return;
            }

            if(password.Length < 5)
            {
                _canClose = false;
                return;
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();


                    string check = "SELECT * FROM projektni.zaposleni_nalog WHERE zaposleni_JMB=@jmb";
                    try
                    {
                        MySqlCommand cmnd1 = new MySqlCommand(check, connection);
                        cmnd1.Parameters.AddWithValue("@jmb", _zaposleni.JMB);
                        MySqlDataReader reader = cmnd1.ExecuteReader();
                        while (reader.Read())
                        {

                            if (_zaposleni.JMB == reader.GetString(0))
                            {
                                MessageBox.Show("Izabrani korisnik već posjeduje nalog!");
                                return;
                            }
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Greska prilikom provjere JMB! " + ex.Message);
                    }
                    string query = "INSERT INTO zaposleni_nalog (zaposleni_JMB, username, sifra, is_menadzer) VALUES (@jmb, @username, @password, @isMenager)";
                    try
                    {
                        using (MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@jmb", _zaposleni.JMB);
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@password", hashedPassword);

                            if (isAdmin)
                            {

                                string checkMenadzer = "SELECT * FROM projektni.menadzer WHERE ZAPOSLENI_JMB=@jmb";
                                try
                                {
                                    MySqlCommand cmnd1 = new MySqlCommand(checkMenadzer, connection);
                                    cmnd1.Parameters.AddWithValue("@jmb", _zaposleni.JMB);
                                    MySqlDataReader reader = cmnd1.ExecuteReader();
                                    while (reader.Read())
                                    {

                                        if (_zaposleni.JMB == reader.GetString(0))
                                        {
                                            isAdminTrue = true;
                                        }
                                    }
                                    if (reader.HasRows != true)
                                    {
                                        MessageBox.Show("Korisnik nema svojstvo Menadzera!");
                                        return;
                                    }
                                    reader.Close();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Greska prilikom provjere JMB! " + ex.Message);
                                }
                            }

                            if(isAdminTrue)
                            {
                                cmd.Parameters.AddWithValue("@isMenager", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@isMenager", 0);
                            }

                                int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                _canClose = true;
                                MessageBox.Show("Nalog zaposlenog uspješno dodat!");
                                employeeAccountNameBox.Clear();
                                employeeAccountPasswordBox.Clear();
                                employeeAccountPasswordConfirmBox.Clear();
                            }
                            else
                            {
                                MessageBox.Show("Greška prilikom dodavanja naloga zaposlenog");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Greska : " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connectiong to database: " + ex.Message);
            }
        }

        //Dugme za dodavanje korisnickog naloga
        private void confirmEmployeeAccountClick(object sender, RoutedEventArgs e)
        {
            ConfirmWindow confirmWindow = new ConfirmWindow(this, "addEmployeeAccount", _adminJmb, _language);
            confirmWindow.ShowDialog();
            this.Close();
        }


        //funkcija za ucitavanje postojeceg korisnickog naloga i sifre
        private void loadExistingEmployeeAccountInfo(Zaposleni selectedZaposleni)
        {
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root";
            try
            {
                if (selectedZaposleni != null)
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "SELECT * FROM projektni.zaposleni_nalog WHERE zaposleni_JMB=@jmb";
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@jmb", selectedZaposleni.JMB);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (selectedZaposleni.JMB == reader.GetString(0))
                            {
                                employeeAccountNameBox.Text = reader.GetString(1);
                                employeeAccountPasswordBox.Password = "";
                                employeeAccountPasswordConfirmBox.Password = "";
                            }
                        }
                        reader.Close();

                    }
                }
                else
                {
                    employeeAccountNameBox.Clear();
                    employeeAccountPasswordBox.Clear();
                    employeeAccountPasswordConfirmBox.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connectiong to database!" + ex.Message);
            }
        }


        //Izmijena postojeceg korisnickog naloga
        public void changeEmployeeAccount(string username, string password)
        {
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root";
            bool isAdmin = isAdminCheckBox.IsChecked == true;
            bool isAdminTrue = false;

            if (employeeAccountPasswordBox.Password == employeeAccountPasswordConfirmBox.Password)
            {
                password = employeeAccountPasswordBox.Password;
            }
            else
            {
                _canClose = false;
                return;
            }
            
            var selectedItem = _zaposleni;
            if (selectedItem == null)
            {
                _canClose = false;
                return;
            }
            string jmb = selectedItem.JMB;


            if (password == "")
            {
                _canClose = false;
                return;
            }

            if(password.Length < 5)
            {
                _canClose = false;
                return;
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();


                    string query = $"UPDATE projektni.zaposleni_nalog SET username = @username, sifra=@password, is_menadzer=@isMenager WHERE zaposleni_JMB = @jmb";
                    try
                    {
                        using (MySqlCommand cmnd1 = new MySqlCommand(query, connection))
                        {
                            cmnd1.Parameters.AddWithValue("@jmb", jmb);
                            cmnd1.Parameters.AddWithValue("@username", username);
                            cmnd1.Parameters.AddWithValue("@password", hashedPassword);

                            if (isAdmin)
                            {

                                string checkMenadzer = "SELECT * FROM projektni.menadzer WHERE ZAPOSLENI_JMB=@jmb";
                                try
                                {
                                    MySqlCommand cmnd2 = new MySqlCommand(checkMenadzer, connection);
                                    cmnd2.Parameters.AddWithValue("@jmb", _zaposleni.JMB);
                                    MySqlDataReader reader = cmnd2.ExecuteReader();
                                    while (reader.Read())
                                    {

                                        if (_zaposleni.JMB == reader.GetString(0))
                                        {
                                            isAdminTrue = true;
                                        }
                                    }
                                    if (reader.HasRows != true)
                                    {
                                        MessageBox.Show("Korisnik nema svojstvo Menadzera!");
                                        return;
                                    }
                                    reader.Close();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Greska prilikom provjere JMB! " + ex.Message);
                                }
                            }

                            if (isAdminTrue)
                            {
                                cmnd1.Parameters.AddWithValue("@isMenager", 1);
                            }
                            else
                            {
                                cmnd1.Parameters.AddWithValue("@isMenager", 0);
                            }


                            cmnd1.ExecuteNonQuery();
                        }
                        _canClose = true;
                        MessageBox.Show("Izmijene uspješno unesene!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Greska prilikom izmijene korisničkog naloga! " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connectiong to database: " + ex.Message);
            }
        }

        //poziv changeEmployeeAccount() funkcije da bi proslijedili u ConfirmWindow
        public void changeEmployeeAccountCall()
        {
            string name = employeeAccountNameBox.Text;
            string pass = employeeAccountPasswordBox.Password;
            changeEmployeeAccount(name, pass);
        }

        private void confirmEmployeeAccountChangeClick(object sender, RoutedEventArgs e)
        {
            ConfirmWindow confirmWindow = new ConfirmWindow(this, "changeEmployeeAccount", _adminJmb, _language);
            confirmWindow.ShowDialog();
            this.Close();
        }

        private void EmployeeAccountWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_canClose == false)
            {
                if (employeeAccountPasswordBox.Password != employeeAccountPasswordConfirmBox.Password)
                {
                    e.Cancel = true;
                    MessageBox.Show("Unijete sifre se ne poklapaju");
                }

                if (employeeAccountPasswordBox.Password == "")
                {
                    e.Cancel = true;
                    MessageBox.Show("Sifra nije unesena!");
                }

                if (employeeAccountPasswordBox.Password.Length < 5)
                {
                    e.Cancel = true;
                    MessageBox.Show("Sifra mora biti najmanje 5 karaktera");
                }

                if (_zaposleni == null)
                {
                    e.Cancel = true;
                    MessageBox.Show("Izaberite zaposlenog za koga mijenjate nalog!");
                }
            }
            _canClose = true;
        }
    }
}
