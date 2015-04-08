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
        private List<Card> table;
        private double pot;
        private int dealerIndex = 0;
        private double smallBlind = 50;
        private double bigBlind = 100;

        public SecurePoker(List<Particpant> players)
        {
            this.players = players;
            this.dealer = new Dealer(this);
            this.table = new List<Card>(5);
            roundInProgress = false;
            pot = 0;
        }

        public void StartRound()
        {
            Console.WriteLine("Round Started with " + players.Count + " players");
            pot = 0;
            foreach (Particpant player in players)
            {
                player.isFold = false;
            }
            this.roundInProgress = true;

            
        }


        public void AnteUp()
        {
            pot += players[(dealerIndex + 1) % players.Count].withdrawMoney(smallBlind);
            pot += players[(dealerIndex + 1) % players.Count].withdrawMoney(bigBlind);
            Console.WriteLine("Anted up");
        }

        public void DealRound()
        {
            for (int i = 0; i < players.Count * 2; i++)
            {
                players[i % players.Count].ReceiveCard(dealer.Deal());
            }
        }

        public void Bets()
        {
            //TODO bets should be happening from the left of the dealer (not in the order of the participant list)
            foreach (Particpant player in players)
            {
                if (!player.isFold)
                {
                    Console.WriteLine("It's " + player.PlayerName + "'s bet: cent amount or f for fold");
                    string input = Console.ReadLine();
                    if (input.Equals("f"))
                    {
                        Console.WriteLine(player.PlayerName + " has folded.");
                        player.isFold = true;
                    }
                    else
                    {
                        //TODO a bet shouldn't be smaller than max bet
                        //TODO handle bad input
                        //TODO keep track of each players bet amount for the entire hand
                        double bet = Double.Parse(input);
                        pot += player.withdrawMoney(bet);
                        Console.WriteLine(player.PlayerName + " bet " + input);
                    }
                }
            }
            Console.WriteLine("\nThe pot is " + pot + " cents\n");
        }

        public void Flop()
        {
            //Flop
            Console.WriteLine("The Flop:");
            table.Add(dealer.Deal());
            table.Add(dealer.Deal());
            table.Add(dealer.Deal());
            foreach (Card c in table)
            {
                Console.WriteLine(c.ToString());
            }
        }

        public void Turn()
        {
            //Turn
            Console.WriteLine("Turn:");
            table.Add(dealer.Deal());
            foreach (Card c in table)
            {
                Console.WriteLine(c.ToString());
            }
        }

        public void River()
        {
            //River
            Console.WriteLine("River:");

            table.Add(dealer.Deal());
            foreach (Card c in table)
            {
                Console.WriteLine(c.ToString());
            }
        }
    }
}
