using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.database.grids
{
    class PregledIsporukaPoProduktu
    {

        private int _nabavkaId;
        private int _isporukeId;
        private int _kolicina;
        private decimal _cena;
        private string _dostavljac;
        private string _datum;
        private int _produktId;

        public PregledIsporukaPoProduktu(int nabavkaId, int isporukeId, int kolicina, decimal cena, string dostavljac, string datum, int produktId)
        {
            _nabavkaId = nabavkaId;
            _isporukeId = isporukeId;
            _kolicina = kolicina;
            _cena = cena;
            _dostavljac = dostavljac;
            _datum = datum;
            _produktId = produktId;
        }

        public int NabavkaId
        {
            get => _nabavkaId;
            set
            {
                if(_nabavkaId != value)
                {
                    _nabavkaId = value;
                    OnPropertyChanged(nameof(NabavkaId));
                }
            }
        }

        public int IsporukaId
        {
            get => _isporukeId;
            set
            {
                if (_isporukeId != value)
                {
                    _isporukeId = value;
                    OnPropertyChanged(nameof(IsporukaId));
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

        public int ProduktId
        {
            get => _produktId;
            set
            {
                if (_produktId != value)
                {
                    _produktId = value;
                    OnPropertyChanged(nameof(ProduktId));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
