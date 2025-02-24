using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.database.employee
{
    [Serializable]
    public class Menadzer : Zaposleni
    {
        // Constructor
        public Menadzer(string jmb, string ime, string prezime, string mjesto)
            : base(jmb, ime, prezime, mjesto)  // Calling the base class constructor (Zaposleni)
        {
            // Additional constructor logic can go here if needed
        }
    }
}
