using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using WpfApp1.database.employee;
using MySqlConnector;

namespace WpfApp1
{

    //Stavke za kreiranje i mijenjanje naloga za zaposlenog
    public partial class MainWindow
    {

        //Funkcija za dodavanje naloga zaposlenog
        public void addEmployeeAccount()
        {
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root";

            string username = employeeAccountNameBox.Text;
            string password = "";
            if (employeeAccountPasswordBox.Password == employeeAccountPasswordConfirmBox.Password)
            {
                password = employeeAccountPasswordBox.Password;
            }
            else
            {
                MessageBox.Show("Unijete sifre se ne poklapaju");
                return;
            }

            var selectedItem = adminEmployeeTable.SelectedItem as Zaposleni;
            if (selectedItem == null)
            {
                MessageBox.Show("Izaberite zaposlenog za koga kreirate nalog!");
                return;
            }
            string jmb = selectedItem.JMB;


            if (password == "")
            {
                MessageBox.Show("Sifra nije unesena!");
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
                        cmnd1.Parameters.AddWithValue("@jmb", jmb);
                        MySqlDataReader reader = cmnd1.ExecuteReader();
                        while (reader.Read())
                        {

                            if (jmb == reader.GetString(0))
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
                            cmd.Parameters.AddWithValue("@jmb", jmb);
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@password", hashedPassword);
                            cmd.Parameters.AddWithValue("@isMenager", 0);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
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
            ConfirmWindow confirmWindow = new ConfirmWindow(this, "addEmployeeAccount", _adminJmb);
            confirmWindow.ShowDialog();
        }


        //funkcija za ucitavanje postojeceg korisnickog naloga i sifre
        private void loadExistingEmployeeAccountInfo(object sender, SelectionChangedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root";
            try
            {
                var selectedZaposleni = adminEmployeeTable.SelectedItem as Zaposleni;
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

            if (employeeAccountPasswordBox.Password == employeeAccountPasswordConfirmBox.Password)
            {
                password = employeeAccountPasswordBox.Password;
            }
            else
            {
                MessageBox.Show("Unijete sifre se ne poklapaju");
                return;
            }

            var selectedItem = adminEmployeeTable.SelectedItem as Zaposleni;
            if (selectedItem == null)
            {
                MessageBox.Show("Izaberite zaposlenog za koga mijenjate nalog!");
                return;
            }
            string jmb = selectedItem.JMB;


            if (password == "")
            {
                MessageBox.Show("Sifra nije unesena!");
                return;
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();


                    string query = $"UPDATE projektni.zaposleni_nalog SET username = @username, sifra=@password WHERE zaposleni_JMB = @jmb";
                    try
                    {
                        using (MySqlCommand cmnd1 = new MySqlCommand(query, connection))
                        {
                            cmnd1.Parameters.AddWithValue("@jmb", jmb);
                            cmnd1.Parameters.AddWithValue("@username", username);
                            cmnd1.Parameters.AddWithValue("@password", hashedPassword);
                            cmnd1.ExecuteNonQuery();
                        }
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
            ConfirmWindow confirmWindow = new ConfirmWindow(this, "changeEmployeeAccount", _adminJmb);
            confirmWindow.ShowDialog();
        }


    }
}
