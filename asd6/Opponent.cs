using System;

namespace asd5
{
    public class Opponent
    {
        private int _maxDepth;
        public bool[] State;

        public Opponent(int maxDepth)
        {
            _maxDepth = maxDepth;
        }

        public bool MakeStep(ref Node current, int dice1, int dice2)
        {
            bool success;
            if (!current.PossibleOneDice)
            {
                ChanceNode root = new ChanceNode(1, current, dice1 + dice2, false);
                success = MakeDecisionTree(root, out Node nextStep);
                current = nextStep;
                State = current.State;
                return success;
            }
            else
            {
                success = MakeDecisionTree(current, out Node nextStep, dice1, dice2);
                current = nextStep;
                State = current.State;
                return success;
            }
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
        
        private bool MakeDecisionTree(Node root, out Node nextStep, int dice1, int dice2)
        {
            nextStep = new Node();
            root.GetChildren();
            Node[] bestStepsOne = new Node[root.Children.Count];
            Node[] bestStepsTwo = new Node[root.Children.Count];
            double minOne = 1000;
            double minTwo = 1000;
            
            for (int i = 0; i<root.Children.Count; i++)
            {
                minOne = 1000;
                minTwo = 1000;
                Node bestOne = new Node();
                Node bestTwo = new Node();
                
                root.Children[i].GetChildren();
                
                foreach (var node in  root.Children[i].Children)
                {
                    double newValue = RecursiveSearch(node, 1);
                    if (root.Children[i].OneDice)
                    {
                        if (newValue < minOne)
                        {
                            minOne = newValue;
                            bestOne = node;
                        }
                    }
                    else
                    {
                        if (newValue < minTwo)
                        {
                            minTwo = newValue;
                            bestTwo = node;
                        }
                    }
                }

                bestStepsOne[i] = bestOne;
                bestStepsTwo[i] = bestTwo;
            }

            if (minOne < minTwo)
            {
                if ((int)minOne == 1000) return false;
                int diceValue = dice1;
                int j;
                for (j = 0; j < root.Children.Count; j++)
                {
                    if (root.Children[j].DiceValue == diceValue) break;
                }
                nextStep = bestStepsOne[j];
                return true;
            }
            
            if ((int)minTwo == 1000) return false;
            int twoDiceValue = dice1 + dice2;
            int k;
            for (k = 0; k < root.Children.Count; k++)
            {
                if (root.Children[k].DiceValue == twoDiceValue) break;
            }

            nextStep = bestStepsOne[k];
            return true;
        }

        private int GetRandom(int diceNum)
        {
            Random random = new Random();
            int value = 0;
            for (int i = 0; i < diceNum; i++)
            {
                value += random.Next(1, 7);
            }
        
            return value;
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