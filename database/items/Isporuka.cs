using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.database.items
{
    [Serializable]  // Marking the class as serializable
    public class Isporuka
    {
        // Properties
        public int IdIsporuke { get; set; }
        public string Datum { get; set; }
        public Dobavljac Dobavljac { get; set; }
        public Nabavka Nabavka { get; set; }
        public Produkt Produkt { get; set; }
        public int KolicinaProdukta { get; set; }

        // Default Constructor

        // Parameterized Constructor
        public Isporuka(int id, string datum, Dobavljac dobavljac, Nabavka nabavka, Produkt produkt, int kolicinaProdukta)
        {
            IdIsporuke = id;
            Datum = datum;
            Dobavljac = dobavljac;
            Nabavka = nabavka;
            Produkt = produkt;
            KolicinaProdukta = kolicinaProdukta;
        }

        // Override Equals
        public override bool Equals(object? obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            Isporuka other = (Isporuka)obj;
            return IdIsporuke == other.IdIsporuke;
        }

        // Override GetHashCode
        public override int GetHashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime * result + IdIsporuke;
            return result;
        }

        // ToString Method
        public override string ToString()
        {
            return $"{Datum} - {Dobavljac.NazivDostavljaca} - ";
        }
    }
}
