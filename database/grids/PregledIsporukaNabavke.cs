using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.database.grids
{
    public class PregledIsporukaNabavke : INotifyPropertyChanged
    {
        private int _idProdukta;
        private string _name;
        private string _type;
        private int _amount;
        private string _maker;
        private decimal _cena;
        private DateOnly? _datum;
        private int _deliveryId;
        private int _shipmentId;

        // Constructor
        public PregledIsporukaNabavke(int id, string name, string type,int amount, string maker, decimal cena, DateOnly? datum, int deliveryId, int shipmentId)
        {
            _idProdukta = id;
            _name = name;
            _type = type;
            _amount = amount;
            _maker = maker;
            _cena = cena;
            _datum = datum;
            _deliveryId = deliveryId;
            _shipmentId = shipmentId;
        }

        // Properties with INotifyPropertyChanged support
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

        public string Vrsta
        {
            get => _type;
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged(nameof(Vrsta));
                }
            }
        }

        public int Kolicina
        {
            get => _amount;
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    OnPropertyChanged(nameof(Kolicina));
                }
            }
        }

        public string Proizvodjac
        {
            get => _maker;
            set
            {
                if (_maker != value)
                {
                    _maker = value;
                    OnPropertyChanged(nameof(Proizvodjac));
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

        public DateOnly? Datum
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

        public int Dostava
        {
            get => _deliveryId;
            set
            {
                if (_deliveryId != value)
                {
                    _deliveryId = value;
                    OnPropertyChanged(nameof(Dostava));
                }
            }
        }

        public int Isporuka
        {
            get => _shipmentId;
            set
            {
                if (_shipmentId != value)
                {
                    _shipmentId = value;
                    OnPropertyChanged(nameof(Isporuka));
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
