using System;

namespace asd5
{
    public class Vitaliy
    {
        private int _maxDepth;

        public Vitaliy(int maxDepth)
        {
            _maxDepth = maxDepth;
        }

        public bool MakeStep(ref Node current)
        {
            Random random = new Random();
            int dice1 = random.Next(1, 7);
            int dice2 = random.Next(1, 7);
            ChanceNode root = new ChanceNode(1, current, dice1 + dice2);
            bool success = MakeDecisionTree(root, out Node nextStep);
            current = nextStep;
            return true;
        }

        private bool MakeDecisionTree(ChanceNode root, out Node nextStep)
        {
            nextStep = new Node();
            root.GetChildren();
            if (root.Children.Count == 0) return false;
            
            double min = 1000;
            foreach (var node in root.Children)
            {
                double newValue = RecursiveSearch(node, 1);
                if (newValue < min)
                {
                    min = newValue;
                    nextStep = node;
                }
            }
            
            return true;
        }
        
        private double RecursiveSearch(Node current, int depth)
        {
            if (depth == _maxDepth)
                return current.GetValue();
            current.GetChildren();
            current.Value = 0;
            foreach (var chanceNode in current.Children)
            {
                chanceNode.GetChildren();
                if (chanceNode.Children.Count == 0) return current.GetValue();

                double min = 1000;
                foreach (var childNode in chanceNode.Children)
                {
                    childNode.Value = RecursiveSearch(childNode, depth + 1);
                    if (childNode.Value < min) min = childNode.Value;
                }

                current.Value += min * chanceNode.Chance;
            }

            return current.Value;
        }
    }
}