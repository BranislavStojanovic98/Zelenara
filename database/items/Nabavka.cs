using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.database.employee;

namespace WpfApp1.database.items
{
    [Serializable]
    public class Nabavka
    {
        // Fields
        public int IdPotvrde { get; set; }
        public string Datum { get; set; }
        public Menadzer Menadzer { get; set; }
        public Dobavljac Dobavljac { get; set; }
        public Produkt Produkt { get; set; }
        public int KolicinaProdukta { get; set; }
        public decimal CenaIsporuke { get; set; }

        // Parameterized Constructor
        public Nabavka(int id, string datum, Dobavljac dobavljac, Menadzer menadzer, Produkt produkt, int kolicinaProdukta, decimal cenaIsporuke)
        {
            IdPotvrde = id;
            Datum = datum;
            Menadzer = menadzer;
            Dobavljac = dobavljac;
            Produkt = produkt;
            KolicinaProdukta = kolicinaProdukta;
            CenaIsporuke = cenaIsporuke;
        }

        // Set the price and round it to 2 decimal places (using ceiling rounding)
        public void SetCenaIsporuke(decimal cenaIsporuke)
        {
            CenaIsporuke = Math.Ceiling(cenaIsporuke * 100) / 100; // Round up to two decimal places
        }

        // Override Equals
        public override bool Equals(object? obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            Nabavka other = (Nabavka)obj;
            return IdPotvrde == other.IdPotvrde;
        }

        // Override GetHashCode
        public override int GetHashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime * result + IdPotvrde;
            return result;
        }

        // ToString method
        public override string ToString()
        {
            return $"{IdPotvrde} - {Datum} - {Dobavljac} - {Menadzer}";
        }
    }
}
