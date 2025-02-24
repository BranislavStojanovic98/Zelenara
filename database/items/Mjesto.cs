using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.database.items
{
    [Serializable]  // Equivalent to Java's Serializable interface
    public class Mjesto
    {
        // Fields
        public int PostanskiBroj { get; set; }
        public string NazivMjesta { get; set; }


        // Parameterized Constructor
        public Mjesto(int pB, string nM)
        {
            PostanskiBroj = pB;
            NazivMjesta = nM;
        }

        // Method to get ID from the name
        public int GetIDfromNazivMjesta(string naziv)
        {
            if (this.NazivMjesta == naziv)
                return PostanskiBroj;
            else
                return 0;
        }

        // Override Equals
        public override bool Equals(object? obj)  // Make 'obj' nullable
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            Mjesto other = (Mjesto)obj;
            return PostanskiBroj == other.PostanskiBroj;
        }

        // Override GetHashCode
        public override int GetHashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime * result + PostanskiBroj;
            return result;
        }

        // ToString method
        public override string ToString()
        {
            return NazivMjesta;
        }
    }
}
