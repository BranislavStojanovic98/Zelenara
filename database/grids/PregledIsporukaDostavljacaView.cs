using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.database.grids
{
    internal class PregledIsporukaDostavljacaView
    {
        private int _idProdukta;
        private string _nazivProdukta;
        private string _vrstaProdukta;
        private int _kolicinaProdukta;
        private string _proizvodjac;
        private string _datum;

        // Constructor
        public PregledIsporukaDostavljacaView(
            int idProdukta, string nazivProdukta, string vrstaProdukta,
            int kolicina, string proizvodjac, string datumDostave)
        {
            _idProdukta = idProdukta;
            _nazivProdukta = nazivProdukta;
            _vrstaProdukta = vrstaProdukta;
            _kolicinaProdukta = kolicina;
            _proizvodjac = proizvodjac;
            _datum = datumDostave;
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

        public int KolicinaProdukta
        {
            get => _kolicinaProdukta;
            set
            {
                if (_kolicinaProdukta != value)
                {
                    _kolicinaProdukta = value;
                    OnPropertyChanged(nameof(KolicinaProdukta));
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

