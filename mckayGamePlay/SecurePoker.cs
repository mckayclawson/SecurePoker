using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class SecurePoker
    {
        private List<Particpant> players;
        private Dealer dealer;
        private bool roundInProgress;

        public SecurePoker(List<Particpant> players)
        {
            this.players = players;
            this.dealer = new Dealer(this);
            roundInProgress = false;
        }

        public void StartRound()
        {
            Console.WriteLine("Round Started with " + players.Count + " players");
            //TODO add actual game play shit
            for (int i = 0; i < players.Count * 2; i++)
            {
                players[i % players.Count].ReceiveCard(dealer.Deal());
            }
        }

        
    }
}
