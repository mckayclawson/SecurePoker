using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Particpant
    {
        public string PlayerName { get; private set; }
        public double money { get; set; }
        public List<Card> hand;
        public Particpant(string playerName, double money)
        {
            this.PlayerName = playerName;
            this.money = money;
            this.hand = new List<Card>(2);
        }

        public void ReceiveCard(Card c)
        {
            hand.Add(c);
            Console.WriteLine("player " + this.PlayerName + " recieved " + c.ToString());
        }
        
        
    }
}
