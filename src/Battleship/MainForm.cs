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

            for (var row = 0; row < 10; row++)
            {
                for (var column = 0; row < 10; row++)
                {
                    var boardCell = GameBoardPanel.GetControlFromPosition(column, row);


                }
            }
        }
    }
}