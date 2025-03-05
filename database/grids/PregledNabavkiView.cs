using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.database.grids
{
    public class PregledNabavkiView : INotifyPropertyChanged
    {
        private int _idNabavke;
        private string _name;   
        private decimal _cena;
        private string _datum;

        // Constructor
        public PregledNabavkiView(int id, string name, decimal cena, string datum)
        {
            _idNabavke = id;
            _name = name;
            _cena = cena;
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

        public string Naziv
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Naziv));
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
