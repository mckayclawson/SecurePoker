using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Card
    {
        string suit { get;  set; }
        char value { get;  set; }

        public Card(string suit, char value)
        {
            this.suit = suit;
            this.value = value;
        }

        public override string ToString()
        {
            return this.value + " of " + this.suit;
        }
    }
}
