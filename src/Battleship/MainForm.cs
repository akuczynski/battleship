using Battleship.Controls;

namespace Battleship
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }



        private void NewGameButton_Click_1(object sender, EventArgs e)
        {
            var gameBoard = new GameBoard();
            var cells = GameBoardPanel.Controls.GetEnumerator();

            gameBoard.StartNewGame(cells);
        }
    }
}