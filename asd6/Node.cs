using System.Collections.Generic;

namespace asd5
{
    public class Node
    {
        public List<ChanceNode> Children;
        public bool[] State;
        public double Value;
        private ChanceNode _parent;
        public bool PossibleOneDice => State[6] && State[7] && State[8];

        public Node()
        {
            Children = new List<ChanceNode>();
            State = new bool[9];
        }
        
        public Node(bool[] state, ChanceNode parent)
        {
            Children = new List<ChanceNode>();
            State = state;
            _parent = parent;
        }

        public double GetValue()
        {
            if (Children.Count != 0) return -1;
            double value = 0;
            for (int i = 0; i < State.Length; i++)
            {
                if (!State[i]) value += i + 1;
            }

            Value = value;
            return value;
        }

        public double GetExpectedValue()
        {
            double expected = 0;
            foreach (var chanceNode in Children)
            {
                expected += chanceNode.Chance * Value;
            }

            return expected;
        }

        public void GetChildren()
        {
            if (Children.Count > 0) return;
            if (PossibleOneDice)
            {
                double probability = 0.167;
                for (int i = 1; i < 7; i++)
                {
                    Children.Add(new ChanceNode(probability, this, i, true));
                }
            }

            double[] chances = new[]
                {0.028, 0.056, 0.083, 0.111, 0.139, 0.167, 0.194, 0.167, 0.139, 0.111, 0.083, 0.056, 0.028};
            for (int i = 2; i < 13; i++)
            {
                Children.Add(new ChanceNode(chances[i-2], this, i, false));
            }
        }

        public static void Copy<T>(T[] source, out T[] destination)
        {
            destination = new T[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                destination[i] = source[i];
            }
        }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < State.Length; i++)
            {
                result += State[i] ? "- " : $"{i + 1} ";
            }
            return result;
        }
    }
}