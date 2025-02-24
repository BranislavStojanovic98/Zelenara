using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.database.grids
{
    public class ZahtjevNabavke : INotifyPropertyChanged
    {
        private int _idNabavke;
        private int _idProdukta;
        private int _idSkladista;
        private string _nazivProdukta;
        private string _vrstaProdukta;
        private int _kolicina;
        private decimal _cena;
        private string _proizvodjac;
        private string _dostavljac;
        private string _datum;

        // Constructor
        public ZahtjevNabavke(int idNabavke, int idProdukta, int idSkladista,
            string nazivProdukta, string vrstaProdukta, int kolicinaProdukta, decimal cena,
            string proizvodjac, string dostavljac, string datum)
        {
            _idNabavke = idNabavke;
            _idProdukta = idProdukta;
            _idSkladista = idSkladista;
            _nazivProdukta = nazivProdukta;
            _vrstaProdukta = vrstaProdukta;
            _kolicina = kolicinaProdukta;
            _cena = cena;
            _proizvodjac = proizvodjac;
            _dostavljac = dostavljac;
            _datum = datum;
        }

        // Properties with INotifyPropertyChanged support
        public int IdNabavke
        {
            get => _idNabavke;
            set
            {
                if (_idNabavke != value)
                {
                    _idNabavke = value;
                    OnPropertyChanged(nameof(IdNabavke));
                }
            }
        }

        public int IdProdukta
        {
            get => _idProdukta;
            set
            {
                if (_idProdukta != value)
                {
                    _idProdukta = value;
                    OnPropertyChanged(nameof(IdProdukta));
                }
            }
        }

        public int IdSkladista
        {
            get => _idSkladista;
            set
            {
                if (_idSkladista != value)
                {
                    _idSkladista = value;
                    OnPropertyChanged(nameof(IdSkladista));
                }
            }
        }

        public string NazivProdukta
        {
            get => _nazivProdukta;
            set
            {
                if (_nazivProdukta != value)
                {
                    _nazivProdukta = value;
                    OnPropertyChanged(nameof(NazivProdukta));
                }
            }
        }

        public string VrstaProdukta
        {
            get => _vrstaProdukta;
            set
            {
                if (_vrstaProdukta != value)
                {
                    _vrstaProdukta = value;
                    OnPropertyChanged(nameof(VrstaProdukta));
                }
            }
        }

        public int Kolicina
        {
            get => _kolicina;
            set
            {
                if (_kolicina != value)
                {
                    _kolicina = value;
                    OnPropertyChanged(nameof(Kolicina));
                }
            }
        }

        public decimal Cena
        {
            get => _cena;
            set
            {
                if (_cena != value)
                {
                    _cena = value;
                    OnPropertyChanged(nameof(Cena));
                }
            }
        }

        public string Proizvodjac
        {
            get => _proizvodjac;
            set
            {
                if (_proizvodjac != value)
                {
                    _proizvodjac = value;
                    OnPropertyChanged(nameof(Proizvodjac));
                }
            }
        }

        public string Dostavljac
        {
            get => _dostavljac;
            set
            {
                if (_dostavljac != value)
                {
                    _dostavljac = value;
                    OnPropertyChanged(nameof(Dostavljac));
                }
            }
        }

        public string Datum
        {
            get => _datum;
            set
            {
                if (_datum != value)
                {
                    _datum = value;
                    OnPropertyChanged(nameof(Datum));
                }
            }
        }

        // Implementing INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
