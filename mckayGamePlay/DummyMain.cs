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
            Particpant m = new Particpant("mckay", 100);
            Particpant z = new Particpant("zak", 100);
            Particpant j = new Particpant("jordan", 100);
            List<Particpant> p = new List<Particpant>();
            p.Add(m);
            p.Add(z);
            p.Add(j);

            SecurePoker game = new SecurePoker(p);

            game.StartRound();
            Console.Read();
        }
    }
}
