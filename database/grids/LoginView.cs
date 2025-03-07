using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.database.grids
{
    class LoginView
    {

        private string _jmb;
        private string _username;
        private string _password;
        private bool _isMenager;

        public LoginView(string jmb, string username, string password, bool isMenager)
        {
            _jmb = jmb;
            _username = username;
            _password = password;
            _isMenager = isMenager;
        }

        public string JMB
        {
            get => _jmb;
            set
            {
                if(_jmb != value)
                {
                    _jmb = value;
                    OnPropertyChanged(nameof(JMB));
                }
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public bool IsMenager
        {
            get => _isMenager;
            set
            {
                if (_isMenager != value)
                {
                    _isMenager = value;
                    OnPropertyChanged(nameof(IsMenager));
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
