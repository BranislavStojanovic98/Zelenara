using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.database.employee
{
    [Serializable]
    public class Zaposleni : INotifyPropertyChanged
    {
        private string _jmb;
        private string _ime;
        private string _prezime;
        private string _mjesto;
        private int? _idMjesta;

        // Property Changed Event
        public event PropertyChangedEventHandler? PropertyChanged;

        // Constructor
        public Zaposleni(string jmb, string ime, string prezime, string mjesto)
        {
            if (jmb.Length != 12)
            {
                throw new ArgumentException("JMB treba biti u formatu od tacno 12 cifara");
            }

            _jmb = jmb;
            _ime = ime;
            _prezime = prezime;
            _mjesto = mjesto;
        }

        // Notify Property Changed
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // JMB Property
        public string JMB
        {
            get => _jmb;
            set
            {
                if (_jmb != value)
                {
                    if (value.Length != 12)
                    {
                        throw new ArgumentException("JMB treba biti u formatu od tacno 12 cifara");
                    }
                    _jmb = value;
                    OnPropertyChanged(nameof(JMB));
                }
            }
        }

        // Ime Property
        public string Ime
        {
            get => _ime;
            set
            {
                if (_ime != value)
                {
                    _ime = value;
                    OnPropertyChanged(nameof(Ime));
                }
            }
        }

        // Prezime Property
        public string Prezime
        {
            get => _prezime;
            set
            {
                if (_prezime != value)
                {
                    _prezime = value;
                    OnPropertyChanged(nameof(Prezime));
                }
            }
        }

        // Mjesto Property
        public string Mjesto
        {
            get => _mjesto;
            set
            {
                if (_mjesto != value)
                {
                    _mjesto = value;
                    OnPropertyChanged(nameof(Mjesto));
                }
            }
        }

        // IdMjesta Property
        public int? IdMjesta
        {
            get => _idMjesta;
            set
            {
                if (_idMjesta != value)
                {
                    _idMjesta = value;
                    OnPropertyChanged(nameof(IdMjesta));
                }
            }
        }

        // ToString Method
        public override string ToString()
        {
            return $"{JMB} - {Ime} {Prezime} - {Mjesto}";
        }
    }
}
