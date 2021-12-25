using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using asd5;

namespace Cram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int DicesValue;
        private int[]? numbers = null;
        private bool[] state;
        private Opponent _opponent;
        private Button[] _playerButtons;
        private Button[] _opponentButtons;
        private double playerScore;
        private double opponentScore;

        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            InitializeButtons();
        }

        private void InitializeButtons()
        {
            state = new bool[9];
            for (int j = 0; j < 2; j++)
            {
                Button[] buttons = new Button[9];
                UniformGrid uniformGrid = new UniformGrid();
                uniformGrid.Rows = 9;
                uniformGrid.Columns = 1;
                for (int i = 0; i < 9; i++)
                {
                    Button button = new Button();
                    button.Content = $"{i + 1}";
                    button.FontSize = 20;
                    button.Name = $"num{i}";
                    if (j == 0) button.Click += ButtonOnClick;
                    button.Background = Brushes.Gainsboro;
                    buttons[i] = button;
                    uniformGrid.Children.Add(button);
                }

                if (j == 0)
                {
                    _playerButtons = buttons;
                    PlayerButtons.Children.Add(uniformGrid);
                }
                else
                {
                    _opponentButtons = buttons;
                    OpponentButtons.Children.Add(uniformGrid);
                }

                _opponent = new Opponent(4);
            }
        }

        private void ButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (DicesValue != 0)
            {
                if (numbers == null)
                {
                    numbers = new int[2];
                    numbers[0] = ((Button) sender).Name.ToCharArray()[3] - 47;
                    ((Button) sender).Background = Brushes.LightBlue;
                }
                else if (numbers[0] != 0)
                {
                    numbers[1] = ((Button) sender).Name.ToCharArray()[3] - 47;
                    ((Button) sender).Background = Brushes.LightBlue;
                    if (!state[numbers[0] - 1] && !state[numbers[1] - 1] && numbers[0] != numbers[1] &&
                        numbers[0] + numbers[1] == DicesValue)
                    {
                        state[numbers[0] - 1] = true;
                        state[numbers[1] - 1] = true;
                        _playerButtons[numbers[0] - 1].IsEnabled = false;
                        _playerButtons[numbers[1] - 1].IsEnabled = false;
                        Roll.IsEnabled = true;
                        numbers = null;
                    }
                    else
                    {
                        _playerButtons[numbers[0] - 1].Background = Brushes.Gainsboro;
                        _playerButtons[numbers[1] - 1].Background = Brushes.Gainsboro;
                        numbers = null;
                    }
                }
            }
        }

        private void NothingOnClick(object sender, RoutedEventArgs e)
        {
        }


        private void Roll_OnClick(object sender, RoutedEventArgs e)
        {
            string[] dices =
            {
                "⚀",
                "⚁",
                "⚂",
                "⚃",
                "⚄",
                "⚅"
            };

            Random random = new Random();
            int dice1 = random.Next(6);
            Dice1.Text = dices[dice1];
            int dice2 = random.Next(6);
            Dice2.Text = dices[dice2];
            DicesValue = dice1 + 1;
            if (OneDice.IsChecked != null && (bool) !OneDice.IsChecked)
                DicesValue += dice2 + 1;
            Sum.Text = $"Sum of dices is: {DicesValue}";
            Roll.IsEnabled = false;
            if (IsEnd())
            {
                playerScore = GetValue();
                End.Text = $"Your penalty is: {playerScore}";
                End.Visibility = Visibility.Visible;
                for (int i = 0; i < _playerButtons.Length; i++)
                {
                    _playerButtons[i].Click += NothingOnClick;
                    Roll.IsEnabled = false;
                }
                
                state = new bool[9];
                RollOpponent.IsEnabled = true;
            }
        }

        private void RollBot_OnClick(object sender, RoutedEventArgs e)
        {
            string[] dices =
            {
                "⚀",
                "⚁",
                "⚂",
                "⚃",
                "⚄",
                "⚅"
            };

            Random random = new Random();
            int dice1 = random.Next(6);
            Dice1Op.Text = dices[dice1];
            int dice2 = random.Next(6);
            Dice2Op.Text = dices[dice2];
            DicesValue = dice1 + 1;
            if (OneDice.IsChecked != null && (bool) !OneDice.IsChecked)
                DicesValue += dice2 + 1;
            SumOpponent.Text = $"Sum of dices is: {DicesValue}";

            if (IsEnd())
            {
                opponentScore = GetValue();
                EndOpponent.Text = $"Opponent penalty is: {opponentScore}";
                EndOpponent.Visibility = Visibility.Visible;
                RollOpponent.IsEnabled = false;
                Win.Text = opponentScore < playerScore ? "Opponent won! Try again?" :
                    opponentScore > playerScore ? "You won! Play again?" : "Draw";
                Win.Visibility = Visibility.Visible;
                TryAgain.Visibility = Visibility.Visible;
            }
            
            Node current = new Node(state, null);
            _opponent.MakeStep(ref current, dice1 + 1, dice2 + 1);
            Node.Copy(current.State, out state);
            for (int i = 0; i < 9; i++)
            {
                if (state[i]) _opponentButtons[i].IsEnabled = false;
            }
        }

        public bool IsEnd()
        {
            for (int i = 0; i < state.Length; i++)
            {
                for (int j = i + 1; j < state.Length; j++)
                {
                    if (i == j) continue;
                    if (!state[i] && !state[j] && i + j + 2 == DicesValue)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public double GetValue()
        {
            double value = 0;
            for (int i = 0; i < state.Length; i++)
            {
                if (!state[i]) value += i + 1;
            }

            return value;
        }

        private void TryAgain_OnClick(object sender, RoutedEventArgs e)
        {
            Sum.Text = "Sum of dices is:";
            SumOpponent.Text = "Sum of dices is:";
            End.Visibility = Visibility.Collapsed;
            EndOpponent.Visibility = Visibility.Collapsed;
            Win.Visibility = Visibility.Collapsed;
            TryAgain.Visibility = Visibility.Collapsed;
            Roll.IsEnabled = true;
            state = new bool[9];
            Dice1.Text = "⚀";
            Dice2.Text = "⚀";
            Dice1Op.Text = "⚀";
            Dice2Op.Text = "⚀";
            InitializeButtons();
        }
    }
}