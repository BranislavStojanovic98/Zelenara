using MySql.Data.MySqlClient;
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



namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for ListaMjestaPopUp.xaml
    /// </summary>
    public partial class ListaMjestaPopUp : Window
    {
        private ObservableCollection<string> nazivMjestaCollection;

        public ListaMjestaPopUp()
        {
            InitializeComponent();
            nazivMjestaCollection = new ObservableCollection<string>();
            loadListaMjesta();

            this.Topmost = true;
        }

        public void loadListaMjesta()
        {
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root;";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    try
                    {
                        nazivMjestaCollection.Clear();
                        string query = "SELECT infoMjesta FROM mjesto_sa_postanskimbr";
                        MySqlCommand getMjesta = new MySqlCommand(query, connection);
                        MySqlDataReader reader = getMjesta.ExecuteReader();
                        while(reader.Read())
                        {
                            nazivMjestaCollection.Add(reader["infoMjesta"].ToString());
                        }
                        reader.Close();

                        ListaMjestaPopUpDataGrid.ItemsSource = null;
                        ListaMjestaPopUpDataGrid.ItemsSource = nazivMjestaCollection;
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Greska: " + ex.Message);
                    }
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("Error connecting to database: " + ex.Message);
            }
        }
    }
}
