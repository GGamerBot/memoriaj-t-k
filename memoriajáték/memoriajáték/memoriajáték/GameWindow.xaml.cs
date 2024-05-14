using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace MemoryGame
{
    public partial class GameWindow : Window
    {
        private int gridSize;
        private Button[,] buttons;
        private int[,] numbers;
        private bool isFirstClick = true;
        private Button firstButton, secondButton;
        private DispatcherTimer timer;
        private bool canClick = true;

        public GameWindow(int gridSize)
        {
            InitializeComponent();
            this.gridSize = gridSize;
            InitializeGame();
        }

        private void InitializeGame()
        {
            buttons = new Button[gridSize, gridSize];
            numbers = GenerateNumbers(gridSize);

            for (int i = 0; i < gridSize; i++)
            {
                GameGrid.RowDefinitions.Add(new RowDefinition());
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    Button button = new Button
                    {
                        FontSize = 24,
                        FontFamily = new FontFamily("Comic Sans MS"),
                        Foreground = Brushes.White, // Szöveg színe
                        Background = Brushes.Black, // Gombok háttérszíne
                        Tag = numbers[i, j]
                    };
                    button.Click += Button_Click;
                    buttons[i, j] = button;
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    GameGrid.Children.Add(button);
                }
            }
        }

        private int[,] GenerateNumbers(int size)
        {
            int[] tempNumbers = new int[size * size];
            for (int i = 0; i < size * size / 2; i++)
            {
                tempNumbers[i * 2] = i + 1;
                tempNumbers[i * 2 + 1] = i + 1;
            }

            Random rand = new Random();
            tempNumbers = tempNumbers.OrderBy(x => rand.Next()).ToArray();

            int[,] result = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    result[i, j] = tempNumbers[i * size + j];
                }
            }

            return result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!canClick) return;

            Button clickedButton = sender as Button;
            if (clickedButton == null || clickedButton.Content != null)
                return;

            clickedButton.Content = clickedButton.Tag;
            clickedButton.Foreground = Brushes.White; // Szín beállítása felfedéskor

            if (isFirstClick)
            {
                firstButton = clickedButton;
                isFirstClick = false;
            }
            else
            {
                secondButton = clickedButton;
                canClick = false;

                if ((int)firstButton.Tag == (int)secondButton.Tag)
                {
                    firstButton = null;
                    secondButton = null;
                    canClick = true;
                    CheckWinCondition();
                }
                else
                {
                    timer = new DispatcherTimer
                    {
                        Interval = TimeSpan.FromSeconds(0.5)
                    };
                    timer.Tick += Timer_Tick;
                    timer.Start();
                }
                isFirstClick = true;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            firstButton.Content = null;
            secondButton.Content = null;
            firstButton = null;
            secondButton = null;
            canClick = true;
        }

        private void CheckWinCondition()
        {
            foreach (Button button in buttons)
            {
                if (button.Content == null)
                    return;
            }

            MessageBox.Show("Gratulálok, nyertél!", "Győzelem", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
