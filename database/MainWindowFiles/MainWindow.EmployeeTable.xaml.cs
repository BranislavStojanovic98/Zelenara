using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp1.database.employee;

namespace WpfApp1
{

    //Stavke za ucitavanje, dodavanje, izmijenu, i brisanje zaposlenog
    public partial class MainWindow
    {
        //Ucitavanje informacija tabele zaposlenih
        private void LoadDataTabelaZaposleni()
        {
            string connectionString = "Server=localhost,3306;Database=projektni;Uid=root;Pwd=root;";

            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    observableZaposleni.Clear();

                    // Load Mesto Options
                    MySqlCommand cmd1 = new MySqlCommand("SELECT DISTINCT infoMjesta FROM projektni.mjesto_sa_postanskimbr", con);
                    MySqlDataReader reader1 = cmd1.ExecuteReader();
                    while (reader1.Read())
                    {
                        mestoOptions.Add(reader1["infoMjesta"].ToString());
                    }
                    reader1.Close();

                    // Load Zaposleni Data
                    MySqlCommand cmd2 = new MySqlCommand("SELECT * FROM projektni.pregled_zaposlenih WHERE JMB != @ExcludedJMB", con);
                    cmd2.Parameters.AddWithValue("@ExcludedJMB", _adminJmb);
                    MySqlDataReader reader2 = cmd2.ExecuteReader();
                    while (reader2.Read())
                    {
                        observableZaposleni.Add(new Zaposleni(
                            reader2.GetString(0), // JMB
                            reader2.GetString(1), // Ime
                            reader2.GetString(2), // Prezime
                            reader2.GetString(3)  // Mjesto
                        ));
                    }
                    reader2.Close();
                }

                adminEmployeeTable.ItemsSource = null;
                adminEmployeeTable.ItemsSource = observableZaposleni;

            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }
        }

        //Dodavanje novog zaposlenog
        public void addNewEmployee()
        {
            try
            {
                EmployeeConfigWindow employeeConfigWindow = new EmployeeConfigWindow("add", _adminJmb, _theme, _language);
                employeeConfigWindow.ShowDialog();

                adminEmployeeTable.SelectedItem = null;
                adminEmployeeTable.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during editing: " + ex.Message);
            }
            LoadDataTabelaZaposleni();
        }

        //Brisanje izabranog zaposlenog
        public void deleteEmployee()
        {
            try
            {
                var selectedZaposleni = adminEmployeeTable.SelectedItem as Zaposleni;
                if (selectedZaposleni != null)
                {
                    EmployeeConfigWindow employeeConfigWindow = new EmployeeConfigWindow(selectedZaposleni, "delete", _adminJmb, _theme, _language);
                    employeeConfigWindow.Show();

                    ConfirmWindow confirmWindow = new ConfirmWindow(employeeConfigWindow, "delete", _adminJmb, _language);
                    confirmWindow.Topmost = true;
                    confirmWindow.ShowDialog();

                    if (employeeConfigWindow != null)
                    {
                        employeeConfigWindow.Close();
                    }

                    // Optionally, deselect the row
                    adminEmployeeTable.SelectedItem = null;
                    adminEmployeeTable.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during deletion: " + ex.Message);
            }
            LoadDataTabelaZaposleni();
        }

        //Izmijena izabranog zaposlenog
        public void editTabelaZaposlenih()
        {
            try
            {
                // Assuming adminEmployeeTable contains the selected Zaposleni item
                var selectedZaposleni = adminEmployeeTable.SelectedItem as Zaposleni;

                // Ensure there is a selected employee to edit
                if (selectedZaposleni != null)
                {

                    EmployeeConfigWindow employeeConfigWindow = new EmployeeConfigWindow(selectedZaposleni, "edit", _adminJmb, _theme, _language);
                    employeeConfigWindow.ShowDialog();

                    // Optionally, deselect the row
                    adminEmployeeTable.SelectedItem = null;
                    adminEmployeeTable.SelectedIndex = -1;

                }
                else
                {
                    MessageBox.Show("No item selected.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during editing: " + ex.Message);
            }
            LoadDataTabelaZaposleni();
        }

        //Prikaz Right-Click menija za adminEmployeeTable
        private void openEmployeeTableContextMenu(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var row = VisualTreeHelper.HitTest(adminEmployeeTable, e.GetPosition(adminEmployeeTable))?.VisualHit;
            while (row != null && !(row is DataGridRow))
            {
                row = VisualTreeHelper.GetParent(row);
            }

            if (row is DataGridRow dataGridRow && dataGridRow.IsSelected)
            {
                employeeTableMenu.PlacementTarget = dataGridRow;
                employeeTableMenu.IsOpen = true;
            }
        }

        //Right-Click menija za adminEmployeeTable, izmjena postojeceg zaposlenog
        private void employeeTableChangeOption(object sender, RoutedEventArgs e)
        {
            editTabelaZaposlenih();
        }

        //Right-Click menija za adminEmployeeTable, dodavanje novog zaposlenog
        private void employeeTableAddOption(object sender, RoutedEventArgs e)
        {
            addNewEmployee();
        }

        //Right-Click menija za adminEmployeeTable, brisanje postojeceg zaposlenog
        private void employeeTableDeleteOption(object sender, RoutedEventArgs e)
        {
            deleteEmployee();
        }


        //Pomocne funkcije//

        private void editTabelaZaposlenih(object sender, RoutedEventArgs e)
        {
            editTabelaZaposlenih();
        }
        private void deleteEmployee(object sender, RoutedEventArgs e)
        {
            deleteEmployee();
        }
        private void addEmployee(object sender, RoutedEventArgs e)
        {
            addNewEmployee();
        }
    
    }
}
