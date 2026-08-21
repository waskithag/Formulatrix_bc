using System.Data;
using System.Reflection.Metadata;

public class GameController
{
    private readonly List<Player> _players;
    private Player _currentPlayer;
    private readonly string rule;
    private readonly bool _flyingKing;
    private readonly bool _forceCapture;
    private Board _board;
    private GameStatus _gameStatus;

    //event move made
    //event gameover

    public void Game(Player playerOne, Player playerTwo, Board board)
    {
        //Function to initialize the game
    }

    public string GetRuleSet()
    {
        //Give ruleset choice to play 
    }

    public void StartGame(string rule, bool _flyingKing, bool _forceCapture)
    {
        //function to start the game
    }

    public void MakeMove(Move move)
    {
        //Move made and update the board condition
    }

    public void SwitchTurn()
    {
        //Change player turn
    }

    public List<Player> GetAllPlayer() => _players;

    public Board GetBoardState() => _board;

    public GameStatus CheckGameStatus() => _gameStatus;

    public Player GetCurrentPlayer() => _currentPlayer;

    public List<Move> GetValidMove(Position currentPosition)
    {
        //Check piece color : Red start from bottom (move to lower index count as forward) Black start from top (move to higher index tiles count as forward)
        List<Move> result = new();

        //Check king?
            //Check flying king rule

        //check forced capture    

        return result;
    }

    public int GetTotalPiece(PieceColor color)
    {
        // Return the total number of piece a color have in the board;
        int rowNow = 0;
        int colNow = 0;
        int totalPiece = 0;

        while (rowNow < _board.Square.GetLenght(0))
        {
            while (colNow < _board.Square.GetLenght(1))
            {
                if(_board.Square[rowNow, colNow].Piece?.Color == color)
                {
                    totalPiece++;
                }
                colNow++;
            }
            rowNow++;
        }

        return totalPiece;
    }

    public void Restart()
    {
        
    }
}