using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using System.Windows;
using WpfApp1.database.employee;
using System.Windows.Controls;
using WpfApp1.database.grids;
using WpfApp1.database.items;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using K4os.Compression.LZ4.Streams.Adapters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using BCrypt.Net;
using static MaterialDesignThemes.Wpf.Theme;
using System.Windows.Media;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ObservableCollection<Zaposleni> observableZaposleni;
        private ObservableCollection<string> mestoOptions;
        private ObservableCollection<PregledDostavljacaView> pregledDostavljacaViews;
        private ObservableCollection<PregledIsporukaView> observablePregledIsporuka;
        private ObservableCollection<PregledNabavkiView> observablePregledNabavki;
        private ObservableCollection<PregledIsporukaNabavke> observablePregledIsporukaNabavki;
        private ObservableCollection<PregledSkladistaView> observablePregledSkladista;
        private ObservableCollection<PregledIsporukaPoProduktu> observablePregledSpecificnihProdukta;

        private string _adminJmb;
        private string _theme;
        private string _language = "Serbian";

        public MainWindow(string adminJmb)
        {
            _adminJmb = adminJmb;
            getTheme();
            InitializeComponent();
            
            if(_theme == "Theme1")
            {
                changeTheme1(null, null);
            }
            else if(_theme == "Theme2")
            {
                changeTheme2(null, null);
            }

            observableZaposleni = new ObservableCollection<Zaposleni>();
            mestoOptions = new ObservableCollection<string>();
            pregledDostavljacaViews = new ObservableCollection<PregledDostavljacaView>();
            observablePregledIsporuka = new ObservableCollection<PregledIsporukaView>();
            observablePregledNabavki = new ObservableCollection<PregledNabavkiView>();
            observablePregledIsporukaNabavki = new ObservableCollection<PregledIsporukaNabavke>();
            observablePregledSkladista = new ObservableCollection<PregledSkladistaView>();
            observablePregledSpecificnihProdukta = new ObservableCollection<PregledIsporukaPoProduktu>();
            this.DataContext = this;

        }

        //Provjeri koju temu je korisnik sacuvao prilikom prosle sesije
        private void getTheme()
        {
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT theme FROM  zaposleni_nalog WHERE zaposleni_JMB = @zaposleni_JMB";

                    try
                    {
                        using (MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@zaposleni_JMB", _adminJmb);
                            object result = cmd.ExecuteScalar();

                            if (result != DBNull.Value && result != null)
                            {
                                _theme = result.ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        //Pomocna funkicja za ciscenje nepotvrdjenih promijena
        private void clearAllBoxData()
        {
            adminEmployeeTable.SelectedItem = null;
            adminEmployeeTable.SelectedIndex = -1;
            employeeAccountNameBox.Clear();
            employeeAccountPasswordBox.Clear();
            employeeAccountPasswordConfirmBox.Clear();
        }

        //Prelazi na Pocetnu stranu
        private void mainViewShow(object sender, RoutedEventArgs e)
        {
            adminEmployeeViewGrid.Visibility = Visibility.Hidden;
            adminDeliveriesViewGrid.Visibility = Visibility.Hidden;
            transportersViewGrid.Visibility = Visibility.Hidden;
            storageViewGrid.Visibility = Visibility.Hidden;
            clearAllBoxData();
        }

        //Prelazi na grid Zaposleni u kojem radimo sa zaposlenima i njihovim nalozima
        private void employeeViewOpen(object sender, RoutedEventArgs e)
        {
            LoadDataTabelaZaposleni();
            adminEmployeeViewGrid.Visibility = Visibility.Visible;
            adminDeliveriesViewGrid.Visibility = Visibility.Hidden;
            transportersViewGrid.Visibility = Visibility.Hidden;
            storageViewGrid.Visibility = Visibility.Hidden;
        }

        //Pomocni prozor koji prikazuje Postanske brojeve odredjenih Mjesta 
        private void openListaMjestaClick(object sender, RoutedEventArgs e)
        {
            ListaMjestaPopUp listaMijestaWindow = new ListaMjestaPopUp(_theme, _language);
            listaMijestaWindow.Show();
        }


        //Log out iz aplikacije
        private void logoutActionClick(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            loginWindow.Focus();
            this.Close();
            MessageBox.Show("Uspjesno ste se izlogovali");
        }


        //Promijena teme
        private void changeThemeDefault(object sender, RoutedEventArgs e)
        {
            welcomeBannerGrid.Background = SystemColors.ControlLightBrush;
            bannerGrid.Background = SystemColors.ControlLightBrush;
            mainGrid.Background = null;

            //Zaposleni

            adminEmployeeViewGrid.Background = SystemColors.ControlLightLightBrush;
            adminEmployeeLoginAccountGrid.Background = SystemColors.ControlLightBrush;
            adminEmployeeTable.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF0F0F0"));
            empolyeeBottomGrid.Background = SystemColors.ControlLightBrush;

            //Isporuke

            adminDeliveriesViewGrid.Background = SystemColors.ControlLightLightBrush;
            adminDeliveriesListDeliveriesGrid.Background = SystemColors.ControlLightBrush;
            adminDeliveriesDataGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF0F0F0"));
            listDeliveriesDataGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF0F0F0"));

            //Dostavljaci

            transportersViewGrid.Background = SystemColors.ControlLightBrush;
            transporterListDataGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF0F0F0"));
            transporterDeliveriesListDataGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF0F0F0"));

            //Skladiste

            storageViewGrid.Background = SystemColors.ControlLightBrush;
            storageAvailableProductsDataGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF0F0F0"));
            storageSpecificProductDataGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF0F0F0"));

            _theme = "Default";
        }

        //Promijena teme
        private void changeTheme1(object sender, RoutedEventArgs e)
        {
            welcomeBannerGrid.Background = new SolidColorBrush(Colors.Bisque);
            bannerGrid.Background = new SolidColorBrush(Colors.Bisque);
            mainGrid.Background = new SolidColorBrush(Colors.Beige);

            //Zaposleni

            adminEmployeeViewGrid.Background = new SolidColorBrush(Colors.Beige);
            adminEmployeeLoginAccountGrid.Background = new SolidColorBrush(Colors.Bisque);
            adminEmployeeTable.Background = new SolidColorBrush(Colors.Bisque);
            empolyeeBottomGrid.Background = new SolidColorBrush (Colors.Bisque);

            //Isporuke

            adminDeliveriesViewGrid.Background = new SolidColorBrush(Colors.Beige);
            adminDeliveriesListDeliveriesGrid.Background = new SolidColorBrush(Colors.Bisque);
            adminDeliveriesDataGrid.Background = new SolidColorBrush(Colors.Bisque);
            listDeliveriesDataGrid.Background = new SolidColorBrush(Colors.Bisque);

            //Dostavljaci

            transportersViewGrid.Background = new SolidColorBrush(Colors.Beige);
            transporterListDataGrid.Background = new SolidColorBrush(Colors.Bisque);
            transporterDeliveriesListDataGrid.Background = new SolidColorBrush(Colors.Bisque);

            //Skladiste

            storageViewGrid.Background = new SolidColorBrush(Colors.Beige);
            storageAvailableProductsDataGrid.Background = new SolidColorBrush(Colors.Bisque);
            storageSpecificProductDataGrid.Background = new SolidColorBrush(Colors.Bisque);

            _theme = "Theme1";
        }

        //Promijena teme
        private void changeTheme2(object sender, RoutedEventArgs e)
        {
            welcomeBannerGrid.Background = new SolidColorBrush(Colors.BurlyWood);
            bannerGrid.Background = new SolidColorBrush(Colors.BurlyWood);
            mainGrid.Background = new SolidColorBrush(Colors.Brown);

            //Zaposleni

            adminEmployeeViewGrid.Background = new SolidColorBrush(Colors.Brown);
            adminEmployeeLoginAccountGrid.Background = new SolidColorBrush(Colors.BurlyWood);
            adminEmployeeTable.Background = new SolidColorBrush(Colors.BurlyWood);
            empolyeeBottomGrid.Background = new SolidColorBrush(Colors.BurlyWood);

            //Isporuke

            adminDeliveriesViewGrid.Background = new SolidColorBrush(Colors.Brown);
            adminDeliveriesListDeliveriesGrid.Background = new SolidColorBrush(Colors.BurlyWood);
            adminDeliveriesDataGrid.Background = new SolidColorBrush(Colors.BurlyWood);
            listDeliveriesDataGrid.Background = new SolidColorBrush(Colors.BurlyWood);

            //Dostavljaci

            transportersViewGrid.Background = new SolidColorBrush(Colors.Brown);
            transporterListDataGrid.Background = new SolidColorBrush(Colors.BurlyWood);
            transporterDeliveriesListDataGrid.Background = new SolidColorBrush(Colors.BurlyWood);

            //Skladiste

            storageViewGrid.Background = new SolidColorBrush(Colors.Brown);
            storageAvailableProductsDataGrid.Background = new SolidColorBrush(Colors.BurlyWood);
            storageSpecificProductDataGrid.Background = new SolidColorBrush(Colors.BurlyWood);

            _theme = "Theme2";
        }

        //Promijena jezika
        private void changeLanguageSerbian(object sender, RoutedEventArgs e)
        {
            //Pocetna

            mainMenuItem1.Header = "Početna";
            mainMenuItem2.Header = "Zaposleni";
            mainMenuItem3.Header = "Isporuke";
            mainMenuItem4.Header = "Dostavljači";
            mainMenuItem5.Header = "SKladište";

            secondaryMenuItem1.Header = "Opcije";
            secondaryMenuSubitem1.Header = "Teme";
            secondaryMenuSubitem2.Header = "Jezik";

            welcomeLabel.Content = "Dobrodošli";
            motdLabel.Content = "Uz velikog čovjeka obično idu i velike greške";



            //Zaposleni

            employeeTableImeColumn.Header = "Ime";
            employeeTablePrezimeColumn.Header = "Prezime";
            employeeTableJMBColumn.Header = "JMB";
            employeeTableMjestoColumn.Header = "Mjesto";
            employeeTableOpcijeColumn.Header = "Opcije";
            employeeTableMenuAddOption.Header = "Dodaj";
            employeeTableMenuChangeOption.Header = "Izmijeni";
            employeeTableMenuDeleteOption.Header = "Izbriši";

            adminEmployeeBackButton.Content = "Nazad";
            employeeBottomLabel.Content = "Poštanski Brojevi:";
            employeeLabel1.Content = "Korisnički nalog";
            employeeLabel2.Content = "zaposlenog";
            employeeLabel3.Content = "Korisničko ime:";
            employeeLabel4.Content = "Lozinka:";
            employeeLabel5.Content = "Potvrda lozinke:";

            employeeAccountConfirmButton.Content = "Potvrdi";
            addEmployeeButton.Content = "Dodaj";
            adminEmployeeBackButton.Content = "Nazad";


            //Isporuke

            adminDeliveriesGridShipmentID.Header = "ID Nabavke";
            adminDeliveriesGridTransporterName.Header = "Dostavljač";
            adminDeliveriesGridDeliveryDate.Header = "Datum dostave";
            adminDeliveriesGridCost.Header = "Cijena";

            listDeliveriesDataProductID.Header = "ID Produkta";
            listDeliveriesDataProductName.Header = "Naziv";
            listDeliveriesDataProductType.Header = "Vrsta";
            listDeliveriesDataAmount.Header = "Količina";
            listDeliveriesDataMakerName.Header = "Proizvođač";
            listDeliveriesDataPrice.Header = "Cijena";
            listDeliveriesDataDate.Header = "Datum narudžbe";

            adminDeliveriesOrderAdd.Content = "Dodaj";
            adminDeliveriesOrderCancel.Content = "Izbriši";
            adminDeliveriesShipmentAdd.Content = "Dodaj";
            adminDeliveriesShipmentCancel.Content = "Izbriši";
            deliveriesBottomLabel1.Content = "Nabavka";
            deliveriesBottomLabel2.Content = "Isporuke";


            //Dostavljaci

            transportersLabel1.Content = "Lista dostupnih dostavljača";
            transporterLabel2.Content = "Lista svih isporuka izabranog dostavljača";
            adminDeliveriesViewBackButton_Copy.Content = "Nazad";

            transporterDataGridIDColumn.Header = "ID Dostavljaca";
            transporterDataGridNameColumn.Header = "Naziv";
            transporterDataGridAdressColumn.Header = "Adresa";
            transporterDataGridCityColumn.Header = "Mjesto";

            transporterDeliveriesListProductIDColumn.Header = "ID Produkta";
            transporterDeliveriesListProductName.Header = "Naziv";
            transporterDeliveriesListProductType.Header = "Vrsta";
            transporterDeliveriesListProductAmount.Header = "Količina";
            transporterDeliveriesListProductMaker.Header = "Proizvodjač";
            transporterDeliveriesListProductDate.Header = "Datum Nabavke";



            //Skladiste

            storageLabel1.Content = "Dostupni produkti";
            storageLabel2.Content = "Lista isporuka selektovanog produkta";
            storageSearchLable.Content = "Pretraga";
            storageViewSearchButton.Content = "Pretraži";

            storageDataGrid1ProductId.Header = "ID Produkta";
            storageDataGrid1Name.Header = "Naziv";
            storageDataGrid1Type.Header = "Vrsta";
            storageDataGrid1Amount.Header = "Kolicina";
            storageDataGrid1Price.Header = "Cena";

            storageDataGrid2DeliveryId.Header = "ID Isporuke";
            storageDataGrid2Amount.Header = "Kolicina";
            storageDataGrid2Producer.Header = "Proizvođač";
            storageDataGrid2Price.Header = "Cena";
            storageDataGrid2Transporter.Header = "Dostavljac";
            storageDataGrid2Date.Header = "Datum Isporuke";

            adminDeliveriesViewBackButton_Copy1.Content = "Nazad";

            _language = "Serbian";
        }

        //Promijena jezika
        private void changeLanguageEnglish(object sender, RoutedEventArgs e)
        {
            //Pocetna

            mainMenuItem1.Header = "Main page";
            mainMenuItem2.Header = "Employees";
            mainMenuItem3.Header = "Shipments";
            mainMenuItem4.Header = "Transporters";
            mainMenuItem5.Header = "Storage";

            secondaryMenuItem1.Header = "Options";
            secondaryMenuSubitem1.Header = "Theme";
            secondaryMenuSubitem2.Header = "Language";

            welcomeLabel.Content = "Welcome";
            motdLabel.Content = "With great man come great mistakes";



            //Zaposleni

            employeeTableImeColumn.Header = "Name";
            employeeTablePrezimeColumn.Header = "Surname";
            employeeTableJMBColumn.Header = "SSN";
            employeeTableMjestoColumn.Header = "City";
            employeeTableOpcijeColumn.Header = "Options";
            employeeTableMenuAddOption.Header = "Add";
            employeeTableMenuChangeOption.Header = "Edit";
            employeeTableMenuDeleteOption.Header = "Delete";

            adminEmployeeBackButton.Content = "Back";
            employeeBottomLabel.Content = "Postal Number:";
            employeeLabel1.Content = "User employee";
            employeeLabel2.Content = "account";
            employeeLabel3.Content = "Username:";
            employeeLabel4.Content = "Password:";
            employeeLabel5.Content = "Confirm Password:";

            employeeAccountConfirmButton.Content = "Confirm";
            addEmployeeButton.Content = "Add";
            adminEmployeeBackButton.Content = "Back";


            //Isporuke

            adminDeliveriesGridShipmentID.Header = "Shipment ID";
            adminDeliveriesGridTransporterName.Header = "Transporter Name";
            adminDeliveriesGridDeliveryDate.Header = "Delivery Date";
            adminDeliveriesGridCost.Header = "Cost";

            listDeliveriesDataProductID.Header = "Product ID";
            listDeliveriesDataProductName.Header = "Name";
            listDeliveriesDataProductType.Header = "Type";
            listDeliveriesDataAmount.Header = "Amount";
            listDeliveriesDataMakerName.Header = "Producer";
            listDeliveriesDataPrice.Header = "Price";
            listDeliveriesDataDate.Header = "Date";

            adminDeliveriesOrderAdd.Content = "Add";
            adminDeliveriesOrderCancel.Content = "Cancel";
            adminDeliveriesShipmentAdd.Content = "Add";
            adminDeliveriesShipmentCancel.Content = "Cancel";
            deliveriesBottomLabel1.Content = "Order";
            deliveriesBottomLabel2.Content = "Shipment";


            //Dostavljaci

            transportersLabel1.Content = "List of Available Transporters";
            transporterLabel2.Content = "List of All Deliveries of Selected Transporter";
            adminDeliveriesViewBackButton_Copy.Content = "Back";

            transporterDataGridIDColumn.Header = "Transporter ID";
            transporterDataGridNameColumn.Header = "Name";
            transporterDataGridAdressColumn.Header = "Address";
            transporterDataGridCityColumn.Header = "City";

            transporterDeliveriesListProductIDColumn.Header = "Product ID";
            transporterDeliveriesListProductName.Header = "Product Name";
            transporterDeliveriesListProductType.Header = "Type";
            transporterDeliveriesListProductAmount.Header = "Amount";
            transporterDeliveriesListProductMaker.Header = "Producer";
            transporterDeliveriesListProductDate.Header = "Date";



            //Skladiste

            storageLabel1.Content = "Available Products";
            storageLabel2.Content = "List of Deliveries of Selected Product";
            storageSearchLable.Content = "Search";
            storageViewSearchButton.Content = "Search";
            storageDataGrid1ProductId.Header = "Product ID";
            storageDataGrid1Name.Header = "Name";
            storageDataGrid1Type.Header = "Type";
            storageDataGrid1Amount.Header = "Amount";
            storageDataGrid1Price.Header = "Price";

            storageDataGrid2DeliveryId.Header = "Delivery ID";
            storageDataGrid2Amount.Header = "Amount";
            storageDataGrid2Producer.Header = "Producer";
            storageDataGrid2Price.Header = "Price";
            storageDataGrid2Transporter.Header = "Transporter";
            storageDataGrid2Date.Header = "Date";

            adminDeliveriesViewBackButton_Copy1.Content = "Back";

            _language = "English";
        }

        //Sacuva trenutnu temu kao preferiranu u zaposleni_nalog tabelu database-a
        private void saveTheme(object sender, EventArgs e)
        {
            string connectionString = "Server=localhost;Database=projektni;Uid=root;Pwd=root";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE zaposleni_nalog SET theme=@theme WHERE zaposleni_JMB = @zaposleni_JMB";

                    try
                    {
                        using (MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@theme", _theme);
                            cmd.Parameters.AddWithValue("@zaposleni_JMB", _adminJmb);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}