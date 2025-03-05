using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.database.grids
{
    class PregledSkladistaView
    {
        private int _skladisteId;
        private int _produktId;
        private string _naziv;
        private string _vrsta;
        private int _kolicina;
        private decimal _cena;

        public PregledSkladistaView(int skladisteId, int produktId, string naziv, string vrsta, int kolicina, decimal cena)
        {
            _skladisteId = skladisteId;
            _produktId = produktId;
            _naziv = naziv;
            _vrsta = vrsta;
            _kolicina = kolicina;
            _cena = cena;
        }

        public int SkladisteId
        {
            get => _skladisteId;
            set
            {
                if (_skladisteId != value)
                {
                    _skladisteId = value;
                    OnPropertyChanged(nameof(SkladisteId));
                }
            }
        }

        public int ProduktId
        {
            get => _produktId;
            set
            {
                if(_produktId != value)
                {
                    _produktId = value;
                    OnPropertyChanged(nameof(ProduktId));
                }
            }
        }

        public string Naziv
        {
            get => _naziv;
            set
            {
                if(_naziv != value)
                {
                    _naziv = value;
                    OnPropertyChanged(nameof(Naziv));
                }
            }
        }

        public string Vrsta
        {
            get => _vrsta;
            set
            {
                if(_vrsta != value)
                {
                    _vrsta = value;
                    OnPropertyChanged(nameof(Vrsta));
                }
            }
        }

        public int Kolicina
        {
            get => _kolicina;
            set
            {
                if(_kolicina != value)
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
                if(_cena != value)
                {
                    _cena = value;
                    OnPropertyChanged(nameof(Cena));
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
