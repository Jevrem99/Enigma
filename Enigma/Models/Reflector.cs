using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Models
{
    class Reflector
    {

        private String code;
        private String Name { get; }

        public Reflector(String type)
        {
            switch (type)
            {
                case "A":
                    code = "EJMZALYXVBWFCRQUONTSPIKHGD";
                    Name = "Reflector A";
                    break;
                case "B":
                    code = "YRUHQSLDPXNGOKMIEBFZCWVJAT";
                    Name = "Reflector B";
                    break;
                case "C":
                    code = "FVPJIAOYEDRZXWGCTKUQSBNMHL";
                    Name = "Reflector C";
                    break;
            }
        }

        public char GetLetter(char A)
        {
            System.Diagnostics.Debug.WriteLine(Name + ": " + A + " -> " + code[A - 65]);
            return code[A - 65];
        }

    }
}
