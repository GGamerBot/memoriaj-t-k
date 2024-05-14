using System.Windows;
using System.Windows.Controls;

namespace MemoryGame
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (SizeComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string size = selectedItem.Content.ToString();
                int gridSize = int.Parse(size.Split('x')[0]);

                GameWindow gameWindow = new GameWindow(gridSize);
                gameWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Kérlek válassz egy méretet!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
