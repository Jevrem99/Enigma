using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Models
{
    class Plugboard
    {
        private Dictionary<char, char> wiring = new Dictionary<char, char>();

        public Plugboard() { }

        public void Plug(char A,char B)
        {
            wiring[A] = B;
            wiring[B] = A;
        }

        public void Unplug(char A, char B)
        {
            if (wiring.ContainsKey(A) && wiring.ContainsKey(B))
            {
                wiring.Remove(A);
                wiring.Remove(B);
            }
        }

        public char Swap(char input)
        {
            System.Diagnostics.Debug.WriteLine("Plugboard: " + input + " -> " + (wiring.ContainsKey(input) ? wiring[input] : input));
            return wiring.ContainsKey(input)? wiring[input]: input;
        }
    }
}
