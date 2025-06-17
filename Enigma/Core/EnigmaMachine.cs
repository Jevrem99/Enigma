using Enigma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Enigma.Core
{
    class EnigmaMachine
    {
        private Rotor firstSlot;
        private Rotor secondSlot;
        private Rotor thirdSlot;

        private Reflector reflector;

        private Plugboard plugboard;

        private int firstConf;
        private int secondConf;
        private int thirdConf;

        public EnigmaMachine()
        {
            plugboard = new Plugboard();
        }
        public void setAll(int typeFirst, int typeSecond, int typeThird, int reflectorType, int firstConf, int secondConf, int thirdConf)
        {
            firstSlot = new Rotor(typeFirst, firstConf);
            secondSlot = new Rotor(typeSecond, secondConf);
            thirdSlot = new Rotor(typeThird, thirdConf);
            reflector = new Reflector(reflectorType);
            this.firstConf = firstConf;
            this.secondConf = secondConf;
            this.thirdConf = thirdConf;
        }

        public void setConf(int firstConf, int secondConf, int thirdConf)
        {
            this.firstConf = firstConf;
            this.secondConf = secondConf;
            this.thirdConf = thirdConf;
        }

        public void setPlug(char A,char B)
        {
            plugboard.Plug(A, B);
        }

        private void TurnRotor()
        {
            thirdConf = ++thirdConf % 26;
            thirdSlot.SetConf(thirdConf);
            if (thirdConf == 0)
            {
                secondConf = ++secondConf % 26;
                secondSlot.SetConf(secondConf);
                if (secondConf == 0)
                {
                    firstConf = ++firstConf % 26;
                    firstSlot.SetConf(firstConf);
                    if (firstConf == 0)
                    {
                        thirdConf = 0;
                        thirdSlot.SetConf(thirdConf);
                        secondConf = 0;
                        secondSlot.SetConf(secondConf);
                    }
                }
            }
        }
        public char Encrypt(char c)
        {
            TurnRotor();
            char x = plugboard.Swap(c);
            x = thirdSlot.GetLetter(x);
            x = secondSlot.GetLetter(x);
            x = firstSlot.GetLetter(x);
            x = reflector.GetLetter(x);
            x = firstSlot.GetBackwards(x);
            x = secondSlot.GetBackwards(x);
            x = thirdSlot.GetBackwards(x);
            x = plugboard.Swap(x);
            System.Diagnostics.Debug.WriteLine("Output: " + x);
            return x;
        }

        public String EncryptText(String text)
        {
            String output = "";
            foreach (char c in text)
            {
                output += Encrypt(c);
            }
            return output;
        }
    }
}
