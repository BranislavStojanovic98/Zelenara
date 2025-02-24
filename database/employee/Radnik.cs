using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.database.employee
{
    [Serializable]
    public class Radnik : Zaposleni
    {
        // Constructor
        public Radnik(string jmb, string ime, string prezime, string mjesto)
            : base(jmb, ime, prezime, mjesto)
        {
        }
    }
}
