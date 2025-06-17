using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Models
{
    class Rotor
    {
        private String code;
        private int currentConf = 0;
        private String Name { get; }

        public Rotor(int type, int currentConf)
        {
            switch (type)
            {
                case 1:
                    code = "EKMFLGDQVZNTOWYHXUSPAIBRCJ";
                    Name = "rotor I";
                    break;
                case 2:
                    code = "AJDKSIRUXBLHWTMCQGZNPYFVOE";
                    Name = "rotor II";
                    break;
                case 3:
                    code = "BDFHJLCPRTXVZNYEIWGAKMUSQO";
                    Name = "rotor III";
                    break;
                case 4:
                    code = "ESOVPZJAYQUIRHXLNFTGKDCMWB";
                    Name = "rotor IV";
                    break;
                case 5:
                    code = "VZBRGITYUPSDNHLXAWMJQOFECK";
                    Name = "rotor V";
                    break;
            }
            this.currentConf = currentConf;
        }

        public void SetConf(int conf)
        {
            currentConf = conf;
        }

        public char GetLetter(char x)
        {
            int input = x - 65;
            int output = (input + currentConf) % 26;
            System.Diagnostics.Debug.WriteLine(this.Name + ": " + x + " -> " + code[output]);
            return code[output];
        }

        public char GetBackwards(char x)
        {
            int indexInWiring = code.IndexOf(x);
            int shiftedIndex = (indexInWiring - currentConf + 26) % 26;
            System.Diagnostics.Debug.WriteLine(this.Name + ": " + x + " -> " + (char)(65 + shiftedIndex));
            return (char)(65 + shiftedIndex);
        }
    }
}
