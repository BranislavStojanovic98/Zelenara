using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.database.grids
{
    public class PregledIsporukaPojedinacno : INotifyPropertyChanged
    {
        private string _ukupno;

        // Property with INotifyPropertyChanged support
        public string Ukupno
        {
            get => _ukupno;
            set
            {
                if (_ukupno != value)
                {
                    _ukupno = value;
                    OnPropertyChanged(nameof(Ukupno));
                }
            }
        }

        // Constructor
        public PregledIsporukaPojedinacno(string ukupno)
        {
            _ukupno = ukupno;
        }

        // Implementing INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
