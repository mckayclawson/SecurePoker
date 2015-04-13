using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Rules
    {
        /**
         * Straight Flush  -> 9
         * Four of a Kind  -> 8
         * Full House      -> 7
         * Flush           -> 6
         * Straight        -> 5
         * Three of a Kind -> 4
         * Two Pair        -> 3
         * One Pair        -> 2
         * High Card       -> 1
         */
        public static int judgeHand(List<Card> hand, List<Card> table)
        {
            List<Card> cards = new List<Card>(7);
            cards.Add(hand[0]);
            cards.Add(hand[1]);
            cards.Add(table[0]);
            cards.Add(table[1]);
            cards.Add(table[2]);
            cards.Add(table[3]);
            cards.Add(table[4]);
            if (isPair(cards))
            {
                return 2;
            }
            else
            {
                return 1;
            }

        }

        public static bool isPair(List<Card> hand)
        {
            foreach (Card c1 in hand)
            {
                foreach (Card c2 in hand)
                {
                    if ((c1.value.Equals(c2.value)) && (c1.suit == c2.suit))
                    {
                        //do nothing
                    }
                    else
                    {
                        if (c1.value == c2.value)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool isTwoPair(List<Card> hand)
        {
            return false;
        }

        public static bool isThreeOfAKind(List<Card> hand)
        {
            return false;
        }

        public static bool isStraight(List<Card> hand)
        {
            return false;
        }

        public static bool isFlush(List<Card> hand)
        {
            return false;
        }

        public static bool isFullHouse(List<Card> hand)
        {
            return false;
        }

        public static bool isFourOfAKind(List<Card> hand)
        {
            return false;
        }

        public static bool isStraightFlush(List<Card> hand)
        {
            return false;
        }

    }
}
