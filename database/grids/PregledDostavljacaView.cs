using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.database.grids
{
    public class PregledDostavljacaView : INotifyPropertyChanged
    {
        // Fields for binding
        private int _idDostavljaca;
        private string _nazivDostavljaca;
        private string _adresaDostavljaca;
        private string _nazivMjesta;

        // Properties with INotifyPropertyChanged support
        public int IdDostavljaca
        {
            get => _idDostavljaca;
            set
            {
                if (_idDostavljaca != value)
                {
                    _idDostavljaca = value;
                    OnPropertyChanged(nameof(IdDostavljaca));
                }
            }
        }

        public string NazivDostavljaca
        {
            get => _nazivDostavljaca;
            set
            {
                if (_nazivDostavljaca != value)
                {
                    _nazivDostavljaca = value;
                    OnPropertyChanged(nameof(NazivDostavljaca));
                }
            }
        }

        public string AdresaDostavljaca
        {
            get => _adresaDostavljaca;
            set
            {
                if (_adresaDostavljaca != value)
                {
                    _adresaDostavljaca = value;
                    OnPropertyChanged(nameof(AdresaDostavljaca));
                }
            }
        }

        public string NazivMjesta
        {
            get => _nazivMjesta;
            set
            {
                if (_nazivMjesta != value)
                {
                    _nazivMjesta = value;
                    OnPropertyChanged(nameof(NazivMjesta));
                }
            }
        }

        // Constructor
        public PregledDostavljacaView(int id, string nazivDost, string adresa, string mjesto)
        {
            _idDostavljaca = id;
            _nazivDostavljaca = nazivDost;
            _adresaDostavljaca = adresa;
            _nazivMjesta = mjesto;
        }



        // Implementing INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
