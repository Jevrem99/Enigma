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

        public Reflector(int type)
        {
            switch (type)
            {
                case 1:
                    code = "EJMZALYXVBWFCRQUONTSPIKHGD";
                    Name = "Reflector A";
                    break;
                case 2:
                    code = "YRUHQSLDPXNGOKMIEBFZCWVJAT";
                    Name = "Reflector B";
                    break;
                case 3:
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
