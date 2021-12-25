using System;

namespace asd5
{
    public static class Program
    {
        public static void Main()
        {
            Vitaliy vitaliy = new Vitaliy(3);
            Node current = new Node();
            while (vitaliy.MakeStep(ref current))
            {
                Console.WriteLine(current);
            }
        }
    }
}