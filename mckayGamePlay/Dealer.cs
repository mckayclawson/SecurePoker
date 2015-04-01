using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Dealer
    {
        private SecurePoker gameSession;
        private Deck deck;
        public Dealer(SecurePoker session)
        {
            this.gameSession = session;
            this.deck = new Deck();
        }

        public Card Deal()
        {
            return deck.Deal();
        }
    }
}
