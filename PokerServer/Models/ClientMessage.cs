using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokerServer.Models
{
    public class ClientMessage
    {
        public bool fold;
        public double bet;

        public ClientMessage(bool fold, double bet)
        {
            this.fold = fold;
            this.bet = bet;
        }
    }
}