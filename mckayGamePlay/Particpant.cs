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
        public bool isFold;
        public Particpant(string playerName, double money)
        {
            this.PlayerName = playerName;
            this.money = money;
            this.hand = new List<Card>(2);
            this.isFold = false;
        }

        public void ReceiveCard(Card c)
        {
            hand.Add(c);
            Console.WriteLine("player " + this.PlayerName + " recieved " + c.ToString());
        }

        public double withdrawMoney(double amount)
        {
            if (money > amount)
            {
                money -= amount;
                return amount;
            }
            else
            {
                //TODO should we kick players out or make them fold??
            }
            return 0;
        }
        
        
    }
}
