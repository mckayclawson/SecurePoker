using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Deck
    {
        List<Card> deck;
        private bool intialized = false;
        private Random shuf;
        private int deckIndex;
        public Deck()
        {
            IntializeDeck();
            //TODO add some sort of seeding
            this.shuf = new Random();
            deckIndex = 0;
        }

        private void IntializeDeck()
        {
            if(!intialized)
            {
                //TODO create all the cards
                deck = new List<Card>(52);
                foreach (string s in new List<string>{"spade","club","heart","diamond"}){
                    foreach (char v in new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' })
                    {
                        deck.Add(new Card(s, v));
                    }
                }
                intialized = true;
            }
            Shuffle();
        }

        public void Shuffle()
        {
            //TODO implement a better shuffling algorithm
            //Assumption: shuffle is only called on full deck
            deckIndex = 0;
            int temp1 = 0;
            int temp2 = 0;
            Card temp = null;
            this.shuf = new Random();
            for (int i = 0; i < 100; i++)
            {
                temp1 = shuf.Next(0,52);
                temp2 = shuf.Next(0, 52);
                temp = deck[temp1];
                deck[temp1] = deck[temp2];
                deck[temp2] = temp;
            }
        }

        public Card Deal()
        {
            return deck[deckIndex++];
        }
    }
}
