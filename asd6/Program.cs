using System;

namespace asd5
{
    public static class Program
    {
        public static void Main()
        {
            Opponent opponent = new Opponent(4);
                Node current = new Node();
                while (opponent.MakeStep(ref current, 6, 6))
                {
                    Console.WriteLine(current);
                }
        }
    }
}