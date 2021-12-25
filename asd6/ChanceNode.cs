using System.Collections.Generic;

namespace asd5
{
    public class ChanceNode
    {
        private Node _parent;
        private double _chance;
        public double Chance => _chance;
        public readonly List<Node> Children;
        private readonly int _diceValue;

        public ChanceNode(double chance, Node parent, int diceValue)
        {
            _chance = chance;
            _parent = parent;
            _diceValue = diceValue;
            Children = new List<Node>();
        }

        public void GetChildren()
        {
            if (Children.Count > 0) return;
            for (int i = 0; i < _parent.State.Length; i++)
            {
                for (int j = i + 1; j < _parent.State.Length; j++)
                {
                    if (i == j) continue;
                    if (!_parent.State[i] && !_parent.State[j] && i + j + 2 == _diceValue)
                    {
                        Node.Copy<bool>(_parent.State, out bool[] newState);
                        newState[i] = true;
                        newState[j] = true;
                        Children.Add(new Node(newState, this));
                    }
                }
            }
        }
    }
}