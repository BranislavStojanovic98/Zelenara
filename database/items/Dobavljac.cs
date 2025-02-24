using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.database.items
{
    [Serializable]  // Equivalent to Java's Serializable interface
    public class Dobavljac
    {
        // Fields
        public int IdDostavljaca { get; set; }
        public string NazivDostavljaca { get; set; }
        public string AdresaDostavljaca { get; set; }
        public Mjesto Mjesto { get; set; }


        // Parameterized Constructor
        public Dobavljac(int id, string naziv, string adresa, Mjesto mjesto)
        {
            IdDostavljaca = id;
            NazivDostavljaca = naziv;
            AdresaDostavljaca = adresa;
            Mjesto = mjesto;
        }

        // ToString Method
        public override string ToString()
        {
            return $"{NazivDostavljaca} - {AdresaDostavljaca}";
        }
    }
}
