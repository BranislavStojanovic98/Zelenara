using MySql.Data.MySqlClient;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void CreateManagerWindow(string zaposleniJMB)
        {
            // Here you can create a specific window for the manager
            MainWindow managerWindow = new MainWindow(zaposleniJMB);  // Pass the JMB to the window's constructor
            managerWindow.Show();
            this.Close();
        }

        private void CreateEmployeeWindow(string zaposleniJMB)
        {
            // Here you can create a specific window for the manager
            EmployeeWindow employeeWindow = new EmployeeWindow(zaposleniJMB);  // Pass the JMB to the window's constructor
            employeeWindow.Show();
            this.Close();
        }

        private void checkAccount(string username, string password)
        {
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT sifra, is_menadzer, zaposleni_JMB " +
                           "FROM projektni.zaposleni_nalog " +
                           "WHERE username = @username";

                    try
                    {
                        using (MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@username", username);

                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {

                                    string storedHashedPassword = reader.GetString(0);
                                    int isManager = reader.GetInt32(1);
                                    string zaposleniJMB = reader.GetString(2);

                                    if (BCrypt.Net.BCrypt.Verify(password, storedHashedPassword))
                                    {
                                        if (isManager == 1)
                                        {
                                            MessageBox.Show("Dobrodosli!");
                                            CreateManagerWindow(zaposleniJMB);

                                        }
                                        else if(isManager == 0)
                                        {
                                            MessageBox.Show("Dobrodosli!");
                                            CreateEmployeeWindow(zaposleniJMB);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Pogresno korisničko ime/šifra!");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Pogresno korisničko ime/šifra!");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Greska: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to connect to database: " + ex.Message);
            }
        }



        private void logInClick(object sender, RoutedEventArgs e)
        {
            string name = loginUsernameBox.Text;
            string pwd = loginPasswordBox.Password;
            checkAccount(name, pwd);

        }
    }
}
