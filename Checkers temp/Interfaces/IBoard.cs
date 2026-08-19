public interface IBoard : ISquare
{
    public Square[] Squares = Square[8, 8];
    public void OnMoveListener(Move move);
    public Board();
}