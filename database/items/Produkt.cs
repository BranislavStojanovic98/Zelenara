using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.database.items
{
    [Serializable]  // Equivalent to Java's Serializable interface
    public class Produkt
    {
        // Fields
        public int IdProdukta { get; set; }
        public string NazivProdukta { get; set; }
        public string VrstaProdukta { get; set; }
        public string Proizvodjac { get; set; }
        public decimal Cena { get; set; }


        // Parameterized Constructor
        public Produkt(int id, string naziv, string vrsta, string proizvodjac, decimal cena)
        {
            IdProdukta = id;
            NazivProdukta = naziv;
            VrstaProdukta = vrsta;
            Proizvodjac = proizvodjac;
            Cena = cena;
        }

        // Set the price and round it to 2 decimal places (using ceiling rounding)
        public void SetCena(decimal cena)
        {
            Cena = Math.Ceiling(cena * 100) / 100; // Round up to two decimal places
        }

        // Override Equals
        public override bool Equals(object? obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            Produkt other = (Produkt)obj;
            return IdProdukta == other.IdProdukta;
        }

        // Override GetHashCode
        public override int GetHashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime * result + IdProdukta;
            return result;
        }
    }
}
