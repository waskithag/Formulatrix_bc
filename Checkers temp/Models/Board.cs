public class Board : IBoard
{
    public Square[] Squares = Square[8, 8];
    public void OnMoveListener(Move move);
    public Board();
}