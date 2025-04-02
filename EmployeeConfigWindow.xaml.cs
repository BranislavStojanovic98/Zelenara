using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for EmployeeConfigWindow.xaml
    /// </summary>
    public partial class EmployeeConfigWindow : Window
    {
        private string _adminJmb;
        private string _action;
        private string _theme;
        private string _language = "Serbian";

        private ObservableCollection<Zaposleni> observableZaposleni;
        private ObservableCollection<string> mestoOptions;
        private Zaposleni _zaposleni;
        private ListaMjestaPopUp _listaMjestaPopUp;

        public EmployeeConfigWindow(string action, string adminJmb, string theme, string language)
        {
            observableZaposleni = new ObservableCollection<Zaposleni>();
            _action = action;
            _adminJmb = adminJmb;
            _language = language;
            InitializeComponent();
            _listaMjestaPopUp = new ListaMjestaPopUp(theme, language);
            _listaMjestaPopUp.Top = 150;
            _listaMjestaPopUp.Left = 1150;
            _listaMjestaPopUp.Show();

            if(theme == "Theme1")
            {
                adminEmployeeInfoChangeGrid.Background = new SolidColorBrush(Colors.Beige);
            }
            else if(theme == "Theme2")
            {
                adminEmployeeInfoChangeGrid.Background = new SolidColorBrush(Colors.BurlyWood);
            }

            if(language == "Serbian")
            {

                label1.Content = "Informacije od zaposlenom";
                employeeInfoNameLabel.Content = "Ime:";
                employeeInfoLastnameLabel.Content = "Prezime:";
                employeeInfoJMBLabel.Content = "JMB:";
                emplyeeInfoPlaceLabel.Content = "Poštanski Broj:";

                employeeInfoBoxAddButton.Content = "Dodaj";
                employeeInfoBoxUpdateButton.Content = "Izmijeni";
                employeeInfoBoxDeleteButton.Content = "Izbriši";
            }
            else if(language == "English")
            {

                label1.Content = "Employee Information";
                employeeInfoNameLabel.Content = "Name:";
                employeeInfoLastnameLabel.Content = "Surname:";
                employeeInfoJMBLabel.Content = "SSN:";
                emplyeeInfoPlaceLabel.Content = "Postal Number:";

                employeeInfoBoxAddButton.Content = "Add";
                employeeInfoBoxUpdateButton.Content = "Change";
                employeeInfoBoxDeleteButton.Content = "Clear";
            }
        }

        public EmployeeConfigWindow(Zaposleni zaposleni, string action, string adminJmb, string theme, string language)
        {
            observableZaposleni = new ObservableCollection<Zaposleni>();
            mestoOptions = new ObservableCollection<string>();
            _zaposleni = zaposleni;
            _adminJmb = adminJmb;
            _action = action;
            _language = language;
            InitializeComponent();

            if (theme == "Theme1")
            {
                adminEmployeeInfoChangeGrid.Background = new SolidColorBrush(Colors.Beige);
            }
            else if (theme == "Theme2")
            {
                adminEmployeeInfoChangeGrid.Background = new SolidColorBrush(Colors.BurlyWood);
            }

            if (language == "Serbian")
            {

                label1.Content = "Informacije od zaposlenom";
                employeeInfoNameLabel.Content = "Ime:";
                employeeInfoLastnameLabel.Content = "Prezime:";
                employeeInfoJMBLabel.Content = "JMB:";
                emplyeeInfoPlaceLabel.Content = "Poštanski Broj:";

                employeeInfoBoxAddButton.Content = "Dodaj";
                employeeInfoBoxUpdateButton.Content = "Izmijeni";
                employeeInfoBoxDeleteButton.Content = "Izbriši";
            }
            else if (language == "English")
            {

                label1.Content = "Employee Information";
                employeeInfoNameLabel.Content = "Name:";
                employeeInfoLastnameLabel.Content = "Surname:";
                employeeInfoJMBLabel.Content = "SSN:";
                emplyeeInfoPlaceLabel.Content = "Postal Number:";

                employeeInfoBoxAddButton.Content = "Add";
                employeeInfoBoxUpdateButton.Content = "Change";
                employeeInfoBoxDeleteButton.Content = "Clear";
            }


            if (zaposleni != null)
            {
                employeeInfoBoxName.Text = zaposleni.Ime;
                employeeInfoBoxLastname.Text = zaposleni.Prezime;
                employeeInfoBoxJMB.Text = zaposleni.JMB;
                employeeInfoBoxCity.Text = getPostanskiBrojFromImeMjesta(zaposleni);
            }

            if (_action == "edit")
            {
                employeeInfoBoxAddButton.Visibility = Visibility.Collapsed;
                employeeInfoBoxUpdateButton.Visibility = Visibility.Visible;

                _listaMjestaPopUp = new ListaMjestaPopUp(theme, language);
                _listaMjestaPopUp.Top = 150;
                _listaMjestaPopUp.Left = 1150;
                _listaMjestaPopUp.Show();
            }

            if (_action == "delete")
            {
                employeeInfoBoxUpdateButton.Visibility = Visibility.Collapsed;
                employeeInfoBoxAddButton.Visibility = Visibility.Collapsed;
                employeeInfoBoxDeleteButton.Visibility = Visibility.Collapsed;
            }
        }
    

        //Admin aplikacija, Dodavanje novog zaposlenog
        public void adminEmployeeInfoAdd(object sender, RoutedEventArgs e)
        {

            ConfirmWindow confirmWindow = new ConfirmWindow(this, "add", _adminJmb, _language);
            confirmWindow.ShowDialog();
        }


        //Admin aplikacija, Brisanje iz tabele zaposlenih
        public void adminEmployeeInfoDelete(object sender, RoutedEventArgs e)
        {
            employeeInfoBoxName.Clear();
            employeeInfoBoxLastname.Clear();
            employeeInfoBoxCity.Clear();
        }

        //Admin aplikacija, Izmjena inforamcija zaposlenih
        public void adminEmployeeInfoUpdate(object sender, RoutedEventArgs e)
        {
            ConfirmWindow confirmWindow = new ConfirmWindow(this, "edit", _adminJmb, _language);
            confirmWindow.ShowDialog();
        }

        //Dodavanje novog zaposlenog u tabelu
        public void DodajNovogZaposlenog()
        {
            // Get values from TextBoxes
            string ime = employeeInfoBoxName.Text;
            string prezime = employeeInfoBoxLastname.Text;
            string jmb = employeeInfoBoxJMB.Text;
            string postBr = employeeInfoBoxCity.Text;
            bool isAdmin = isAdminCheckBox.IsChecked == true;

            // Check if any field is empty
            if (string.IsNullOrWhiteSpace(ime) || string.IsNullOrWhiteSpace(prezime) || string.IsNullOrWhiteSpace(jmb) || string.IsNullOrWhiteSpace(postBr))
            {
                MessageBox.Show("Sva polja moraju biti popunjena!");
                return;
            }

            // Check if JMB is exactly 13 characters
            if (jmb.Length != 13)
            {
                MessageBox.Show("JMB mora biti tačno 13 brojeva");
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

                        using (MySqlCommand cmd = new MySqlCommand(sql, con))
                        {
                        cmd.Parameters.AddWithValue("@jmb", jmb);
                        cmd.Parameters.AddWithValue("@ime", ime);
                        cmd.Parameters.AddWithValue("@prezime", prezime);
                        cmd.Parameters.AddWithValue("@postBr", postBr);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            MessageBox.Show("Greška prilikom dodavanja zaposlenog");
                            return;
                        }
                        }
                        
                        if (isAdmin)
                        {
                            string sqlInsertMenadzer = "INSERT INTO menadzer (ZAPOSLENI_JMB) VALUES (@jmb)";
                            using (MySqlCommand cmdInsertMenadzer = new MySqlCommand(sqlInsertMenadzer, con))
                            {
                                cmdInsertMenadzer.Parameters.AddWithValue("@jmb", jmb);
                                int rowsAffectedMenadzer = cmdInsertMenadzer.ExecuteNonQuery();
                                if (rowsAffectedMenadzer == 0)
                                {
                                    MessageBox.Show("Greška prilikom dodavanja menadžera");
                                    return;
                                }
                            }
                        }
                    

                    MessageBox.Show("Novi zaposleni uspješno dodat");
                    observableZaposleni.Add(new Zaposleni(jmb, ime, prezime, postBr));
                    employeeInfoBoxName.Clear();
                    employeeInfoBoxLastname.Clear();
                    employeeInfoBoxJMB.Clear();
                    employeeInfoBoxCity.Clear();

                }

            }
            catch (Exception ex)
            {
                // Handle any errors that occurred during the database operation
                MessageBox.Show("Greška: " + ex.Message);
            }

            if (_listaMjestaPopUp != null)
            {
                _listaMjestaPopUp.Close();
            }

            this.Close();
        }

        //Brisanje postojeceg zaposlenog
        public void IzbrisiZaposlenog()
        {
            // Check if the JMB input is valid (must be 13 characters long)
            if (employeeInfoBoxJMB.Text.Length != 13)
            {
                MessageBox.Show("JMB mora biti tačno 13 brojeva");
                return;
            }

            string jmb = employeeInfoBoxJMB.Text;

            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root;";

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
                        MessageBox.Show("Greska prilikom brisanja u menadzer tabeli: " + e.Message);
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

            this.Close();
        }

        public void IzmjeniZaposlenog()
        {

            // Get the new values from the TextBoxes
            string newIme = employeeInfoBoxName.Text; // TextBox for Ime
            string newPrezime = employeeInfoBoxLastname.Text; // TextBox for Prezime
            string newMjesto = employeeInfoBoxCity.Text; // TextBox for Mjesto

            // Check if the values are different before updating
            if (!string.IsNullOrEmpty(newIme) && newIme != _zaposleni.Ime)
            {
                _zaposleni.Ime = newIme; // Update the property
                UpdateDatabaseField("Ime", newIme, _zaposleni.JMB); // Update the database
            }

            if (!string.IsNullOrEmpty(newPrezime) && newPrezime != _zaposleni.Prezime)
            {
                _zaposleni.Prezime = newPrezime; // Update the property
                UpdateDatabaseField("Prezime", newPrezime, _zaposleni.JMB); // Update the database
            }

            if (!string.IsNullOrEmpty(newMjesto) && newMjesto != _zaposleni.Mjesto)
            {
                _zaposleni.Mjesto = newMjesto; // Update the property
                UpdateDatabaseField("MJESTO_PostanskiBroj", newMjesto, _zaposleni.JMB); // Update the database
            }

            if (_listaMjestaPopUp != null)
            {
                _listaMjestaPopUp.Close();
            }

            this.Close();
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



        //Funkcija za trazenje Postanskog broja na osnovu imena grada
        private string? getPostanskiBrojFromImeMjesta(Zaposleni zaposleni)
        {
            string connectionString = "Server=localhost,3306;Database=projektni;Uid=root;Pwd=root;";
            try
            {

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


                    foreach (var item in mestoOptions)
                    {
                        string listMjesta = item.ToString();
                        string[] tmp = listMjesta.Split(new string[] { "-" }, StringSplitOptions.None);
                        if (tmp[1] == zaposleni?.Mjesto)
                        {
                            return tmp[0];
                        }
                    }
                    reader1.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connectiong to database: " + ex.Message);
            }
            return null;
        }

        private void employeeConfigClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_listaMjestaPopUp != null)
            {
                _listaMjestaPopUp.Close();
            }
        }
    }
}
