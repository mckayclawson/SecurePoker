using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConsoleApplication2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace PokerServer.Models
{
    public class PokerModel
    {
        private List<Particpant> players;
        private Deck deck;
        private List<Card> table;
        private double pot;
        private Particpant currentTurn;
        private int currentTurnIndex = 0;
        private int dealIndex = 0;
        private double stakes;
        private int roundNumber;
        public PokerModel()
        {
            this.players = new List<Particpant>();
            Particpant m = new Particpant("mckay", 100000);
            Particpant z = new Particpant("zak", 100000);
            Particpant j = new Particpant("jordan", 100000);
            this.players.Add(m);
            this.players.Add(z);
            this.players.Add(j);
            this.deck = new Deck();
            this.table = new List<Card>(5);
            stakes = 0;
            pot = 0;
            currentTurn = players[currentTurnIndex];
            roundNumber = 0;
            Debug.WriteLine("PokerModelConstructor");
            PlayRound();
        }



        public void PlayRound()
        {
            roundNumber += 1;
            Console.WriteLine("Round Started with " + players.Count + " players");
            pot = 0;
            foreach (Particpant player in players)
            {
                player.isFold = false;
                player.currentBet = 0;
                player.hand = new List<Card>(2);
            }
            this.deck = new Deck();
            this.currentTurnIndex = 0;
            this.dealIndex = 0;
            stakes = 0;
            this.table = new List<Card>(5);
            DealRound();
            //Advance();
            
        }


        public void DealRound()
        {
            for (int i = 0; i < players.Count * 2; i++)
            {
                players[i % players.Count].ReceiveCard(deck.Deal());
            }
        }


        public void Flop()
        {
            //Flop
            table.Add(deck.Deal());
            table.Add(deck.Deal());
            table.Add(deck.Deal());
        }

        public void Turn()
        {
            //Turn
            table.Add(deck.Deal());
        }

        public void River()
        {
            //River
            table.Add(deck.Deal());
        }

        public void judgeWinner()
        {
            int winningIndex = 0;
            int bestHand = 0;
            for (int i = 0; i < players.Count; i++)
            {
                if(!(players[i].isFold))
                {
                    int thisHand = Rules.judgeHand(players[i].hand, table);
                    if (thisHand > bestHand)
                    {
                        bestHand = thisHand;
                        winningIndex = i;
                    }
                }
            }
            //TODO Account for possible tied hands (need to judge high card and be able to split the pot
            Console.WriteLine("Player " + players[winningIndex].PlayerName + " has won pot of " + pot + " cents.\nRound Over.");
            players[winningIndex].depositMoney(pot);
        }

        public Newtonsoft.Json.Linq.JObject getState()
        {
            Newtonsoft.Json.Linq.JObject state = new Newtonsoft.Json.Linq.JObject();
            state["roundNumber"] = this.roundNumber;
            state["pot"] = (int)this.pot;
            state["currentTurn"] = currentTurn.PlayerName;
            state["maxBet"] = (int)this.stakes;
            state["tableCards"] = SerializeCards(this.table);
            state["players"] = SerializePlayers();
            return state;
        }

        public Newtonsoft.Json.Linq.JArray SerializePlayers()
        {
            Newtonsoft.Json.Linq.JArray jPlayers = new Newtonsoft.Json.Linq.JArray();
            foreach (Particpant p in players)
            {
                JObject temp = new JObject();
                temp["playerNum"] = players.IndexOf(p);
                temp["username"] = p.PlayerName;
                temp["playerMoney"] = (int)p.money;
                temp["out"] = p.isFold;
                temp["currentBet"] = (int)p.currentBet;
                temp["cards"] = SerializeCards(p.hand);
                jPlayers.Add(temp);
            }
            return jPlayers;
        }

        public JArray SerializeCards(List<Card> cards)
        {
            JArray jCards = new JArray();
            foreach (Card card in cards)
            {
                JObject temp = new JObject();
                temp["face"] = card.ServerString();
                jCards.Add(temp);
            }
            return jCards;
        }

        public void ProcessMessage(ClientMessage message)
        {
            if (message.fold)
            {
                currentTurn.isFold = true;
            }
            else
            {
                //TODO add amount checking
                if (currentTurn.currentBet + message.bet >= stakes)
                {
                    pot += currentTurn.withdrawMoney(message.bet);
                    updateStakes();
                }
                else
                {
                    //need to send some message indicating invalid bet
                    //maybe just make the player fold
                }
                
            }
            //TODO advance current turn
            Advance();
        }

        public void Advance()
        {
            if (currentTurnIndex == players.Count)
            {
                currentTurnIndex = 0;
                if (betEqual())
                {
                    //the current round is over now the dealer needs to deal
                    DealNext();
                }
            }
            currentTurn = players[currentTurnIndex];
            currentTurnIndex += 1;
            //if the player has folded they cannot bet
            if (currentTurn.isFold)
            {
                Advance();
            }

            
        }

        public void updateStakes()
        {
            foreach(Particpant p in players)
            {
                if (p.currentBet > this.stakes)
                {
                    this.stakes = p.currentBet;
                }
            }
        }

        public bool betEqual()
        {
            double pBet = 0;
            bool start = true;
            foreach (Particpant p in players)
            {
                if (!p.isFold)
                {
                    if (start)
                    {
                        pBet = p.currentBet;
                        start = false;
                    }
                    if (pBet != p.currentBet)
                    {
                        return false;
                    }   
                }
            }
            return true;
        }

        public void DealNext()
        {
            if (this.dealIndex == 0)
            {
                Flop();
                dealIndex += 1;
            }
            else if(this.dealIndex == 1)
            {
                Turn();
                dealIndex += 1;
            }
            else if (this.dealIndex == 2)
            {
                River();
                dealIndex += 1;
            }
            else if (this.dealIndex == 3)
            {
                this.dealIndex = 0;
                judgeWinner();
                PlayRound();
            }
        }
    }
}