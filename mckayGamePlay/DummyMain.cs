using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class DummyMain
    {
        static void Main(string[] args)
        {
            Particpant m = new Particpant("mckay", 1000);
            Particpant z = new Particpant("zak", 1000);
            Particpant j = new Particpant("jordan", 1000);
            List<Particpant> p = new List<Particpant>();
            p.Add(m);
            p.Add(z);
            p.Add(j);

            SecurePoker game = new SecurePoker(p);

            game.StartRound();
            game.AnteUp();
            game.DealRound();
            game.Bets();
            game.Flop();
            game.Bets();
            game.Turn();
            game.Bets();
            game.River();
            game.Bets();
            game.judgeWinner();
            Console.Read();
        }
    }
}
